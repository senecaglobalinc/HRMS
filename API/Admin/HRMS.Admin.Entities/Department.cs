using System.Collections.Generic;

namespace HRMS.Admin.Entities
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

        /// <summary>
        /// DepartmentType
        /// </summary>
        public virtual DepartmentType DepartmentType { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<RoleMaster> RoleMasters { get; set; }
        public virtual ICollection<SGRole> SGRoles { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }

}
