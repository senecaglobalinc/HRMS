using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class Role : BaseEntity
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }  
        public Nullable<int> DepartmentId { get; set; }
        public string KeyResponsibilities { get; set; }
        public string EducationQualification { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual Department Department { get; set; }


    }
}
