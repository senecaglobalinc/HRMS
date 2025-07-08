using System;

namespace HRMS.Project.Entities
{
    public class ProjectWorkFlow : BaseEntity
    {
        public int WorkFlowId { get; set; }
        public int SubmittedBy { get; set; }
        public int SubmittedTo { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int WorkFlowStatus { get; set; }
        public int ProjectId { get; set; }
        public string Comments { get; set; }

        public virtual Project Project { get; set; }
    }
}
