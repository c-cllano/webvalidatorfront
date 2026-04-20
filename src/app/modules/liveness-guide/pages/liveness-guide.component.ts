import { Component, computed, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { ContentComponent } from '@components/controls/content/content.component';
import { ImgComponent } from '@components/controls/img/img.component';
import { SubtitleComponent } from '@components/controls/subtitle/subtitle.component';
import { TitleComponent } from '@components/controls/title/title.component';
import { AlertMessageService } from '@utils/services/alert.service';
import { UtilsService, Pages, State } from '@utils/services/utils.service';
import { Subject, takeUntil } from 'rxjs';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { TrackEventService } from '@utils/services/trackevent.service';
import { GetResponse } from '@utils/models/utils.interface';
import { BulletsComponent } from '@components/controls/bullets/bullets.component';
import { StateService } from '@utils/services/state.service';
import { HttpStatusCode } from '@angular/common/http';
import { LivenessGuideService } from '@modules/liveness-guide/services/liveness-guide.service';
import { FlowNavigatorService } from '@utils/services/flownavigator.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-liveness-guide',
  imports: [
    ContentComponent,
    TitleComponent,
    ImgComponent,
    SubtitleComponent,
    BulletsComponent],
  templateUrl: './liveness-guide.component.html',
  styleUrl: './liveness-guide.component.css'
})
export class LivenessGuideComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<any>();
  public contentTitle = signal<any>(null);
  public contentDescription = signal<any>(null);
  public image_content = signal<any>(null);
  public contentSubtitle = signal<any>(null);
  public contentParagraph = signal<any>(null);
  public bullet_list = signal<any>(null);
  private Alert = inject(AlertMessageService);
  private utils = inject(UtilsService);
  private livenessGuideService = inject(LivenessGuideService);
  public stateService = inject(StateService);
  private router = inject(Router);

  private traficSrv = inject(TrackEventService);
  private flowNavigatorService = inject(FlowNavigatorService);

  ngOnInit(): void {
    this.track('carga de componente: livenessguide');
    this.LoadCheckActivePages(this.stateService.guidCon());
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

  private async process() {
    this.track('livenessguide:se envia al proceso de captura facial');
    const Next = await this.flowNavigatorService.goNext();
    if (Next) {
      this.router.navigate([`/${Next}`, this.stateService.guidPro()], { queryParams: { state: State.process } });
    }
  }

  private async next() {
    this.track('subir fotos');
    const Next = await this.flowNavigatorService.goNext();
    if (Next) {
      this.router.navigate([`/${Next}`, this.stateService.guidPro()], { queryParams: { state: State.process } });
    }
  }

  public LoadCheckActivePages(procesoConvenioGuid: string) {
    this.livenessGuideService
      .QueryCheckActivePages(procesoConvenioGuid, Pages.livenessguide)
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
            } else {
              this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.livenessguide}, asociada a este proceso: ${procesoConvenioGuid}`);
            }
          } else {
            this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.livenessguide}, asociada a este proceso: ${procesoConvenioGuid}`);
          }

        }
      });
  }


  private track(action: string): void {
    this.traficSrv.RegisterEvent(<Partial<ActionEvent>>{
      componente: 'liveness-guide',
      accion: action
    });
  }
}
