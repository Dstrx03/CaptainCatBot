export class LoginViewModel {
    UserName: string;
    Password: string;
    RememberMe: boolean;

    constructor() {
        this.UserName = '';
        this.Password = '';
        this.RememberMe = false;
    }
}
