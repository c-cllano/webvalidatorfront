import { Component, computed, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ContentComponent } from '@components/controls/content/content.component';
import { ImgComponent } from '@components/controls/img/img.component';
import { SubtitleComponent } from '@components/controls/subtitle/subtitle.component';
import { TitleComponent } from '@components/controls/title/title.component';
import { AlertMessageService } from '@utils/services/alert.service';
import { UtilsService, Pages, State } from '@utils/services/utils.service';
import { Subject, takeUntil } from 'rxjs';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { TrackEventService } from '@utils/services/trackevent.service';
import { DocValidationService } from '@doc-validation/services/doc-validation.service';
import { GetResponse } from '@utils/models/utils.interface';
import { BulletsComponent } from '@components/controls/bullets/bullets.component';
import { StateService } from '@utils/services/state.service';
import { HttpStatusCode } from '@angular/common/http';
import { FlowNavigatorService } from '@utils/services/flownavigator.service';


@Component({
  selector: 'app-doc-validation',
  imports: [
    ContentComponent,
    TitleComponent,
    ImgComponent,
    SubtitleComponent,
    BulletsComponent],
  templateUrl: './doc-validation.component.html',
  styleUrl: './doc-validation.component.css'
})
export class DocValidationComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<any>();
  public contentTitle = signal<any>(null);
  public contentDescription = signal<any>(null);
  public image_content = signal<any>(null);
  public contentSubtitle = signal<any>(null);
  public contentParagraph = signal<any>(null);
  public bullet_list = signal<any>(null);
  private router = inject(Router);
  private Alert = inject(AlertMessageService);
  private utils = inject(UtilsService);
  private docValidationService = inject(DocValidationService);
  public stateService = inject(StateService);
  private flowNavigatorService = inject(FlowNavigatorService);

  private traficSrv = inject(TrackEventService);

  ngOnInit(): void {
    this.track('carga de componente: DocValidationComponent');
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



  private async next() {
    this.track('DocValidationComponent: subir fotos');
    const Next = await this.flowNavigatorService.goNext();
    if (Next) {
      this.router.navigate([`/${Next}`, this.stateService.guidPro()], { queryParams: { state: State.process } });
    }
  }



  private process() {
    this.track('DocValidationComponent:Tomar foto');
    this.Alert.Warning('Has ejecutado la opción  Tomar foto, pero por el momento no tiene funcionalidad en (doc-validation).');
  }


  public LoadCheckActivePages(procesoConvenioGuid: string) {
    this.docValidationService
      .QueryCheckActivePages(procesoConvenioGuid, Pages.docvalidation)
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
              this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.docvalidation}, asociada a este proceso: ${procesoConvenioGuid}`);
            }
          } else {
            this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.docvalidation}, asociada a este proceso: ${procesoConvenioGuid}`);
          }

        }
      });
  }


  private track(action: string): void {
    this.traficSrv.RegisterEvent(<Partial<ActionEvent>>{
      componente: 'doc-validation',
      accion: action
    });
  }
}
