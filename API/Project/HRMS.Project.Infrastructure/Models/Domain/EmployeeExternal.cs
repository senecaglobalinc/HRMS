using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class EmployeeExternal
    {
        public int EmployeeId { get; set; }
        public int CompetencyGroup { get; set; }
        public int DepartmentId { get; set; }
        public string External { get; set; }
        public int ReportingManagerId { get; set; }
    }
}
