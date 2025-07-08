using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class SGRoleSuffix : BaseEntity
    {
        /// <summary>
        /// SuffixID
        /// </summary>
        public int SuffixID { get; set; }

        /// <summary>
        /// SuffixName
        /// </summary>
        public string SuffixName { get; set; }

        public virtual ICollection<RoleMaster> RoleMasters { get; set; }
    }
}
