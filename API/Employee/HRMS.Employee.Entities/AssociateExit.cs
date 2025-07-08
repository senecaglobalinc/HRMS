using System;
using System.Collections.Generic;

namespace HRMS.Employee.Entities
{
    public class AssociateExit : BaseEntity
    {
        /// <summary>
        /// AssociateExitId
        /// </summary>
        public int AssociateExitId { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// ExitTypeId
        /// </summary>
        public int ExitTypeId { get; set; }

        /// <summary>
        /// ActualExitReasonId
        /// </summary>
        public int? ActualExitReasonId { get; set; }

        /// <summary>
        /// ActualExitReasonDetail
        /// </summary>
        public string ActualExitReasonDetail { get; set; }

        /// <summary>
        /// ExitReasonId
        /// </summary>
        public int? ExitReasonId { get; set; }

        /// <summary>
        /// ExitReasonDetail
        /// </summary>
        public string ExitReasonDetail { get; set; }

        /// <summary>
        /// ResignationRecomendation
        /// </summary>
        public string ResignationRecommendation { get; set; }        

        /// <summary>
        /// CalculatedExitDate
        /// </summary>
        public DateTime? CalculatedExitDate { get; set; }

        /// <summary>
        /// ActualExitDate
        /// </summary>
        public DateTime? ActualExitDate { get; set; }

        /// <summary>
        /// RehireEligibility
        /// </summary>
        public bool? RehireEligibility { get; set; }

        /// <summary>
        /// RehireEligibilityDetail
        /// </summary>
        public string RehireEligibilityDetail { get; set; }

        /// <summary>
        /// ResignationWithdrawn
        /// </summary>
        public bool ResignationWithdrawn { get; set; }

        /// <summary>
        /// WithdrawReason
        /// </summary>
        public string WithdrawReason { get; set; }

        /// <summary>
        /// WithdrawRemarks
        /// </summary>
        public string WithdrawRemarks { get; set; }

        /// <summary>
        /// TransitionRequired
        /// </summary>
        public bool? TransitionRequired { get; set; }

        /// <summary>
        /// TransitionRemarks
        /// </summary>
        public string TransitionRemarks { get; set; }

        /// <summary>
        /// ImpactOnClientDelivery
        /// </summary>
        public bool? ImpactOnClientDelivery { get; set; }

        /// <summary>
        /// ImpactOnClientDeliveryDetail
        /// </summary>
        public string ImpactOnClientDeliveryDetail { get; set; }

        /// <summary>
        /// Tenure
        /// </summary>
        public decimal? Tenure { get; set; }

        /// <summary>
        /// Retained
        /// </summary>
        public bool? Retained { get; set; }

        /// <summary>
        /// RetainedDetail
        /// </summary>
        public string RetainedDetail { get; set; }

        /// <summary>
        /// LegalExit
        /// </summary>
        public bool LegalExit { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Resignation Date
        /// </summary>
        public DateTime? ResignationDate { get; set; }

        /// <summary>
        /// Exit Date
        /// </summary>
        public DateTime? ExitDate { get; set; }

        /// <summary>
        ///Assocaite Remarks
        /// </summary>
        public string AssociateRemarks{ get; set; }
        public int? ProjectId{ get; set; }
        public int? AssociateAllocationId{ get; set; }
        /// <summary>
        ///Absconded From Date
        /// </summary>
        
        public virtual ICollection<AssociateExitWorkflow> AssociateExitWorkflow { get; set; }
        public virtual ICollection<AssociateExitActivity> AssociateExitActivity { get; set; }
        public virtual ICollection<AssociateExitInterview> AssociateExitInterview { get; set; }
        public virtual ICollection<TransitionPlan> TransitionPlan { get; set; }
        public virtual AssociateExitAnalysis AssociateExitAnalysis { get; set; }
        public virtual ICollection<Remarks> Remarks { get; set; }
    }
}
