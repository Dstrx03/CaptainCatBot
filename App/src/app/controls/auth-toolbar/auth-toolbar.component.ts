import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../../services/identity/identity.service';
import { AuthInfo } from '../../models/authInfo';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth-toolbar',
  templateUrl: './auth-toolbar.component.html',
  styleUrls: ['./auth-toolbar.component.css']
})
export class AuthToolbarComponent implements OnInit {

  authInfo: AuthInfo;

  logOff() {
    this.identitySvc.logOff()
      .subscribe(signedOut => {
        if (signedOut) { this.router.navigateByUrl('Login'); }
      });
  }

  constructor(private identitySvc: IdentityService, private router: Router) { }

  ngOnInit() {
    this.identitySvc.currentAuthInfo()
      .subscribe(currentAuthInfo => this.authInfo = currentAuthInfo);
  }

}
