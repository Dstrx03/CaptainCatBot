import { Injectable, Inject } from '@angular/core';
import { HttpBaseService } from '../http-base.service';
import { GlobalService } from 'src/app/infrastructure/global.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { RefresherSettings } from 'src/app/models/RefresherSettings';
import { ATriggerSettings } from 'src/app/models/atriggerSettings';

@Injectable({
  providedIn: 'root'
})
export class InternalServicesService extends HttpBaseService  {

  protected controllerName(): string {
    return 'InternalServices/';
  }

  refresherGetSettings(): Observable<RefresherSettings> {
    return this.http.get<RefresherSettings>(this.apiUrl + 'RefresherGetSettings').pipe(
      tap((res: RefresherSettings) => {}),
      catchError(this.handleError<RefresherSettings>('refresherGetSettings'))
    );
  }

  refresherSaveSettings(settings: RefresherSettings): Observable<RefresherSettings> {
    return this.http.post<RefresherSettings>(this.apiUrl + 'RefresherSaveSettings', settings, this.httpOptions).pipe(
      tap((res: RefresherSettings) => {}),
      catchError(this.handleError<RefresherSettings>('refresherSaveSettings'))
    );
  }

  atriggerGetSettings(): Observable<ATriggerSettings> {
    return this.http.get<ATriggerSettings>(this.apiUrl + 'ATriggerGetSettings').pipe(
      tap((res: ATriggerSettings) => {}),
      catchError(this.handleError<ATriggerSettings>('atriggerGetSettings'))
    );
  }

  atriggerSaveSettings(settings: ATriggerSettings): Observable<ATriggerSettings> {
    return this.http.post<ATriggerSettings>(this.apiUrl + 'ATriggerSaveSettings', settings, this.httpOptions).pipe(
      tap((res: ATriggerSettings) => {}),
      catchError(this.handleError<ATriggerSettings>('atriggerSaveSettings'))
    );
  }

  constructor(
    @Inject(GlobalService) global: GlobalService, 
    @Inject(HttpClient) http: HttpClient) {
    super(global, http);
  }
}
