import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable, PLATFORM_ID, inject, signal } from '@angular/core';
import { DeviceDetectorService } from 'ngx-device-detector';
import { EventTraffic, ActionEvent, EventDeviceInfo } from '@utils/models/trackevent.interface';
import { StateService } from '@utils/services/state.service';

@Injectable({ providedIn: 'root' })
export class TrackEventService {
  private platformId = inject<Object>(PLATFORM_ID);
  public stateService = inject(StateService);

  // ⏱️ inicio de la sesión para calcular permanencia
  private readonly sessionStartTime = (typeof performance !== 'undefined') ? performance.now() : 0;

  private readonly events = signal<EventTraffic>({
    Actions: [],
    Devices: [],
    sessionDurationSeconds: 0
  });

  private readonly lastDeviceKey = signal<string>(''); // para evitar duplicados

  private http = inject(HttpClient);
  private deviceService = inject(DeviceDetectorService);

  // Dentro de TrackEventService

private getSessionDurationSecondsNow(): number {
  if (typeof performance === 'undefined') return 0;
  const now = performance.now();
  return Math.round((now - this.sessionStartTime) / 1000);
}

RegisterEvent(
  eventAction: Partial<ActionEvent>,
  enableCamera: boolean = false,
  calcDuration: boolean = false
): void {
  if (!isPlatformBrowser(this.platformId)) return;

  // ✅ solo recalcula si calcDuration es true
  const durationToSet = calcDuration
    ? this.getSessionDurationSecondsNow()
    : this.events().sessionDurationSeconds;

  const baseDevice: EventDeviceInfo = {
    devicetype: this.getDeviceType(),
    browser: this.deviceService.browser,
    browserVersion: this.deviceService.browser_version,
    os: this.getEnhancedOS(),
    osVersion: this.getEnhancedOSVersion(),
    language: navigator.language,
    screenResolution: `${window.screen.width}x${window.screen.height}`,
    screenOrientation: (window.screen.orientation as ScreenOrientation | undefined)?.type ?? 'unknown',
    online: navigator.onLine
  };

  const deviceKey = JSON.stringify(baseDevice);
  const shouldAddDevice = deviceKey !== this.lastDeviceKey();

  let nextState: EventTraffic = {
    Actions: [...this.events().Actions],
    Devices: [...this.events().Devices],
    sessionDurationSeconds: durationToSet
  };

  if (shouldAddDevice) {
    this.lastDeviceKey.set(deviceKey);
    nextState.Devices.push(baseDevice);
  }

  // metadata del action
  eventAction.user = this.stateService.process()?.data?.datos?.numDoc;
  eventAction.agreement = this.stateService.guidCon();
  eventAction.guidagreementprocess = this.stateService.guidPro();

  const newAction: ActionEvent = {
    ...eventAction,
    date: new Date().toISOString()
  };

  const exists = nextState.Actions.some(e =>
    e.componente === newAction.componente &&
    e.accion === newAction.accion &&
    e.user === newAction.user &&
    e.agreement === newAction.agreement &&
    (e.passed ?? null) === (newAction.passed ?? null)
  );

  if (!exists) nextState.Actions.push(newAction);

  // guarda estado (duración según flag)
  this.events.set(nextState);

  // enriquecimiento de cámara (async) — también respeta calcDuration
  if (shouldAddDevice && enableCamera) {
    this.detectCameraInfo().then(info => {
      if (!info) return;

      const updatedDevice: EventDeviceInfo = {
        ...baseDevice,
        cameraLabel: info.label,
        cameraWidth: info.width,
        cameraHeight: info.height
      };

      const devices = [...this.events().Devices];
      devices[devices.length - 1] = updatedDevice;

      this.events.set({
        ...this.events(),
        Devices: devices,
        sessionDurationSeconds: calcDuration
          ? this.getSessionDurationSecondsNow()
          : this.events().sessionDurationSeconds
      });
    });
  }

  console.log(this.events())
}



  saveEvents(): void {
    const currentEvents = this.events();
    if (!currentEvents.Actions.length && !currentEvents.Devices.length) return;

    // Siempre recalculamos al guardar, independiente del flag
    const durationSeconds = this.getSessionDurationSecondsNow();

    const updatedEvents: EventTraffic = {
      ...currentEvents,
      sessionDurationSeconds: durationSeconds
    };

    // limpiar buffer local
    this.events.set({ Actions: [], Devices: [], sessionDurationSeconds: 0 });

    console.log(`Tiempo en pantalla: ${durationSeconds} segundos`);
    console.log('Eventos guardados', updatedEvents);

    /*
    this.http.post('/api/trafico', updatedEvents).subscribe({
      next: () => console.log('Eventos enviados'),
      error: () => {
        console.error('Error al enviar eventos');
        this.events.set({
          Actions: [...currentEvents.Actions, ...this.events().Actions],
          Devices: [...currentEvents.Devices, ...this.events().Devices],
          sessionDurationSeconds: durationSeconds
        });
      }
    });
    */
  }

