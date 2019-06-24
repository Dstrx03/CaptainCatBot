import { Component, OnInit, Input } from '@angular/core';
import { AppMenuItem } from 'src/app/infrastructure/navigation/menu/models/appMenuItems';
import { IdentityService } from 'src/app/services/identity/identity.service';

@Component({
  selector: 'app-sidenav-menu',
  templateUrl: './sidenav-menu.component.html',
  styleUrls: ['./sidenav-menu.component.scss']
})
export class SidenavMenuComponent implements OnInit {

  @Input() menuId: string;

  menuItems: AppMenuItem[];

  constructor(private identitySvc: IdentityService) { }

  ngOnInit() {
    this.identitySvc.currentMenuItems()
      .subscribe(currentMenuItems => {
        const systemItem = currentMenuItems.find(x => x.Id === this.menuId);
        if (systemItem !== undefined) this.menuItems = systemItem.Children;
      });
  }

}
