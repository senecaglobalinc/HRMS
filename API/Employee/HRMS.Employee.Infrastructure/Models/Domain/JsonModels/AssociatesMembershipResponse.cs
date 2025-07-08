using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociatesMembershipResponse
    {
        /// <summary>
        /// PersonalInfoId
        /// </summary>
        public Guid PersonalInfoId { get; set; }

        /// <summary>
        /// ProgramTitle
        /// </summary>
        public string ProgramTitle { get; set; }

        /// <summary>
        /// ValidFrom
        /// </summary>
        public string ValidFrom { get; set; }

        /// <summary>
        /// Institution
        /// </summary>
        public string Institution { get; set; }

        /// <summary>
        /// Specialization
        /// </summary>
        public string Specialization { get; set; }

        /// <summary>
        /// ValidUpto
        /// </summary>
        public string ValidUpto { get; set; }

        public bool IsActive { get; set; }

    }
}
