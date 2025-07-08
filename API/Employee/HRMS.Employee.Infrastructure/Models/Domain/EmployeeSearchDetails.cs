using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmployeeSearchDetails
    { 
        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// EmpCode
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// EmpName
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// MiddleName
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }
    }
}
