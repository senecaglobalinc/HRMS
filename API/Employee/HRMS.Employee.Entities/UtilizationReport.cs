using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class UtilizationReport
    {
        /// <summary>
        /// AssociateCode
        /// </summary>
        public string AssociateCode { get; set; }
        /// <summary>
        /// AssociateName
        /// </summary>
        public string AssociateName { get; set; }
        /// <summary>
        /// DateOfJoining
        /// </summary>
        public DateTime DateOfJoining { get; set; }
        /// <summary>
        /// EmploymentStartDate
        /// </summary>
        public DateTime? EmploymentStartDate { get; set; }
        /// <summary>
        /// LastWorkingDate
        /// </summary>
        public DateTime? LastWorkingDate { get; set; }
        /// <summary>
        /// ProjectsWorked
        /// </summary>
        public string ProjectsWorked { get; set; }
        /// <summary>
        /// TimeTakenForBillable
        /// </summary>
        public int TimeTakenForBillable { get; set; }
        /// <summary>
        /// TotalWorkedDays
        /// </summary>
        public int TotalWorkedDays { get; set; }
        /// <summary>
        /// TotalBillingDays
        /// </summary>
        public int TotalBillingDays { get; set; }
        /// <summary>
        /// TotalNonBillingDays
        /// </summary>
        public int TotalNonBillingDays { get; set; }
        /// <summary>
        /// BillingDaysPercentage
        /// </summary>
        public decimal BillingDaysPercentage { get; set; }
        /// <summary>
        /// ExperienceExcludingCareerBreak
        /// </summary>
        public decimal ExperienceExcludingCareerBreak { get; set; }
        /// <summary>
        /// CompetencyGroup
        /// </summary>
        public string CompetencyGroup { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Fresher
        /// </summary>
        public bool Fresher { get; set; }
        /// <summary>
        /// Skills
        /// </summary>
        public string Skills { get; set; }
        /// <summary>
        /// LastBillingDate
        /// </summary>
        public DateTime? LastBillingDate { get; set; }
    }
}
