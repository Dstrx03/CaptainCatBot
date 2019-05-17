import { Injectable } from '@angular/core';
import { GlobalService } from '../infrastructure/global.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  private apiUrl: string;

  login(email: string, password: string, rememberMe: boolean): Observable<any> {
    return this.http.post(this.apiUrl + 'Login', { Email: email, Password: password, RememberMe: rememberMe }, httpOptions).pipe(
      tap((response: any) => console.log(`Login response=${response}`)),
      catchError(this.handleError<any>('login'))
    );
  }

  logOff(): Observable<any> {
    return this.http.post(this.apiUrl + 'LogOff', null, httpOptions).pipe(
      tap((response: any) => console.log(`LogOff response=${response}`)),
      catchError(this.handleError<any>('logOff'))
    );
  }

  isAuthentificated(): Observable<boolean> {
    return this.http.get<boolean>(this.apiUrl + 'IsAuthenticated').pipe(
      tap((response: boolean) => console.log(`isAuthentificated response=${response}`)),
      catchError(this.handleError<boolean>('isAuthentificated'))
    );
  }

  constructor(private global: GlobalService, private http: HttpClient) {
    this.apiUrl = `${global.baseUrl}api/v1/Identity/`;
  }

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
