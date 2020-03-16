import { Injectable, Injector } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';
import { ThemeService } from '../theme/theme.service';

@Injectable({
  providedIn: 'root'
})
export class AppLoadService {

  private identitySvc: IdentityService;
  private themeSvc: ThemeService;

  constructor(private injector: Injector) { }

  load(): Promise<any>{
    this.initTheme();
    return this.initAuthInfo();
  }

  private initAuthInfo(): Promise<any> {
    this.identitySvc = this.injector.get(IdentityService);
    return this.identitySvc.getAuthInfo().toPromise();
  }

  private initTheme(): void {
    this.themeSvc = this.injector.get(ThemeService);
    this.themeSvc.initTheme();
  }
}
