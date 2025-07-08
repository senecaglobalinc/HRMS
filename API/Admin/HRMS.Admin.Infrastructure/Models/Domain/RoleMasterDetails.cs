using System;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class RoleMasterDetails
    {
        public int RoleMasterId { get; set; }
        public string RoleName { get; set; }
        public int SGRoleID { get; set; }
        public Nullable<int> PrefixID { get; set; }
        public Nullable<int> SuffixID { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string RoleDescription { get; set; }
        public string KeyResponsibilities { get; set; }
        public string EducationQualification { get; set; }
        public Nullable<int> KRAGroupId { get; set; }
        public string PrefixName { get; set; }
        public string SuffixName { get; set; }
        public string SGRoleName { get; set; }
    }
}
