import { Directive, effect, ElementRef, inject, input, Renderer2 } from '@angular/core';
import { StateService } from '@utils/services/state.service';

@Directive({
  selector: '[appBullets]'
})
export class BulletsDirective {
  private el = inject(ElementRef);
  private renderer = inject(Renderer2);
  private stateService = inject(StateService);
  constructor() {
    effect(() => {
      const globalStyle = Object.fromEntries(
        Object.entries(this.stateService.globalStyles()?.typography ?? {})
          .filter(([__, value]) => typeof value !== 'object')
      ) as { [key: string]: string };
      const styles: { [key: string]: string } = {
        ...globalStyle
      };
      for (const [key, value] of Object.entries(styles)) {
        if (value) {
          this.renderer.setStyle(this.el.nativeElement, key, value);
        }
      }
    });

  }

}