  /** ✅ Datos actuales de la sesión (duración calculada al momento de invocar) */
  public getSessionData(): { devices: EventDeviceInfo[]; sessionDurationSeconds: number } {
    return {
      devices: [...this.events().Devices],
      sessionDurationSeconds: this.getSessionDurationSecondsNow()
    };
  }

  // ========== Detalles de dispositivo/OS ==========
  private getDeviceType(): string {
    if (this.deviceService.isMobile()) return 'Mobile';
    if (this.deviceService.isTablet()) return 'Tablet';
    return 'Desktop';
  }

  // ⭐ Windows 10 vs 11 con UA-CH y fallback a UA clásico
  private getWindowsFromUAOrCH(): { osName: string; osVersion: string } | null {
    if (typeof navigator === 'undefined') return null;

    const navAny = navigator as any;
    const isWindows =
      /Win/i.test(navigator.platform || '') ||
      navAny?.userAgentData?.platform === 'Windows';

    if (!isWindows) return null;

    // 1) User-Agent Client Hints (Chromium/Edge)
    const pvStr: string | undefined = navAny?.userAgentData?.platformVersion;
    if (pvStr) {
      const major = parseInt((pvStr.split?.('.')?.[0] ?? '0'), 10);
      if (!Number.isNaN(major) && major > 0) {
        const isWin11 = major >= 13; // 13+ = Windows 11
        const osName = isWin11 ? 'Windows 11' : 'Windows 10';
        return { osName, osVersion: `${osName} (platformVersion ${pvStr})` };
      }
    }

    // 2) Fallback UA clásico
    const ua = navigator.userAgent || '';
    const nt = ua.match(/Windows NT (\d+\.\d+)/)?.[1];
    if (nt) {
      if (nt === '10.0') return { osName: 'Windows 10/11', osVersion: 'NT 10.0' };
      if (nt === '6.3')  return { osName: 'Windows 8.1',  osVersion: 'NT 6.3' };
      if (nt === '6.1')  return { osName: 'Windows 7',    osVersion: 'NT 6.1' };
      return { osName: 'Windows', osVersion: `NT ${nt}` };
    }

    return { osName: 'Windows', osVersion: 'Unknown' };
  }

  private getEnhancedOS(): string {
    if (typeof navigator === 'undefined') return 'Unknown';

    const win = this.getWindowsFromUAOrCH();
    if (win) return win.osName;

    const ua = navigator.userAgent || '';
    const platform = navigator.platform || '';

    if (/Mac/i.test(platform)) return 'macOS';
    if (/iPhone|iPad|iPod/i.test(platform)) return 'iOS';
    if (/Android/i.test(ua)) return 'Android';
    if (/Linux/i.test(platform)) return 'Linux';

    return 'Unknown';
  }

  private getEnhancedOSVersion(): string {
    if (typeof navigator === 'undefined') return 'Unknown';

    const win = this.getWindowsFromUAOrCH();
    if (win) return win.osVersion;

    const ua = navigator.userAgent || '';

    // iOS
    const iosMatch = ua.match(/OS (\d+_\d+(_\d+)?)/i);
    if (iosMatch) return iosMatch[1].replace(/_/g, '.');

    // macOS
    const macMatch = ua.match(/Mac OS X (\d+[_\.]\d+([_\.]\d+)?)/);
    if (macMatch) return macMatch[1].replace(/_/g, '.');

    // Android
    const androidMatch = ua.match(/Android (\d+(\.\d+)?)/);
    if (androidMatch) return androidMatch[1];

    if (/Linux/i.test(navigator.platform || '')) return 'Unknown';
    return 'Unknown';
  }

  // ========== Cámara ==========
  private async detectCameraInfo(): Promise<{ label: string; width: number; height: number } | null> {
    if (!navigator.mediaDevices?.getUserMedia) return null;
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ video: true });
      const track = stream.getVideoTracks()[0];
      const { width = 0, height = 0 } = track.getSettings();
      const label = track.label || 'Sin nombre';
      stream.getTracks().forEach(t => t.stop());
      return { label, width, height };
    } catch {
      console.warn('No se pudo acceder a la cámara');
      return null;
    }
  }
}
