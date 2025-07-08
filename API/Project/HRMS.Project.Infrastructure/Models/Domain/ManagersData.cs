using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ManagersData
    {
        public string ReportingManagerName { get; set; }
        public string LeadName { get; set; }
        public string ProgramManagerName { get; set; }
        public int? ProgramManagerId { get; set; }
        public int? LeadId { get; set; }
        public int? ReportingManagerId { get; set; }
    }
}
