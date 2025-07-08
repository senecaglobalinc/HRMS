using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Project;
using System.ComponentModel;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class ProjectClosureReportRequest: Entities.ProjectClosureReport
    {
        public int ProjectId { get; set; }
        public string type { get; set; }
       
    }
}
