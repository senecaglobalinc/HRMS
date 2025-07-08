using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Project;
using System.ComponentModel;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class ProjectRequest: Entities.Project
    {
        public int ManagerId { get; set; }
        public string UserRole { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? EmployeeId { get; set; }
        public int? LeadId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int DepartmentHeadId { get; set; }
        
    }
}
