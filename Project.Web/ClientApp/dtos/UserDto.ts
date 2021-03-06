/*** THIS FILE IS AUTOMATICALLY GENERATED FROM DtoTemplate.txt TEMPLATE --- DO NOT MODIFY !!! ***/

 

import { IBaseDto, BaseDto } from "./BaseDto";

// TS Interface for: project.Domain.Dtos.UserDto
export interface IUserDto extends IBaseDto {
    
    Id: number;
    UserName: string;
    FirstName: string;
    SecondName: string;
    Phone: string;
    Email: string;
    NewPassword: string;
    PasswordSet: boolean;
    InRoles: string[];
    InFacilitys: number[];
    Deleted: boolean;
}
 
// TS Class for project.Domain.Dtos.UserDto
export class UserDto extends BaseDto implements IUserDto {
        
    Id!: number;
    UserName!: string;
    FirstName!: string;
    SecondName!: string;
    Phone!: string;
    Email!: string;
    NewPassword!: string;
    PasswordSet!: boolean;
    InRoles: string[] = [];
    InFacilitys: number[] = [];
    Deleted!: boolean;

    constructor() {        
        super();
    }
}
