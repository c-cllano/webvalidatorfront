import { Component, computed, inject, OnDestroy, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { Subject } from 'rxjs';
import { ModalComponent } from '@components/controls/modal/modal.component';
import { ProgressBarComponent } from '@components/controls/progress-bar/progress-bar.component';
import { StateService } from '@utils/services/state.service';
import { Router } from '@angular/router';
import { ConfirmDialogComponent } from '@components/controls/confirm-dialog/confirm-dialog.component';
import { TrackEventService } from '@utils/services/trackevent.service';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { AlertMessageService } from '@utils/services/alert.service';
import { Pages, State, UtilsService } from '@utils/services/utils.service';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ParameterizationConsumeService } from '@utils/services/parameterization-consume.service';
import { FlowNavigatorService } from '@utils/services/flownavigator.service';

@Component({
  selector: 'app-header',
  imports: [
    ModalComponent,
    ProgressBarComponent,
    ConfirmDialogComponent,
    CommonModule
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<any>();
  public showModal = signal<boolean>(false);
  public confirmVisible = signal<boolean>(false);
  private stateService = inject(StateService);
  private router = inject(Router);
  private Alert = inject(AlertMessageService);
  private utilsService = inject(UtilsService);
  private trafico = inject(TrackEventService);
  private flowNavigatorService = inject(FlowNavigatorService);
  private ParameterizationConsumeService = inject(ParameterizationConsumeService);
  public iconFontLoaded = signal<boolean>(false);
  private platformId = inject<Object>(PLATFORM_ID);

  eventHandlers: { [key: string]: () => void } = {
    handleInfo: () => {
      this.track(`Boton info`);
      this.showModal.set(true);
    },
    handleBack: async () => {
      this.track(`Boton volver`);
      const Next = await this.flowNavigatorService.goBack();
      if (Next) {
        this.router.navigate([`/${Next}`, this.stateService.guidPro()], { queryParams: { state: State.process } });
      }
    },
    handleCancel: () => {
      this.track(`Boton Cancelar`);
      this.confirmVisible.set(true);
    }
  };

  ngOnInit(): void {
    this.track(`Carga componente header`);
    if (isPlatformBrowser(this.platformId)) {
      document.fonts.load('1em "Material Symbols Rounded"').then(() => {
        this.iconFontLoaded.set(true);
      });
    }

  }

  ngOnDestroy(): void {
    this.destroy$.next({});
    this.destroy$.complete();
  }

  public getIconFontLoaded = computed(() => {
    return this.iconFontLoaded() || false
  });

  public getProgressbarDisplay = computed(() => {
    return this.stateService.globalStyles()?.progressbar?.display || false
  });

  public modalDisplay = computed(() => {
    return this.modalConfig()?.display || false;
  });


  public modalConfig = computed(() => {
    return this.utilsService?.header()?.right?.modal || {};
  });

  public configData = computed(() => {
    return this.utilsService?.header()?.left || {};
  });

  public getColorIcons = computed(() => {
    return this.stateService.globalStyles()?.logo_cons_native?.colorIcons || '#349BD0';
  });

  public getLeftDisplay = computed(() => {
    return this.utilsService?.header()?.left?.display || false
  });

  public getCenterDisplay = computed(() => {
    return this.utilsService?.header()?.center?.display || false
  });

  public getRightDisplay = computed(() => {
    return this.utilsService?.header()?.right?.display || false
  });

  public getLeftType = computed(() => {
    return this.utilsService?.header()?.left?.content?.type === 'icon' ? true : false;
  });

  public getCenterType = computed(() => {
    return this.utilsService?.header()?.center?.content?.type === 'icon' ? true : false;
  });

  public getRightType = computed(() => {
    return this.utilsService?.header()?.right?.content?.type === 'icon' ? true : false;
  });

  public getLeftIsFilled = computed(() => {
    return this.utilsService?.header()?.left?.content?.isFilled || false
  });


  public getCenterIsFilled = computed(() => {
    return this.utilsService?.header()?.center?.content?.isFilled || false
  });


  public getRightIsFilled = computed(() => {
    return this.utilsService?.header()?.right?.content?.isFilled || false
  });

  public getLeftName = computed(() => {
    return this.utilsService?.header()?.left?.content?.name || '';
  });

  public getCenterName = computed(() => {
    return this.stateService.globalStyles()?.logo_cons_native?.logo.name || '';
  });

  public getRightName = computed(() => {
    return this.utilsService?.header()?.right?.content?.name || '';
  });


  public getLeftClick_event = computed(() => {
    return this.utilsService?.header()?.left?.click_event || '';
  });

  public getRightClick_event = computed(() => {
    return this.utilsService?.header()?.right?.click_event || '';
  });


  executeClickEvent(eventName: string | undefined) {
    const cleanName = eventName?.replace('()', '').trim();
    if (cleanName && this.eventHandlers[cleanName]) {
      this.eventHandlers[cleanName]();
    } else {
      this.Alert.Warning(`No se encontró ninguna acción para el evento: ${cleanName}`);
      this.track(`No se encontró ninguna acción para el evento: ${cleanName}`);
    }
  }

  onAccept() {
    this.confirmVisible.set(false);
    this.router.navigate([`/${Pages.flowcompletion}`, this.stateService.guidPro()], { queryParams: { state: State.end } });
    this.track('redireccion a flow-completion');
  }

  onCancel() {
    this.confirmVisible.set(false);
  }

  private track(action: string): void {
    this.trafico.RegisterEvent(<Partial<ActionEvent>>{
      componente: 'HeaderComponent',
      accion: action,
    });
  }
}
