using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class ClientBillingRoles : BaseEntity
    {
        /// <summary>
        /// ClientBillingRoleId
        /// </summary>
        public int ClientBillingRoleId { get; set; }

        /// <summary>
        /// ClientBillingRoleCode
        /// </summary>
        public string ClientBillingRoleCode { get; set; }

        /// <summary>
        /// ClientBillingRoleName
        /// </summary>
        public string ClientBillingRoleName { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// NoOfPositions
        /// </summary>
        public int? NoOfPositions { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// EndDate
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// ClientBillingPercentage
        /// </summary>
        public int? ClientBillingPercentage { get; set; }

        /// <summary>
        /// ClientId
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Project
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }
    }
}
