using HRMS.Employee.Infrastructure.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class TransitionDetail
    {
        public string TransistionType { get; set; }
        public int EmployeeId { get; set; }
        public string Type { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TransitionFrom { get; set; }
        public int TransitionTo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool KnowledgeTransferred { get; set; }
        public string KnowledgeTransferredRemarks { get; set; }
        public string Others { get; set; }
        public string Status { get; set; }
        public string StatusDesc { get; set; }
        public string Remarks { get; set; }
        public bool TransitionNotRequired { get; set; }

        public List<UpdateTransitionDetail> UpdateTransitionDetail { get; set; }
    }
}
