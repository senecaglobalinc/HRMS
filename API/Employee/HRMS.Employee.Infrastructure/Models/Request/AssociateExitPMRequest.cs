using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Request
{
    public class AssociateExitPMRequest 
    {
        public int AssociateExitId { get; set; }
        public int EmployeeId { get; set; }        
        public int? ExitTypeId { get; set; }
        public bool? RehireEligibility { get; set; }
        public string RehireEligibilityDetail { get; set; }
        public bool? ImpactOnClientDelivery { get; set; }
        public string ImpactOnClientDeliveryDetail { get; set; }
        public int ProgramManagerId { get; set; }
    }
}
