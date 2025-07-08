using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateExitInterviewReview : BaseEntity
    {
        /// <summary>
        /// AssociateExitInterviewReviewId
        /// </summary>
        public int AssociateExitInterviewReviewId { get; set; }

        /// <summary>
        /// AssociateExitInterviewId
        /// </summary>
        public int AssociateExitInterviewId { get; set; }

        /// <summary>
        /// ShowInitialRemarks
        /// </summary>
        public bool ShowInitialRemarks { get; set; }

        /// <summary>
        /// FinalRemarks
        /// </summary>
        public string FinalRemarks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual AssociateExitInterview AssociateExitInterview { get; set; }
    }
}
