import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AppMenu } from 'src/app/infrastructure/navigation/models/appMenu';
import { MenuItemsService } from 'src/app/infrastructure/navigation/menu-items/menu-items.service';
import { AppRoutes } from 'src/app/infrastructure/navigation/models/appRoutes';

@Component({
  selector: 'app-sidenav-list',
  templateUrl: './sidenav-list.component.html',
  styleUrls: ['./sidenav-list.component.scss']
})
export class SidenavListComponent implements OnInit {

  @Output() sidenavClose = new EventEmitter();

  appMenu: AppMenu;
  appRoutes = AppRoutes;

  constructor(private menuItemsSvc: MenuItemsService) { 
    this.appMenu = new AppMenu();
  }

  ngOnInit() {
    this.menuItemsSvc.currentAppMenu()
      .subscribe(currentAppMenu => this.appMenu = currentAppMenu);
  }

  public onSidenavClose = () => {
    this.sidenavClose.emit();
  }

}
