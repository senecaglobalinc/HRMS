using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociatesRMDetails
    {
        public int ReportingManagerId { get; set; }
        public  List<Associate> Associates { get; set; }
    }

    public class Associate
    {
       public int EmployeeId { get; set; }

    }

    public class AssociateRM
    {
        public int EmployeeId { get; set; }
        public string EmpName { get; set; }
        public int? ReportingManagerId { get; set; }
        public string ReportingManager { get; set; }

    }
}
