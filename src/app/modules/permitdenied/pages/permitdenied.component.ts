import { Component, computed, inject, signal, input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BulletsComponent } from '@components/controls/bullets/bullets.component';
import { ContentComponent } from '@components/controls/content/content.component';
import { ImgComponent } from '@components/controls/img/img.component';
import { SubtitleComponent } from '@components/controls/subtitle/subtitle.component';
import { TitleComponent } from '@components/controls/title/title.component';
import { AlertMessageService } from '@utils/services/alert.service';
import { Pages, State, UtilsService } from '@utils/services/utils.service';
import { Subject, takeUntil } from 'rxjs';
import { TrackEventService } from '@utils/services/trackevent.service';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { GetResponse } from '@utils/models/utils.interface';
import { PermitdeniedService } from '@permitdenied/services/permitdenied.service';
import { StateService } from '@utils/services/state.service';
import { HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-permitdenied',
  imports: [ContentComponent,
    ImgComponent,
    TitleComponent,
    SubtitleComponent,
    BulletsComponent],
  templateUrl: './permitdenied.component.html',
  styleUrl: './permitdenied.component.css'
})
export class PermitdeniedComponent {

  private destroy$ = new Subject<any>();
  public contentTitle = signal<any>(null);
  public contentDescription = signal<any>(null);
  public image_content = signal<any>(null);
  public contentSubtitle = signal<any>(null);
  public contentParagraph = signal<any>(null);
  public bullet_list = signal<any>(null);
  private Alert = inject(AlertMessageService);
  private router = inject(Router);
  private utils = inject(UtilsService);
  private permitdeniedService = inject(PermitdeniedService);
  private route = inject(ActivatedRoute);
  private traficSrv = inject(TrackEventService);
  public stateService = inject(StateService);


  ngOnInit(): void {
    this.track('PermitdeniedComponent: carga componente');
    this.route.queryParams
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        document.querySelectorAll('.Permitdenied').forEach(el => {
          el.classList.add('skeleton');
        });
        const TypePermission = params['TypePermission'];
        this.LoadCheckActivePages(this.stateService.guidCon(), TypePermission!);
      });
    this.utils.setfunctionMap({
      CloseValidation: this.CloseValidation.bind(this),
      process: this.process.bind(this),
    });
    this.utils.setProgressbarDisplay(false);
  }


  ngOnDestroy(): void {
    this.destroy$.next({});
    this.destroy$.complete();
    this.utils.setProgressbarDisplay(true);
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


  private CloseValidation() {
    this.track('PermitdeniedComponent: se cierra la validacion');
    this.router.navigate([`/${Pages.flowcompletion}`, this.stateService.guidPro()], { queryParams: { state: State.end } });
  }

  private process() {
    this.track('PermitdeniedComponent: Cambio de dispositivo');
    this.Alert.Warning('Has ejecutado la opción Cambiar Dispositivo, pero por el momento no tiene funcionalidad en (permitdenied).');
  }


  public LoadCheckActivePages(procesoConvenioGuid: string, TypePermission?: string): void {
    this.permitdeniedService
      .QueryCheckActivePages(procesoConvenioGuid, Pages.permitdenied)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (resul: GetResponse) => {
          if (resul.code == HttpStatusCode.Ok) {
            if (resul.data != null) {
              const content = resul?.data;
              const section = content?.[`content_${TypePermission}`];
              this.contentTitle.set(section?.title);
              this.contentDescription.set(section?.description);
              this.image_content.set(section?.image_content);
              this.contentSubtitle.set(section?.subtitle);
              this.contentParagraph.set(section?.paragraph);
              this.bullet_list.set(section?.bullet_list);
              this.utils.setBehavior(resul.data?.behavior);
              this.utils.setHeader(resul.data?.header);
            } else {
              this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.permitdenied}, asociada a este proceso: ${procesoConvenioGuid}`);
            }
          } else {
            this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.permitdenied}, asociada a este proceso: ${procesoConvenioGuid}`);
          }

        }
      });
  }



  private track(action: string): void {
    this.traficSrv.RegisterEvent(<Partial<ActionEvent>>{
      componente: 'Permitdenied',
      accion: action
    });
  }
}
