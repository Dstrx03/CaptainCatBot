import { AppRoles } from './appRoles';

export class AppMenuItem {
    Id: string;
    Caption: string;
    Position: number;
    Path: string;
    RequiredAuth: boolean;
    RequiredRoles: string[];
    Children: AppMenuItem[];
}

export class AppMenu {
    static Items: AppMenuItem[] = [
        { Id: 'dashb', Caption: 'Dashboard', Position: 100, Path: 'Dashboard', RequiredAuth: true, RequiredRoles: [], Children: undefined },
        { Id: 'sys', Caption: 'System', Position: 200, Path: 'System', RequiredAuth: true, RequiredRoles: [AppRoles.Admin], Children: [
            { Id: 'sys.usrs', Caption: 'Users', Position: 100, Path: 'Users', RequiredAuth: true, RequiredRoles: [AppRoles.Admin], Children: undefined }
        ]},
    ];
}