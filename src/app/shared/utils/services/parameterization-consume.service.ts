import { inject, Injectable, OnDestroy, signal } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ParameterizationService } from './parameterization.service';
import { LocalStorageService } from 'ngx-localstorage';
import { AlertMessageService } from '@utils/services/alert.service';
import { ClientCredentialEntry, Conditional, GetConsultProcessResponse, GetTokenResponse } from '@utils/models/parameterization.interface';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Pages, State, UtilsService } from '@utils/services/utils.service';
import { StateService } from '@utils/services/state.service';
import { GetResponse } from '@utils/models/utils.interface';
import { Router } from '@angular/router';
import { FontLoaderService } from '@utils/services/font-loader.service';
import { HttpStatusCode } from '@angular/common/http';



const helper = new JwtHelperService();

@Injectable({
  providedIn: 'root'
})
export class ParameterizationConsumeService implements OnDestroy {
  private destroy$ = new Subject<any>();
  private parameterizationService = inject(ParameterizationService);
  private storageService = inject(LocalStorageService);
  private Alert = inject(AlertMessageService);
  public stateService = inject(StateService);
  private router = inject(Router);
  private fontLoaderService = inject(FontLoaderService);
  private utils = inject(UtilsService);
  private validatingToken = signal<boolean>(false)
  private validationPromise?: Promise<boolean>;

  ngOnDestroy(): void {
    this.destroy$.next({});
    this.destroy$.complete();
  }



  private LoadBringToken() {
    return new Promise((resolve) => {
      const ClientCredentialEntry: ClientCredentialEntry = {
        grant_type: "password",
        client_id: "58b6dc96-aa9f-4db8-bf51-897cbefd4f83",
        username: "felix.pardo@olimpiait.com",
        password: "Olimpiait-5"
      };
      this.parameterizationService
        .QueryBringToken(ClientCredentialEntry)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (resul: GetTokenResponse) => {
            if (resul.id_token != null) {
              this.SaveId_token(resul.id_token);
              resolve(true);
            } else {
              this.Alert.Error('Error al cargar el token de acceso.');
              resolve(false);
            }
          }
        });
    });
  }




  public LoadConsultGlobalConfiguration(procesoConvenioGuid: string): Promise<boolean> {
    return new Promise((resolve) => {
      this.parameterizationService
        .QueryConsultGlobalConfiguration(procesoConvenioGuid)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (resul: GetResponse) => {
            if (resul.code === HttpStatusCode.Ok) {
              if (resul.data != null) {
                resolve(true);
                this.stateService.setGlobalStyles(resul?.data);
                this.fontLoaderService.loadFont(resul?.data?.typography['font-family'] || 'Poppins');
              } else {
                this.Alert.Warning('No hay una configuración de interfaz establecida en los parámetros globales.');
                resolve(false);
              }
            } else {
              resolve(false);
              this.Alert.Warning('No hay una configuración de interfaz establecida en los parámetros globales.');
            }

          }
        });
    });
  }




  public ValidateToken(): Promise<boolean> {
    return new Promise((resolve) => {
      const id_token = this.utils.decrypt<string>(this.storageService.get('id_token'));
      const ExpiredToken = helper.isTokenExpired(id_token!);
      if (ExpiredToken) {
        this.LoadBringToken().then((Result: any) => {
          resolve(!!Result);
        });
      } else {
        resolve(true);
      }
    });
  }



  public startTokenValidation(): Promise<boolean> {
    if (this.validatingToken() && this.validationPromise) {
      return this.validationPromise;
    }
    this.validationPromise = new Promise((resolve) => {
      const id_token = this.utils.decrypt<string>(this.storageService.get('id_token'));
      const ExpiredToken = helper.isTokenExpired(id_token!);
      if (ExpiredToken) {
        this.validatingToken.set(true);
        this.Alert.Confirm('Su sesión ha caducado. ¿Desea renovarla?').then(
          (result: any) => {
            if (result?.isConfirmed) {
              this.LoadBringToken().then((Result: any) => {
                this.validatingToken.set(false);
                this.validationPromise = undefined;
                resolve(!!Result);
              });
            } else {
              this.router.navigate([`/${Pages.flowcompletion}`, this.stateService.guidPro()], { queryParams: { state: State.end } });
              this.validatingToken.set(false);
              this.validationPromise = undefined;
              resolve(false);
            }
          }
        );
      } else {
        resolve(true);
      }
    });
    return this.validationPromise;
  }





  public LoadAgreementProcess(Guid: string): Promise<GetConsultProcessResponse | null> {
    return new Promise((resolve) => {
      this.parameterizationService
        .QueryAgreementProcess(Guid)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (resul: GetConsultProcessResponse) => {
            if (resul.code === HttpStatusCode.Ok) {
              this.stateService.setProcessSignal(resul);
              resolve(resul);
            } else {
              resolve(null);
            }
          }
        });
    });
  }


  public LoadGetProcessFlow(workFlowId: number, agreementId: string, typeCurrent: string, Conditional: Conditional): Promise<GetResponse | null> {
    return new Promise((resolve) => {
      this.parameterizationService
        .QueryGetProcessFlow(workFlowId, agreementId, typeCurrent, Conditional)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (resul: GetResponse) => {
            if (resul.code === HttpStatusCode.Ok) {
              if (resul.data != null) {
                resolve(resul);
              } else {
                this.Alert.Warning('No hay una configuración de interfaz establecida.');
                resolve(null);
              }
            } else {
              this.Alert.Warning('No hay una configuración de interfaz establecida.');
              resolve(null);
            }
          }
        });
    });
  }

  private SaveId_token(value: string): void {
    this.storageService.set('id_token', this.utils.encrypt(value!));
  }


}
