import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';
import { AuthInfo } from 'src/app/models/authInfo';
import { AppMenu, AppMenuItem } from 'src/app/infrastructure/navigation/models/appMenu';
import { GlobalService } from 'src/app/infrastructure/global.service';
import { MenuItemsService } from 'src/app/infrastructure/navigation/menu-items/menu-items.service';
import { AppRoutes } from 'src/app/infrastructure/navigation/models/appRoutes';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  @Output() public sidenavToggle = new EventEmitter();

  authInfo: AuthInfo;
  appMenu: AppMenu;
  appRoutes = AppRoutes;

  constructor(private identitySvc: IdentityService, private menuItemsSvc: MenuItemsService, public globalSvc: GlobalService) { 
    this.appMenu = new AppMenu();
  }

  public onToggleSidenav = () => {
    this.sidenavToggle.emit();
  }

  getToolbarColor() {
    return this.authInfo.IsAuthenticated ? 'primary' : null;
  }

  ngOnInit() {
    this.identitySvc.currentAuthInfo()
      .subscribe(currentAuthInfo => this.authInfo = currentAuthInfo);
    this.menuItemsSvc.currentAppMenu()
      .subscribe(currentAppMenu => this.appMenu = currentAppMenu);
  }

}
