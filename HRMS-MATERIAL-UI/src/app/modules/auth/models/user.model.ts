
export interface IUser {
    email?: string;
    password?: string;
    firstName?: string;
    lastName?: string;
    fullName?: string;
    roleName?: string;
    employeeId?: number;
    roles?: string;
}

export class User {
    email!: string;
    password?: string;
    firstName?: string;
    lastName?: string;
    fullName?: string;
    roleName?: string;
    employeeId?: number;
    roles?: string;
    departmentId?: number;
    employeeCode: string;
}
