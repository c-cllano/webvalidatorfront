import { Component, computed, inject, OnDestroy, output, signal } from '@angular/core';
import { CheckboxComponent } from "@components/controls/checkbox/checkbox.component";
import { CommonModule } from '@angular/common';
import { StateService } from '@utils/services/state.service';
import { UtilsService } from '@utils/services/utils.service';
import { ParameterizationConsumeService } from '@utils/services/parameterization-consume.service';
import { TrackEventService } from '@utils/services/trackevent.service';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { ParameterizationService } from '@utils/services/parameterization.service';
import { Subject, takeUntil } from 'rxjs';
import { AlertMessageService } from '@utils/services/alert.service';
import { GetResponse } from '@utils/models/utils.interface';
import { ModalComponent } from "@components/controls/modal/modal.component";
import { FormsModule } from '@angular/forms';
import { LocalStorageService } from "ngx-localstorage";
import { SubtitleDirective } from '@directives/subtitle.directive';
import { HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-atdp',
  imports: [CheckboxComponent, CommonModule, ModalComponent, FormsModule, SubtitleDirective],
  templateUrl: './atdp.component.html',
  styleUrl: './atdp.component.css'
})
export class AtdpComponent implements OnDestroy {
  private destroy$ = new Subject<any>();
  private stateService = inject(StateService);
  private utilsService = inject(UtilsService);
  private ParameterizationConsumeService = inject(ParameterizationConsumeService);
  private trafico = inject(TrackEventService);
  private parameterizationService = inject(ParameterizationService);
  private Alert = inject(AlertMessageService);
  private storageService = inject(LocalStorageService);
  public ishtmlModal = signal<boolean>(false);
  public htmlModal = signal<string>("");
  public modalConfig = signal<any | null>(null);
  public showModal = signal<boolean>(false);

  constructor() {
    this.restoreFromStorage();
  }


  ngOnDestroy(): void {
    this.destroy$.next({});
    this.destroy$.complete();
  }


  eventHandlers: { [key: string]: () => void } = {
    handleATDP: () => {
      this.GetQueryATDP(1);
      this.track('Llamado ATDP.');
    },
  };


  public atdpText = computed(() => {
    return this.stateService.globalStyles()?.atdp?.text || '';
  });

  public atdpTextLink = computed(() => {
    return this.stateService.globalStyles()?.atdp?.textLink || '';
  });


  public atdpClickEvent = computed(() => {
    return this.utilsService.behavior()?.atdp?.button.click_event;
  });


  public executeClickEvent(eventName: string | undefined) {
    const cleanName = eventName?.replace('()', '').trim();
    if (cleanName && this.eventHandlers[cleanName]) {
      this.eventHandlers[cleanName]();
    } else {
      this.Alert.Warning(`Método "${cleanName}" no está en functionMap`);
    }
  }

  public eventChecked(value: boolean) {
    this.utilsService.setEventCheckedAtdp(value);
    this.SaveEventCheckedAtdp(value);
  }

  public atdpDisplay = computed(() => {
    return this.utilsService.behavior()?.atdp?.display || false;
  });

  public eventCheckedAtdp = computed(() => {
    return this.utilsService?.eventCheckedAtdp() || false;
  });

  private SaveEventCheckedAtdp(value: boolean): void {
    this.storageService.set('EventCheckedAtdp', this.utilsService.encrypt(value.toString()!));
  }

  private restoreFromStorage(): void {
    this.utilsService.setEventCheckedAtdp(this.utilsService.decrypt<boolean>(this.storageService.get('EventCheckedAtdp'))!);
  }


  private GetQueryATDP(versionId: number): void {
    this.parameterizationService
      .QueryATDP(versionId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result: GetResponse) => {
          if (result.code == HttpStatusCode.Ok) {
            if (result.data.fileContent != null) {
              this.htmlModal.set(result.data.fileContent);
              this.ishtmlModal.set(true);
              this.track('Se cargo la ATDP.');
            } else {
              this.track('Validacion configuracion : No hay ATDP configurada.');
              this.Alert.Warning("No hay ATDP configurada.");
            }
          } else {
            this.Alert.Warning("No hay ATDP configurada.");
            this.track('Validacion configuracion : No hay ATDP configurada.');
          }
          this.showModal.set(true);
        }
      });
  }

  private track(action: string): void {
    this.trafico.RegisterEvent(<Partial<ActionEvent>>{
      componente: 'AtdpComponent',
      accion: action,
    });
  }


}
