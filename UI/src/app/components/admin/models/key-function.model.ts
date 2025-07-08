import { Department } from "./department.model";
import { DepartmentDetails } from "src/app/models/role.model";

export class KeyFunction extends DepartmentDetails{
  SGRoleName: string;
  SGRoleID: number;
  IsActive: boolean
  Department : Department;
}