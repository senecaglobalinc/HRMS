using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmployeeProfileStatus
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string NotificationType { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string HRAdvisor {get;set;}
    }
}
