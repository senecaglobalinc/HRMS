using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class TransitionPlan : BaseEntity
    {
        /// <summary>
        /// TransitionPlanId
        /// </summary>
        public int TransitionPlanId { get; set; }
        /// <summary>
        /// AssociateExitId
        /// </summary>
        public int AssociateExitId { get; set; }
        /// <summary>
        /// ProjectClosureId
        /// </summary>
        public int ProjectClosureId { get; set; }
        /// <summary>
        /// AssociateReleaseId
        /// </summary>
        public int AssociateReleaseId { get; set; }       
        /// <summary>
        /// TransitionFrom
        /// </summary>
        public int TransitionFrom { get; set; }
        /// <summary>
        /// TransitionTo
        /// </summary>
        public int TransitionTo { get; set; }
        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// EndDate
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// KnowledgeTransferred
        /// </summary>
        public bool KnowledgeTransferred { get; set; }
        /// <summary>
        /// KnowledgeTransaferredRemarks
        /// </summary>
        public string KnowledgeTransaferredRemarks { get; set; }
        /// <summary>
        /// Others
        /// </summary>
        public string Others { get; set; }
        /// <summary>
        /// StatusId
        /// </summary>
        public int StatusId { get; set; }

        public virtual AssociateExit AssociateExit { get; set; }
        public virtual ICollection<TransitionPlanDetail> TransitionPlanDetail { get; set; }

    }
}
