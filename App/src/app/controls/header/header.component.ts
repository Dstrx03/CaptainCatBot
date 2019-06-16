import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';
import { AuthInfo } from 'src/app/models/authInfo';
import { AppMenuItem } from 'src/app/infrastructure/navigation/menu/models/appMenuItems';
import { GlobalService } from 'src/app/infrastructure/global.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  @Output() public sidenavToggle = new EventEmitter();

  authInfo: AuthInfo;
  menuItems: AppMenuItem[];

  constructor(private identitySvc: IdentityService, public globalSvc: GlobalService) { }

  public onToggleSidenav = () => {
    this.sidenavToggle.emit();
  }

  getToolbarColor() {
    return this.authInfo.IsAuthenticated ? 'primary' : null;
  }

  ngOnInit() {
    this.identitySvc.currentAuthInfo()
      .subscribe(currentAuthInfo => this.authInfo = currentAuthInfo);
    this.identitySvc.currentMenuItems()
      .subscribe(currentMenuItems => this.menuItems = currentMenuItems);
  }

}
