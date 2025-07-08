using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateExitReport
    {
        /// <summary>
        /// AssociateId
        /// </summary>
        public int AssociateId { get; set; }
        /// <summary>
        /// AssociateExitId
        /// </summary>
        public int AssociateExitId { get; set; }
        /// <summary>
        /// AssociateCode
        /// </summary>
        public string AssociateCode { get; set; }
        /// <summary>
        /// AssociateName
        /// </summary>
        public string AssociateName { get; set; }
        /// <summary>
        /// Grade
        /// </summary>
        public string Grade { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// TechnologyGroup
        /// </summary>
        public string TechnologyGroup { get; set; }
        /// <summary>
        /// DateOfJoining
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// DateOfExit
        /// </summary>
        public DateTime ExitDate { get; set; }
        /// <summary>
        /// DateOfResignation
        /// </summary>
        public DateTime? ResignedDate { get; set; }        
        /// <summary>
        /// Project
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// ProgramManager
        /// </summary>
        public string ProgramManager { get; set; }
        /// <summary>
        /// ReportingManager
        /// </summary>
        public string ReportingManager { get; set; }        
        /// <summary>
        /// FinancialYear
        /// </summary>
        public string FinancialYear { get; set; }
        /// <summary>
        /// Quarter
        /// </summary>
        public string Quarter { get; set; }
        /// <summary>
        /// ExitType
        /// </summary>
        public string ExitType { get; set; }
        /// <summary>
        /// ExitCause
        /// </summary>
        public string ExitCause { get; set; }
        /// <summary>
        /// RehireEligibility
        /// </summary>
        public bool RehireEligibility { get; set; }
        /// <summary>
        /// LegalExit
        /// </summary>
        public bool LegalExit { get; set; }
        /// <summary>
        /// ImpactOnClientDelivery
        /// </summary>
        public bool ImpactOnClientDelivery { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// ServiceTenure
        /// </summary>
        public Decimal ServiceTenure { get; set; }
        /// <summary>
        /// ServiceTenureWithSG
        /// </summary>
        public Decimal ServiceTenureWithSG { get; set; }
        /// <summary>
        /// ServiceTenurePriorToSG
        /// </summary>
        public Decimal ServiceTenurePriorToSG { get; set; }
        /// <summary>
        /// ServiceTenureRange
        /// </summary>
        public string ServiceTenureRange { get; set; }
        /// <summary>
        /// ServiceTenureWithSGRange
        /// </summary>
        public string ServiceTenureWithSGRange { get; set; }
    }
}
