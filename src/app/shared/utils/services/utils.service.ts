import { isPlatformBrowser } from '@angular/common';
import { computed, inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';
import CryptoES from 'crypto-es';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {
  private platformId = inject<Object>(PLATFORM_ID);
  private spinner = inject(NgxSpinnerService);
  private functionMapSignal = signal<any>(null);
  private ProgressbarDisplaySignal = signal<boolean>(true);
  private BehaviorSignal = signal<any>(null);
  private HeaderSignal = signal<any>(null);
  private eventCheckedAtdpSignal = signal<any>(null);
  private DisplayButtonSignal = signal<any>(false);
  readonly functionMap = computed(() => this.functionMapSignal());
  readonly ProgressbarDisplay = computed(() => this.ProgressbarDisplaySignal());
  readonly behavior = computed(() => this.BehaviorSignal());
  readonly header = computed(() => this.HeaderSignal());
  readonly eventCheckedAtdp = computed(() => this.eventCheckedAtdpSignal());
  readonly displayButton = computed(() => this.DisplayButtonSignal());


  public async handlePermission(step: TypePermission): Promise<boolean> {
    if (!isPlatformBrowser(this.platformId)) {
      return true;
    }
    if (step === TypePermission.cameradenied) {
      try {
        const permissionStatus = await navigator.permissions.query({ name: "camera" as PermissionName });
        if (permissionStatus.state === "granted") {
          return true;
        }
        this.spinner.show();
        await navigator.mediaDevices.getUserMedia({ video: true });
        this.spinner.hide();
        return true;
      } catch (error) {
        this.spinner.hide();
        return false;
      }
    }
    if (step === TypePermission.locationdenied) {
      try {
        const permissionStatus = await navigator.permissions.query({ name: "geolocation" });
        if (permissionStatus.state === "granted") {
          return true;
        }
        this.spinner.show();
        return new Promise<boolean>((resolve) => {
          navigator.geolocation.getCurrentPosition(
            (position) => {
              this.spinner.hide();
              resolve(true);
            },
            (error) => {
              this.spinner.hide();
              resolve(false);
            },
            { enableHighAccuracy: true }
          );
        });
      } catch {
        this.spinner.show();
        return new Promise<boolean>((resolve) => {
          navigator.geolocation.getCurrentPosition(
            (position) => {
              this.spinner.hide();
              resolve(true);
            },
            (error) => {
              this.spinner.hide();
              resolve(false);
            },
            { enableHighAccuracy: true }
          );
        });
      }
    }

    this.spinner.hide();
    return false;
  }


  public setfunctionMap(map: Record<string, () => void>) {
    this.functionMapSignal.set(map);
  }

  public setProgressbarDisplay(value: boolean) {
    this.ProgressbarDisplaySignal.set(value);
  }

  public setBehavior(value: boolean) {
    this.BehaviorSignal.set(value);
  }

  public setHeader(value: boolean) {
    this.HeaderSignal.set(value);
  }

  public setEventCheckedAtdp(value: boolean) {
    this.eventCheckedAtdpSignal.set(value);
  }

  public setDisplayButton(value: boolean) {
    this.DisplayButtonSignal.set(value);
  }

  public decrypt<T = string>(texto: string | null): T | null {
    if (!texto) return null;

    try {
      const bytes = CryptoES.AES.decrypt(texto, environment.decryptKey);
      const result = bytes.toString(CryptoES.enc.Utf8);

      if (result === 'true') return true as T;
      if (result === 'false') return false as T;
      if (!isNaN(Number(result)) && result.trim() !== '') {
        return Number(result) as T;
      }

      try {
        return JSON.parse(result) as T;
      } catch {
        return result as T;
      }
    } catch {
      return null;
    }
  }

  public encrypt(texto: string | null): string | null {
    try {
      return CryptoES.AES.encrypt(texto ?? '', environment.decryptKey).toString();
    } catch {
      return null;
    }
  }


}

export function stringFormat(template: string, ...args: string[]) {
  return template.replace(/{(\d+)}/g, (_match, index) => args[index] || "");
}

export function numberToRem(value: number | string): string {
  if (typeof value === 'string') {
    return '1.5rem';
  }
  if (isNaN(value)) {
    return '1.5rem';
  }
  return `${value}rem`;
}


export function getPageAndGuidFromUrl(url: string): { page: string; guid: string } {
  const [pathPart] = url.split('?');
  const parts = pathPart.split('/').filter(Boolean);
  const page = parts[0] ?? '';
  const guid = parts[1] ?? '';
  return { page, guid };
}


export enum ResponseMessage {
  TOO_MANY_REQUESTS = 'Demasiadas solicitudes. Inténtalo de nuevo más tarde.',
  INTERNAL_SERVER_ERROR = 'Comuníquese con al administrador error.',
  OK = 'Éxito'
}

export enum TypeButton {
  button_main = 'button_main',
  button_secondary = 'button_secondary',
}


export enum TypePermission {
  cameradenied = 'cameradenied',
  locationdenied = 'locationdenied',
  permitdenied = 'permitdenied'
}

export enum State {
  start = 'start',
  end = 'end',
  process = 'process'
}

export enum typeCurrent {
  Inicio = 'inicio-1',
  Fin = 'Fin',
}


export enum Pages {
  redirect = 'redirect',
  welcome = 'welcome',
  permit = 'permit',
  permitdenied = 'permitdenied',
  docvalidation = 'docvalidation',
  livenessguide = 'livenessguide',
  flowcompletion = 'flow-completion',
  uploaddoc = 'uploaddoc',
  liveness= 'liveness',
}

