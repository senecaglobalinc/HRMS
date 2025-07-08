using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ProgramManager
    {
        public int UserRolesId { get; set; }
        public int EmployeeId { get; set; }
        public string ManagerName { get; set; }
    }
}
