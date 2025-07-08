using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class RevokeRequest
    {
        public int EmployeeId { get; set; }
        public string RevokeReason { get; set; }
        public string SubmitType { get; set; }
        public string Comment { get; set; }
    }
}
