using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class ProjectRoles : BaseEntity
    {
        public int ProjectRoleId { get; set; }
        public int ProjectId { get; set; }
        public int RoleMasterId { get; set; }
        public string Responsibilities { get; set; }

        public virtual Project Project { get; set; }
    }
}
