import { Injectable } from '@angular/core';
import { Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { IdentityService } from '../../../services/identity/identity.service';
import { GlobalService } from '../../global.service';
import { CanActivateAuthGuard } from './can-activate-auth-guard';
import { AppRoutes, AppRoutesItem } from '../models/appRoutes';

@Injectable({
    providedIn: 'root'
})
export class CanActivateNoAuthGuard extends CanActivateAuthGuard {

    constructor(identitySvc: IdentityService, globalSvc: GlobalService, router: Router) {
        super(identitySvc, globalSvc, router);
    }

    protected applyCanActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot, canActivate: boolean, rolesRedirectItem?: AppRoutesItem) : boolean {
        if (canActivate && state.url.toLowerCase() === AppRoutes.Login.getFullRelativePath().toLowerCase()) 
            this.router.navigate([AppRoutes.Dashboard.getRouterLink()]);
        return !canActivate;
    }

}
