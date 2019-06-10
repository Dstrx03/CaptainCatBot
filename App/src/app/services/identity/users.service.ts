import { Injectable, Inject } from '@angular/core';
import { HttpBaseService } from '../http-base.service';
import { GlobalService } from '../../infrastructure/global.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, tap, distinctUntilChanged } from 'rxjs/operators';
import { AppUserModel } from '../../models/appUserModel';
import { CatProcedureResult } from '../../infrastructure/webApi/catProcedureResult';

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

  getUser(id: string): Observable<AppUserModel> {
    return this.http.get<AppUserModel>(this.apiUrl + `GetUser/${id}`).pipe(
      tap(_ => {}),
      catchError(this.handleError<AppUserModel>('getUser'))
    );
  }

  addUser(user: AppUserModel, errorMsgs?: string[]): Observable<any> {
    return this.http.post<any>(this.apiUrl + 'AddUser', user, this.httpOptions).pipe(
      tap(_ => {}),
      catchError(this.handleErrorProcedureResult(errorMsgs, 'addUser'))
    );
  }

  updateUser(user: AppUserModel, errorMsgs?: string[]): Observable<any> {
    return this.http.put<any>(this.apiUrl + 'UpdateUser', user, this.httpOptions).pipe(
      tap(_ => {}),
      catchError(this.handleErrorProcedureResult(errorMsgs, 'updateUser'))
    );
  }

  removeUser(id: string, errorMsgs?: string[]): Observable<CatProcedureResult> {
    return this.http.delete<CatProcedureResult>(this.apiUrl + `RemoveUser/${id}`, this.httpOptions).pipe(
      tap(_ => {}),
      catchError(this.handleErrorProcedureResult(errorMsgs, 'removeUser'))
    );
  }

  constructor(@Inject(GlobalService) global: GlobalService, @Inject(HttpClient) http: HttpClient) {
    super(global, http);
  }
}
