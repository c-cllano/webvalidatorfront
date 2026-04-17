import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { GetResponse } from '@utils/models/utils.interface';
import { Observable } from 'rxjs';
import { endpoint } from 'src/environments/environment.api_url';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UploadDocService {

  private http = inject(HttpClient);

  QueryCheckActivePages(procesoConvenioGuid: string, PageName: string): Observable<GetResponse> {
    return this.http.get<GetResponse>(`${endpoint.CONSULTAR_PAGINAS_ACTIVAS}?procesoConvenioGuid=${procesoConvenioGuid}&PageName=${PageName}`)
  }

  saveBothSidesDocument(payload: any): Observable<any> {
    return this.http.post<any>(`${endpoint.GUARDAR_DOCUMENTO_AMBAS_CARAS}`, payload).pipe(
      map(response => {
        return response;
      })
    );
  }
}
