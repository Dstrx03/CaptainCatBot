import { Component, OnInit } from '@angular/core';
import { LoginViewModel } from '../../models/loginViewModel';
import { IdentityService } from '../../services/identity/identity.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginViewModel: LoginViewModel = new LoginViewModel();

  login() {
    this.identitySvc.login(this.loginViewModel)
      .subscribe(loginResult => {
        switch (loginResult.SignInStatus) {
          case 0:
            this.router.navigateByUrl('Dashboard');
            break;
          case 1:
          case 2:
          case 3:
          default:
            alert('login failure: ' + JSON.stringify(loginResult.Errors)); // TODO: login failure UI
            break;
        }
      });
  }

  constructor(private identitySvc: IdentityService, private router: Router) {}

  ngOnInit() {
  }

}
