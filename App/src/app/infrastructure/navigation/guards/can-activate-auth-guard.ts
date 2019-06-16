import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { IdentityService } from '../../../services/identity/identity.service';
import { AuthInfo } from '../../../models/authInfo';

@Injectable({
    providedIn: 'root'
})
export class CanActivateAuthGuard implements CanActivate {

    authInfo: AuthInfo;

    constructor(private identitySvc: IdentityService) {
        this.identitySvc.currentAuthInfo()
            .subscribe(currentAuthInfo => this.authInfo = currentAuthInfo);
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        const isAuth = this.authInfo.IsAuthenticated;
        if (!isAuth) return false;

        const requiredRoles = route.data.roles !== undefined && route.data.roles !== null ? 
            route.data.roles as Array<string> : [];
        const userRoles = this.authInfo !== undefined && this.authInfo.AuthUserInfo !== undefined && this.authInfo.AuthUserInfo.Roles !== undefined ?
            this.authInfo.AuthUserInfo.Roles : [];

        let isRolesAuth = true;
        requiredRoles.forEach(r => {
            if (userRoles.indexOf(r) === -1) isRolesAuth = false;
        })

        return isRolesAuth;
    }

}
