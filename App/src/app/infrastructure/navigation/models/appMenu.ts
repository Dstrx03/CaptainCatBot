import { AppRoles } from './appRoles';
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
        { 
            Id: AppRoutes.Dashboard.Id, 
            Caption: 'Dashboard', 
            Position: 100, 
            Path: AppRoutes.Dashboard.Path, 
            IsHref: AppRoutes.Dashboard.IsHref, 
            RequiredAuth: true, 
            RequiredRoles: [], 
            Children: undefined 
        },
        { 
            Id: AppRoutes.Telegram.Id, 
            Caption: 'Telegram', 
            Position: 200, 
            Path: AppRoutes.Telegram.Path, 
            IsHref: AppRoutes.Telegram.IsHref, 
            RequiredAuth: true, 
            RequiredRoles: [], 
            Children: [
                { 
                    Id: AppRoutes.Status.Id, 
                    Caption: 'Status', 
                    Position: 100, 
                    Path: AppRoutes.Status.Path, 
                    IsHref: AppRoutes.Status.IsHref, 
                    RequiredAuth: true, 
                    RequiredRoles: [AppRoles.Admin], 
                    Children: undefined 
                }
            ]
        },
        { 
            Id: AppRoutes.System.Id, 
            Caption: 'System', 
            Position: 300, 
            Path: AppRoutes.System.Path, 
            IsHref: AppRoutes.System.IsHref, 
            RequiredAuth: true, 
            RequiredRoles: [AppRoles.Admin], 
            Children: [
                { 
                    Id: AppRoutes.Users.Id, 
                    Caption: 'Users', 
                    Position: 100, 
                    Path: AppRoutes.Users.Path, 
                    IsHref: AppRoutes.Users.IsHref, 
                    RequiredAuth: true, 
                    RequiredRoles: [AppRoles.Admin], 
                    Children: undefined 
                },
                { 
                    Id: AppRoutes.InternalServices.Id, 
                    Caption: 'Internal Services', 
                    Position: 200, 
                    Path: AppRoutes.InternalServices.Path, 
                    IsHref: AppRoutes.InternalServices.IsHref, 
                    RequiredAuth: true, 
                    RequiredRoles: [AppRoles.Admin], 
                    Children: undefined 
                },
                { 
                    Id: AppRoutes.SystemLogging.Id, 
                    Caption: 'System Logging', 
                    Position: 300, 
                    Path: AppRoutes.SystemLogging.Path, 
                    IsHref: AppRoutes.SystemLogging.IsHref, 
                    RequiredAuth: true, 
                    RequiredRoles: [AppRoles.Admin], 
                    Children: undefined 
                },
                { 
                    Id: AppRoutes.HangfireDashboard.Id, 
                    Caption: 'Hangfire Dashboard', 
                    Position: 400, 
                    Path: AppRoutes.HangfireDashboard.Path, 
                    IsHref: AppRoutes.HangfireDashboard.IsHref, 
                    RequiredAuth: true, 
                    RequiredRoles: [AppRoles.Admin], 
                    Children: undefined 
                }
            ]
        },
    ];
}