import { NgModule, APP_INITIALIZER, Injector } from '@angular/core';
import { AppLoadService } from './app-load.service';
import { IdentityService } from 'src/app/services/identity/identity.service';
import { MenuItemsService } from '../navigation/menu/menu-items.service';

export function initAuthInfo(appLoadService: AppLoadService) {
  return () => appLoadService.initAuthInfo();
}

@NgModule({
  imports: [],
  providers: [
    AppLoadService, IdentityService, MenuItemsService,
    { provide: APP_INITIALIZER, useFactory: initAuthInfo, deps: [AppLoadService], multi: true }
  ]
})
export class AppLoadModule { }
