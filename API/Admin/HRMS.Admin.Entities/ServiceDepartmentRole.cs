using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class ServiceDepartmentRole : BaseEntity
    {
        public int ServiceDepartmentRoleId { get; set; }
        public int RoleMasterId { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
    }
}
