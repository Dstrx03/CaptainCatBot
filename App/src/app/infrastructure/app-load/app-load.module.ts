import { NgModule, APP_INITIALIZER } from '@angular/core';
import { AppLoadService } from './app-load.service';

export function initAuthInfo(appLoadService: AppLoadService) {
    return () => appLoadService.initAuthInfo();
}

@NgModule({
  imports: [],
  providers: [
    AppLoadService,
    { provide: APP_INITIALIZER, useFactory: initAuthInfo, deps: [AppLoadService], multi: true }
  ]
})
export class AppLoadModule { }
