import { isPlatformBrowser } from '@angular/common';
import { Injectable, PLATFORM_ID, computed, inject, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EsqueletonService {
  private platformId = inject<Object>(PLATFORM_ID);
  private activeRequests = signal<number>(0);
  private finEsqueletonSignal = signal<boolean>(false);
  readonly finEsqueleton = computed(() => this.finEsqueletonSignal());

  public startRequest(): void {
    this.finEsqueletonSignal.set(false);
    this.activeRequests.update(v => v + 1);
  }

  public endRequest(): void {
    this.activeRequests.update(v => v - 1);
    if (this.activeRequests() <= 0) {
      this.activeRequests.set(0);
      setTimeout(() => {
        if (this.activeRequests() === 0) {
          this.hide();
        }
      }, 200);
    }
  }

  private hide(): void {
    if (isPlatformBrowser(this.platformId)) {
      document.fonts.load('1em "Material Symbols Rounded"').then(() => {
        setTimeout(() => {
          document.querySelectorAll('.skeleton').forEach(el => {
            el.classList.remove('skeleton');
          });
          this.finEsqueletonSignal.set(true);
        }, 1000);
      });
    }
  }
}