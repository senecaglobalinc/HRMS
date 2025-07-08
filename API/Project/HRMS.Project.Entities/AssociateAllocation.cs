using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class AssociateAllocation : BaseEntity
    {
        /// <summary>
        /// AssociateAllocationId 
        /// </summary>
        public int AssociateAllocationId { get; set; }

        /// <summary>
        /// Trid 
        /// </summary>
        public int? Trid { get; set; }

        /// <summary>
        /// ProjectId 
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// EmployeeId 
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// RoleMasterId 
        /// </summary>
        public int? RoleMasterId { get; set; }

        /// <summary>
        /// AllocationPercentage 
        /// </summary>
        public int? AllocationPercentage { get; set; }

        /// <summary>
        /// InternalBillingPercentage 
        /// </summary>
        public decimal? InternalBillingPercentage { get; set; }

        /// <summary>
        /// IsCritical 
        /// </summary>
        public bool? IsCritical { get; set; }

        /// <summary>
        /// EffectiveDate 
        /// </summary>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// AllocationDate 
        /// </summary>
        public DateTime? AllocationDate { get; set; }

        /// <summary>
        /// ReportingManagerId 
        /// </summary>
        public int? ReportingManagerId { get; set; }

        /// <summary>
        /// IsPrimary 
        /// </summary>
        public bool? IsPrimary { get; set; }

        /// <summary>
        /// IsBillable 
        /// </summary>
        public bool? IsBillable { get; set; }

        /// <summary>
        /// InternalBillingRoleId 
        /// </summary>
        public int? InternalBillingRoleId { get; set; }

        /// <summary>
        /// ClientBillingRoleId 
        /// </summary>
        public int? ClientBillingRoleId { get; set; }

        /// <summary>
        /// ReleaseDate 
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// ClientBillingPercentage
        /// </summary>
        public decimal? ClientBillingPercentage { get; set; }

        /// <summary>
        /// ProgramManagerId
        /// </summary>
        public int? ProgramManagerId { get; set; }

        /// <summary>
        /// LeadId
        /// </summary>
        public int? LeadId { get; set; }


        /// <summary>
        /// AllocationPercentageNavigation
        /// </summary>
        public virtual AllocationPercentage AllocationPercentageNavigation { get; set; }

        /// <summary>
        /// Project
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Tr
        /// </summary>
        public virtual TalentRequisition Tr { get; set; }
    }
}
