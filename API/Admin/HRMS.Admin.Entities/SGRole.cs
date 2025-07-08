using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class SGRole : BaseEntity
    {
        /// <summary>
        /// SGRoleID
        /// </summary>
        public int SGRoleID { get; set; }

        /// <summary>
        /// SGRoleName
        /// </summary>
        public string SGRoleName { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<RoleMaster> RoleMasters { get; set; }
    }
}
