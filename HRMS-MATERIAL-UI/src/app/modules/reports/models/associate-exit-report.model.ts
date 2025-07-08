export class AssociateExitReportResponse{
     AssociateCode: string;
     AssociateName: string;   
     Grade: string;
     Gender: string;
     Department: string;
     TechnologyGroup: string;
     JoinDate: Date;
     ExitDate: Date 
     Project: string; 
     ProgramManager: string;
     ReportingManager: string; 
     ExitType: string; 
     ExitCause: string; 
     RehireEligibility:boolean; 
     LegalExit:boolean;
     ImpactOnClientDelivery:boolean;
     ServiceTenure: number;
     ServiceTenureWithSG: number;
     ServiceTenurePriorToSG: number;
     FinancialYear: string;
     Quarter: string;
     RehireEligibilityStr:string; 
     LegalExitStr:string;
     ImpactOnClientDeliveryStr:string;
     ServiceTenureRange: string;
     ServiceTenureWithSGRange: string;
}

export class ChartData{
     Label: string;
     Value: number;  
} 

export class AssociateExitReportFilter
{
    ReportType: number;
    FromDate : string;
    ToDate: string;       
}

export class AssociateExitReportRequestForPm {
     FromDate:string;
     ToDate: string;
}
