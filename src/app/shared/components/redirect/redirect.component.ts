import { Component, inject, input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ParameterizationConsumeService } from '@utils/services/parameterization-consume.service';
import { Pages, State } from "@utils/services/utils.service";
import { StateService } from '@utils/services/state.service';
import { LocalStorageService } from "ngx-localstorage";
import { NgxSpinnerService } from 'ngx-spinner';
import { FlowNavigatorService } from '@utils/services/flownavigator.service';

@Component({
  selector: 'app-redirect',
  imports: [],
  templateUrl: './redirect.component.html',
  styleUrl: './redirect.component.css'
})
export class RedirectComponent implements OnInit {

  readonly Guid = input<string>("");
  private parameterizationConsumeService = inject(ParameterizationConsumeService);
  private router = inject(Router);
  public stateService = inject(StateService);
  private storageService = inject(LocalStorageService);
  private spinner = inject(NgxSpinnerService);
  private flowNavigatorService = inject(FlowNavigatorService);
  

  async ngOnInit(): Promise<void> {
    this.navigateWithState(State.start);
    this.stateService.setStateSignal(State.start);
    this.spinner.show();
    if (!(await this.parameterizationConsumeService.ValidateToken())) {
      return this.stopSpinner();
    }
    const datos = await this.parameterizationConsumeService.LoadAgreementProcess(this.Guid());
    if (!datos) {
      return this.stopSpinner();
    }
    this.stateService.setProcessSignal(datos);
    const config = await this.parameterizationConsumeService
      .LoadConsultGlobalConfiguration(datos.data.datos.guidConv);
    if (!config) {
      return this.stopSpinner();
    }
    this.stateService.setGuidConSignal(datos.data.datos.guidConv);
    this.stateService.setGuidProSignal(this.Guid());
    const next = await this.flowNavigatorService.init();
    if (!next) {
      return this.stopSpinner();
    }
    this.cleanupStorage();
    this.stopSpinner();
    this.router.navigate([`/${next}`, this.Guid()], { queryParams: { state: State.process } });
  }

  private navigateWithState(state: State): void {
    this.router.navigate([`/${Pages.redirect}`, this.Guid()], {queryParams: { state },queryParamsHandling: 'merge',});
  }

  private cleanupStorage(): void {
    this.storageService.remove('EventCheckedAtdp');
    this.storageService.remove('CurrentIndex');
    this.storageService.remove('PwaVisible');
    
  }

  private stopSpinner(): void {
    this.spinner.hide();
  }

}
