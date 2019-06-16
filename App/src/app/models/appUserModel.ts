export interface AppUserModel {
    Id: string;
    UserName: string;
    Email: string;
    Password: string;
    OldPassword: string;
    Roles: string[];
    RolesView: string;
}
