import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CanActivateAuthGuard } from './guards/can-activate-auth-guard';
import { CanActivateHttpsGuard } from './guards/can-activate-https-guard';
import { AppRoles } from './menu/models/appRoles';

import { HomeComponent } from '../../views/home/home.component';
import { LoginComponent } from '../../views/login/login.component';
import { DashboardComponent } from '../../views/dashboard/dashboard.component';
import { UsersComponent } from '../../views/users/users.component';
import { SystemComponent } from 'src/app/views/system/system.component';
import { InternalServicesComponent } from 'src/app/views/internal-services/internal-services.component';
import { TelegramComponent } from 'src/app/views/telegram/telegram.component';
import { TelegramStatusComponent } from 'src/app/views/telegram-status/telegram-status.component';
import { SystemLoggingComponent } from 'src/app/views/system-logging/system-logging.component';



export class AppRoutes {
  static readonly RoutesAppSet: Routes = [
    { path: '', component: HomeComponent },
    { path: 'Login', component: LoginComponent, canActivate: [CanActivateHttpsGuard] },
    { path: 'Dashboard', component: DashboardComponent, canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard] },
    { path: 'Telegram', component: TelegramComponent, canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], children: [
      { path: 'Status', component: TelegramStatusComponent, canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], data: {roles: [AppRoles.Admin]} },
    ]},
    { path: 'System', component: SystemComponent, canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], data: {roles: [AppRoles.Admin]}, children: [
      { path: 'Users', component: UsersComponent, canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], data: {roles: [AppRoles.Admin]} },
      { path: 'InternalServices', component: InternalServicesComponent, canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], data: {roles: [AppRoles.Admin]} },
      { path: 'SystemLogging', component: SystemLoggingComponent, canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], data: {roles: [AppRoles.Admin]} }
    ]}
  ];
}

@NgModule({
  declarations: [],
  imports: [ RouterModule.forRoot(AppRoutes.RoutesAppSet, {useHash: true}) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}