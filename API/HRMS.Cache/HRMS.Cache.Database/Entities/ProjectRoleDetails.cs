using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class ProjectRoleDetails
    {
        public int RoleAssignmentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
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
