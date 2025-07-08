using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ExitDashboardResponse
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string StatusCode { get; set; }
        public string StatusDesc { get; set; }
        public string RevokeStatusCode { get; set; }
        public string RevokeStatusDesc { get; set; }
        public string RevokeComment { get; set; }
        public DateTime ExitDate { get; set; }
        public string SubStatusCode { get; set; }
        public string SubStatusDesc { get; set; }
        public bool TransitionRequired { get; set; }
    }
}
