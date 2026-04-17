import { Component, computed, inject, OnDestroy, OnInit, signal, } from '@angular/core';
import { BulletsComponent } from '@components/controls/bullets/bullets.component';
import { ImgComponent } from '@components/controls/img/img.component';
import { Subject, takeUntil } from 'rxjs';
import { TitleComponent } from '@components/controls/title/title.component';
import { SubtitleComponent } from '@components/controls/subtitle/subtitle.component';
import { ContentComponent } from '@components/controls/content/content.component';
import { Router } from '@angular/router';
import { AlertMessageService } from '@utils/services/alert.service';
import { UtilsService, Pages, State } from '@utils/services/utils.service';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { TrackEventService } from '@utils/services/trackevent.service';
import { WelcomeService } from '@welcome/services/welcome.service';
import { StateService } from '@utils/services/state.service';
import { GetResponse } from '@utils/models/utils.interface';
import { HttpStatusCode } from '@angular/common/http';
import { FlowNavigatorService } from '@utils/services/flownavigator.service';


@Component({
  selector: 'app-welcome',
  imports: [
    ContentComponent,
    ImgComponent,
    TitleComponent,
    SubtitleComponent,
    BulletsComponent
  ],
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.css',
})
export class WelcomeComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<any>();
  public contentTitle = signal<any>(null);
  public contentDescription = signal<any>(null);
  public image_content = signal<any>(null);
  public contentSubtitle = signal<any>(null);
  public contentParagraph = signal<any>(null);
  public bullet_list = signal<any>(null);
  public firstname = signal<string>("");
  private Alert = inject(AlertMessageService);
  private router = inject(Router);
  private utils = inject(UtilsService);
  private welcomeService = inject(WelcomeService);
  public stateService = inject(StateService);
  private flowNavigatorService = inject(FlowNavigatorService);

  private traficSrv = inject(TrackEventService);
  ngOnInit(): void {
    this.LoadCheckActivePages().then((Result: boolean) => {
      if (Result) {
        this.track('Carga componente');
      }
    });
    this.utils.setfunctionMap({
      next: this.next.bind(this),
      process: this.process.bind(this),
    });


  }

  ngOnDestroy(): void {
    this.destroy$.next({});
    this.destroy$.complete();
  }

  public displayImg = computed(() => {
    return this.image_content()?.display || false;
  });

  public displayTitle = computed(() => {
    return this.contentTitle()?.display || false;
  });

  public displayDescription = computed(() => {
    return this.contentDescription()?.display || false;
  });

  public displaySubtitle = computed(() => {
    return this.contentSubtitle()?.display || false;
  });

  public displayParagraph = computed(() => {
    return this.contentParagraph()?.display || false;
  });


  public bulletDisplay = computed(() => {
    return this.bullet_list()?.display || false;
  });


  private async next() {
    this.track('Inicio de validacion');
    const Next = await this.flowNavigatorService.goNext();
    if (Next) {
      this.router.navigate([`/${Next}`, this.stateService.guidPro()], { queryParams: { state: State.process } });
    }

  }

  private process() {
    this.track('cambio de dispositivo');
    this.Alert.Warning('Has ejecutado la opción Cambiar Dispositivo, pero por el momento no tiene funcionalidad en (welcome).');
  }



  public LoadCheckActivePages(): Promise<boolean> {
    return new Promise((resolve) => {
      this.welcomeService
        .QueryCheckActivePages(this.stateService.guidCon(), Pages.welcome)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (resul: GetResponse) => {
            if (resul.code == HttpStatusCode.Ok) {
              if (resul.data != null) {
                this.contentTitle.set(resul.data?.content?.title);
                this.contentDescription.set(resul.data?.content?.description);
                this.image_content.set(resul.data?.content?.image_content);
                this.contentSubtitle.set(resul.data?.content?.subtitle);
                this.contentParagraph.set(resul.data?.content?.paragraph);
                this.bullet_list.set(resul.data?.content?.bullet_list);
                this.utils.setBehavior(resul.data?.behavior);
                this.utils.setHeader(resul.data?.header);
                resolve(true);
              } else {
                this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.welcome}, asociada a este proceso.`);
                resolve(false);
              }
            } else {
              this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.welcome}, asociada a este proceso.`);
              resolve(false);
            }

          }
        });
    });
  }


  private track(action: string): void {
    this.traficSrv.RegisterEvent(<Partial<ActionEvent>>{
      componente: 'WelcomeComponent',
      accion: action,
    });
  }

}
