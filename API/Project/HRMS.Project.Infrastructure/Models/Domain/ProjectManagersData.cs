using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ProjectManagersData
    {
        public string ReportingManagerName { get; set; }
        public string LeadName { get; set; }
        public string ProgramManagerName { get; set; }
        public int? ProgramManagerId { get; set; }
        public int? LeadId { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? ManagerId { get; set; }
        public DateTime? EffectiveDate { get; set; } 
    }
}
