export class AuthInfo {
    IsAuthenticated: boolean;
    AuthUserInfo: AuthUserInfo;
    RegisteredRoles: ParsedAppRole[];
}

export class AuthUserInfo {
    Name: string;
    Roles: string[];
}

export class ParsedAppRole {
    ViewName: string;
    SystemName: string;
}
