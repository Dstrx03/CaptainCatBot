import { Component, OnInit, Injector } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';
import { AuthInfo } from '../../models/authInfo';
import { Router } from '@angular/router';
import { ThemeService } from 'src/app/infrastructure/theme/theme.service';
import { AppToolbarMenuItem, AppToolbarMenuComponent } from '../app-toolbar-menu/app-toolbar-menu.component';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component';
import { tap, mergeMap } from 'rxjs/operators';

@Component({
  selector: 'app-toolbar-controls',
  templateUrl: './app-toolbar-controls.component.html',
  styleUrls: ['./app-toolbar-controls.component.scss']
})
export class AppToolbarControlsComponent implements OnInit {

  authInfo: AuthInfo;
  isDarkMode: boolean;

  darkModeMenuItem = AppToolbarMenuComponent.BuildTemplateMenuItem("Dark Mode", "far fa-moon", "tmpltDarkMode");
  menuPersonalizationItems: AppToolbarMenuItem[] = [
    this.darkModeMenuItem
  ];

  menuAuthItems: AppToolbarMenuItem[] = [
    AppToolbarMenuComponent.BuildOnClickMenuItem("Log out", "fas fa-sign-out-alt", this.logOff)
  ];

  logOff(injector: Injector) {
    let dialog = injector.get(MatDialog);
    let identitySvc = injector.get(IdentityService);
    let router = injector.get(Router);
    dialog.open(ConfirmDialogComponent, {
      autoFocus: false,
      data: {
        header: `Log out`,
        contentHtml: `<p>Are you sure you wish to log out?</p>`,
        useAgent: true,
        agent: identitySvc.logOff().pipe(
          tap(signedOut => {
            if (signedOut) { router.navigateByUrl('Login'); }
          })
        )
      }
    });
  }

  toggleThemeMode() {
    this.themeSvc.toggleThemeMode();
  }

  constructor(private identitySvc: IdentityService, private router: Router, private themeSvc: ThemeService) { }

  onCurrentIsDarkModeUpdate(isDarkMode: boolean) {
    this.isDarkMode = isDarkMode;
    this.darkModeMenuItem.Caption = `Dark Mode: ${this.isDarkMode ? "On" : "Off"}`;
    this.darkModeMenuItem.Icon.FontSet = this.isDarkMode ? "fas" : "far";
  }

  ngOnInit() {
    this.identitySvc.currentAuthInfo()
      .subscribe(currentAuthInfo => this.authInfo = currentAuthInfo);
    this.themeSvc.currentIsDarkMode()
      .subscribe(currentIsDarkMode => this.onCurrentIsDarkModeUpdate(currentIsDarkMode));
  }

}
