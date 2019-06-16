import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CanActivateAuthGuard } from './guards/can-activate-auth-guard';
import { AppRoles } from './menu/models/appRoles';

import { HomeComponent } from '../../views/home/home.component';
import { LoginComponent } from '../../views/login/login.component';
import { DashboardComponent } from '../../views/dashboard/dashboard.component';
import { UsersComponent } from '../../views/users/users.component';
import { SystemComponent } from 'src/app/views/system/system.component';



export class AppRoutes {
  static Routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'Login', component: LoginComponent },
    { path: 'Dashboard', component: DashboardComponent, canActivate: [CanActivateAuthGuard] },
    { path: 'System', component: SystemComponent, canActivate: [CanActivateAuthGuard], data: {roles: [AppRoles.Admin]}, children: [
      { path: 'Users', component: UsersComponent, canActivate: [CanActivateAuthGuard], data: {roles: [AppRoles.Admin]} }
    ]}
  ];
}

@NgModule({
  declarations: [],
  imports: [ RouterModule.forRoot(AppRoutes.Routes, {useHash: true}) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}