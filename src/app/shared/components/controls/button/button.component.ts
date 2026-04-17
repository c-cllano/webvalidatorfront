import { ChangeDetectionStrategy, Component, computed, inject, input, output } from '@angular/core';
import { ButtonDirective } from '@directives/button.directive';
import { AlertMessageService } from '@utils/services/alert.service';
import { EsqueletonService } from '@utils/services/esqueleton.service';
import { StateService } from '@utils/services/state.service';
import { TypeButton } from '@utils/services/utils.service';
import { numberToRem } from '@utils/services/utils.service';



@Component({
  selector: 'app-button',
  imports: [ButtonDirective],
  templateUrl: './button.component.html',
  styleUrl: './button.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ButtonComponent {
  private Alert = inject(AlertMessageService);
  private stateService = inject(StateService);
  public Esqueleton = inject(EsqueletonService);
  public configButton = input.required<any>();
  public typeButton = input.required<TypeButton>();
  public functionMap = input.required<Record<string, () => void>>();
  public disabled = input<boolean>(false);
  public invertButtonPosition = input<boolean>(false);
  readonly click_event = output<void>();

  public getDisplay = computed(() => {
    return this.configButton()?.display || false
  });

  public getText = computed(() => {
    return this.configButton()?.text || ''
  });

  public getDisabled = computed(() => {
    return this.disabled() || !this.getfinEsqueleton();
  });

  public getclick_event = computed(() => {
    return this.configButton()?.click_event || ''
  });

   public getfinEsqueleton = computed(() => {
    return this.Esqueleton.finEsqueleton() || false
  });

  public getGlobalButtonbutton_main = computed(() => {
    const button_main = this.stateService.globalStyles()?.buttons?.button_main || {};
    const borderButton = this.stateService.globalStyles()?.buttons?.borderRadius;
    const fontfamily = this.stateService.globalStyles()?.typography['font-family'] || 'Poppins';
    const disabled = this.getDisabled();
    const invertButtonPosition = this.invertButtonPosition();
    const styles: { [key: string]: string | undefined } = {
      ...button_main,
      borderRadius: numberToRem(borderButton),
      border: !disabled ? `0.0625rem solid ${button_main?.color}` : '0.0625rem solid #C5C5C5',
      'font-family': fontfamily,
      ...(!invertButtonPosition ? { 'margin-right': 'var(--spacing-5)' } : ''),
      ...(disabled ? { backgroundColor: '#FFF', color: '#606060' } : {})

    };
    return styles;
  });

  public getGlobalButtonbutton_secondary = computed(() => {
    const button_secondary = this.stateService.globalStyles()?.buttons?.button_secondary || {};
    const borderButton = this.stateService.globalStyles()?.buttons.borderRadius;
    const fontfamily = this.stateService.globalStyles()?.typography['font-family'] || 'Poppins';
    const invertButtonPosition = this.invertButtonPosition();
    const disabled = this.getDisabled();
    const styles: { [key: string]: string | undefined } = {
      ...button_secondary,
      borderRadius: numberToRem(borderButton),
      'font-family': fontfamily,
      ...(invertButtonPosition ? { 'margin-right': 'var(--spacing-5)' } : ''),
      ...(disabled ? { backgroundColor: '#DDDFE2', color: '#606060' } : {})
    };
    return styles;
  });


  executeClickEvent(): void {
    const disabled = this.getDisabled();
    if (!disabled) {
      const raw = this.getclick_event();
      const methodName = raw.replace('()', '').trim();
      const fn = this.functionMap()[methodName];
      if (typeof fn === 'function') {
        fn();
      } else {
        this.Alert.Warning(`Método "${methodName}" no está en functionMap`)
      }
      this.click_event.emit();
    }
  }

}
