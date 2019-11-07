import { Injectable, Inject } from '@angular/core';
import { HttpBaseService } from '../http-base.service';
import { GlobalService } from 'src/app/infrastructure/global.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { SystemLogEntry, SystemLogEntriesPackage } from 'src/app/models/systemLogEntry';

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

  getNextEntries(descriptor: string, lastEntryId: string, count: number): Observable<SystemLogEntriesPackage> {
    return this.http.get<SystemLogEntriesPackage>(this.apiUrl + `GetNextEntries?descriptor=${descriptor}&lastEntryId=${lastEntryId === null ? '' : lastEntryId}&count=${count}`).pipe(
      tap((res: SystemLogEntriesPackage) => {}),
      catchError(this.handleError<SystemLogEntriesPackage>('getNextEntries'))
    );
  }

  clean(descriptor: string, secondsThreshold: number, prevLoadedCount: number): Observable<SystemLogEntriesPackage> {
    return this.http.post<SystemLogEntriesPackage>(this.apiUrl + `Clean?descriptor=${descriptor}&secondsThreshold=${secondsThreshold}&prevLoadedCount=${prevLoadedCount}`, null).pipe(
      tap((res: SystemLogEntriesPackage) => {}),
      catchError(this.handleError<SystemLogEntriesPackage>('clean'))
    );
  }

  constructor(
    @Inject(GlobalService) global: GlobalService, 
    @Inject(HttpClient) http: HttpClient) {
    super(global, http);
  }
}
