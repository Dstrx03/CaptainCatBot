import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CanActivateAuthGuard } from './guards/can-activate-auth-guard';

import { HomeComponent } from '../../views/home/home.component';
import { LoginComponent } from '../../views/login/login.component';
import { DashboardComponent } from '../../views/dashboard/dashboard.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'Login', component: LoginComponent },
  { path: 'Dashboard', component: DashboardComponent, canActivate: [CanActivateAuthGuard] }
];

@NgModule({
  declarations: [],
  imports: [ RouterModule.forRoot(routes, {useHash: true}) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
