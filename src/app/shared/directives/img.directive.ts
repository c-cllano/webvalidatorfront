import { Directive, effect, ElementRef, input, Renderer2, inject } from '@angular/core';
import { numberToRem } from '@utils/services/utils.service';

@Directive({
  selector: '[appImg]'
})
export class ImgDirective {
  private el = inject(ElementRef);
  private renderer = inject(Renderer2);
  public customStyles = input<Record<string, string> | undefined>();
  constructor() {
    effect(() => {
      const custom = this.customStyles() ?? {};
      delete custom['img'];
      const styles: { [key: string]: string } = {
        ...custom,
        width: numberToRem(custom['width']),
        height: numberToRem(custom['height'])
      };
      for (const [key, value] of Object.entries(styles)) {
        if (value) {
          this.renderer.setStyle(this.el.nativeElement, key, value);
        }
      }
    });

  }

}
