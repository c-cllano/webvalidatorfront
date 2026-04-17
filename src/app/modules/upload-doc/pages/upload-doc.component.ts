import { ChangeDetectorRef, Component, computed, ElementRef, inject, input, NgZone, OnDestroy, OnInit, signal, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ContentComponent } from '@components/controls/content/content.component';
import { ImgComponent } from '@components/controls/img/img.component';
import { SubtitleComponent } from '@components/controls/subtitle/subtitle.component';
import { TitleComponent } from '@components/controls/title/title.component';
import { AlertMessageService } from '@utils/services/alert.service';
import { UtilsService, Pages, numberToRem, State } from '@utils/services/utils.service';
import { Subject, takeUntil, timeout } from 'rxjs';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { TrackEventService } from '@utils/services/trackevent.service';
import { UploadDocService } from '@upload-doc/services/upload-doc.service';
import { GetResponse } from '@utils/models/utils.interface';
import { BulletsComponent } from '@components/controls/bullets/bullets.component';
import { ButtonDirective } from '@directives/button.directive';
import { StateService } from '@utils/services/state.service';
import { SubtitleDirective } from '@directives/subtitle.directive';
import { FlowNavigatorService } from '@utils/services/flownavigator.service';
import { HttpStatusCode } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-upload-doc',
  imports: [
    ContentComponent,
    TitleComponent,
    ImgComponent,
    SubtitleComponent,
    BulletsComponent,
    ButtonDirective,
    SubtitleDirective],
  templateUrl: './upload-doc.component.html',
  styleUrl: './upload-doc.component.css'
})
export class UploadDocComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<any>();
  public contentTitle = signal<any>(null);
  public contentDescription = signal<any>(null);
  public image_content = signal<any>(null);
  public contentSubtitle = signal<any>(null);
  public contentParagraph = signal<any>(null);
  public bullet_list = signal<any>(null);
  private content_permitdeniedDisplay = signal<boolean>(false);
  private content_locationdeniedDisplay = signal<any>(false);
  private content_cameradeniedDisplay = signal<any>(false);
  private router = inject(Router);
  private Alert = inject(AlertMessageService);
  private utils = inject(UtilsService);
  private uploadDocService = inject(UploadDocService);
  private stateService = inject(StateService);
  private flowNavigatorService = inject(FlowNavigatorService);

  subtitle1: any;
  subtitle2: any;
  imageContent1: any;
  imageContent2: any;
  button_action1 = signal<any>(null);
  button_action2 = signal<any>(null);

  showButton1 = signal<boolean>(false);
  showButton2 = signal<boolean>(false);

  imagenFrontal = signal<string | null>(null);
  imagenPosterior = signal<string | null>(null);

  nombreFrontal = signal<string | null>(null);
  nombrePosterior = signal<string | null>(null);

  isCargandoFrontal = signal<boolean>(false);
  isCargandoPosterior = signal<boolean>(false);
  @ViewChild('fileInputFrontal') fileInputFrontal!: ElementRef<HTMLInputElement>;
  @ViewChild('fileInputPosterior') fileInputPosterior!: ElementRef<HTMLInputElement>;
  tipoError = signal<'danada' | 'formato' | 'peso' | null>(null);
  mostrarError = signal<boolean>(false);

  anverso = signal<{ value: string, format: string } | null>(null);
  reverso = signal<{ value: string, format: string } | null>(null);

  isEscaneandoFrontal = signal<boolean>(false);
  isEscaneandoPosterior = signal<boolean>(false);
  frontalValidationResult = signal<boolean | null>(null);
  posteriorValidationResult = signal<boolean | null>(null);
  isValidating = signal<boolean>(false);

  frontalMessage = signal<string>('');
  reverseMessage = signal<string>('');
  showErrorMessages = signal(false);

  showFrontalErrorIcon = signal<boolean>(false);
  showPosteriorErrorIcon = signal<boolean>(false);

  frontalButtonText = computed(() => {
    return (this.showErrorMessages() && this.frontalValidationResult() === false) ? 'Reintentar' : (this.button_action1()?.text || '');
  });

  posteriorButtonText = computed(() => {
    return (this.showErrorMessages() && this.posteriorValidationResult() === false) ? 'Reintentar' : (this.button_action2()?.text || '');
  });

  constructor(private traficSrv: TrackEventService, private cdr: ChangeDetectorRef,
    private ngZone: NgZone) { }

  ngOnInit(): void {
    this.track('carga de componente: UploadDocComponent');
    this.LoadCheckActivePages();
    this.utils.setfunctionMap({
      confirmimages: this.confirmimages.bind(this),
    });
    this.utils.setDisplayButton(true);
  }

  ngOnDestroy(): void {
    this.destroy$.next({});
    this.destroy$.complete();
    this.utils.setDisplayButton(false);
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

  private permitdeniedDisplay = computed(() => {
    return this.content_permitdeniedDisplay() || false;
  });

  private locationdeniedDisplay = computed(() => {
    return this.content_locationdeniedDisplay() || false;
  });

  private cameradeniedDisplay = computed(() => {
    return this.content_cameradeniedDisplay() || false;
  });

  public bulletDisplay = computed(() => {
    return this.bullet_list()?.display || false;
  });

  private async next() {

    this.track('UploadDocComponent: subir fotos');

    const Next = await this.flowNavigatorService.goNext();
    if (Next) {
      this.router.navigate([`/${Next}`, this.stateService.guidPro()], { queryParams: { state: State.process } });
    }
  }

  private process() {
    this.track('UploadDocComponent:Camobio dispositivo');
    this.Alert.Warning('Has ejecutado la opción Cambiar Dispositivo, pero por el momento no tiene funcionalidad en (upload-doc).');
  }

  public LoadCheckActivePages(): Promise<boolean> {
    return new Promise((resolve) => {
      this.uploadDocService
        .QueryCheckActivePages(this.stateService.guidCon(), Pages.uploaddoc)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (resul: GetResponse) => {
            if (resul.code == HttpStatusCode.Ok) {
              if (resul.data != null) {
                console.log(resul.data?.content)
                this.contentTitle.set(resul.data?.content?.title);
                this.contentDescription.set(resul.data?.content?.description);
                this.image_content.set(resul.data?.content?.image_content);
                this.contentSubtitle.set(resul.data?.content?.subtitle);
                this.contentParagraph.set(resul.data?.content?.paragraph);
                this.bullet_list.set(resul.data?.content?.bullet_list);
                this.utils.setBehavior(resul.data?.behavior);
                this.utils.setHeader(resul.data?.header);

                this.subtitle1 = resul.data?.content.subtitle1;
                this.subtitle2 = resul.data?.content.subtitle2;
                this.imageContent1 = resul.data?.content.image_content1;
                this.imageContent2 = resul.data?.content.image_content2;
                this.button_action1.set(resul.data?.content.button_action1);
                this.button_action2.set(resul.data?.content.button_action2);

                this.showButton1.set(true);
                this.showButton2.set(true);
                resolve(true);
              } else {
                this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.uploaddoc}, asociada a este proceso: ${this.stateService.guidCon()}`);
                resolve(false);
              }
            } else {
              this.Alert.Warning(`No hay una configuración de interfaz establecida para la pantalla: ${Pages.uploaddoc}, asociada a este proceso: ${this.stateService.guidCon()}`);
              resolve(false);
            }

          }
        });
    });
  }

  private track(action: string): void {
    this.traficSrv.RegisterEvent(<Partial<ActionEvent>>{
      componente: 'upload-doc',
      accion: action
    });
  }

  onFileChange(event: any, tipo: 'frontal' | 'posterior') {
    const file = event.target.files[0];

    if (file) {
      const esFormatoValido = ['image/jpeg', 'image/jpg', 'image/png'].includes(file.type);
      const esPesoValido = file.size <= 10 * 1024 * 1024;
      const esArchivoVacio = file.size === 0;

      if (esArchivoVacio) {
        this.tipoError.set('danada');
        this.mostrarError.set(true);
        return;
      }

      if (!esFormatoValido) {
        this.tipoError.set('formato');
        this.mostrarError.set(true);
        return;
      }

      if (!esPesoValido) {
        this.tipoError.set('peso');
        this.mostrarError.set(true);
        return;
      }

      const reader = new FileReader();

      if (tipo === 'frontal') {
        this.isCargandoFrontal.set(true);
      } else {
        this.isCargandoPosterior.set(true);
      }

      reader.onload = () => {
        const img = new Image();

        img.onload = () => {
          this.ngZone.run(() => {
            const base64SinEncabezado = (reader.result as string).split(',')[1];
            if (tipo === 'frontal') {
              this.imagenFrontal.set(reader.result as string);
              this.nombreFrontal.set(file.name);
              this.frontalMessage.set('');
              this.frontalValidationResult.set(null);
              this.showButton1.set(false);
              this.anverso.set({
                value: base64SinEncabezado,
                format: file.type
              });
              this.isCargandoFrontal.set(false);
              this.fileInputFrontal.nativeElement.value = '';
            } else {
              this.imagenPosterior.set(reader.result as string);
              this.nombrePosterior.set(file.name);
              this.reverseMessage.set('');
              this.posteriorValidationResult.set(null);
              this.showButton2.set(false);
              this.reverso.set({
                value: base64SinEncabezado,
                format: file.type
              });
              this.isCargandoPosterior.set(false);
              this.fileInputPosterior.nativeElement.value = '';
            }

            if (this.anverso() && this.reverso()) {
              this.utils.setDisplayButton(false);
            }
          });
        };

        img.onerror = () => {
          this.ngZone.run(() => {
            if (tipo === 'frontal') {
              this.isCargandoFrontal.set(false);
              this.imagenFrontal.set(null);
              this.nombreFrontal.set(null);
              this.anverso.set(null);
              this.fileInputFrontal.nativeElement.value = '';
            } else {
              this.isCargandoPosterior.set(false);
              this.imagenPosterior.set(null);
              this.nombrePosterior.set(null);
              this.reverso.set(null);
              this.fileInputPosterior.nativeElement.value = '';
            }
            this.tipoError.set('danada');
            this.mostrarError.set(true);
          });
        };

        img.src = reader.result as string;
      };

      reader.onerror = () => {
        this.ngZone.run(() => {
          if (tipo === 'frontal') {
            this.isCargandoFrontal.set(false);
            this.imagenFrontal.set(null);
            this.nombreFrontal.set(null);
            this.anverso.set(null);
            this.fileInputFrontal.nativeElement.value = '';
          } else {
            this.isCargandoPosterior.set(false);
            this.imagenPosterior.set(null);
            this.nombrePosterior.set(null);
            this.reverso.set(null);
            this.fileInputPosterior.nativeElement.value = '';
          }
          this.tipoError.set('danada');
          this.mostrarError.set(true);
        });
      };

      reader.readAsDataURL(file);
    }
  }

  cerrarModal() {
    this.mostrarError.set(false);
    this.tipoError.set(null);
  }

  eliminarImagen(tipo: 'frontal' | 'posterior') {
    if (tipo === 'frontal') {
      this.imagenFrontal.set(null);
      this.nombreFrontal.set(null);
      this.anverso.set(null);
      this.isCargandoFrontal.set(false);
      this.fileInputFrontal.nativeElement.value = '';
      this.frontalMessage.set('');
      this.frontalValidationResult.set(null);
      this.showButton1.set(true);
    } else {
      this.imagenPosterior.set(null);
      this.nombrePosterior.set(null);
      this.reverso.set(null);
      this.isCargandoPosterior.set(false);
      this.fileInputPosterior.nativeElement.value = '';
      this.reverseMessage.set('');
      this.posteriorValidationResult.set(null);
      this.showButton2.set(true);
    }

    if (!this.frontalMessage() && !this.reverseMessage()) {
      this.showErrorMessages.set(false);
    }

    if (!this.anverso() || !this.reverso()) {
      this.utils.setDisplayButton(true);
    }
  }

  confirmimages() {
    this.utils.setDisplayButton(true);

    const payload = {
      validationProcessId: this.stateService.process()?.data?.datos?.validationProcessId,
      citizenGUID: this.stateService.process()?.data?.datos?.guidCiu,
      aditionalData: this.stateService.process()?.data?.datos?.tipoDoc,
      frontal: this.anverso(),
      reverse: this.reverso(),
      user: environment.user //this.stateService.process()?.data?.datos?.asesor
    };

    this.isValidating.set(true);
    this.isEscaneandoFrontal.set(true);
    this.isEscaneandoPosterior.set(true);

    this.frontalValidationResult.set(null);
    this.posteriorValidationResult.set(null);
    this.showErrorMessages.set(false);
    this.frontalMessage.set('');
    this.reverseMessage.set('');

    console.log(payload);

    this.uploadDocService.saveBothSidesDocument(payload)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          console.log('Response:', response);

          this.isEscaneandoFrontal.set(false);
          this.isEscaneandoPosterior.set(false);
          this.isValidating.set(false);

          if (response.code === 200 && response.data) {
            const frontalSuccess = response.data.frontalSuccessful || false;
            const posteriorSuccess = response.data.reverseSuccessful || false;

            this.frontalValidationResult.set(frontalSuccess);
            this.posteriorValidationResult.set(posteriorSuccess);

            if (!frontalSuccess && response.data.frontalMessage) {
              this.frontalMessage.set(response.data.frontalMessage);
              this.anverso.set(null);
            }
            if (!posteriorSuccess && response.data.reverseMessage) {
              this.reverseMessage.set(response.data.reverseMessage);
              this.reverso.set(null);
            }

            if (frontalSuccess && posteriorSuccess) {
              setTimeout(() => {
                this.next();
              }, 3000);
            } else {
              if (!frontalSuccess) {
                this.showFrontalErrorIcon.set(true);
              }
              if (!posteriorSuccess) {
                this.showPosteriorErrorIcon.set(true);
              }

              setTimeout(() => {
                this.showFrontalErrorIcon.set(false);
                this.showPosteriorErrorIcon.set(false);

                this.frontalValidationResult.set(frontalSuccess);
                this.posteriorValidationResult.set(posteriorSuccess);
                this.showErrorMessages.set(true);

                if (!frontalSuccess) {
                  this.showButton1.set(true);
                }
                if (!posteriorSuccess) {
                  this.showButton2.set(true);
                }
              }, 3000);
            }
          } else {
            this.showFrontalErrorIcon.set(true);
            this.showPosteriorErrorIcon.set(true);

            setTimeout(() => {
              this.showFrontalErrorIcon.set(false);
              this.showPosteriorErrorIcon.set(false);

              this.frontalValidationResult.set(false);
              this.posteriorValidationResult.set(false);
              this.Alert.Warning('Error al validar los documentos. Por favor, intente nuevamente.');
            }, 3000);
          }
        },
        error: (error) => {
          console.error('Error:', error);

          this.isEscaneandoFrontal.set(false);
          this.isEscaneandoPosterior.set(false);
          this.isValidating.set(false);

          this.showFrontalErrorIcon.set(true);
          this.showPosteriorErrorIcon.set(true);

          setTimeout(() => {
            this.showFrontalErrorIcon.set(false);
            this.showPosteriorErrorIcon.set(false);

            this.frontalValidationResult.set(false);
            this.posteriorValidationResult.set(false);
            this.Alert.Warning('Error al procesar la solicitud. Por favor, intente nuevamente.');
          }, 3000);
        }
      });
  }

  public getGlobalButtonbutton_main = computed(() => {
    const button_main = this.stateService.globalStyles()?.buttons?.button_main || {};
    const borderButton = this.stateService.globalStyles()?.buttons?.borderRadius;
    const fontfamily = this.stateService.globalStyles()?.typography['font-family'] || 'Poppins';
    const styles: { [key: string]: string | undefined } = {
      ...button_main,
      borderRadius: numberToRem(borderButton),
      border: `0.0625rem solid ${button_main?.color}`,
      'font-family': fontfamily,
    };
    return styles;
  });
}
