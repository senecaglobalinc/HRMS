using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class PreviousEmploymentDetailsResponse 
    {        
        /// <summary>
        /// PersonalInfoId
        /// </summary>
        public Guid PersonalInfoId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// ServiceFrom
        /// </summary>
        public DateTime? ServiceFrom { get; set; }

        /// <summary>
        /// ServiceTo
        /// </summary>
        public DateTime? ServiceTo { get; set; }

        /// <summary>
        /// LeavingReason
        /// </summary>
        public string LeavingReason { get; set; }

        public bool IsActive { get; set; }
    }
}
