import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { IdentityService } from '../../../services/identity/identity.service';
import { GlobalService } from '../../global.service';
import { CanActivateAuthGuard } from './can-activate-auth-guard';

@Injectable({
    providedIn: 'root'
})
export class CanActivateNoAuthGuard extends CanActivateAuthGuard {

    constructor(identitySvc: IdentityService, globalSvc: GlobalService, router: Router) {
        super(identitySvc, globalSvc, router);
    }

    protected applyCanActivate(canActivate: boolean) : boolean {
        return !canActivate;
    }

}
