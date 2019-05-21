import { Injectable } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';

@Injectable({
  providedIn: 'root'
})
export class AppLoadService {

  constructor(private identitySvc: IdentityService) {}

  initAuthInfo(): Promise<any> {
    return this.identitySvc.getAuthInfo().toPromise();
  }
}
