using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class ProjectRoles
    {
        public int ProjectRoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int ProjectId { get; set; }
        public int RoleMasterId { get; set; }
        public string Responsibilities { get; set; }

        public virtual Projects Project { get; set; }
    }
}
