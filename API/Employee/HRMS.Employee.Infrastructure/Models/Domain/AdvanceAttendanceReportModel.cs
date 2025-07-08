using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AdvanceAttendanceReportModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeMail { get; set; }
        public string ProjectName { get; set; }
        public string ReportingManager { get; set; }
        public decimal CompliancePercentage { get; set; }
        public Dictionary<string, string> AttendanceByDate { get; set; }
    }
}
