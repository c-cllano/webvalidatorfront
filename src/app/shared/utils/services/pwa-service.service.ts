import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PwaService {
  private deferredPrompt = signal<any>(null);

  constructor() {
    window.addEventListener('beforeinstallprompt', (event: Event) => {
      event.preventDefault();
      this.deferredPrompt.set(event);
    });
  }

  public canPrompt(): boolean {
    return !!this.deferredPrompt();
  }

  public async promptInstall(): Promise<boolean> {
    if (!this.deferredPrompt()) return false;
    this.deferredPrompt().prompt();
    const { outcome } = await this.deferredPrompt().userChoice;
    this.deferredPrompt.set(null);
    return outcome === 'accepted';
  }

  public isStandalone(): boolean {
    return (window.matchMedia('(display-mode: standalone)').matches) ||
      ((window.navigator as any).standalone === true);
  }

  public isAndroid(): boolean {
    return /android/i.test(navigator.userAgent);
  }

  public isIOS(): boolean {
    return /iphone|ipad|ipod/i.test(navigator.userAgent);
  }

  public isDesktop(): boolean {
    return !this.isAndroid() && !this.isIOS();
  }
}
