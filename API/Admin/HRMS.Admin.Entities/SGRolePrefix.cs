using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class SGRolePrefix : BaseEntity
    {
        /// <summary>
        /// PrefixID
        /// </summary>
        public int PrefixID { get; set; }

        /// <summary>
        /// PrefixName
        /// </summary>
        public string PrefixName { get; set; }

        public virtual ICollection<RoleMaster> RoleMasters { get; set; }
    }
}
