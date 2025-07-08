using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ExitInterviewRequest 
    {
        /// <summary>
        /// Id
        /// </summary>
        public int AssociateExitInterviewId { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

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
        /// IsNotified
        /// </summary>
        public bool IsNotified { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// SystemInfo
        /// </summary>
        public string SystemInfo { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
