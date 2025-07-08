using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
  public  class AssociateInformationReports
    {
       
            public string EmployeeCode { get; set; }
            public string AssociateName { get; set; }
            public string Designation { get; set; }
            public string Grade { get; set; }
            public string Experience { get; set; }
            public string Technology { get; set; }
            public string ProjectName { get; set; }
            public string Skill { get; set; }
            public bool? IsCritical { get; set; }
            public int ? TechnologyCount { get; set; }
            public int EmployeeId { get; set; }
            public DateTime? LastBillingDate { get; set; }
    }
}
