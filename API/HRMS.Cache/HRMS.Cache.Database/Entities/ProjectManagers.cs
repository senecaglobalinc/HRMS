using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class ProjectManagers
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int? ProjectId { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? ProgramManagerId { get; set; }
        public int? LeadId { get; set; }

        public virtual Projects Project { get; set; }
    }
}
