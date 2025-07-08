using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Response
{
    public class ProjectRespose : BaseServiceResponse
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public int ClientId { get; set; }
        public int? StatusId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int ProjectStateId { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int DepartmentId { get; set; }
        public int PracticeAreaId { get; set; }
        public int? DomainId { get; set; }
        public bool? IsActive { get; set; }
        public int ProgramManagerId { get; set; }
        public string UserRole { get; set; }
    }
}
