using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class ApproveOrRejectClosureRequest
    {
        public int projectId{get;set;}
        public string status { get; set; }
        public int employeeId { get; set; }
    }
}
