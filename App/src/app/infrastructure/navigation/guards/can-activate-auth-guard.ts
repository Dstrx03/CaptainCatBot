import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
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

    canActivate() {
        console.log('guard isAuth=' + this.authInfo.IsAuthenticated);
        return this.authInfo.IsAuthenticated;
    }

}
