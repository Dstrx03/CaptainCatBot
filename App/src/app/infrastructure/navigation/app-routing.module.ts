import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRoutesRegistry } from './models/appRoutesRegistry';

@NgModule({
  declarations: [],
  imports: [ RouterModule.forRoot(AppRoutesRegistry.AppRoutesSet, {useHash: true}) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}