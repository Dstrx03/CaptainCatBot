import { Injectable, Inject } from '@angular/core';
import { HttpBaseService } from '../http-base.service';
import { GlobalService } from '../../infrastructure/global.service';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { catchError, map, tap, mergeMap } from 'rxjs/operators';
import { LoginViewModel } from '../../models/loginViewModel';
import { LoginResult } from '../../models/loginResult';
import { AuthInfo } from '../../models/authInfo';
import { MenuItemsService } from 'src/app/infrastructure/navigation/menu-items/menu-items.service';

@Injectable({
  providedIn: 'root'
})
export class IdentityService extends HttpBaseService {

  private authInfo: BehaviorSubject<AuthInfo>;

  protected controllerName(): string {
    return 'Identity/';
  }

  currentAuthInfo(): Observable<AuthInfo> {
    return this.authInfo.asObservable();
  }

  login(loginViewModel: LoginViewModel): Observable<LoginResult> {
    let loginResult: LoginResult = null;
    return this.http.post(this.apiUrl + 'Login', loginViewModel, this.httpOptions).pipe(
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
    return this.http.post(this.apiUrl + 'LogOff', null, this.httpOptions).pipe(
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
      tap((res: AuthInfo) => {
        this.authInfo.next(res)
        this.menuItemsSvc.updateMenuItems(res);
      }),
      catchError(this.handleError<AuthInfo>('getAuthInfo', new AuthInfo()))
    );
  }

  constructor(
      global: GlobalService, 
      http: HttpClient,
      private menuItemsSvc: MenuItemsService) {
    super(global, http);
    this.authInfo = new BehaviorSubject<AuthInfo>(new AuthInfo());
  }
}
