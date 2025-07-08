using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class UtilizationReportFilter
    {
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public bool? IsFutureProjectMarked { get; set; }
    }
}
