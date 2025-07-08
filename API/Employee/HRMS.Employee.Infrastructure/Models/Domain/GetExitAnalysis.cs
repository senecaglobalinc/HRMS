using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class GetExitAnalysis
    {
       public string EmployeeCode { get; set; }
        public int AssociateExitId { get; set; }
        public string RootCause { get; set; }
        public string ActionItem { get; set; }
        public string Responsibility { get; set; }
        public DateTime? TagretDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public string Remarks { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int EmployeeId { get; set; }

        public int ExitTypeId { get; set; }
        public string ExitType { get; set; }
       
        public int? ExitReasonId { get; set; }
        public string ExitReasonDetail { get; set; }
        public DateTime? ActualExitDate { get; set; }
        public DateTime? ResignationDate { get; set; }
        public DateTime? ExitDate { get; set; }

        public string EmployeeName { get; set; }
        public string SummaryOfExitFeedback { get; set; }
    }
}
