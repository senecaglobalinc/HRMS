using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class RolesDepartmentsList
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public int? DepartmentHeadId { get; set; }
    }
}
