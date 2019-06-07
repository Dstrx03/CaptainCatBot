import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';
import { AuthInfo } from 'src/app/models/authInfo';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  @Input() title: string;

  @Output() public sidenavToggle = new EventEmitter();

  authInfo: AuthInfo;

  constructor(private identitySvc: IdentityService) { }

  public onToggleSidenav = () => {
    this.sidenavToggle.emit();
  }

  getToolbarColor() {
    return this.authInfo.IsAuthenticated ? 'primary' : null;
  }

  ngOnInit() {
    this.identitySvc.currentAuthInfo()
      .subscribe(currentAuthInfo => this.authInfo = currentAuthInfo);
  }

}
