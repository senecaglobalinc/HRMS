using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ClientBillingRole
    {
        public int ClientBillingRoleId { get; set; }
        public string ClientBillingRoleName { get; set; }
        public int? NoOfPositions { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClientBillingPercentage { get; set; }
        public bool? IsActive { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal Percentage { get; set; }
        public int AllocationCount { get; set; }
    }
}
