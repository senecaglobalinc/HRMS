using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class ProjectRole : BaseEntity
    {
        /// <summary>
        /// ProjectRoleId
        /// </summary>
        public int ProjectRoleId { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public int RoleId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<ProjectSkillsRequired> ProjectSkillsRequired { get; set; }
        public virtual ICollection<ProjectRoleAssociateMapping> ProjectRoleAssociateMapping { get; set; }
    }
}
