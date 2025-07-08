using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class Contacts : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// AddressType
        /// </summary>
        public string AddressType { get; set; }

        /// <summary>
        /// AddressLine1
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// AddressLine2
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }
    }
}
