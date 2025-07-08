export class AttendanceReport{
     EmployeeCode: string;
     EmployeeName: string; 
     DepartmentName: string;   
     ProjectName: string;
     ReportingManagerName: string;
     TotalDaysWorked: number; 
     TotalWorkingDays: number;
     TotalHolidays: number;
     WorkFromHome : boolean;
     TotalWFHDays : number;
     TotalWFODays : number;
     TotalLeaves : number;
     CompliancePrecentage:number;
}

export class AttendanceReportFilter
{
     ProjectId: number;
     EmployeeName: string;
     ManagerName: string;
     ManagerId: number;
     RoleName: string;
     FromDate : string;
     ToDate: string; 
     isLeadership: boolean;
     EmployeeCode: string;
     EmployeeId: number;
}

export class AttendanceDetailReport
{
     title: string;
     date: string;            
}

export class AttendanceDetailReportData
{
     eventData: AttendanceDetailReport[];
     filter:AttendanceReportFilter;  
     daysWorked : number;
     employeeName:string;        
}

export class  SelectItem {
     value: number;
     label: string;
   }
