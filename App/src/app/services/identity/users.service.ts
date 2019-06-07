import { Injectable, Inject } from '@angular/core';
import { HttpBaseService } from '../http-base.service';
import { GlobalService } from '../../infrastructure/global.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, tap, distinctUntilChanged } from 'rxjs/operators';
import { AppUserModel } from '../../models/appUserModel';

@Injectable({
  providedIn: 'root'
})
export class UsersService extends HttpBaseService {

  protected controllerName(): string {
    return 'Users/';
  }

  getUsersList(): Observable<AppUserModel[]> {
    return this.http.get<AppUserModel[]>(this.apiUrl + 'GetUsersList').pipe(
      distinctUntilChanged(),
      tap(_ => {}),
      catchError(this.handleError<AppUserModel[]>('getUsersList'))
    );
  }

  removeUser(id: string): Observable<{Success: boolean, Error: string}> {
    return this.http.delete<{Success: boolean, Error: string}>(this.apiUrl + `RemoveUser/${id}`, this.httpOptions).pipe(
      tap(_ => {}),
      catchError(this.handleError<{Success: boolean, Error: string}>('removeUser'))
    );
  }

  constructor(@Inject(GlobalService) global: GlobalService, @Inject(HttpClient) http: HttpClient) {
    super(global, http);
  }
}
