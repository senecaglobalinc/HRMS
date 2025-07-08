using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    /// <summary>
    /// EmployeeSkillsModel class
    /// </summary>
    public class EmployeeSkills
    {
        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// EmployeeName
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// PrimarySkills
        /// </summary>
        public string PrimarySkills { get; set; }

        /// <summary>
        /// SecondarySkills
        /// </summary>
        public string SecondarySkills { get; set; }

        /// <summary>
        /// Grade
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Experience
        /// </summary>
        public Decimal? Experience { get; set; }

        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// IsBillable
        /// </summary>
        public bool? IsBillable { get; set; }

        /// <summary>
        /// IsCritical
        /// </summary>
        public bool? IsCritical { get; set; }

        /// <summary>
        /// IsSkillPrimary
        /// </summary>
        public bool? IsSkillPrimary { get; set; }

        /// <summary>
        /// LeadName
        /// </summary>
        public string LeadName { get; set; }

        /// <summary>
        /// ManagerName
        /// </summary>
        public string ManagerName { get; set; }

        /// <summary>
        /// AllocationPercentage
        /// </summary>
        public decimal AllocationPercentage { get; set; }

        /// <summary>
        /// SkillName
        /// </summary>
        public string SkillName { get; set; }
    }
}
