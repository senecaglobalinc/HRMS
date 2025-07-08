using System;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AttendanceReportFilter
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string RoleName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? ProjectId { get; set; }
        public bool? IsLeadership { get; set; }
    }
}
