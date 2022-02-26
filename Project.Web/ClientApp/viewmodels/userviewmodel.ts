export class UserViewModel {

    constructor() {
        this.name = "";
        this.permissions = [];
        this.needSetPassword = false;
    }
    name: string;
    permissions: string[];
    needSetPassword: boolean;
}