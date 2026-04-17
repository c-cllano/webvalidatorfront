import { Directive, effect, ElementRef, inject, input } from '@angular/core';
import { TrackEventService } from '@utils/services/trackevent.service';

@Directive({
  selector: '[appTrackevent]'
})
export class TrackeventDirective {
  private el = inject(ElementRef);
  public guidagreementprocess = input<string>('No definido');
  public componente = input<string>('No definido');
  public accion = input<string>('No definido');
  public user = input<string>('No definido');
  public passed = input<number | undefined>();
  public agreement = input<string>('No definido');
  private trackeventService = inject(TrackEventService);
  constructor() {
    effect(() => {
      const native = this.el.nativeElement;
      const listener = () => {
        this.trackeventService.RegisterEvent({
          guidagreementprocess: this.guidagreementprocess(),
          componente: this.componente(),
          accion: this.accion(),
          user: this.user(),
          passed: this.passed(),
          agreement: this.agreement()
        });
      };
      native.addEventListener('click', listener);
      return () => native.removeEventListener('click', listener);
    });
  }

}
