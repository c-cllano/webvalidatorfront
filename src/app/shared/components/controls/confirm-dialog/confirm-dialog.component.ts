import { CommonModule, DOCUMENT } from '@angular/common';
import { OnChanges, HostListener, Renderer2, model, Component, input, computed, signal, ChangeDetectionStrategy, output, inject } from '@angular/core';
import { ButtonComponent } from '@components/controls/button/button.component';
import { TypeButton } from '@utils/services/utils.service';
import { SubtitleDirective } from '@directives/subtitle.directive';
import { TitleDirective } from '@directives/title.directive';
import { StateService } from '@utils/services/state.service';

@Component({
  selector: 'app-confirm-dialog',
  imports: [CommonModule, ButtonComponent, SubtitleDirective, TitleDirective],
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ConfirmDialogComponent implements OnChanges {
  public visible = model<boolean>(false);
  public config = input.required<any>();
  public Button_main = signal<TypeButton>(TypeButton.button_main);
  public Button_secondary = signal<TypeButton>(TypeButton.button_secondary);
  private stateService = inject(StateService);
  private renderer = inject(Renderer2);
  private document = inject<Document>(DOCUMENT);

  readonly accepted = output<void>();
  readonly cancelled = output<void>();



  public functionMap: Record<string, () => void> = {
    onCancel: this.onCancel.bind(this),
    onAccept: this.onAccept.bind(this),
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

  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: KeyboardEvent): void {
    if (this.visible()) {
      this.onCancel();
    }
  }

  onOverlayClick(event: MouseEvent): void {
    if ((event.target as HTMLElement).classList.contains('modal')) {
      this.onCancel();
    }
  }

  onCancel() {
    this.cancelled.emit();
    this.visible.set(false);
  }

  onAccept() {
    this.accepted.emit();
    this.visible.set(false);
  }


  public getConfigTitle = computed(() => {
    return this.config()?.modal_confirm?.title || ''
  });

  public getConfigDescription = computed(() => {
    return this.config()?.modal_confirm?.description || ''
  });

  public getConfigButton_main = computed(() => {
    return this.config()?.modal_confirm?.button_main || {}
  });

  public getConfigButton_secondary = computed(() => {
    return this.config()?.modal_confirm?.button_secondary || {}
  });

   public getColorIcons = computed(() => {
    return this.stateService.globalStyles()?.logo_cons_native?.colorIcons || '#349BD0'
  });
}
