import { Directive, ElementRef, Renderer2, effect, input, inject } from '@angular/core';
@Directive({
  selector: '[button]',
  host: {
    class: 'btn'
  }
})
export class ButtonDirective {
  private el = inject(ElementRef);
  private renderer = inject(Renderer2);
  public customStyles = input<Record<string, string> | any>()
  constructor() {
    effect(() => {
      const styles: { [key: string]: string | undefined } = {
        ...this.customStyles(),
      }
      for (const [key, value] of Object.entries(styles)) {
        if (value !== null && value !== undefined) {
          this.renderer.setStyle(this.el.nativeElement, key, value);
        }
      }
    });
  }
}
