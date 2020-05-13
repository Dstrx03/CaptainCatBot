import { AppMenuItem, AppMenu } from './appMenu';
import { AppRoles } from './appRoles';

export class AppRoutesItem {
  Id: string;
  Path: string;
  IsHref: boolean;
  RequiredAuth: boolean;
  RequiredRoles: string[];
  Parent?: AppRoutesItem;

  getRouterLink(): string {
    return this.getRouterLinkRecursive(this, null);
  }

  getFullRelativePath(): string {
    return `/${this.getRouterLinkRecursive(this, null)}`;
  }

  getRouterLinkRecursive(item: AppRoutesItem, path?: string): string {
    if (item.IsHref) return item.Path;
    path = path === null || path === undefined ? item.Path : `${item.Path}/${path}`;
    if (item.Parent !== undefined && item.Parent !== null) return this.getRouterLinkRecursive(item.Parent, path);
    return path;
  }
}

export class AppRoutes {

  // Had to hardcode relations between Angular's routes and application menu items. Unfortunately due Angular's decorators constraints 
  // it's impossible to wire up routes and menu items through runtime generation of routes (AppRoutesRegistry.AppRoutesSet) and 
  // menu items (AppMenuItemsRegistry.AppMenuItemsSet) from some abstract structure (AppRoutes class members, for example). 
  // TODO: Create a tool that generate AppRoutesRegistry.AppRoutesSet & AppMenuItemsRegistry.AppMenuItemsSet code from abstract structure.
  
  static readonly Home: AppRoutesItem = <AppRoutesItem>{
    Id: 'home', 
    Path: '', 
    IsHref: false, 
    RequiredAuth: false,
    RequiredRoles: []
  };
  static readonly Login: AppRoutesItem = <AppRoutesItem>{
    Id: 'login', 
    Path: 'Login', 
    IsHref: false, 
    RequiredAuth: false,
    RequiredRoles: []
  };
  static readonly Dashboard: AppRoutesItem = <AppRoutesItem>{
    Id: 'dashboard', 
    Path: 'Dashboard', 
    IsHref: false, 
    RequiredAuth: true,
    RequiredRoles: []
  };
  static readonly Telegram: AppRoutesItem = <AppRoutesItem>{
    Id: 'telegram', 
    Path: 'Telegram', 
    IsHref: false, 
    RequiredAuth: true,
    RequiredRoles: []
  };
  static readonly Status: AppRoutesItem = <AppRoutesItem>{
    Id: 'telegram.status', 
    Path: 'Status', 
    IsHref: false, 
    RequiredAuth: true,
    RequiredRoles: [AppRoles.Admin],
    Parent: AppRoutes.Telegram
  };
  static readonly System: AppRoutesItem = <AppRoutesItem>{
    Id: 'system', 
    Path: 'System', 
    IsHref: false, 
    RequiredAuth: true,
    RequiredRoles: []
  };
  static readonly Users: AppRoutesItem = <AppRoutesItem>{
    Id: 'system.users', 
    Path: 'Users', 
    IsHref: false, 
    RequiredAuth: true,
    RequiredRoles: [AppRoles.Admin],
    Parent: AppRoutes.System
  };
  static readonly InternalServices: AppRoutesItem = <AppRoutesItem>{
    Id: 'system.internal_services', 
    Path: 'InternalServices', 
    IsHref: false, 
    RequiredAuth: true,
    RequiredRoles: [AppRoles.Admin],
    Parent: AppRoutes.System
  };
  static readonly SystemLogging: AppRoutesItem = <AppRoutesItem>{
    Id: 'system.system_logging', 
    Path: 'SystemLogging', 
    IsHref: false,  
    RequiredAuth: true,
    RequiredRoles: [AppRoles.Admin],
    Parent: AppRoutes.System
  };
  static readonly HangfireDashboard: AppRoutesItem = <AppRoutesItem>{
    Id: 'system.hangfire', 
    Path: '../../hangfire', 
    IsHref: true, 
    RequiredAuth: true,
    RequiredRoles: [AppRoles.Admin],
    Parent: AppRoutes.System
  };

  // ====== Static functions ======  
  static initMembersFunctions(): void {
    Object.keys(this).forEach(key => {
      let appRoutesItem = this[key] as AppRoutesItem;
      if (!(appRoutesItem instanceof Function)) {
        appRoutesItem.getRouterLink = AppRoutesItem.prototype.getRouterLink;
        appRoutesItem.getFullRelativePath = AppRoutesItem.prototype.getFullRelativePath;
        appRoutesItem.getRouterLinkRecursive = AppRoutesItem.prototype.getRouterLinkRecursive;
      }
    });
  }

  static findById(id: string): AppRoutesItem {
    let keys = Object.keys(this);
    for (let i = 0; i < keys.length; i++){
      let appRoutesItem = this[keys[i]] as AppRoutesItem;
      if (!(appRoutesItem instanceof Function) && appRoutesItem.Id === id) return appRoutesItem; 
    }
    return null;
  }

  static getRouterLink(item: AppMenuItem): string {
    let appRoutesItem = this.findById(item.Id);
    if (appRoutesItem !== null) return appRoutesItem.getRouterLink();
    return null;
  }
}

AppRoutes.initMembersFunctions();