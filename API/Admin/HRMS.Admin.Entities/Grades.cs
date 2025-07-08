using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class Grade : BaseEntity
    {
        /// <summary>
        /// GradeId
        /// </summary>
        public int GradeId { get; set; }

        /// <summary>
        /// GradeCode
        /// </summary>
        public string GradeCode { get; set; }

        /// <summary>
        /// GradeName
        /// </summary>
        public string GradeName { get; set; }

        public virtual ICollection<Designation> Designations { get; set; }
        public virtual ICollection<GradeRoleType> GradeRoleTypes { get; set; }
    }
}
