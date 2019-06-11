import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormControl, Validators, AbstractControl } from '@angular/forms';
import { FormControlHelper } from '../../../infrastructure/helpers/formControlHelper';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppUserModel } from '../../../models/appUserModel';
import { Observable } from 'rxjs';
import { UsersService } from '../../../services/identity/users.service';

@Component({
  selector: 'app-edit-user-dialog',
  templateUrl: './edit-user-dialog.component.html',
  styleUrls: ['./edit-user-dialog.component.scss']
})
export class EditUserDialogComponent implements OnInit {

  @ViewChild('applySpinnerBtn') applySpinnerBtn;

  user: AppUserModel;
  loadingUser = false;

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
    private usersSvc: UsersService
  ) { 
    this.user = {} as AppUserModel;
  }

  apply() {
    this.applySpinnerBtn.options.active = true;
    this.dialogRef.disableClose = true;
    this.data.agentFn(this.user).subscribe(_ => this.dialogRef.close());
  }

  ngOnInit() {
    if (!this.data.addMode) {
      this.loadingUser = true;
      this.formControlHelper.disable();
      this.usersSvc.getUser(this.data.userId).subscribe((user) => {
        this.user = user;
        this.formControlHelper.enable();
        this.loadingUser = false;
      });
    }
  }

}

export class EditUserDialogData {
  addMode: boolean;
  userId: string;
  agentFn: (user: AppUserModel) => Observable<any>;
}
