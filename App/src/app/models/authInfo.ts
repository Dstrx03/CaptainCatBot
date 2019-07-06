export class AuthInfo {
    IsAuthenticated: boolean;
    AuthUserInfo: AuthUserInfo;
    RegisteredRoles: ParsedAppRole[];

    constructor() {
        this.IsAuthenticated = false;
        this.AuthUserInfo = {
            Name: '',
            Roles: []
        };
        this.RegisteredRoles = [];
    }
}

export class AuthUserInfo {
    Name: string;
    Roles: string[];
}

export class ParsedAppRole {
    ViewName: string;
    SystemName: string;
}
