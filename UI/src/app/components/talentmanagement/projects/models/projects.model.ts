import { GenericType } from "../../../../models/dropdowntype.model";

//import { GenericType } from '../models/index';
export class ProjectDetails {
    ProjectId: number;
    ProjectCode: string;
    ProjectName: string;
    ProjectType:number;
}
export class ProjectsData extends ProjectDetails {

    ActualStartDate: string;
    ActualEndDate: string;
    PlannedEndDate: string;
    PlannedStartDate: string;
    CustomerId: number;
    PracticeAreaId: number;
    ReportingManagerId: number;
    ManagerId: number; //ProgramManagerId
    LeadId: any;
    ReportingManager: GenericType;
    ProgramManager: GenericType;
    Lead: GenericType;
    ProgramManagerName: string;
    ReportingManagerName: string;
    LeadName: string;
    IsActive: boolean;
    EmpCode: number;
    actualStartDate: Date;
    actualEndDate: Date;
    plannedEndDate: Date;
    plannedStartDate: Date;
    DepartmentId: number;
    RoleIds: any[];
    DeletedRoleIds: any[];
    CustomerName : string;
    ClientBillingRoles : any[];
}
