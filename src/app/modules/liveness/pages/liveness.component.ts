import { HttpStatusCode } from '@angular/common/http';
import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  AfterViewInit,
  ViewChild,
  signal,
  computed,
  inject,
} from '@angular/core';
import { ContentComponent } from '@components/controls/content/content.component';
import { TitleComponent } from '@components/controls/title/title.component';
import { BiometricRequest } from '@liveness/models/biometric-request.interface';
import { ValidateBiometric } from '@liveness/models/validation-biometric.interface';
import { LivenessBiometricService } from '@liveness/services/liveness-biometric.service';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { GetResponse } from '@utils/models/utils.interface';
import { AlertMessageService } from '@utils/services/alert.service';
import { StateService } from '@utils/services/state.service';
import { TrackEventService } from '@utils/services/trackevent.service';
import { Pages, UtilsService } from '@utils/services/utils.service';
import { Subject, takeUntil } from 'rxjs';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-liveness',
  standalone: true,
  imports: [ContentComponent, TitleComponent],
  templateUrl: './liveness.component.html',
  styleUrls: ['./liveness.component.css'],
})
export class LivenessComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('videoEl', { static: true })
  videoEl!: ElementRef<HTMLVideoElement>;
  @ViewChild('canvasEl', { static: true })
  canvasEl!: ElementRef<HTMLCanvasElement>;

  private stream?: MediaStream;

  // Secuencia de capturas
  private capturesTarget = 2;
  private captureDelayMs = 6000;
  private betweenCapturesMs = 700;
  private seqTimeoutId?: number;
  private typeFlow: number = 0;

  // Espera simulada: overlay 20s (se maneja fuera del TS con CSS/HTML si la usas)
  private submitDelayMs = 20_000;
  private submitTimeoutId?: number;
  public isSubmitting = signal(false);

  // UI / estado
  public isPreview = signal(false);
  public photos = signal<string[]>([]);
  public imgLoaded = signal(false);
  public imgSrc = signal<string | null>(null);
  public errorMsg = signal<string | null>(null);

  // Derivados
  public firstPhoto = computed(() =>
    this.photos().length ? this.photos()[0] : null
  );

  // Contenido
  private destroy$ = new Subject<void>();
  public contentTitle = signal<any>(null);
  public contentDescription = signal<any>(null);

  // Inyecciones
  private Alert = inject(AlertMessageService);
  private utils = inject(UtilsService);
  private livenessService = inject(LivenessBiometricService);
  public stateService = inject(StateService);
  private traficSrv = inject(TrackEventService);

  // Config de comportamiento global (de LoadCheckActivePages)
  private behaviorCfg: any | null = null;

  // ===== Helpers =====
  private get2dCtx(
    c: HTMLCanvasElement,
    opts?: CanvasRenderingContext2DSettings
  ) {
    return c.getContext('2d', opts) as CanvasRenderingContext2D | null;
  }
  private mimeFromDataUrl(s: string) {
    return s.match(/^data:([^;]+);base64,/)?.[1] ?? null;
  }
  private dataUrlToBase64(s: string) {
    const i = s.indexOf(',');
    return i >= 0 ? s.slice(i + 1) : s;
  }

  /** Congela frame del <video> a dataURL; null si no se pudo. */
  private freezeCurrentFrame(): string | null {
    const v = this.videoEl?.nativeElement;
    const c = this.canvasEl?.nativeElement;
    if (!v || !c || !v.videoWidth || !v.videoHeight) return null;

    const size = Math.min(v.videoWidth, v.videoHeight, 640);
    c.width = size;
    c.height = size;
    const ctx = this.get2dCtx(c);
    if (!ctx) return null;

    const sx = (v.videoWidth - size) / 2;
    const sy = (v.videoHeight - size) / 2;
    ctx.drawImage(v, sx, sy, size, size, 0, 0, size, size);
    return c.toDataURL('image/jpeg', 0.9);
  }

  public onImgLoad() {
    this.imgLoaded.set(true);
  }
  public onImgError() {
    this.imgLoaded.set(true);
  }

  /** Mostrar error (marco rojo + texto) */
  public setError(msg: string) {
    this.errorMsg.set(msg);

    let src = this.firstPhoto();
    if (!src) {
      const frozen = this.freezeCurrentFrame();
      if (frozen) {
        src = frozen;
        this.photos.set([frozen]);
      }
    }
    if (src) this.imgSrc.set(src);

    this.isPreview.set(true);
    this.imgLoaded.set(true);

    this.clearSeqTimeout();
    this.stopStream(true);
  }
  private clearError() {
    this.errorMsg.set(null);
  }

  // ===== Ciclo de vida =====
  ngOnInit(): void {
    this.track('Inicio CapFacLivID');
    this.LoadCheckActivePages(this.stateService.guidCon());
    this.utils.setfunctionMap({
      confirmphoto: this.confirmphoto.bind(this),
      retryphoto: this.retryphoto.bind(this),
    });
  }

  async ngAfterViewInit() {
    await this.startCameraAuto();
    this.typeFlow =
      this.stateService.process()?.data?.datos?.estadoProceso ?? 0;
  }

  ngOnDestroy(): void {
    this.clearSeqTimeout();
    this.clearSubmitTimeout();
    this.stopStream(true);
    this.destroy$.next();
    this.destroy$.complete();
  }

  // ===== Cámara =====
  private async startCameraAuto() {
    try {
      if (
        !('mediaDevices' in navigator) ||
        !navigator.mediaDevices.getUserMedia
      )
        return;
      if (!isSecureContext) return;

      try {
        await this.openStream({
          video: {
            facingMode: { ideal: 'user' },
            width: { ideal: 1280 },
            height: { ideal: 720 },
            aspectRatio: { ideal: 1 },
          },
          audio: false,
        });
      } catch {
        await this.openStream({
          video: {
            facingMode: { ideal: 'user' },
            width: { ideal: 640 },
            height: { ideal: 480 },
          },
          audio: false,
        });
      }

      const v = this.videoEl.nativeElement;
      v.muted = true;
      v.setAttribute('playsinline', 'true');
      try {
        await v.play();
      } catch {
        setTimeout(() => v.play().catch(() => {}), 100);
      }

      if (v.readyState >= 2) this.startAutoCaptureSequence();
      else
        v.addEventListener(
          'loadedmetadata',
          () => this.startAutoCaptureSequence(),
          { once: true }
        );
    } catch (err) {
      console.log('❌ No se pudo acceder a la cámara.', err);
    }
  }

  private async openStream(constraints: MediaStreamConstraints) {
    this.stopStream(true);
    this.stream = await navigator.mediaDevices.getUserMedia(constraints);
    this.videoEl.nativeElement.srcObject = this.stream;
    this.stream
      .getTracks()
      .forEach(
        (t) => (t.onended = () => console.log(`🔚 Track finalizada: ${t.kind}`))
      );
  }

  /** Apaga cámara; si clearEl=true limpia también el <video> (evita cuadro negro) */
  private stopStream(clearEl = false) {
    this.stream?.getTracks().forEach((t) => t.stop());
    this.stream = undefined;

    if (clearEl) {
      const v = this.videoEl?.nativeElement;
      if (v) {
        try {
          v.pause();
        } catch {}
        try {
          (v as any).srcObject = null;
        } catch {}
        v.removeAttribute('src');
        try {
          v.load?.();
        } catch {}
      }
    }
  }

  // ===== Timeouts capturas =====
  private schedule(cb: () => void, delay: number) {
    this.clearSeqTimeout();
    this.seqTimeoutId = window.setTimeout(cb, delay);
  }
  private clearSeqTimeout() {
    if (this.seqTimeoutId) {
      clearTimeout(this.seqTimeoutId);
      this.seqTimeoutId = undefined;
    }
  }

  private clearSubmitTimeout() {
    if (this.submitTimeoutId) {
      clearTimeout(this.submitTimeoutId);
      this.submitTimeoutId = undefined;
    }
  }

  // ===== Capturas =====
  private startAutoCaptureSequence() {
    this.schedule(() => this.captureNext(0), this.captureDelayMs);
  }

  private captureNext(i: number) {
    this.track('CapturasFotos --> ' + i + '');
    if (i >= this.capturesTarget) {
      if (this.photos().length > 0) this.processBometric();
      return;
    }
    const ok = this.captureOnce();
    if (ok)
      this.schedule(() => this.captureNext(i + 1), this.betweenCapturesMs);
    else this.schedule(() => this.captureNext(i), 2000);
  }

  private captureOnce(): boolean {
    const v = this.videoEl.nativeElement;
    const c = this.canvasEl.nativeElement;
    if (!v.videoWidth || !v.videoHeight) return false;

    const size = Math.min(v.videoWidth, v.videoHeight, 640);
    c.width = size;
    c.height = size;

    const ctx = this.get2dCtx(c);
    if (!ctx) return false;

    const sx = (v.videoWidth - size) / 2;
    const sy = (v.videoHeight - size) / 2;
    ctx.drawImage(v, sx, sy, size, size, 0, 0, size, size);

    const dataUrl = c.toDataURL('image/jpeg', 0.9);

    this.isPreview.set(true);
    this.clearError();
    this.imgLoaded.set(false);
    this.imgSrc.set(dataUrl);
    this.photos.set([...this.photos(), dataUrl]);

    this.clearSeqTimeout();
    this.stopStream(true);
    this.utils.setDisplayButton(false);
    return true;
  }

  private processBometric() {
    debugger;
    if (this.typeFlow == 1) {
      this.saveBiomentic();
    } else if (this.typeFlow == 2) {
      this.validateBiometric();
    }
  }

  // ===== Enrolamiento: ENVÍA 1ª como principal y 2ª como gesto =====
  private saveBiomentic() {
    const first = this.firstPhoto();
    if (!first) {
      this.Alert.Warning('No hay foto capturada para enviar.');
      return;
    }
    this.track('Envio--> saveBiomentic');
    // Principal (1ª)
    const detectedFormat = this.mimeFromDataUrl(first) ?? 'image/jpeg';
    const base64 = this.dataUrlToBase64(first);

    // Gesto (2ª si existe; fallback a 1ª)
    const second = this.photos().length > 1 ? this.photos()[1] : first;
    const detectedGestureFormat = this.mimeFromDataUrl(second) ?? 'image/jpeg';
    const base64Gesture = this.dataUrlToBase64(second);

    const citizenGuid = this.stateService.process()?.data?.datos?.guidCiu;
    if (!citizenGuid) {
      this.Alert.Warning('Falta CitizenGUID.');
      return;
    }

    const payloadSave: BiometricRequest = {
      citizenGUID: citizenGuid,
      subType: 'Frontal',
      serviceId: 5,
      value: base64,
      format: detectedFormat,
      aditionalData: '',
      user: 'lugoale',
      update: true,
      codeParameter: '',
      biometricGesture: base64Gesture,
      formatGesture: detectedGestureFormat,
    };

    this.isSubmitting.set(true);
    this.livenessService
      .saveBiometric(payloadSave)
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => this.isSubmitting.set(false))
      )
      .subscribe({
        next: (res) => {
          const ok = res?.code === 200 && !!res?.data?.isSuccessful;
          if (ok) {
            this.errorMsg.set(null);
            this.Alert.Success('cargada exitosamente');
            return;
          }

          const msg =
            this.joinTxErrors(res?.data?.transactionError) ||
            'No pudimos procesar la foto. Intenta de nuevo.';
          this.setError(msg);
        },
        error: () => {
          this.setError('No pudimos procesar la foto. Intenta de nuevo.');
        },
      });
    this.utils.setDisplayButton(false);
  }

  // ===== Validación: ENVÍA 1ª como principal y 2ª como gesto =====
  private validateBiometric() {
    const first = this.firstPhoto();
    if (!first) {
      this.Alert.Warning('No hay foto capturada para enviar.');
      return;
    }
    this.track('Envio-->validateBiometric');
    // Principal (1ª)
    const detectedFormat = this.mimeFromDataUrl(first) ?? 'image/jpeg';
    const base64 = this.dataUrlToBase64(first);

    // Gesto (2ª si existe; fallback a 1ª)
    const second = this.photos().length > 1 ? this.photos()[1] : first;
    const detectedGestureFormat = this.mimeFromDataUrl(second) ?? 'image/jpeg';
    const base64Gesture = this.dataUrlToBase64(second);

    const citizenGuid = this.stateService.process()?.data?.datos?.guidCiu;
    if (!citizenGuid) {
      this.Alert.Warning('Falta CitizenGUID.');
      return;
    }

    const rawId = this.stateService.process()?.data?.datos?.validationProcessId;
    if (rawId == null) {
      this.Alert.Warning('Falta ValidationProcessId.');
      return;
    }
    const validationProcessId =
      typeof rawId === 'number' ? rawId : Number(rawId);

    const payload: ValidateBiometric = {
      CitizenGUID: citizenGuid,
      ValidationProcessId: validationProcessId,
      Format: detectedFormat,
      SubType: 'Frontal',
      ServiceId: 5,
      Biometric: base64,
      BiometricGesture: base64Gesture,
      FormatGesture: detectedGestureFormat,
    };

    this.isSubmitting.set(true);
    this.livenessService
      .sentValidationBiometric(payload)
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => {
          this.isSubmitting.set(false);
        })
      )
      .subscribe({
        next: (res) => {
          const ok =
            res?.code === 200 &&
            !!res?.data?.isSuccessful &&
            !!res?.data?.isValid;

          if (ok) {
            this.errorMsg.set(null);
            this.Alert.Success('Validacion terminada exitosamente');
          } else {
            const txErr = res?.data?.transactionError;
            const msg =
              (Array.isArray(txErr) && txErr.length && String(txErr[0])) ||
              res?.data?.result ||
              'No pudimos validar la foto. Intenta de nuevo.';
            this.setError(msg);
          }
        },
        error: () => {
          this.setError('No pudimos validar la foto. Intenta de nuevo.');
        },
      });
    this.utils.setDisplayButton(false);
  }

  public displayTitle = computed(() => this.contentTitle()?.display || false);
  public displayDescription = computed(
    () => this.contentDescription()?.display || false
  );

  private retryphoto() {
    this.track('retryphoto', 'ReintentarLiveness');
    this.goBackToCapture();
  }
  private confirmphoto() {
    this.track('confirmphoto', 'ConfirmacionLiveness');
    this.processBometric();
  }

  public LoadCheckActivePages(procesoConvenioGuid: string) {
    this.livenessService
      .QueryCheckActivePages(procesoConvenioGuid, Pages.liveness)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: GetResponse) => {
          if (res.code == HttpStatusCode.Ok && res.data) {
            this.contentTitle.set(res.data?.content?.title);
            this.behaviorCfg = res.data?.behavior;
            this.utils.setBehavior(this.behaviorCfg);
            this.utils.setHeader(res.data?.header);
            this.utils.setDisplayButton(true);
          } else {
            this.Alert.Warning(
              `No hay configuración para ${Pages.liveness} (${procesoConvenioGuid})`
            );
          }
        },
      });
  }

  private track(action: string, status: string = '') {
    this.traficSrv.RegisterEvent(
      <Partial<ActionEvent>>{
        componente: 'liveness',
        accion: action,
        status: status,
      },
      false,
      true
    );
  }

  // ===== Regresar a la cámara =====
  public goBackToCapture() {
    this.isSubmitting.set(false);
    this.clearSubmitTimeout();

    this.photos.set([]);
    this.imgSrc.set(null);
    this.imgLoaded.set(false);
    this.isPreview.set(false);
    this.clearError();

    this.clearSeqTimeout();
    this.startCameraAuto().catch(() => {});
    this.schedule(() => this.startAutoCaptureSequence(), 20_000); // (mantengo tal como lo tenías)
  }

  /** Une un array de errores (strings) en un solo mensaje legible */
  private joinTxErrors(errors?: string[] | null): string | null {
    if (!errors || errors.length === 0) return null;
    const parts = errors.map((e) => (e ?? '').trim()).filter(Boolean);
    return parts.length ? parts.join(' • ') : null;
  }
}
