using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class ProjectDetails
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int ProjectStateId { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int DepartmentId { get; set; }
        public int PracticeAreaId { get; set; }
        public int? DomainId { get; set; }
        public int DepartmentHeadId { get; set; }
    }
}
