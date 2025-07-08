using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class FinanceReportFilter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? ProjectId { get; set; }
    }

    public class UtilizationReportFilter
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public int ClientId { get; set; }
        public int AllocationPercentageId { get; set; }
        public int ProgramManagerId { get; set; }
        public int IsBillable { get; set; } = -1;
        public int IsCritical { get; set; } = -1;        
    }   
}
