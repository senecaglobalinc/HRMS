using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class Associate
    {
        public int EmployeeId { get; set; }
        public string EmpName { get; set; }
        public int ProjectId { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

}
