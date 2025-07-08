using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateExitWorkflow : BaseEntity
    {
        public int AssociateExitWorkflowId { get; set; }
        public int SubmittedBy { get; set; }
        public int SubmittedTo { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int WorkflowStatus { get; set; }
        public int AssociateExitId { get; set; }
        public string Comments { get; set; }
        public virtual AssociateExit AssociateExit { get; set; }
    }
}
