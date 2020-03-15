import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AppMenu } from 'src/app/infrastructure/navigation/menu/models/appMenu';
import { MenuItemsService } from 'src/app/infrastructure/navigation/menu/menu-items.service';

@Component({
  selector: 'app-sidenav-list',
  templateUrl: './sidenav-list.component.html',
  styleUrls: ['./sidenav-list.component.scss']
})
export class SidenavListComponent implements OnInit {

  @Output() sidenavClose = new EventEmitter();

  appMenu: AppMenu;

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
