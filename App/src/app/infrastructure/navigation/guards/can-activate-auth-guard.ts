import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { IdentityService } from '../../../services/identity/identity.service';
import { AuthInfo } from '../../../models/authInfo';
import { GlobalService } from '../../global.service';
import { AppRoutes, AppRoutesItem } from '../models/appRoutes';

@Injectable({
    providedIn: 'root'
})
export class CanActivateAuthGuard implements CanActivate {

    authInfo: AuthInfo;

    constructor(private identitySvc: IdentityService, private globalSvc: GlobalService, protected router: Router) {
        this.identitySvc.currentAuthInfo()
            .subscribe(currentAuthInfo => this.authInfo = currentAuthInfo);
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        const isAuth = this.authInfo.IsAuthenticated;
        if (!isAuth) return this.applyCanActivate(route, state, false);

        const canAcitvate = this.isRolesAuth(this.getRequiredRoles(route));
        const redirectItem = this.getRedirectItem(route);
        return this.applyCanActivate(route, state, canAcitvate, redirectItem);
    }

    protected applyCanActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot, canActivate: boolean, redirectItem?: AppRoutesItem) : boolean {
        if (redirectItem === null || redirectItem === undefined) {
            if (!canActivate) this.router.navigate([AppRoutes.Login.getRouterLink()]);
            return canActivate;
        }
        
        const path = redirectItem.getRouterLink();
        if (!canActivate && redirectItem.IsHref) window.location.href = path;
        else if (!canActivate) this.router.navigate([path]);
        return canActivate;
    }

    private isRolesAuth(requiredRoles: string[]): boolean {
        const userRoles = this.getUserRoles();
        let isRolesAuth = true;
        requiredRoles.forEach(r => {
            if (userRoles.indexOf(r) === -1) isRolesAuth = false;
        });
        return isRolesAuth;
    }

    private getRequiredRoles(route: ActivatedRouteSnapshot): string[] {
        return route.data.roles !== undefined && route.data.roles !== null ? 
            route.data.roles as Array<string> : [];
    }

    private getUserRoles(): string[] {
        return this.authInfo !== undefined && this.authInfo.AuthUserInfo !== undefined && this.authInfo.AuthUserInfo.Roles !== undefined ?
            this.authInfo.AuthUserInfo.Roles : [];
    }

    private getRedirectItem(route: ActivatedRouteSnapshot): AppRoutesItem {
        const redirectOptions = route.data.redirectOptions !== undefined && route.data.redirectOptions !== null ? 
            route.data.redirectOptions as Array<string> : [];
        for(let i = 0; i < redirectOptions.length; i++) {
            let redirectItem: AppRoutesItem = AppRoutes.findById(redirectOptions[i]);
            if (redirectItem !== null && this.isRolesAuth(redirectItem.RequiredRoles)) return redirectItem;
        }
        return null;
    }

}
