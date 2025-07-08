import { Department } from "./department.model";
import { DepartmentDetails } from "../../master-layout/models/role.model";

export class KeyFunction extends DepartmentDetails{
  SGRoleName: string;
  SGRoleID: number;
  IsActive: boolean
  Department : Department;
}