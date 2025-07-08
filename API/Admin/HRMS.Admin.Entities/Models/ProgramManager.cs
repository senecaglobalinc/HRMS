using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities.Models
{
    public class ProgramManager : BaseEntity
    {
        public int UserRolesId { get; set; }
        public int EmployeeId { get; set; }
        public string ManagerName { get; set; }
    }
}
