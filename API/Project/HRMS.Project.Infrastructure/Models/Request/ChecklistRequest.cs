using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class ChecklistRequest:Entities.ProjectClosureActivity
    {
        public int ProjectClosureId { get; set; }
        public int DepartmentId { get; set; }
        public int ProjectId { get; set; }
        public int ActivityId { get; set; }
        //public int RemarksId { get; set; }
    }
}
