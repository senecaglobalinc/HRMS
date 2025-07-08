using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ClearanceRequest
    {
        public int employeeId { get; set; }
        public DateTime ActualExitDate { get; set; }
        public string UserRole { get; set; }
        public string RemarksByHRA { get; set; }
        public string RemarksByHRM { get; set; }
        public string RemarksByOperationsHead { get; set; }
        public bool UpdateExitDateRequired { get; set; } = false;

    }
}
