using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class AssociateAllocation 
    {
        /// <summary>
        /// AssociateAllocationId 
        /// </summary>
        public int? AssociateAllocationId { get; set; }

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
        /// IsActive
        /// </summary>
        public bool IsActive { get; set; }

        public string ProjectName { get; set; }


        public string ReportingManagerName { get; set; }
        public string LeadName { get; set; }
        public string ProgramManagerName { get; set; }

    }

    public class CompetencyAreaMananagers
    {
        public int? ReportingManagerId { get; set; }
        public int? ProgramManagerId { get; set; }
        public int? LeadId { get; set; }
    }

    public class ActiveAllocationDetails
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? ClientId { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public int? ProgramManagerId { get; set; }
        public bool? IsPrimary { get; set; }
        public int EmployeeId { get; set; }
        public string ClientName { get; set; }
        public int? RoleMasterId { get; set; }
        public string RoleDescription { get; set; }
    }
}
