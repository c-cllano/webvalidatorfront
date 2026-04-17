import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { ImgDirective } from '@directives/img.directive';

@Component({
  selector: 'app-img',
  imports: [ImgDirective],
  templateUrl: './img.component.html',
  styleUrl: './img.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ImgComponent {

  public config = input<any>(null);

  public getImg = computed(() => {
    return this.config()?.img || {}
  })

  public displayImg = computed(() => {
    return this.config()?.display || false
  })

  public getTextTooltip = computed(() => {
    return this.config()?.text || ''
  })

  public displayTooltip = computed(() => {
    return this.config()?.text !== undefined && this.config()?.text !== ''
  })

}
