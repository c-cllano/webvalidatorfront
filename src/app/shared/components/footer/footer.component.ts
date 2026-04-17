import { AfterViewInit, Component, computed, inject, input, OnDestroy, OnInit, signal } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ParameterizationService } from '@utils/services/parameterization.service';
import { AlertMessageService } from '@utils/services/alert.service';
import { ButtonComponent } from '@components/controls/button/button.component';
import { TypeButton, UtilsService } from '@utils/services/utils.service';
import { TrackEventService } from '@utils/services/trackevent.service';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { GetResponse } from '@utils/models/utils.interface';
import { SaveTransactionRequest } from '@utils/models/parameterization.interface';
import { CommonModule } from '@angular/common';
import { AtdpComponent } from '@components/controls/atdp/atdp.component';
import { EsqueletonService } from '@utils/services/esqueleton.service';
import { HttpStatusCode } from '@angular/common/http';
import { StateService } from '@utils/services/state.service';

@Component({
  selector: 'app-footer',
  imports: [ButtonComponent, CommonModule, AtdpComponent],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent implements OnInit, OnDestroy, AfterViewInit {

  public titleModal = signal<string>("");
  public descriptionModal = signal<string>("");
  public DataulnModal = signal<any[]>([]);
  public Button_main = signal<TypeButton>(TypeButton.button_main);
  public Button_secondary = signal<TypeButton>(TypeButton.button_secondary);
  private destroy$ = new Subject<any>();
  public functionMap = input.required<Record<string, () => void>>();
  private Alert = inject(AlertMessageService);
  private parameterizationService = inject(ParameterizationService);
  private utilsService = inject(UtilsService);
  public Esqueleton = inject(EsqueletonService);
  readonly screenWidth = signal(0);
  readonly isDesktop = computed(() => this.screenWidth() >= 1024);
  private trafico = inject(TrackEventService);
  public stateService = inject(StateService);


  ngOnInit(): void {
    this.track('Carga componente footer');
  }

  ngOnDestroy(): void {
    this.destroy$.next({});
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    this.screenWidth.set(window.innerWidth);
    window.addEventListener('resize', () => {
      this.screenWidth.set(window.innerWidth);
      if (!this.isDesktop()) {
        this.Esqueleton.endRequest()
      }
    });
  }

  public atdpDisplay = computed(() => {
    return this.utilsService.behavior()?.atdp?.display || false;
  });

  public eventCheckedAtdp = computed(() => {
    return this.utilsService?.eventCheckedAtdp() || false;
  });

  public invertButtonPosition = computed(() => {
    return this.utilsService.behavior()?.reverseOrder || false;
  });


  public getConfigButton_main = computed(() => {
    return this.utilsService.behavior()?.button_main || {}
  });

  public getConfigButton_secondary = computed(() => {
    return this.utilsService.behavior()?.button_secondary || {}
  });

  public getConfigButton_main_Display = computed(() => {
    return this.utilsService.behavior()?.button_main?.display || false
  });

  public getConfigButton_secondary_Display = computed(() => {
    return this.utilsService.behavior()?.button_secondary?.display || false
  });

  public DisplayButton = computed(() => {
    return this.utilsService.displayButton() || false;
  });

  public SaveATDPTransaction(): void {
    if (this.atdpDisplay() && this.eventCheckedAtdp()) {
      const SaveTransactionRequest: SaveTransactionRequest = {
        atdpVersionID: 1,
        documentTypeID: 1,
        documentNumber: this.stateService.process()?.data?.datos?.numDoc || "0",
        transactionID: "", // esta valor en opcional 
        commerce: "NextGenID Facial",
        firstName: this.stateService.process()?.data?.datos?.primerNombre || "null",
        secondName: this.stateService.process()?.data?.datos?.segundoNombre || "null", // esta valor en opcional 
        firstLastName: this.stateService.process()?.data?.datos?.primerApellido || "null",
        secondLastName: this.stateService.process()?.data?.datos?.segundoApellido || "null",
        email: this.stateService.process()?.data?.datos?.email || "null",
        file: "",// esta valor en opcional 
        signature: false,
        date: new Date().toISOString(),
        isApproved:false
      }
      
      this.parameterizationService
        .QuerySaveATDPTransaction(SaveTransactionRequest)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (result: GetResponse) => {
            if (result.code == HttpStatusCode.Ok) {
              if (result.data.atdpTransactionID != null) {
                this.track('Se guardó la transacción de la ATDP.');
              } else {
                this.Alert.Warning("No se guardó la transacción de la ATDP.");
                this.track('No se guardó la transacción de la ATDP.');
              }
            } else {
              this.Alert.Warning("No se guardó la transacción de la ATDP.");
              this.track('No se guardó la transacción de la ATDP.');
            }
          }
        });
    }

  }

  private track(action: string): void {
    this.trafico.RegisterEvent(<Partial<ActionEvent>>{
      componente: 'FooterComponent',
      accion: action,
    });
  }


}