import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { GetResponse } from '@utils/models/utils.interface';
import { Observable } from 'rxjs';
import { endpoint } from 'src/environments/environment.api_url';

@Injectable({
  providedIn: 'root'
})
export class PermitdeniedService {

 private http = inject(HttpClient);

   QueryCheckActivePages(procesoConvenioGuid: string, PageName: string): Observable<GetResponse> {
    return this.http.get<GetResponse>(`${endpoint.CONSULTAR_PAGINAS_ACTIVAS}?procesoConvenioGuid=${procesoConvenioGuid}&PageName=${PageName}`)
  }
}
