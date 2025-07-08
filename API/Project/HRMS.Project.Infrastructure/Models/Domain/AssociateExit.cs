using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class AssociateExit
    {
        public DateTime? ExitDate { get; set; }
        public int EmployeeId { get; set; }
        public bool? IsActive { get; set; }
      

    }
}
