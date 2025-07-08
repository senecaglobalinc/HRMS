using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class ProjectWorkFlow
    {
        public int WorkFlowId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int SubmittedBy { get; set; }
        public int SubmittedTo { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public int WorkFlowStatus { get; set; }
        public int ProjectId { get; set; }
        public string Comments { get; set; }

        public virtual Projects Project { get; set; }
    }
}
