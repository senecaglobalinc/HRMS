using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ExitInterviewReviewCreateRequest
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
        /// FinalRemarks
        /// </summary>
        public string FinalRemarks { get; set; }

        /// <summary>
        /// ShowInitialRemarks
        /// </summary>
        public bool ShowInitialRemarks { get; set; }
    }
}
