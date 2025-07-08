using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Report.Infrastructure.Models.Domain
{
   public class AssociateInformationReport
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
        public string JoinDate { get; set; }
        public string ClientName { get; set; }
        public decimal? Allocationpercentage { get; set; }
        public string LeadName { get; set; }
        public string ReportingManagerName { get; set; }
        public string ProgramManagerName { get; set; }
        public bool? IsResigned { get; set; }
        public string ResignationDate { get; set; }
        public string LastWorkingDate { get; set; }
        public bool? IsLongLeave { get; set; }
        public string LongLeaveStartDate { get; set; }
        public string TentativeJoinDate { get; set; }
        public bool? IsFutureProjectMarked { get; set; }
        public string FutureProjectName { get; set; }
        public string FutureProjectTentativeDate { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? LastBillingDate { get; set; }

    }
    
}
