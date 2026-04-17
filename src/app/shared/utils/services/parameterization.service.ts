import { HttpClient } from '@angular/common/http';
import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ClientCredentialEntry, GetConsultProcessResponse, GetTokenResponse, SaveTransactionRequest,Conditional } from '@utils/models/parameterization.interface';
import { endpoint } from 'src/environments/environment.api_url';
import { isPlatformBrowser } from '@angular/common';
import { GetResponse } from '@utils/models/utils.interface';


@Injectable({
  providedIn: 'root'
})
export class ParameterizationService {
  private platformId = inject<Object>(PLATFORM_ID);
  private http = inject(HttpClient);


  public QueryConsultGlobalConfiguration(procesoConvenioGuid: string): Observable<GetResponse> {
    return this.http.get<GetResponse>(`${endpoint.CONSULTAR_CONFIGURACION_GLOBAL}?procesoConvenioGuid=${procesoConvenioGuid}`)
  }

  public QueryBringToken(ClientCredentialEntry: ClientCredentialEntry): Observable<GetTokenResponse> {
    if (isPlatformBrowser(this.platformId)) {
      return this.http.post<GetTokenResponse>(endpoint.TRAER_TOKEN, ClientCredentialEntry)
    }
    else {
      return of();
    }
  }

  public QueryATDP(versionId: number): Observable<GetResponse> {
    return this.http.get<GetResponse>(`${endpoint.CONSULTAR_ATDP}/${versionId}`)
  }

  public QuerySaveATDPTransaction(SaveTransactionRequest: SaveTransactionRequest): Observable<GetResponse> {
    return this.http.post<GetResponse>(`${endpoint.GUARDAR_TRANSACCION_ATDP}`, SaveTransactionRequest)
  }

  public QueryAgreementProcess(Guid: string): Observable<GetConsultProcessResponse> {
    return this.http.post<GetConsultProcessResponse>(`${endpoint.CONSULTAR_PROCESO_CONVENIO}?Guid=${Guid}`, null)
  }

  public QueryGetProcessFlow(workFlowId: number, agreementId: string, typeCurrent: string,Conditional: Conditional): Observable<GetResponse> {
    return this.http.post<GetResponse>(`${endpoint.CONSULTAR_FLUJO_DE_PROCESO}?workFlowId=${workFlowId}&agreementId=${agreementId}&typeCurrent=${typeCurrent}`,Conditional)
  }
}
