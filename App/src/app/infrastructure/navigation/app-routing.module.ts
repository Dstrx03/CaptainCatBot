import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRoutes } from './menu/models/appRoutes';

@NgModule({
  declarations: [],
  imports: [ RouterModule.forRoot(AppRoutes.RoutesAppSet, {useHash: true}) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}