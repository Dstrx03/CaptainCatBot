import { AppRoutes, AppRoutesItem } from './appRoutes';

export class AppMenuItem {
    Id: string;
    Caption: string;
    Position: number;
    Path: string;
    RequiredAuth: boolean;
    RequiredRoles: string[];
    Children: AppMenuItem[];
    IsHref: boolean;
    IsActivatedRoute?: boolean;

    constructor(appRoutesItem: AppRoutesItem, caption: string, position: number, children?: AppMenuItem[]) {
        this.Id = appRoutesItem.Id;
        this.Caption = caption;
        this.Position = position;
        this.Path = appRoutesItem.Path;
        this.IsHref = appRoutesItem.IsHref;
        this.RequiredAuth = appRoutesItem.RequiredAuth;
        this.RequiredRoles = appRoutesItem.RequiredRoles;
        this.Children = children === undefined || children === null ? undefined : children;
    }
}

export class AppMenu {
    constructor() {
        this.nonItemActivatedRouteId = null;
        this.menuItems = [];
    }

    nonItemActivatedRouteId: string; 
    menuItems: AppMenuItem[];

    isItemActivatedRoute(item: AppRoutesItem): boolean {
        return this.nonItemActivatedRouteId === item.Id;
    }
}

export class AppMenuItemsRegistry {
    static readonly AppMenuItemsSet: AppMenuItem[] = [
        new AppMenuItem(AppRoutes.Dashboard, 'Dashboard', 100),
        new AppMenuItem(AppRoutes.Telegram, 'Telegram', 200, [
            new AppMenuItem(AppRoutes.Status, 'Status', 100)
        ]),
        new AppMenuItem(AppRoutes.System, 'System', 300, [
            new AppMenuItem(AppRoutes.Users, 'Users', 100),
            new AppMenuItem(AppRoutes.InternalServices, 'Internal Services', 200),
            new AppMenuItem(AppRoutes.SystemLogging, 'System Logging', 300),
            new AppMenuItem(AppRoutes.HangfireDashboard, 'Hangfire Dashboard', 400)
        ])
    ];
}