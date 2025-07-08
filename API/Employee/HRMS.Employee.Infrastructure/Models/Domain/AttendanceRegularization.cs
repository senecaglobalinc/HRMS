using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
   public class AttendanceRegularization
    {
        public int? ReportingManagerId { get; set; }
        public string ReportingManager { get; set; }
      public List<string> AttendanceRegularizationDates { get; set; }
    }

    public class AttendanceRegularizationFilter
    {
        public string AssociateId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
