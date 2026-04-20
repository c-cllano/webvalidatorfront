import { Directive, effect, ElementRef, inject, input, Renderer2 } from '@angular/core';
import { StateService } from '@utils/services/state.service';
import { numberToRem } from '@utils/services/utils.service';
@Directive({
  selector: '[appTitle]'
})
export class TitleDirective {
  private el = inject(ElementRef);
  private renderer = inject(Renderer2);
  public customStyles = input<Record<string, string> | undefined>();
  public typeTitleDefaul = input.required<string>();
  private stateService = inject(StateService);
  constructor() {
    effect(() => {
      const globalStyle = {
        ...Object.fromEntries(Object.entries(this.stateService.globalStyles()?.typography ?? {})
          .filter(([__, value]) => typeof value !== 'object')),
        ...(this.typeTitleDefaul() === 'Title'
          ? this.stateService.globalStyles()?.typography?.title : this.stateService.globalStyles()?.typography?.description)
      };
      const custom = this.customStyles() ?? {};
      const styles: { [key: string]: string } = {
        ...custom,
        ...globalStyle,
        'font-size': numberToRem(globalStyle['font-size'])
      };
      for (const [key, value] of Object.entries(styles)) {
        if (value) {
          this.renderer.setStyle(this.el.nativeElement, key, value);
        }
      }
    });

  }

}
