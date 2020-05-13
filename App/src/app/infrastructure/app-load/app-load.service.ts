import { Injectable, Injector } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';
import { ThemeService } from '../theme/theme.service';
import { UserPreferencesService } from '../user-preferences/user-preferences.service';

@Injectable({
  providedIn: 'root'
})
export class AppLoadService {

  private identitySvc: IdentityService;
  private themeSvc: ThemeService;
  private userPreferencesSvc: UserPreferencesService

  constructor(private injector: Injector) { }

  load(): Promise<any> {
    this.initUserPreferences();
    this.initTheme();
    return this.initAuthInfo();
  }

  private initUserPreferences(): void {
    this.userPreferencesSvc = this.injector.get(UserPreferencesService);
    this.userPreferencesSvc.initUserPreferences();
  }

  private initTheme(): void {
    this.themeSvc = this.injector.get(ThemeService);
    this.themeSvc.initTheme();
  }

  private initAuthInfo(): Promise<any> {
    this.identitySvc = this.injector.get(IdentityService);
    return this.identitySvc.getAuthInfo().toPromise();
  }

}
