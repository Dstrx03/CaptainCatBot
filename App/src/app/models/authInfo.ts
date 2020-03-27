export class AuthInfo {
    IsAuthenticated: boolean;
    AuthUserInfo: AuthUserInfo;
    RegisteredRoles: ParsedAppRole[];

    constructor() {
        this.IsAuthenticated = false;
        this.AuthUserInfo = {
            Name: '',
            Roles: [],
            RolesView: ''
        };
        this.RegisteredRoles = [];
    }
}

export class AuthUserInfo {
    Name: string;
    Roles: string[];
    RolesView: string;
}

export class ParsedAppRole {
    ViewName: string;
    SystemName: string;
}
