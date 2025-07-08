using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models.Domain
{
    public class GradeRoleType
    {
        public int GradeRoleTypeId { get; set; }
        public int GradeId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleTypeId { get; set; }
        public string GradeDescription { get; set; }
        public string RoleTypeDescription { get; set; }
        public bool? IsActive { get; set; }
    }
}
