using System;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AttendanceReport
    {        
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string ProjectName { get; set; }
        public string ReportingManagerName { get; set; }
        public decimal TotalDaysWorked { get; set; }  
        public decimal TotalWFHDays { get; set; }  
        public decimal TotalWFODays { get; set; }  
        public decimal TotalWorkingDays { get; set; }
        public decimal CompliancePrecentage { get; set; }
        public int TotalHolidays { get; set; }
        public decimal TotalLeaves { get; set; }
        public bool WorkFromHome { get; set; }
    }
}
