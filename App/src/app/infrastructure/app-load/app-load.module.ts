import { NgModule, APP_INITIALIZER, Renderer2 } from '@angular/core';
import { AppLoadService } from './app-load.service';
import { IdentityService } from 'src/app/services/identity/identity.service';
import { MenuItemsService } from '../navigation/menu-items/menu-items.service';
import { ThemeService } from '../theme/theme.service';

export function load(appLoadService: AppLoadService) {
  return () => appLoadService.load();
}

@NgModule({
  imports: [],
  providers: [
    AppLoadService, IdentityService, MenuItemsService, ThemeService,
    { provide: APP_INITIALIZER, useFactory: load, deps: [AppLoadService], multi: true }
  ]
})
export class AppLoadModule { }
