using HRMS.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
   public  class ActivityChecklist
    {
        public int ProjectId { get; set; }
        public int DepartmentId { get; set; }
        public string Remarks { get; set; }
        public int StatusId { get; set; }
        public bool? IsActive { get; set; }
        public string Type { get; set; }
        public List<ProjectClosureActivityDetail> ActivityDetails { get; set; }
    }
}
