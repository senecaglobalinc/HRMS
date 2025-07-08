using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class EmployeeType : BaseEntity
    {
        /// <summary>
        /// EmployeeTypeId
        /// </summary>
        public int EmployeeTypeId { get; set; }

        /// <summary>
        /// EmployeeTypeCode
        /// </summary>
        public string EmployeeTypeCode { get; set; }

        /// <summary>
        /// EmpType
        /// </summary>
        public string EmpType { get; set; }
    }
}
