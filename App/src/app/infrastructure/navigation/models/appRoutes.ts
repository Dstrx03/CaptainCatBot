import { AppMenuItem } from './appMenu';

export class AppRoutesItem {
  Id: string;
  Path: string;
  IsHref: boolean;
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
  static readonly Home: AppRoutesItem = {
    Id: 'home', 
    Path: '', 
    IsHref: false, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };
  static readonly Login: AppRoutesItem = {
    Id: 'login', 
    Path: 'Login', 
    IsHref: false, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };
  static readonly Dashboard: AppRoutesItem = {
    Id: 'dashboard', 
    Path: 'Dashboard', 
    IsHref: false, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };
  static readonly Telegram: AppRoutesItem = {
    Id: 'telegram', 
    Path: 'Telegram', 
    IsHref: false, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };
  static readonly Status: AppRoutesItem = {
    Id: 'telegram.status', 
    Path: 'Status', 
    IsHref: false, 
    Parent: AppRoutes.Telegram, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };
  static readonly System: AppRoutesItem = {
    Id: 'system', 
    Path: 'System', 
    IsHref: false, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };
  static readonly Users: AppRoutesItem = {
    Id: 'system.users', 
    Path: 'Users', 
    IsHref: false, 
    Parent: AppRoutes.System, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };
  static readonly InternalServices: AppRoutesItem = {
    Id: 'system.internal_services', 
    Path: 'InternalServices', 
    IsHref: false, 
    Parent: AppRoutes.System, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };
  static readonly SystemLogging: AppRoutesItem = {
    Id: 'system.system_logging', 
    Path: 'SystemLogging', 
    IsHref: false,  
    Parent: AppRoutes.System, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };
  static readonly HangfireDashboard: AppRoutesItem = {
    Id: 'system.hangfire', 
    Path: '../../hangfire', 
    IsHref: true, 
    Parent: AppRoutes.System, 
    getRouterLink: AppRoutesItem.prototype.getRouterLink,
    getFullRelativePath: AppRoutesItem.prototype.getFullRelativePath,
    getRouterLinkRecursive: AppRoutesItem.prototype.getRouterLinkRecursive
  };

  static getRouterLink(item: AppMenuItem): string {
    let routerLink = null;
    Object.keys(this).forEach(key => {
      let appRoutesItem = this[key] as AppRoutesItem;
      if (appRoutesItem !== null && appRoutesItem !== undefined && appRoutesItem.Id === item.Id) routerLink = appRoutesItem.getRouterLink(); 
    });
    return routerLink;
  }
}
