export class ApplicableRoleType {
  financialYearId: number;
  departmentId: number;
  gradeRoleTypeId: number;
  //gradeRoleTypeId:number[];
  statusId: number;
  applicableRoleTypeId: number;
  gradeId: number;
  roleTypeId: number;
  status: string;
}

export class ApplicableRoleTypeData {
  ApplicableRoleTypeId: number;
  RoleTypeId: number;
  GradeRoleTypeId: number;
  FinancialYearId: number;
  DepartmentId: number;
  FinancialYearName: string;
  RoleTypeName: string;
  Grade: string;
  DepartmentName: string;
  GradeId: number;
}
