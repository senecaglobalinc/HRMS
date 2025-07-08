using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ProjectDetails
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string PracticeAreaCode { get; set; }
        public int? AllocationPercentage { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int PracticeAreaId { get; set; }
    }

    public class ProjectDTO
    {
        public List<Projects> Projects { get; set; }
    }
    public class Projects
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
    }
}
