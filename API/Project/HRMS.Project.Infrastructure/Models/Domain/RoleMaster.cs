using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class RoleMaster
    {
        /// <summary>
        /// RoleId
        /// </summary>
        public int RoleMasterId { get; set; }
        public string RoleName { get; set; }

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
        public string PrefixName { get; set; }
        public string SuffixName { get; set; }
        public string SGRoleName { get; set; }
    }
}
