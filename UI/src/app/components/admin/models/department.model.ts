import { GenericType } from "../../../models/dropdowntype.model";
export class Department {
  DepartmentId: number;
  DepartmentCode: string;
  Description: string;
  DepartmentTypeId: number;
  DepartmentTypeDescription:string;
  DepartmentHeadId: number;
  DepartmentHeadName: string;
  DepartmentHead?: GenericType;
}
