using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateHistory : BaseEntity
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
        /// Designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// GradeId
        /// </summary>
        public Nullable<int> GradeId { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public Nullable<int> DepartmentId { get; set; }

        /// <summary>
        /// PracticeAreaId
        /// </summary>
        public Nullable<int> PracticeAreaId { get; set; }


    }
}
