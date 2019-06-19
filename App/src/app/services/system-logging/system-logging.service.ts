import { Injectable, Inject } from '@angular/core';
import { HttpBaseService } from '../http-base.service';
import { GlobalService } from 'src/app/infrastructure/global.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { SystemLogEntry } from 'src/app/models/systemLogEntry';

@Injectable({
  providedIn: 'root'
})
export class SystemLoggingService extends HttpBaseService {

  protected controllerName(): string {
    return 'SystemLogging/';
  }

  getEntries(descriptor: string): Observable<SystemLogEntry[]> {
    return this.http.get<SystemLogEntry[]>(this.apiUrl + `GetEntries?descriptor=${descriptor}`).pipe(
      tap((res: SystemLogEntry[]) => {}),
      catchError(this.handleError<SystemLogEntry[]>('getEntries'))
    );
  }

  clean(descriptor: string, secondsThreshold: number): Observable<SystemLogEntry[]> {
    return this.http.post<SystemLogEntry[]>(this.apiUrl + `Clean?descriptor=${descriptor}&secondsThreshold=${secondsThreshold}`, null).pipe(
      tap((res: SystemLogEntry[]) => {}),
      catchError(this.handleError<SystemLogEntry[]>('clean'))
    );
  }

  constructor(
    @Inject(GlobalService) global: GlobalService, 
    @Inject(HttpClient) http: HttpClient) {
    super(global, http);
  }
}
