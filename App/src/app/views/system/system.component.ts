import { Component, OnInit } from '@angular/core';
import { AppMenuItem } from 'src/app/infrastructure/navigation/menu/models/appMenuItems';
import { IdentityService } from 'src/app/services/identity/identity.service';

@Component({
  selector: 'app-system',
  templateUrl: './system.component.html',
  styleUrls: ['./system.component.scss']
})
export class SystemComponent implements OnInit {

  menuItems: AppMenuItem[];

  constructor(private identitySvc: IdentityService) { }

  ngOnInit() {
    this.identitySvc.currentMenuItems()
      .subscribe(currentMenuItems => {
        const systemItem = currentMenuItems.find(x => x.Id === 'sys');
        if (systemItem !== undefined) this.menuItems = systemItem.Children;
      });
  }

}
