using System;
using System.Collections.Generic;

namespace HRMS.Employee.Entities
{
    public class AssociateExitInterview : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int AssociateExitInterviewId { get; set; }

        /// <summary>
        /// AssociateExitId
        /// </summary>
        public int AssociateExitId { get; set; }

        /// <summary>
        /// ReasonId
        /// </summary>
        public int ReasonId { get; set; }

        /// <summary>
        /// ReasonDetail
        /// </summary>
        public string ReasonDetail { get; set; }

        /// <summary>
        /// AlternateMobileNo
        /// </summary>
        public string AlternateMobileNo { get; set; }

        /// <summary>
        /// AlternateEmail
        /// </summary>
        public string AlternateEmail { get; set; }

        /// <summary>
        /// AlternateAddress
        /// </summary>
        public string AlternateAddress { get; set; }

        /// <summary>
        /// ShareEmploymentInfo
        /// </summary>
        public bool ShareEmploymentInfo { get; set; }

        /// <summary>
        /// IncludeExAssociateGroup
        /// </summary>
        public bool IncludeInExAssociateGroup { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// IncludeInExAssociateGroup
        /// </summary>
        public bool IsNotified { get; set; }

        public virtual AssociateExit AssociateExit { get; set; }

        public virtual ICollection<AssociateExitInterviewReview> AssociateExitInterviewReviews { get; set; }
    }
}
