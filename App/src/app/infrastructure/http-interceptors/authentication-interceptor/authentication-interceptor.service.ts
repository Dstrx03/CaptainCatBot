import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler,HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, throwError, from } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { IdentityService } from 'src/app/services/identity/identity.service';
import { MatDialog } from '@angular/material/dialog';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationInterceptor implements HttpInterceptor {

  constructor(private router: Router, private identitySvc: IdentityService, private matDialog: MatDialog) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status == 401) {
          return this.identitySvc.getAuthInfo().pipe(switchMap(() => {
            return from(this.router.navigate(['Login'])).pipe(switchMap(() => {
              this.matDialog.closeAll();
              return throwError(err);
            }));
          }));
        } else {
          return throwError(err);
        }
      })
    );
  }

}
