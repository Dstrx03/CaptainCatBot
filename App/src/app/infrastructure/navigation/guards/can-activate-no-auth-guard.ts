import { Injectable } from '@angular/core';
import { Router, RouterStateSnapshot } from '@angular/router';
import { IdentityService } from '../../../services/identity/identity.service';
import { GlobalService } from '../../global.service';
import { CanActivateAuthGuard } from './can-activate-auth-guard';
import { AppRoutes } from '../models/appRoutes';

@Injectable({
    providedIn: 'root'
})
export class CanActivateNoAuthGuard extends CanActivateAuthGuard {

    constructor(identitySvc: IdentityService, globalSvc: GlobalService, router: Router) {
        super(identitySvc, globalSvc, router);
    }

    protected applyCanActivate(state: RouterStateSnapshot, canActivate: boolean) : boolean {
        if (canActivate && state.url.toLowerCase() === AppRoutes.Login.getFullRelativePath().toLowerCase()) 
            this.router.navigate([AppRoutes.Dashboard.getRouterLink()]);
        return !canActivate;
    }

}
