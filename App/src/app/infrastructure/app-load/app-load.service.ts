import { Injectable, Injector } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';

@Injectable({
  providedIn: 'root'
})
export class AppLoadService {

  private identitySvc: IdentityService;

  constructor(private injector: Injector) {}

  initAuthInfo(): Promise<any> {
    this.identitySvc = this.injector.get(IdentityService);
    return this.identitySvc.getAuthInfo().toPromise();
  }
}
