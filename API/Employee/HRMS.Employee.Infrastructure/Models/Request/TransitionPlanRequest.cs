using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Request
{
    public class TransitionPlanRequest
    {
        public string TransistionType { get; set; }
        public int EmployeeId { get; set; }
       
        
        public int AssociateExitId { get; set; }
       
        public int ProjectClosureId { get; set; }
      
        public int AssociateReleaseId { get; set; }
      
        public int ProjectId { get; set; }
      
        public int TransitionFrom { get; set; }
       
        public int TransitionTo { get; set; }
       
        public DateTime? StartDate { get; set; }
       
        public DateTime? EndDate { get; set; }
      
        public bool KnowledgeTransferred { get; set; }
       
        public string KnowledgeTransaferredRemarks { get; set; }
      
        public string Others { get; set; }
       
        public int StatusId { get; set; }

    }
}
