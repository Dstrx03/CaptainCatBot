import { Injectable } from '@angular/core';
import { GlobalService } from '../infrastructure/global.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { CatProcedureResult } from '../infrastructure/web-api/cat-procedure-result';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export abstract class HttpBaseService {

  protected httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  protected apiUrl: string;
  protected abstract controllerName(): string;

  constructor(protected global: GlobalService, protected http: HttpClient) {
    this.apiUrl = `${global.baseUrl}api/v1/${this.controllerName()}`;
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  protected handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // If 401 (Unauthorized) error occurred then stop the pipeline instead of returning result.
      if (error.status == 401) return throwError(`${operation} request is unauthorized: ${error.message}`);

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      console.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /**
   * Handle CatProcedureResult operation that failed.
   * Initialize CatProcedureResult object.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  protected handleErrorProcedureResult(errorMsgs: string[] = ['System error occured!'], operation = 'operation', result?: CatProcedureResult) {
    if (result === undefined || result === null) result = {IsSuccess: false, ErrorMsgs: errorMsgs} as CatProcedureResult;
    return this.handleError(operation, result);
  }
}
