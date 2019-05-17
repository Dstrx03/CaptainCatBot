import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CanActivateAuthGuard } from '../identity/guards/can-activate-auth-guard';

import { HomeComponent } from '../home/home.component';
import { LoginComponent } from '../login/login.component';
import { DashboardComponent } from '../dashboard/dashboard.component';

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
