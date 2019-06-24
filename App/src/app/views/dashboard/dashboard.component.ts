import { Component, OnInit } from '@angular/core';
import { AppRoles } from 'src/app/infrastructure/navigation/menu/models/appRoles';
import { AuthInfo } from 'src/app/models/authInfo';
import { IdentityService } from 'src/app/services/identity/identity.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  authInfo: AuthInfo;

  constructor(private identitySvc: IdentityService) { }

  hasRoles(requiredRoles: string[]) {
    const userRoles = this.authInfo !== undefined && this.authInfo.AuthUserInfo !== undefined && this.authInfo.AuthUserInfo.Roles !== undefined ?
      this.authInfo.AuthUserInfo.Roles : [];

    let isRolesAuth = true;
    requiredRoles.forEach(r => {
      if (userRoles.indexOf(r) === -1) isRolesAuth = false;
    })

    return isRolesAuth;
  }

  ngOnInit() {
    this.identitySvc.currentAuthInfo()
      .subscribe(currentAuthInfo => this.authInfo = currentAuthInfo);
  }

  get AppRoles() {
    return AppRoles;
  }

}
