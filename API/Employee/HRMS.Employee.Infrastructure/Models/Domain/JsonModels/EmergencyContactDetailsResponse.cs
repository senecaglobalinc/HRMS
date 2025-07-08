using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmergencyContactDetailsResponse 
    {        
        /// <summary>
        /// EmployeeId
        /// </summary>
        public Guid PersonalInfoId { get; set; }

        /// <summary>
        /// ContactType
        /// </summary>
        public string ContactType { get; set; }

        /// <summary>
        /// ContactName
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Relationship
        /// </summary>
        public int Relationship { get; set; }

        /// <summary>
        /// AddressLine1
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// AddressLine2
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// TelePhoneNo
        /// </summary>
        public string TelePhoneNo { get; set; }

        /// <summary>
        /// MobileNo
        /// </summary>
        public string MobileNo { get; set; }

        /// <summary>
        /// EmailAddress
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// IsPrimary
        /// </summary>
        public bool? IsPrimary { get; set; }

        public bool IsActive { get; set; }


    }
}
