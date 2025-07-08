using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ResourceAllocation
    {
        public string AssociateCode { get; set; }
        public string AssociateName { get; set; }
        public decimal AllocationPercentage { get; set; }
        public string InternalBillingRoleName { get; set; }
        public string ClientBillingRoleName { get; set; }
        public int ClientBillingRoleId { get; set; }        
        public string IsPrimaryProject { get; set; }
        public string IsCriticalResource { get; set; }
    }
}
