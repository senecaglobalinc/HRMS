using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ExitInterviewReviewGetAllResponse
    {
        /// <summary>
        /// AssociateExitInterviewId
        /// </summary>
        public int AssociateExitInterviewId { get; set; }

        /// <summary>
        /// AssociateExitId
        /// </summary>
        public int AssociateExitId { get; set; }

        /// <summary>
        /// AssociateName
        /// </summary>
        public string AssociateName { get; set; }

        /// <summary>
        /// InitialRemarks
        /// </summary>
        public string InitialRemarks { get; set; }

        /// <summary>
        /// InitialRemarksNoHtml
        /// </summary>
        public string InitialRemarksNoHtml { get; set; }

        /// <summary>
        /// FinalRemarks
        /// </summary>
        public string FinalRemarks { get; set; }

        /// <summary>
        /// FinalRemarksNoHtml
        /// </summary>
        public string FinalRemarksNoHtml { get; set; }

        /// <summary>
        /// ShowInitialRemarks
        /// </summary>
        public bool ShowInitialRemarks { get; set; }
    }
}
