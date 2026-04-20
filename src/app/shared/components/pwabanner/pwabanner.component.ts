import { Component, effect, inject, signal } from '@angular/core';
import { EsqueletonService } from '@utils/services/esqueleton.service';
import { PwaService } from '@utils/services/pwa-service.service';
import { UtilsService } from '@utils/services/utils.service';
import { LocalStorageService } from 'ngx-localstorage';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-pwabanner',
  imports: [],
  templateUrl: './pwabanner.component.html',
  styleUrl: './pwabanner.component.css'
})
export class PwabannerComponent {
  public visible = signal<boolean>(false);
  public PwaVisible = signal<boolean>(false);
  private pwa = inject(PwaService);
  private esqueleton = inject(EsqueletonService);
  private utils = inject(UtilsService);
  private storageService = inject(LocalStorageService);



  constructor() {
    effect(() => {
      const finEsqueleton = this.esqueleton.finEsqueleton();
      if (finEsqueleton) {
        setTimeout(() => {
          this.restoreFromStorage();
          if (!this.PwaVisible()) {
            this.visible.set(this.pwa.canPrompt());
          }
          if (!this.visible()) {
            if (!this.pwa.isStandalone()) {
              // this.openInstalledApp()
            }
          }
        }, 1000);
      }
    });

  }


  public async install() {
    await this.pwa.promptInstall();
    this.visible.set(false);
    this.SavePwaVisible(true);
  }

  public close() {
    this.visible.set(false);
    this.SavePwaVisible(true);
  }

  private SavePwaVisible(value: boolean): void {
    this.storageService.set('PwaVisible', this.utils.encrypt(value.toString()!));
  }

  private restoreFromStorage(): void {
    const PwaVisible = this.utils.decrypt<boolean>(this.storageService.get('PwaVisible'));
    this.PwaVisible.set(PwaVisible!);
  }

  private openInstalledApp()                  {
    if (environment.production) {
      if (this.pwa.isAndroid()) {
        window.location.href = 'https://okeypre.olimpiait.com';
      }
      else if (this.pwa.isDesktop()) {
        window.open('https://okeypre.olimpiait.com', '_blank', 'width=800,height=600');
      }
    }
  }

}
