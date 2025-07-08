using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class Department : BaseEntity
    {
        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// DepartmentCode
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// DepartmentHeadId
        /// </summary>
        public int? DepartmentHeadId { get; set; }

        /// <summary>
        /// DepartmentTypeId
        /// </summary>
        public int DepartmentTypeId { get; set; }
    }
}
