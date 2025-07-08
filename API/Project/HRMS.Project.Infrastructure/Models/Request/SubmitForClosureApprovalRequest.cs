using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class SubmitForClosureApprovalRequest
    {
        public int projectId { get; set; }
        public string userRole { get; set; }
        public int employeeId { get; set; } 
    }
}
