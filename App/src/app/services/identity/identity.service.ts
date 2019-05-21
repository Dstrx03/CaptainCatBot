import { Injectable } from '@angular/core';
import { GlobalService } from '../../infrastructure/global.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError, map, tap, mergeMap } from 'rxjs/operators';
import { LoginViewModel } from '../../models/loginViewModel';
import { LoginResult } from '../../models/loginResult';
import { AuthInfo } from '../../models/authInfo';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  private apiUrl: string;
  private authInfo: BehaviorSubject<AuthInfo>;

  currentAuthInfo(): Observable<AuthInfo> {
    return this.authInfo.asObservable();
  }

  login(loginViewModel: LoginViewModel): Observable<LoginResult> {
    let loginResult: LoginResult = null;
    return this.http.post(this.apiUrl + 'Login', loginViewModel, httpOptions).pipe(
      mergeMap((res: LoginResult) => {
        loginResult = res;
        return this.getAuthInfo();
      }),
      map((res: AuthInfo) => loginResult),
      tap((res: LoginResult) => {}),
      catchError(this.handleError<LoginResult>('login'))
    );
  }

  logOff(): Observable<boolean> {
    let loggedOff: boolean = null;
    return this.http.post(this.apiUrl + 'LogOff', null, httpOptions).pipe(
      mergeMap((res: boolean) => {
        loggedOff = res;
        return this.getAuthInfo();
      }),
      map((res: AuthInfo) => loggedOff),
      tap((res: boolean) => {}),
      catchError(this.handleError<boolean>('logOff'))
    );
  }

  getAuthInfo(): Observable<AuthInfo> {
    return this.http.get<AuthInfo>(this.apiUrl + 'GetAuthInfo').pipe(
      tap((res: AuthInfo) => this.authInfo.next(res)),
      catchError(this.handleError<AuthInfo>('getAuthInfo'))
    );
  }

  constructor(private global: GlobalService, private http: HttpClient) {
    this.apiUrl = `${global.baseUrl}api/v1/Identity/`;
    this.authInfo = new BehaviorSubject<AuthInfo>(new AuthInfo());
  }


  // TODO: redesign
  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      console.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
