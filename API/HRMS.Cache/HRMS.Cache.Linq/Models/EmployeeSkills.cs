using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Cache.Linq.Models
{
    public class EmployeeSkills
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string PrimarySkills { get; set; }
        public string SecondarySkills { get; set; }
        public string Grade { get; set; }
        public Decimal? Experience { get; set; }
        public string ProjectName { get; set; }
        public int? ProjectId { get; set; }
        public bool? IsBillable { get; set; }
        public bool? IsCritical { get; set; }
        public bool? IsSkillPrimary { get; set; }
        public string LeadName { get; set; }
        public string ManagerName { get; set; }
        public decimal AllocationPercentage { get; set; }
        public string SkillName { get; set; }
    }
}
