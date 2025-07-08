export class AssociateSkillSearch {
    IsBillable: boolean;
    IsCritical: boolean;
    IsnonBillable: boolean;
    IsnonCritical: boolean;
    IsPrimary: boolean;
    IsSecondary: boolean;

    SkillID:number;
    SkillIds:string;    
    selectedSkillIds?:Skill[];

    EmployeeId: number;
    Experience: number;
    ProjectId: number;

    Designation: string;
    EmployeeName: string;
    Grade: string;
    LeadName: string;
    PrimarySkill: string;
    ProjectName: string;
    ManagerName: string;
    SecondarySkill: string;
    Allocationpercentage:number;
}

export class Skill{
    Id:string;
    value:string
}
