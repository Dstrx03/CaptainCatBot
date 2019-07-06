import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { GlobalService } from '../../global.service';

@Injectable({
    providedIn: 'root'
})
export class CanActivateHttpsGuard implements CanActivate {

    constructor(private gloablSvc: GlobalService) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        if (this.gloablSvc.baseUrl.toLowerCase().startsWith('https:') && 
        location.protocol.toLowerCase() === 'http:') {
            window.location.href = `${this.gloablSvc.baseUrl}#${state.url}`;
            return false;
        }
        return true;
    }
}
