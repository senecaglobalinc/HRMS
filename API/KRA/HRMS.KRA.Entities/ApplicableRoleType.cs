using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Entities
{
    /// <summary>
    /// ApplicableRoleType
    /// </summary>
    public class ApplicableRoleType : BaseEntity
    {
        /// <summary>
        /// ApplicableRoleTypeId
        /// </summary>
        public int ApplicableRoleTypeId { get; set; }

        /// <summary>
        /// FinancialYearId
        /// </summary>
        public int FinancialYearId { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// GradeRoleTypeId
        /// </summary>
        public int GradeRoleTypeId { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public virtual Status Status { get; set; }

        /// <summary>
        /// Definitions
        /// </summary>
        public virtual ICollection<Definition> Definitions { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
