using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class ProjectsHistory
    {
        public int ProjectHistoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
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
    }
}
