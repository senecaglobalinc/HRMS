using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ProjectClosureActivities
    {
        public int ProjectClosureActivityDetailId { get; set; }
        public int ProjectClosureActivityId { get; set; }
        public int ActivityId { get; set; }
        public string Value { get; set; }
        public string ActivityRemarks { get; set; }
        public int ProjectClosureId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentRemarks { get; set; }
        public int StatusId { get; set; }
    }
}
