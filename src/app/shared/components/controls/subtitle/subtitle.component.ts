import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { SubtitleDirective } from '@directives/subtitle.directive';

@Component({
  selector: 'app-subtitle',
  imports: [SubtitleDirective],
  templateUrl: './subtitle.component.html',
  styleUrl: './subtitle.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SubtitleComponent {
  public configSubtitle = input<any>(null);
  public confiParagraph = input<any>(null);

  public customStylesSubtitle = computed(() => {
    const config = this.configSubtitle();
    const configDescription = this.confiParagraph();
    const enhancedConfig = {
      ...config,
      'margin-bottom': configDescription?.display === false ? '1.5rem' : ''
    };
    return enhancedConfig;
  });

  public customStylesParagraph = computed(() => {
    return this.confiParagraph() || ''
  });

  public getDisplaySubtitle = computed(() => {
    return this.configSubtitle()?.display || false
  });

  public getDisplayParagraph = computed(() => {
    return this.confiParagraph()?.display || false
  });

  public TextSubtitle = computed(() => {
    return this.configSubtitle()?.text || ''

  });

  public TextParagraph = computed(() => {
    return this.confiParagraph()?.text.replace(/&nbsp;|\u00A0/g, ' ') || '';
  });

}
