using System;

namespace HRMS.Project.Entities
{
    public class ProjectClosureWorkflow : BaseEntity
    {
        public int ProjectClosureWorkflowId { get; set; }
        public int SubmittedBy { get; set; }
        public int SubmittedTo { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int WorkflowStatus { get; set; }
        public int ProjectClosureId { get; set; }
        public string Comments { get; set; }

        public virtual ProjectClosure ProjectClosure { get; set; }
    }
}
