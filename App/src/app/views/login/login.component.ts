import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators} from '@angular/forms';
import { LoginViewModel } from '../../models/loginViewModel';
import { IdentityService } from '../../services/identity/identity.service';
import { Router } from '@angular/router';
import { FormControlHelper } from 'src/app/infrastructure/helpers/form-control-helper';
import { MatSnackBar } from '@angular/material';
import { GlobalService } from 'src/app/infrastructure/global.service';
import { AppMatSpinnerButtonComponent } from 'src/app/controls/app-mat-spinner-button/app-mat-spinner-button.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  @ViewChild('loginBtn') loginBtn: AppMatSpinnerButtonComponent;

  imgLoaded = false;
  passwordHide = true;

  usernameFormControl = new FormControl('', [
    Validators.required
  ]);
  passwordFormControl = new FormControl('', [
    Validators.required
  ]);
  formControlHelper = new FormControlHelper([this.usernameFormControl, this.passwordFormControl]);

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

  constructor(
    private identitySvc: IdentityService,
    private router: Router,
    private snackBar: MatSnackBar,
    public globalSvc: GlobalService) { }

  ngOnInit() {
  }

}
