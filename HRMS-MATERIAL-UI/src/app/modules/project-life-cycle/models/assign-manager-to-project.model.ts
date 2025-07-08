import { GenericType } from "../../master-layout/models/dropdowntype.model";
//import { GenericType } from '../models/index'

export class Employee {
    public EmployeeId: number;
    public ManagerId: number;
    public ProjectId: number;
    public ReportingManagerId: number;
    public LeadId: number;
    public ProgramManager: GenericType;
    public ReportingManager: GenericType;
    public Lead: GenericType;
    public LeadName: string;
    public ReportingManagerName: string;
    public ProgramManagerName: string;
    public EffectiveDate:string;
}

export class DepartmentAssociates {
    public EmployeeId: number;
    public AssociateName: string;
    public DepartmentId: number;
    public EmployeeName: string;
}
