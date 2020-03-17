import { Routes } from '@angular/router';
import { HomeComponent } from 'src/app/views/home/home.component';
import { LoginComponent } from 'src/app/views/login/login.component';
import { CanActivateNoAuthGuard } from '../../guards/can-activate-no-auth-guard';
import { CanActivateHttpsGuard } from '../../guards/can-activate-https-guard';
import { DashboardComponent } from 'src/app/views/dashboard/dashboard.component';
import { CanActivateAuthGuard } from '../../guards/can-activate-auth-guard';
import { TelegramComponent } from 'src/app/views/telegram/telegram.component';
import { TelegramStatusComponent } from 'src/app/views/telegram-status/telegram-status.component';
import { AppRoles } from './appRoles';
import { SystemComponent } from 'src/app/views/system/system.component';
import { UsersComponent } from 'src/app/views/users/users.component';
import { InternalServicesComponent } from 'src/app/views/internal-services/internal-services.component';
import { SystemLoggingComponent } from 'src/app/views/system-logging/system-logging.component';

export class AppRoutes {
    static readonly RoutesAppSet: Routes = [
      { path: '', component: HomeComponent },
      { path: 'Login', component: LoginComponent, canActivate: [CanActivateNoAuthGuard, CanActivateHttpsGuard] },
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