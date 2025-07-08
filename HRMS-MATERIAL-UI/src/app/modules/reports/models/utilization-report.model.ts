export class UtilizationReportResponse{
    AssociateCode: string;
    AssociateName: string;
    DateOfJoining: Date;
    EmploymentStartDate: string;
    LastWorkingDate: string;
    LastBillingDate: string;
    ProjectsWorked: string;
    TimeTakenForBillable: number;
    TotalWorkedDays: number;
    TotalBillingDays: number;
    TotalNonBillingDays: number;
    BillingDaysPercentage: number; 
    Active: boolean;  
    Resigned:string; 
    CompetencyGroup: string;
    ExperienceExcludingCareerBreak:number;
    Fresher: boolean;
    IsFresher: string;
    Skills: string;
}
