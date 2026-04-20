import { Observable, throwError, from } from 'rxjs';
import { HttpErrorResponse, HttpEvent, HttpInterceptorFn, HttpStatusCode } from '@angular/common/http';
import { catchError, finalize, switchMap } from 'rxjs/operators';
import { inject } from '@angular/core';
import { AlertMessageService } from '@utils/services/alert.service';
import { LocalStorageService } from 'ngx-localstorage';
import { environment } from 'src/environments/environment';
import { Pages, ResponseMessage, State, UtilsService } from '@utils/services/utils.service';
import { EsqueletonService } from '@utils/services/esqueleton.service';
import { ParameterizationConsumeService } from '@utils/services/parameterization-consume.service';
import { endpoint } from 'src/environments/environment.api_url';
import { StateService } from '@utils/services/state.service';
import { Router } from '@angular/router';

export const TokenInterceptorFunction: HttpInterceptorFn = (req, next): Observable<HttpEvent<any>> => {
  const alertService = inject(AlertMessageService);
  const storageService = inject(LocalStorageService);
  const esqueleton = inject(EsqueletonService);
  const utils = inject(UtilsService);
  const stateService = inject(StateService);
  const router = inject(Router);
  const parameterizationService = inject(ParameterizationConsumeService);
  esqueleton.startRequest();
  if (req.url.includes(endpoint.TRAER_TOKEN)) {
    const modifiedReq = req.clone({ url: `${environment.API_URL}${req.url}` });
    return next(modifiedReq).pipe(finalize(() => esqueleton.endRequest()));
  }
  return from(parameterizationService.startTokenValidation()).pipe(
    switchMap((isValid: boolean) => {
      if (!isValid) {
        return throwError(() => new Error('Sesión expirada'));
      }
      const id_token = utils.decrypt<string>(storageService.get('id_token'));
      const modifiedReq = req.clone({
        setHeaders: { Authorization: `Bearer ${id_token!}` },
        url: `${environment.API_URL}${req.url}`,
      });
      return next(modifiedReq).pipe(
        catchError((error: HttpErrorResponse) => {
          switch (error.status) {
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.RequestTimeout:
              alertService.Warning(error.error);
              break;
            case HttpStatusCode.TooManyRequests:
              alertService.Warning(ResponseMessage.TOO_MANY_REQUESTS);
              break;
            case HttpStatusCode.InternalServerError:
              alertService.Error(error.error?.message || ResponseMessage.INTERNAL_SERVER_ERROR);
              break;
            case HttpStatusCode.Unauthorized:
               router.navigate([`/${Pages.flowcompletion}`, stateService.guidPro()], { queryParams: { state: State.end } });
              break;
            default:
              alertService.Error(`${ResponseMessage.INTERNAL_SERVER_ERROR}: ${error.status}`);
              break;
          }
          return throwError(() => error);
        }),
        finalize(() => esqueleton.endRequest())
      );
    })
  );
};
