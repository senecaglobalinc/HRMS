using System;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string KeyResponsibilities { get; set; }
        public string EducationQualification { get; set; }
    }
}
