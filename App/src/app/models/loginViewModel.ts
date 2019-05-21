export class LoginViewModel {
    Email: string;
    Password: string;
    RememberMe: boolean;

    constructor() {
        this.Email = '';
        this.Password = '';
        this.RememberMe = false;
    }
}
