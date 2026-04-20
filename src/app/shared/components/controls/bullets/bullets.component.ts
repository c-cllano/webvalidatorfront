import { ChangeDetectionStrategy, Component, computed, model } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BulletsDirective } from '@directives/bullets.directive';
import { numberToRem } from '@utils/services/utils.service';


@Component({
  selector: 'app-bullets',
  imports: [CommonModule, BulletsDirective],
  templateUrl: './bullets.component.html',
  styleUrl: './bullets.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BulletsComponent {
  public numberToRem = numberToRem;
  public bullet_list = model.required<any>();


  public visible_bullets = computed(() => {
    return this.bullet_list()?.items.slice(0, 7)
  });

  public truncateText(text: string): string {
    return text.length > 200 ? text.slice(0, 200) + '...' : text;
  }

  public getBulletIcon(item: any): string {
    const icon = item?.bullet_img?.icon;
    return (!icon || icon.trim() === '') ? '' : icon;
  }



}
