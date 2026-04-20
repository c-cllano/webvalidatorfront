import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { GetResponse } from '@utils/models/utils.interface';
import { Observable } from 'rxjs';
import { endpoint } from 'src/environments/environment.api_url';
import { map } from 'rxjs/operators';
import { ValidateBiometric } from '@liveness/models/validation-biometric.interface';
import { ValidateBiometricResponse } from '@liveness/models/validate-biometric-response.interface';
import { BiometricRequest } from '@liveness/models/biometric-request.interface';
import { saveBiometricResponse } from '@liveness/models/save-biometric.response.interface';
@Injectable({
  providedIn: 'root'
})
export class LivenessBiometricService {

  private http = inject(HttpClient);

  QueryCheckActivePages(procesoConvenioGuid: string, PageName: string): Observable<GetResponse> {
    return this.http.get<GetResponse>(`${endpoint.CONSULTAR_PAGINAS_ACTIVAS}?procesoConvenioGuid=${procesoConvenioGuid}&PageName=${PageName}`)
  }

  sentValidationBiometric(payload: ValidateBiometric): Observable<ValidateBiometricResponse> {
    return this.http.post<any>(`${endpoint.VALIDA_BIOMETRIA}`, payload).pipe(
      map(response => {
        console.log('validacion de biometria response:', response);
        return response;
      })
    );
  }

  saveBiometric(payload: BiometricRequest): Observable<saveBiometricResponse> {
    return this.http.post<any>(`${endpoint.SAVE_BIOMETRIC}`, payload).pipe(
      map(response => {
        console.log('guardo de infomracion de biometria:', response);
        return response;
      })
    );
  }
}
