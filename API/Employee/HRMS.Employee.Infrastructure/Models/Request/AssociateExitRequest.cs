using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Request
{
    public class AssociateExitRequest
    {
        public int AssociateExitId { get; set; }
        public int AssociateAbscondId { get; set; }
        public string SubmitType { get; set; }
        public int? ExitTypeId { get; set; }
        public string ExitType { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int EmployeeId { get; set; }
        public int? ReasonId { get; set; }
        public string ExitCause { get; set; }
        public string ReasonDetail { get; set; }
        public DateTime ExitDate { get; set; }
        public string ResignationRecommendation { get; set; }
        public bool? RehireEligibility { get; set; }
        public string RehireEligibilityDetail { get; set; }
        public bool? TransitionRequired { get; set; }
        public bool? ImpactOnClientDelivery { get; set; }
        public string ImpactOnClientDeliveryDetail { get; set; }
        public bool? LegalExit { get; set; }
        public string Status { get; set; }
        public string StatusDesc { get; set; }
        public DateTime? ResignationDate { get; set; }
        public string AssociateRemarks { get; set; }
        public DateTime CalculatedExitDate { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public DateTime DateOfJoin { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string Department { get; set; }
        public string Technology { get; set; }
        public int ReportingManagerId { get; set; }
        public int ProgramManagerId { get; set; }
        public string ReportingManager { get; set; }
        public Decimal TotalExperience { get; set; }
        public Decimal ExperiencePriorSG { get; set; }
        public int DepartmentId { get; set; }
        public string ProgramManager { get; set; }
        public string WithdrawReason { get; set; }
        public string Quarter { get; set; }
        public DateTime ActualExitDate { get; set; }
        public string FinancialYear { get; set; }
        public Decimal ServiceWithSG { get; set; }
        public int? HRAId { get; set; }
        public decimal? Tenure { get; set; }
        public int? NoticePeriodInDays { get; set; }
        public string BloodGroup { get; set; }
        public int LeadId { get; set; }
        public string Lead { get; set; }
        public int? AssociateAllocationId { get; set; }
    }
}
