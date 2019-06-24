import { AppRoles } from './appRoles';

export class AppMenuItem {
    Id: string;
    Caption: string;
    Position: number;
    Path: string;
    RequiredAuth: boolean;
    RequiredRoles: string[];
    Children: AppMenuItem[];
    IsHref: boolean;
}

export class AppMenu {
    static Items: AppMenuItem[] = [
        { Id: 'dashb', Caption: 'Dashboard', Position: 100, Path: 'Dashboard', IsHref: false, RequiredAuth: true, RequiredRoles: [], Children: undefined },
        { Id: 'tele', Caption: 'Telegram', Position: 200, Path: 'Telegram', IsHref: false, RequiredAuth: true, RequiredRoles: [], Children: [
            { Id: 'tele.status', Caption: 'Status', Position: 100, Path: 'Status', IsHref: false, RequiredAuth: true, RequiredRoles: [AppRoles.Admin], Children: undefined },
        ]},
        { Id: 'sys', Caption: 'System', Position: 300, Path: 'System', IsHref: false, RequiredAuth: true, RequiredRoles: [AppRoles.Admin], Children: [
            { Id: 'sys.usrs', Caption: 'Users', Position: 100, Path: 'Users', IsHref: false, RequiredAuth: true, RequiredRoles: [AppRoles.Admin], Children: undefined },
            { Id: 'sys.intsvcs', Caption: 'Internal Services', Position: 200, Path: 'InternalServices', IsHref: false, RequiredAuth: true, RequiredRoles: [AppRoles.Admin], Children: undefined },
            { Id: 'sys.hangfire', Caption: 'Hangfire Dashboard', Position: 300, Path: '../../hangfire', IsHref: true, RequiredAuth: true, RequiredRoles: [AppRoles.Admin], Children: undefined },
        ]},
    ];
}