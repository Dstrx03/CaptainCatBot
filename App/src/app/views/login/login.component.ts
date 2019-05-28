import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators} from '@angular/forms';
import { LoginViewModel } from '../../models/loginViewModel';
import { IdentityService } from '../../services/identity/identity.service';
import { Router } from '@angular/router';
import { FormControlHelper } from 'src/app/infrastructure/helpers/formControlHelper';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  @ViewChild('loginBtn') loginBtn;

  imgLoaded = false;
  passwordHide = true;

  emailFormControl = new FormControl('', [
    Validators.required
  ]);
  passwordFormControl = new FormControl('', [
    Validators.required
  ]);
  formControlHelper = new FormControlHelper([this.emailFormControl, this.passwordFormControl]);

  loginViewModel: LoginViewModel = new LoginViewModel();
  loginFailSnackbar: any;

  login() {
    this.loginBtn.options.active = true;
    this.identitySvc.login(this.loginViewModel)
      .subscribe(loginResult => {
        this.loginBtn.options.active = false;
        switch (loginResult.SignInStatus) {
          case 0:
            if (this.loginFailSnackbar !== undefined)
              this.loginFailSnackbar.dismiss();
            this.router.navigateByUrl('Dashboard');
            break;
          case 1:
          case 2:
          case 3:
          default:
            this.loginFailSnackbar = this.snackBar.open(`Login failed! ${loginResult.Errors.join(' ')}`, 'Got it');
            break;
        }
      });
  }

  test(){
    console.log('img loaded');
  }

  constructor(
    private identitySvc: IdentityService,
    private router: Router,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
  }

}
