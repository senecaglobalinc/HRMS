using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class RoleMaster : BaseEntity
    {
        /// <summary>
        /// RoleId
        /// </summary>
        public int RoleMasterID { get; set; }
        
        /// <summary>
        /// SGRoleID
        /// </summary>
        public int SGRoleID { get; set; }

        /// <summary>
        /// PrefixID
        /// </summary>
        public Nullable<int> PrefixID { get; set; }

        /// <summary>
        /// SuffixID
        /// </summary>
        public Nullable<int> SuffixID { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public Nullable<int> DepartmentId { get; set; }

        /// <summary>
        /// RoleDescription
        /// </summary>
        public string RoleDescription { get; set; }
        
        /// <summary>
        /// KeyResponsibilities
        /// </summary>
        public string KeyResponsibilities { get; set; }

        /// <summary>
        /// EducationQualification
        /// </summary>
        public string EducationQualification { get; set; }

        /// <summary>
        /// KRAGroupId
        /// </summary>
        public Nullable<int> KRAGroupId { get; set; }

        public virtual SGRole SGRole { get; set; }
        public virtual SGRolePrefix SGRolePrefix { get; set; }
        public virtual SGRoleSuffix SGRoleSuffix { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<CompetencySkill> CompetencySkills { get; set; }
    }
}
