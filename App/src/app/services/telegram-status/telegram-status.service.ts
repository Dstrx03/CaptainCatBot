import { Injectable, Inject } from '@angular/core';
import { HttpBaseService } from '../http-base.service';
import { GlobalService } from 'src/app/infrastructure/global.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TelegramStatusViewModel } from 'src/app/models/telegramStatusViewModel';
import { tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TelegramStatusService extends HttpBaseService {

  protected controllerName(): string {
    return 'TelegramStatus/';
  }

  getTelegramStatusInfo(): Observable<TelegramStatusViewModel> {
    return this.http.get<TelegramStatusViewModel>(this.apiUrl + 'GetTelegramStatusInfo').pipe(
      tap((res: TelegramStatusViewModel) => {}),
      catchError(this.handleError<TelegramStatusViewModel>('getTelegramStatusInfo'))
    );
  }

  registerClient(): Observable<any> {
    return this.http.post<any>(this.apiUrl + 'RegisterClient', null).pipe(
      tap((res: any) => {}),
      catchError(this.handleError<any>('registerClient'))
    );
  }

  unregisterClient(): Observable<any> {
    return this.http.post<any>(this.apiUrl + 'UnregisterClient', null).pipe(
      tap((res: any) => {}),
      catchError(this.handleError<any>('unregisterClient'))
    );
  }

  registerWebhook(): Observable<any> {
    return this.http.post<any>(this.apiUrl + 'RegisterWebhook', null).pipe(
      tap((res: any) => {}),
      catchError(this.handleError<any>('registerWebhook'))
    );
  }

  unregisterWebhook(): Observable<any> {
    return this.http.post<any>(this.apiUrl + 'UnregisterWebhook', null).pipe(
      tap((res: any) => {}),
      catchError(this.handleError<any>('unregisterWebhook'))
    );
  }

  checkWebhook(): Observable<any> {
    return this.http.post<any>(this.apiUrl + 'CheckWebhook', null).pipe(
      tap((res: any) => {}),
      catchError(this.handleError<any>('checkWebhook'))
    );
  }

  updateWebhook(): Observable<any> {
    return this.http.post<any>(this.apiUrl + 'UpdateWebhook', null).pipe(
      tap((res: any) => {}),
      catchError(this.handleError<any>('updateWebhook'))
    );
  }

  constructor(
    @Inject(GlobalService) global: GlobalService, 
    @Inject(HttpClient) http: HttpClient) {
    super(global, http);
  }
}
