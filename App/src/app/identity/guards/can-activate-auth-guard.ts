import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { IdentityService } from '../identity.service';

@Injectable({
    providedIn: 'root'
})
export class CanActivateAuthGuard implements CanActivate {

    constructor(private identitySvc: IdentityService) {}

    canActivate() {
        const isAuth = this.identitySvc.isAuthentificated();
        console.log('isAuth=' + isAuth);
        return isAuth;
    }
}
