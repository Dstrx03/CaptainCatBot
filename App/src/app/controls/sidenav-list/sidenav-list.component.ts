import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AppMenuItem } from 'src/app/infrastructure/navigation/menu/models/appMenuItems';
import { IdentityService } from 'src/app/services/identity/identity.service';

@Component({
  selector: 'app-sidenav-list',
  templateUrl: './sidenav-list.component.html',
  styleUrls: ['./sidenav-list.component.scss']
})
export class SidenavListComponent implements OnInit {

  @Output() sidenavClose = new EventEmitter();

  menuItems: AppMenuItem[];

  constructor(private identitySvc: IdentityService) { }

  ngOnInit() {
    this.identitySvc.currentMenuItems()
      .subscribe(currentMenuItems => this.menuItems = currentMenuItems);
  }

  public onSidenavClose = () => {
    this.sidenavClose.emit();
  }

}
