import { Role } from "../../../models/associate-skills.model";

export class UserRoles {
    UserRolesID: number;
    RoleId: number;
    RoleName: string;
    UserName: string;
    UserId: number;
    EmailAddress: string;
    IsPrimary: boolean;
    IsActive: boolean;
    DepartmentId: number;
    IsAssigned: boolean;
    Role : Role;
}