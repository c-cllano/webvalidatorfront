import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { StateService } from '@utils/services/state.service';
import { UtilsService } from '@utils/services/utils.service';

@Component({
  selector: 'app-progress-bar',
  templateUrl: './progress-bar.component.html',
  styleUrl: './progress-bar.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProgressBarComponent {
  public stateService = inject(StateService);
  private utils = inject(UtilsService);
  public totalSteps = signal(0);

  public getProgressbarDisplay = computed(() => {
    return this.utils?.ProgressbarDisplay()
      ? this.stateService.globalStyles()?.progressbar?.display || false
      : false;
  });

  public getProgressbarBackground = computed(() => {
    return this.stateService.globalStyles()?.progressbar?.background || '#DDDFE2';
  });

  public steps = computed(() => {
    return Array.from({ length: this.stateService.totalSteps() }, (_, index) => ({
      index
    }));
  });

  public currentIndex = computed(() => {
    return this.stateService.currentIndex() || 0;
  });


}
