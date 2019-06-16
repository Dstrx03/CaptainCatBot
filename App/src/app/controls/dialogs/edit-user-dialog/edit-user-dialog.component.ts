import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormControl, Validators, AbstractControl } from '@angular/forms';
import { FormControlHelper } from '../../../infrastructure/helpers/formControlHelper';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppUserModel } from '../../../models/appUserModel';
import { ParsedAppRole, AuthInfo } from '../../../models/authInfo';
import { Observable, of, Subscription } from 'rxjs';
import { tap, mergeMap } from 'rxjs/operators';
import { UsersService } from '../../../services/identity/users.service';
import { IdentityService } from '../../../services/identity/identity.service';

@Component({
  selector: 'app-edit-user-dialog',
  templateUrl: './edit-user-dialog.component.html',
  styleUrls: ['./edit-user-dialog.component.scss']
})
export class EditUserDialogComponent implements OnInit {

  @ViewChild('applySpinnerBtn') applySpinnerBtn;

  private dSubscription: Subscription;

  user: AppUserModel;
  registeredRoles: ParsedAppRole[];
  selectedRoles: any;
  loadingData = false;

  passwordHide = true;
  oldPasswordHide = true;
  confirmPassword = undefined;

  confirmPasswordRequired(): boolean {
    if (this.data.addMode) return true;
    return this.user !== undefined && this.user.Password !== undefined && this.user.Password !== null && this.user.Password !== '';
  }

  usernameFormControl = new FormControl('', [
    (control: AbstractControl): { [key: string]: boolean } | null => {
      const regexp = /^(?=.*[A-Za-z])([0-9A-Za-z_]){6,32}$/;
      return (control.value !== undefined && !regexp.test(control.value)) ? { username: true } : null;
    }
  ]);
  emailFormControl = new FormControl('', [
    Validators.email
  ]);
  passwordFormControl = new FormControl('', [
    (control: AbstractControl): {[key: string]: boolean} | null => {
      const regexp = /^([0-9A-Za-z!@#$%^&*])+$/;
      return (control.value !== undefined && control.value !== '' && !regexp.test(control.value)) ? { password: true } : null;
    }
  ]);
  confirmPasswordFormControl = new FormControl('', [
    (control: AbstractControl): { [key: string]: boolean } | null => {
      return (control.value !== undefined && this.user !== undefined && control.value != this.user.Password) ? { confirmPassword: true } : null;
    }
  ]);
  oldPasswordFormControl = new FormControl('');
  formControlHelper = new FormControlHelper([
    this.usernameFormControl, 
    this.emailFormControl,
    this.passwordFormControl,
    this.confirmPasswordFormControl,
    this.oldPasswordFormControl
  ], 
  [
    {
      error: 'username', 
      msg: 'Only latin letters, numbers and underscores allowed, must be 6-32 characters long.'
    },
    {
      error: 'password',
      msg: 'Only latin letters, numbers and !@#$%^&* symbols allowed.'
    },
    {
      error: 'confirmPassword',
      msg: 'The confirmation value differ from Password value!'
    }
  ]);

  constructor(
    public dialogRef: MatDialogRef<EditUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditUserDialogData,
    private identitySvc: IdentityService,
    private usersSvc: UsersService
  ) { 
    this.user = {} as AppUserModel;
    this.selectedRoles = {};
  }

  private initSelectedRoles(roles: string[] = undefined){
    this.selectedRoles = {};
    this.registeredRoles.forEach(role => {
      const checked = roles === undefined || roles === null ? 
        false : 
        roles.indexOf(role.SystemName) !== -1;
      this.selectedRoles[role.SystemName] = checked;
    })
  }

  private initUserRoles(user: AppUserModel) {
    user.Roles = [];
    this.registeredRoles.forEach(role => {
      if (this.selectedRoles[role.SystemName] === true) 
        user.Roles.push(role.SystemName);
    });
  }

  apply() {
    this.applySpinnerBtn.options.active = true;
    this.dialogRef.disableClose = true;
    this.initUserRoles(this.user);
    this.data.agentFn(this.user).subscribe(_ => this.dialogRef.close());
  }

  ngOnInit() {
    this.loadingData = true;
    this.formControlHelper.disable();
    this.dSubscription = this.identitySvc.currentAuthInfo().pipe(
      mergeMap((currentAuthInfo: AuthInfo) => {
        this.registeredRoles = currentAuthInfo.RegisteredRoles
        return !this.data.addMode ? this.usersSvc.getUser(this.data.userId) : of({} as AppUserModel);
      }),
      tap((user: AppUserModel) => {
        if (user !== undefined) 
          this.initSelectedRoles(user.Roles);
        this.user = user
      })
    ).subscribe(_ => {
      this.formControlHelper.enable();
      this.loadingData = false;
    });
  }

  ngOnDestroy() {
    this.dSubscription.unsubscribe();
  }

}

export class EditUserDialogData {
  addMode: boolean;
  userId: string;
  agentFn: (user: AppUserModel) => Observable<any>;
}
