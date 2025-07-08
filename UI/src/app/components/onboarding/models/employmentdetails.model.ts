export interface EmploymentDetails  {
    EmployeeId?: number;
    Id?: number;
    Name: string;
    Address: string;
    Designation: string;
    ServiceFrom: any;
    ServiceTo: any;
    LeavingReason: string;
    lastDrawnSalary?: number;
    // fromYear?: string;
    // toYear?: string
} 

export interface ProfessionalReferences {
    EmployeeId?: number;
    Id?: number;
    Name: string;
    CompanyName: string;
    Designation: string;
    MobileNo: string;
    companyAddress: string;
    lastDrawnSalary?: number;
    OfficeEmailAddress: string
} 


      