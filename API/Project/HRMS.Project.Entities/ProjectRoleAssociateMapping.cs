using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class ProjectRoleAssociateMapping : BaseEntity
    {
        /// <summary>
        /// ProjectRoleAssociateMappingId
        /// </summary>
        public int ProjectRoleAssociateMappingId { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// ProjectRoleId
        /// </summary>
        public int ProjectRoleId { get; set; }

        /// <summary>
        /// AssociateId
        /// </summary>
        public int AssociateId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ProjectRole ProjectRole { get; set; }
    }
}
