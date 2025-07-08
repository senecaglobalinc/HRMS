using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmployeeInfo
    {
        public int AssociateId { get; set; }
        public string AssociateCode { get; set; }        
        public string AssociateName { get; set; }
        public int? ProgramManagerId { get; set; }
        public string ProgramManagerName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }        
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }        
        public int ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
    }
}
