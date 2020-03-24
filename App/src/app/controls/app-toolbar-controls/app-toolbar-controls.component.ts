import { Component, OnInit, Injector } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';
import { AuthInfo } from '../../models/authInfo';
import { Router } from '@angular/router';
import { ThemeService } from 'src/app/infrastructure/theme/theme.service';
import { AppToolbarMenuItem, AppToolbarMenuComponent } from '../app-toolbar-menu/app-toolbar-menu.component';

@Component({
  selector: 'app-toolbar-controls',
  templateUrl: './app-toolbar-controls.component.html',
  styleUrls: ['./app-toolbar-controls.component.scss']
})
export class AppToolbarControlsComponent implements OnInit {

  authInfo: AuthInfo;
  isDarkMode: boolean;

  darkModeMenuItem = AppToolbarMenuComponent.BuildTemplateMenuItem("Dark Mode", "far fa-moon", "tmpltDarkMode");
  menuSettingsItems: AppToolbarMenuItem[] = [
    this.darkModeMenuItem
  ];

  logOff() {
    this.identitySvc.logOff()
      .subscribe(signedOut => {
        if (signedOut) { this.router.navigateByUrl('Login'); }
      });
  }

  toggleThemeMode() {
    this.themeSvc.toggleThemeMode();
  }

  onCurrentIsDarkModeUpdate(isDarkMode: boolean) {
    this.isDarkMode = isDarkMode;
    this.darkModeMenuItem.Caption = `Dark Mode: ${this.isDarkMode ? "On" : "Off"}`;
    this.darkModeMenuItem.Icon.FontSet = this.isDarkMode ? "fas" : "far";
  }

  constructor(private identitySvc: IdentityService, private router: Router, private themeSvc: ThemeService) { }

  ngOnInit() {
    this.identitySvc.currentAuthInfo()
      .subscribe(currentAuthInfo => this.authInfo = currentAuthInfo);
    this.themeSvc.currentIsDarkMode()
      .subscribe(currentIsDarkMode => this.onCurrentIsDarkModeUpdate(currentIsDarkMode));
  }

}
