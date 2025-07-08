using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateDesignation: BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// DesignationId
        /// </summary>
        public int? DesignationId { get; set; }

        /// <summary>
        /// GradeId
        /// </summary>
        public int? GradeId { get; set; }

        /// <summary>
        /// FromDate
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// ToDate
        /// </summary>
        public DateTime? ToDate { get; set; }
    }
}
