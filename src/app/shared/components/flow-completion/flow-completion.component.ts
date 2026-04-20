import { Component, computed, effect, inject } from '@angular/core';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { StateService } from '@utils/services/state.service';
import { TrackEventService } from '@utils/services/trackevent.service';
import { State } from '@utils/services/utils.service';
import { LocalStorageService } from 'ngx-localstorage';
import { TitleDirective } from '@directives/title.directive';


@Component({
  selector: 'app-flow-completion',
  imports: [TitleDirective],
  templateUrl: './flow-completion.component.html',
  styleUrl: './flow-completion.component.css'
})
export class FlowCompletionComponent {
  private storageService = inject(LocalStorageService);
  private stateService = inject(StateService);
  private traficSrv = inject(TrackEventService);
  constructor() {
    effect(() => {
      this.stateService.setStateSignal(State.end);
      this.storageService.remove('EventCheckedAtdp');
      this.storageService.remove('CurrentIndex');
      this.storageService.remove('PwaVisible');
    });
    this.track('Validación finalizada.');
    setTimeout(() => {
      document.querySelectorAll('.skeleton').forEach(el => {
        el.classList.remove('skeleton');
      });
    }, 3000);

  }

  public getColorIcons = computed(() => {
    return this.stateService.globalStyles()?.logo_cons_native?.colorIcons || '#349BD0'
  });

  private track(action: string): void {
    this.traficSrv.RegisterEvent(<Partial<ActionEvent>>{
      componente: 'FlowCompletion',
      accion: action,
    });
  }

}
