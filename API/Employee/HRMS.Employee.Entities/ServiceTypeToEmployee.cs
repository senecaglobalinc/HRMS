using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class ServiceTypeToEmployee : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// ServiceTypeId
        /// </summary>
        public int ServiceTypeId { get; set; }

        public virtual Employee employee { get; set; }
      
    }
}
