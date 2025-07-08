using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class ClientBillingRoles
    {
        public int ClientBillingRoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ClientBillingRoleCode { get; set; }
        public string ClientBillingRoleName { get; set; }
        public int ProjectId { get; set; }
        public int? NoOfPositions { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClientBillingPercentage { get; set; }

        public virtual Projects Project { get; set; }
    }
}
