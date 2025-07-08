export class ProjectRole {
RoleAssignmentId : number;
EmployeeId : any="";
ProjectId : any="";
RoleId : any="";
RoleName: string;
KeyResponsibilities: string;
IsPrimaryRole : boolean;
FromDate : Date; 
ToDate : Date;
StatusId : number;
LeadId: number;
ManagerId : number;
LeadName : string;
ManagerName : string;
RejectReason: string;
}

export class ProjectRoleParam{
    ProjectRoleData: ProjectRole;
    Status: string;
}

export class SkillsData {
    SkillsSubmittedForApprovalId?: number;
    StatusCode: string;
    SkillName:string;
    EmployeeName:string;
    experience?: number;
    LastUsed?: number;
    isPrimary?: boolean;
    ProficiencyLevel: string;
    Name:string;
    proficiencyLevelId?: number;
    ID?: number;
    CompetencyAreaID?: number;
    skillID: number;
    empID?: number;
    IsActive?: boolean;
    SkillGroupID?: number;
    SubmittedBy?:number;
    SubmittedTo?:number;
    StatusId?: number;
    RequisitionId?: number;
    WorkFlowId?: number;
    SkillExperience?: number;

    }