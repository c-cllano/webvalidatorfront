import { DOCUMENT } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, ElementRef, HostListener, inject, input, Renderer2, signal, output, viewChild } from '@angular/core';
import { TitleComponent } from "../title/title.component";
import { ImgComponent } from "../img/img.component";
import { SubtitleComponent } from "../subtitle/subtitle.component";
import { BulletsComponent } from "../bullets/bullets.component";
import { ButtonComponent } from '@components/controls/button/button.component';
import { TypeButton } from '@utils/services/utils.service';
import { StateService } from '@utils/services/state.service';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
  imports: [TitleComponent, ImgComponent, SubtitleComponent, BulletsComponent, ButtonComponent],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ModalComponent  {
  public visible = input.required<boolean>();
  public config = input.required<any>();
  readonly closed = output<void>();
  readonly modalRef = viewChild.required<ElementRef>('modal');
  public ishtml = input.required<boolean>();
  public content = input<string>();
  public Button_main = signal<TypeButton>(TypeButton.button_main);
  public Button_secondary = signal<TypeButton>(TypeButton.button_secondary);
  private stateService = inject(StateService);
  private renderer = inject(Renderer2);
  private document = inject<Document>(DOCUMENT);

  public functionMap: Record<string, () => void> = {
    onCancel: this.onCancel.bind(this),
    onAccept: this.onAccept.bind(this),
    onclose: this.onclose.bind(this),
  };

  ngOnChanges(): void {
    if (typeof window !== 'undefined' && this.document) {
      if (this.visible()) {
        this.renderer.addClass(this.document.body, 'no-scroll');
      } else {
        this.renderer.removeClass(this.document.body, 'no-scroll');
      }
    }
  }

  closeModal(): void {
    this.closed.emit();
  }

  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: KeyboardEvent): void {
    if (this.visible()) {
      this.closeModal();
    }
  }

  onOverlayClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal')) {
      this.closeModal();
    }
  }



  public titleDisplay = computed(() => {
    return this.config()?.title?.display || false
  });

  public title = computed(() => {
    return this.config()?.title || ''
  });

  public descriptionDisplay = computed(() => {
    return this.config()?.description?.display || false
  });

  public description = computed(() => {
    return this.config()?.description || ''
  });

  public imageContentDisplay = computed(() => {
    return this.config()?.image_content?.display || false
  });

  public imageContent = computed(() => {
    return this.config()?.image_content || ''
  });

  public subtitleDisplay = computed(() => {
    return this.config()?.subtitle?.display || false
  });

  public subtitle = computed(() => {
    return this.config()?.subtitle || ''
  });

  public paragraphDisplay = computed(() => {
    return this.config()?.paragraph?.display || false
  });

  public paragraph = computed(() => {
    return this.config()?.paragraph || ''
  });

  public bulletListDisplay = computed(() => {
    return this.config()?.bullet_list?.display || false
  });


  public bulletList = computed(() => {
    return this.config()?.bullet_list || []
  });

  public mainButtonDisplay = computed(() => {
    return this.config()?.behavior?.button_main?.display || false
  });

  public secondaryButtonDisplay = computed(() => {
    return this.config()?.behavior?.button_secondary?.display || false
  });

  public invertButtonPosition = computed(() => {
    return this.config()?.behavior.reverseOrder || false;
  });

  public getConfigButton_main = computed(() => {
    return this.config()?.behavior?.button_main || {}
  });

  public getConfigButton_secondary = computed(() => {
    return this.config()?.behavior?.button_secondary || {}
  });


  public atdpModalTitleDisplay = computed(() => {
    return this.stateService.globalStyles()?.atdp?.modal?.title?.display || false;
  });

  public atdpModalTitle = computed(() => {
    return this.stateService.globalStyles()?.atdp?.modal?.title || ''
  });


  public atdpModalSubtitleDisplay = computed(() => {
    return this.stateService.globalStyles()?.atdp?.modal?.subtitle?.display || false;
  });

  public atdpModalSubtitle = computed(() => {
    return this.stateService.globalStyles()?.atdp?.modal?.subtitle || ''
  });

  public getColorIcons = computed(() => {
    return this.stateService.globalStyles()?.logo_cons_native?.colorIcons || '#349BD0'
  });


  public atdpfontfamily = computed(() => {
    return this.stateService.globalStyles()?.typography['font-family'] || 'Poppins';
  });

  public atdpColor = computed(() => {
    return this.stateService.globalStyles()?.typography?.color || '#349BD0';
  });

  public atdpModalButton_main = computed(() => {
    const button_main = this.stateService.globalStyles()?.atdp?.modal?.button_main;;
    const styles: { [key: string]: string | undefined } = {
      ...button_main,
      display: true,
      click_event: 'onclose()'
    };
    return styles;
  });


  onclose() {
    this.closeModal();
  }

  onCancel() {
    this.closeModal();
  }

  onAccept() {
    this.closeModal();
  }
}
