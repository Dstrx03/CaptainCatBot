import { Injectable } from '@angular/core';
import { AuthInfo } from '../../../models/authInfo';
import { AppMenuItem, AppMenu } from './models/appMenuItems';

@Injectable({
  providedIn: 'root'
})
export class MenuItemsService {

  constructor() {}

  generateMenuItemsForAuthInfo(authInfo: AuthInfo): AppMenuItem[] {
    return this.generateLevel(AppMenu.Items, authInfo);
  }

  private generateLevel(items: AppMenuItem[], authInfo: AuthInfo) {
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
      if (i.Children !== undefined) resultItem.Children = this.generateLevel(i.Children, authInfo);
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

}
