import { EntityModel } from "./entity.model";

export class UserModel extends EntityModel{    
    name: string = "";
    firstName: string  ="";
    lastName: string = "";
    fullName: string = "";
    userName: string = "";
    isEmailConfirmed: boolean = false;
    isActive: boolean = false;
    email: string = "";
    password: string = "";
    }