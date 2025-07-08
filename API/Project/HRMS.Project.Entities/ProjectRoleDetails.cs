using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class ProjectRoleDetails : BaseEntity
    {
        public int RoleAssignmentId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }
        public int? RoleMasterId { get; set; }
        public bool? IsPrimaryRole { get; set; }
        public int? StatusId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string RejectReason { get; set; }
    }
}
