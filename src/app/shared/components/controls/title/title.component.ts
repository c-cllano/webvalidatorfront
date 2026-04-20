import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { TitleDirective } from '@directives/title.directive';
import { stringFormat } from '@utils/services/utils.service';

@Component({
  selector: 'app-title',
  imports: [TitleDirective],
  templateUrl: './title.component.html',
  styleUrl: './title.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TitleComponent  {
  public configTitle = input<any>(null);
  public configDescription = input<any>(null);
  public firstname = input<string>("");


  public CustomStylesTitle = computed(() => {
    const config = this.configTitle();
    const configDescription = this.configDescription();
    const enhancedConfig = {
      ...config,
      'margin-bottom': configDescription?.display === false ? '1.5rem' : ''
    };
    return enhancedConfig;
  });

  public CustomStylesDescription = computed(() => {
    return this.configDescription() || ''
  });

  public getDisplayTitle = computed(() => {
    return this.configTitle()?.display || false
  });

  public getDisplayDescription = computed(() => {
    return this.configDescription()?.display || false
  });

  public TextTitle = computed(() => {
    return stringFormat(this.configTitle()?.text || '', this.firstname());
  });

  public TextDescription = computed(() => {
    return this.configDescription()?.text || ''
  });

}
