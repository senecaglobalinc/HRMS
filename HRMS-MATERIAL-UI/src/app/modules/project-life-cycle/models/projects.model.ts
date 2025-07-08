import { GenericType } from "../../master-layout/models/dropdowntype.model";

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
}
export class ProjectClosureReport  {

    
     ProjectId: number;
    // clientFeedback: string;
    // deliveryPerformance: string;
    rejectRemarks:string;
    type:string;
    ValueDelivered: string;
    ManagementChallenges: string;
    TechnologyChallenges: string;
    EngineeringChallenges: string;
    BestPractices  : string  ;
    LessonsLearned  : string  ;
    ReusableArtifacts  : string  ;
    ProcessImprovements  : string  ;
    Awards  : string  ;
    NewTechnicalSkills  : string  ;
    NewTools  : string  ;
    Remarks  : string  ;
   // caseStudy  : string  ;
    StatusId  : number;
    CurrentUser  : string  ;
    CreatedDate  : Date  ;
    ModifiedDate  : Date  ;
    SystemInfo  : string  ;
    IsActive  : boolean;
    CreatedBy  : string  ;
    ModifiedBy  : string;
    ClientFeedbackFile:any;
    DeliveryPerformanceFile:any;

  }

  export class ProjectClosureReject{
    ProjectId: number;
    rejectRemarks:string;

  }