import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { StateService } from '@utils/services/state.service';
import { State } from '@utils/services/utils.service';
import { NgxSpinnerModule } from "ngx-spinner";

@Component({
  selector: 'app-spinner',
  imports: [NgxSpinnerModule],
  templateUrl: './spinner.component.html',
  styleUrl: './spinner.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SpinnerComponent {

  private stateService = inject(StateService);
  
  public getglobalStylesColor = computed(() => {
    return  this.stateService.state() === State.process ?  this.stateService.globalStyles()?.buttons?.button_secondary['background-color'] : "rgba(52, 155, 208, 1)";
  });


}
