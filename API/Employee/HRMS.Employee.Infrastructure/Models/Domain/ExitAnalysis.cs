using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ExitAnalysis: BaseEntity
    {

        public int EmployeeId { get; set; }
        public string SubmitType { get; set; }
        public int AssociateExitId { get; set; }
        public string RootCause { get; set; }
        public string ActionItem { get; set; }
        public string Responsibility { get; set; }
        public DateTime? TagretDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public string Remarks { get; set; }
        public int StatusId { get; set; }

    }
}
