import { Component, OnInit, Input } from '@angular/core';
import { AppMenu } from 'src/app/infrastructure/navigation/menu/models/appMenu';
import { MenuItemsService } from 'src/app/infrastructure/navigation/menu/menu-items.service';

@Component({
  selector: 'app-sidenav-menu',
  templateUrl: './sidenav-menu.component.html',
  styleUrls: ['./sidenav-menu.component.scss']
})
export class SidenavMenuComponent implements OnInit {

  @Input() menuId: string;

  appMenu: AppMenu;

  constructor(private menuItemsSvc: MenuItemsService) { 
    this.appMenu = new AppMenu();
  }

  ngOnInit() {
    this.menuItemsSvc.currentAppMenu()
      .subscribe(currentAppMenu => {
        const systemItem = currentAppMenu.menuItems.find(x => x.Id === this.menuId);
        if (systemItem !== undefined) this.appMenu.menuItems = systemItem.Children;
      });
  }

}
