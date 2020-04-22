import { Routes } from '@angular/router';
import { HomeComponent } from 'src/app/views/home/home.component';
import { LoginComponent } from 'src/app/views/login/login.component';
import { CanActivateNoAuthGuard } from '../guards/can-activate-no-auth-guard';
import { CanActivateHttpsGuard } from '../guards/can-activate-https-guard';
import { DashboardComponent } from 'src/app/views/dashboard/dashboard.component';
import { CanActivateAuthGuard } from '../guards/can-activate-auth-guard';
import { TelegramComponent } from 'src/app/views/telegram/telegram.component';
import { TelegramStatusComponent } from 'src/app/views/telegram-status/telegram-status.component';
import { AppRoles } from './appRoles';
import { SystemComponent } from 'src/app/views/system/system.component';
import { UsersComponent } from 'src/app/views/users/users.component';
import { InternalServicesComponent } from 'src/app/views/internal-services/internal-services.component';
import { SystemLoggingComponent } from 'src/app/views/system-logging/system-logging.component';
import { AppRoutes } from './appRoutes';

export class AppRoutesRegistry {
    static readonly AppRoutesSet: Routes = [
        { 
            path: AppRoutes.Home.Path, 
            component: HomeComponent 
        },
        { 
            path: AppRoutes.Login.Path, 
            component: LoginComponent, 
            canActivate: [CanActivateNoAuthGuard, CanActivateHttpsGuard] 
        },
        { 
            path: AppRoutes.Dashboard.Path, 
            component: DashboardComponent, 
            canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard] 
        },
        { 
            path: AppRoutes.Telegram.Path, 
            component: TelegramComponent, 
            children: [
                { 
                    path: '', 
                    pathMatch: 'full', 
                    redirectTo: AppRoutes.Status.Path 
                },
                { 
                    path: AppRoutes.Status.Path, 
                    component: TelegramStatusComponent, 
                    canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], 
                    data: {roles: [AppRoles.Admin]} 
                }
            ]
        },
        { 
            path: AppRoutes.System.Path, 
            component: SystemComponent, 
            children: [
                { 
                    path: '', 
                    pathMatch: 'full', 
                    redirectTo: AppRoutes.Users.Path 
                },
                { 
                    path: AppRoutes.Users.Path, 
                    component: UsersComponent, 
                    canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], 
                    data: {roles: [AppRoles.Admin]} 
                },
                { 
                    path: AppRoutes.InternalServices.Path, 
                    component: InternalServicesComponent, 
                    canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], 
                    data: {roles: [AppRoles.Admin]} 
                },
                { 
                    path: AppRoutes.SystemLogging.Path, 
                    component: SystemLoggingComponent, 
                    canActivate: [CanActivateAuthGuard, CanActivateHttpsGuard], 
                    data: {roles: [AppRoles.Admin]} 
                }
            ]
        }
    ];
}