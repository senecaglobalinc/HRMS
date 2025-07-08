
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
    employeeCode: string;
    departmentId : string;
    allowedWfoInHrms: boolean;
}

export class Profile{
      EmployeeName: string;
      Designation:string;
      Grade:string;
      DateOfJoin:string;
      BloodGroup:string;
      ReportingManager:string;
      ProgramManager:string;
      Lead:string;
      DepartmentId:number;
}


