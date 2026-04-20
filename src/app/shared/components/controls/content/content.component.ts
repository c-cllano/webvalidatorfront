import { AfterViewInit, ChangeDetectionStrategy, Component, computed, effect, inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';
import { UtilsService } from '@utils/services/utils.service';
import { AtdpComponent } from '@components/controls/atdp/atdp.component';
import { EsqueletonService } from '@utils/services/esqueleton.service';


@Component({
  selector: 'app-content',
  imports: [AtdpComponent],
  templateUrl: './content.component.html',
  styleUrl: './content.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ContentComponent implements AfterViewInit {
  private utilsService = inject(UtilsService);
  public Esqueleton = inject(EsqueletonService);
  private isBrowser: boolean;
  readonly screenWidth = signal(0);
  readonly scrollY = signal(0);
  readonly isDesktop = computed(() => this.screenWidth() >= 1024);
  private platformId = inject<Object>(PLATFORM_ID);
  constructor() {
    this.isBrowser = isPlatformBrowser(this.platformId);
    if (this.isBrowser) {
      effect(() => this.adjustHeader());
    }
  }

  ngAfterViewInit(): void {
    if (!this.isBrowser) return;
    this.screenWidth.set(window.innerWidth);
    const mainEl = document.querySelector('main')!;
    this.scrollY.set(mainEl.scrollTop);
    window.addEventListener('resize', () => {
      this.screenWidth.set(window.innerWidth);
      if (this.isDesktop()) {
        this.Esqueleton.endRequest();
      }
    });
    mainEl.addEventListener('scroll', () => {
      this.scrollY.set(mainEl.scrollTop);
    });
  }


  private adjustHeader() {
    const header = document.querySelector('.sticky') as HTMLElement;
    const wrapper = document.querySelector('.content') as HTMLElement;
    if (!header || !wrapper) return;
    if (this.scrollY() > 0) {
      header.classList.add('fx--sticky');
      wrapper.style.paddingTop = `${header.offsetHeight}px`;
    } else {
      header.classList.remove('fx--sticky');
      wrapper.style.paddingTop = '0';
    }
  }

  public atdpDisplay = computed(() => {
    return this.utilsService.behavior()?.atdp?.display || false;
  });


}
