import { AfterViewInit, Component, computed, HostListener, inject, OnDestroy, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { HeaderComponent } from "@components/header/header.component";
import { ParameterizationConsumeService, } from '@utils/services/parameterization-consume.service';
import { StateService } from '@utils/services/state.service';
import { getPageAndGuidFromUrl } from '@utils/services/utils.service';
import { filter, Subject, takeUntil } from 'rxjs';
import { FooterComponent } from "@components/footer/footer.component";
import { UtilsService } from '@utils/services/utils.service';
import { SpinnerComponent } from "@components/controls/spinner/spinner.component";
import { State } from '@utils/services/utils.service';
import { OrientationBlockComponent } from "@components/orientation-block/orientation-block.component";
import { isPlatformBrowser } from '@angular/common';
import { LocalStorageService } from 'ngx-localstorage';
import { PwabannerComponent } from '@components/pwabanner/pwabanner.component';
import { Conditional } from '@utils/models/parameterization.interface';



@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    HeaderComponent,
    FooterComponent,
    SpinnerComponent,
    OrientationBlockComponent,
    PwabannerComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit, OnDestroy, AfterViewInit {
  private platformId = inject<Object>(PLATFORM_ID);
  private destroy$ = new Subject<any>();
  public stateService = inject(StateService);
  private utils = inject(UtilsService);
  public process = signal<State>(State.process);
  public isBlocked = signal(false);
  private router = inject(Router);
  private parameterizationConsumeService = inject(ParameterizationConsumeService);
  private route = inject(ActivatedRoute);
  private storageService = inject(LocalStorageService);


  ngOnInit(): void {
    this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd),
        takeUntil(this.destroy$)
      )
      .subscribe(async event => {
        const { guid } = getPageAndGuidFromUrl(event.urlAfterRedirects);
        const state = this.getValidState();
        this.stateService.setStateSignal(state);
        if (state === State.process) {
          await this.handleProcessState(guid);
        } else if (state === State.end) {
          await this.handleEndState();
        }
      });
  }

  private getValidState(): State {
    const stateParam = this.route.snapshot.queryParamMap.get('state') as State;
    const validStates = [State.process, State.start, State.end];
    return validStates.includes(stateParam) ? stateParam : State.start;
  }

  private async handleProcessState(guid: string): Promise<void> {
    if (this.stateService.process()) return;
    const datos = await this.parameterizationConsumeService.LoadAgreementProcess(guid);
    if (!datos) return;
    this.stateService.setProcessSignal(datos);
    const configOk = await this.parameterizationConsumeService
      .LoadConsultGlobalConfiguration(datos.data.datos.guidConv);
    if (!configOk) return;
    this.stateService.setGuidConSignal(datos.data.datos.guidConv);
    this.stateService.setGuidProSignal(guid);
    await this.loadFlowData();
  }

  private async loadFlowData(): Promise<void> {
    const currentStep = this.utils.decrypt<string>(this.storageService.get('currentStep'));
    if (!currentStep) return;
    /// verificar bien la condicion
    const Conditional: Conditional = {
      value: "0"
    }
    const flowData = await this.parameterizationConsumeService
      .LoadGetProcessFlow(this.stateService.process()?.data.datos.workflowId ?? 0, this.stateService.guidCon(), currentStep, Conditional);
    if (!flowData) return;
    this.stateService.setTotalStepsSignal(flowData.data.countPages);
  }

  private async handleEndState(): Promise<void> {
    if (!(await this.parameterizationConsumeService.ValidateToken())) return;
    this.parameterizationConsumeService.LoadConsultGlobalConfiguration(this.stateService.guidCon());
  }


  ngAfterViewInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.checkOrientation();
    }
  }



  @HostListener('window:orientationchange')
  @HostListener('window:resize')
  checkOrientation() {
    const userAgent = navigator.userAgent.toLowerCase();
    const isMobile = /android|iphone|ipod/.test(userAgent) && !/ipad|tablet|crkey|x11|smarttv|tv/.test(userAgent);
    const isPortrait = window.matchMedia('(orientation: portrait)').matches;
    if (isMobile) {
      this.isBlocked.set(!isPortrait);
    } else {
      this.isBlocked.set(false);
    }
  }


  public functionMap = computed(() => {
    return this.utils.functionMap();
  });



  ngOnDestroy(): void {
    this.destroy$.next({});
    this.destroy$.complete();
  }
}
