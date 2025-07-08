using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ProfessionalReferencesResponse
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
        /// Designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// CompanyName
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// CompanyAddress
        /// </summary>
        public string CompanyAddress { get; set; }

        /// <summary>
        /// OfficeEmailAddress
        /// </summary>
        public string OfficeEmailAddress { get; set; }

        /// <summary>
        /// MobileNo
        /// </summary>
        public string MobileNo { get; set; }

        public bool IsActive { get; set; }

    }
}
