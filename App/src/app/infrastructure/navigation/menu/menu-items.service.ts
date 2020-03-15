import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router, NavigationEnd } from '@angular/router';
import { AuthInfo } from '../../../models/authInfo';
import { AppMenuItem, AppMenu } from './models/appMenu';

@Injectable({
  providedIn: 'root'
})
export class MenuItemsService {

  private appMenu: BehaviorSubject<AppMenu>;
  private currentUrl: string;

  currentAppMenu(): Observable<AppMenu> {
    return this.appMenu.asObservable();
  }

  constructor(private router: Router) {
    this.appMenu = new BehaviorSubject<AppMenu>(new AppMenu());
    this.currentUrl = '';
    this.subscribeToRouterNavigation(router);
  }

  updateMenuItems(authInfo: AuthInfo) {
    let appMenu = this.appMenu.getValue();
    appMenu.menuItems = this.generateMenuItemsLevel(AppMenu.ItemsAppSet, authInfo);
    this.updateUrl(appMenu, this.currentUrl);
  }

  private updateUrl(appMenu: AppMenu, url: string) {
    this.clearActivatedRouteTree(appMenu.menuItems);
    if (url !== '/'){
      let urlSplitted = url.split('/');
      for (let i = 0; i < urlSplitted.length; i++) {
        let path = urlSplitted[i];
        if (path.length === 0) continue;
        this.applyActivatedRoute(appMenu.menuItems, path)
      }
      appMenu.isActivatedHomeRoute = false;
    } else {
      appMenu.isActivatedHomeRoute = true;
    }
    this.completeActivatedRouteTree(appMenu.menuItems);
    this.currentUrl = url;
    this.appMenu.next(appMenu);
  }

  private subscribeToRouterNavigation(router: Router) {
    router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        let navigationEnd: NavigationEnd = val;
        let appMenu = this.appMenu.getValue();
        this.updateUrl(appMenu, navigationEnd.urlAfterRedirects);
      }
    });
  }

  private generateMenuItemsLevel(items: AppMenuItem[], authInfo: AuthInfo) {
    let result: AppMenuItem[] = [];

    items.forEach(i => {
      if (i.RequiredAuth && !authInfo.IsAuthenticated) return;
      if (i.RequiredRoles.length > 0 && authInfo.AuthUserInfo === undefined && authInfo.AuthUserInfo.Roles === undefined) return;
      let isRolesAuth = true;
      i.RequiredRoles.forEach(r => {
        if (authInfo.AuthUserInfo.Roles.indexOf(r) === -1) isRolesAuth = false;
      })
      if (!isRolesAuth) return;
      const resultItem: AppMenuItem = {Id: i.Id, Caption: i.Caption, Position: i.Position, Path: i.Path, RequiredAuth: i.RequiredAuth, RequiredRoles: i.RequiredRoles, Children: undefined, IsHref: i.IsHref};
      if (i.Children !== undefined) {
        resultItem.Children = this.generateMenuItemsLevel(i.Children, authInfo);
        if (resultItem.Children.length == 0) return;
      }
      result.push(resultItem);
    });

    result = result.sort(function(a, b) {
      if (a.Position < b.Position)
        return -1;
      if (a.Position > b.Position)
        return 1;
      return 0;
    });
    
    return result;
  }

  private applyActivatedRoute(menuItems: AppMenuItem[], path: string): boolean {
    let applied = false;
    menuItems.forEach(i => {
      if (i.IsActivatedRoute === undefined && i.Path === path) { 
        i.IsActivatedRoute = applied = true; 
      } 
      if (i.Children !== undefined) this.applyActivatedRoute(i.Children, path);
    });
    return applied;
  }

  private clearActivatedRouteTree(menuItems: AppMenuItem[]) {
    menuItems.forEach(i => {
      i.IsActivatedRoute = undefined; 
      if (i.Children !== undefined) this.clearActivatedRouteTree(i.Children);
    });
  }

  private completeActivatedRouteTree(menuItems: AppMenuItem[]) {
    menuItems.forEach(i => {
      if (i.IsActivatedRoute === undefined) i.IsActivatedRoute = false; 
      if (i.Children !== undefined) this.completeActivatedRouteTree(i.Children);
    });
  }

}
