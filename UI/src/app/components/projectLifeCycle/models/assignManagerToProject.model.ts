import { GenericType } from "src/app/models/dropdowntype.model";
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
}

export class DepartmentAssociates {
    public EmployeeId: number;
    public EmployeeName: string;
    public DepartmentId: number;
}