using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Report.Infrastructure.Models.Domain
{
   public class AssociateSkillSearch
    {        
        public int EmployeeId { get; set; }
        public bool IsBillable { get; set; }
        public bool IsCritical { get; set; }
        public decimal Experience { get; set; }       
        public string Designation { get; set; }
        public string EmployeeName { get; set; }
        public string Grade { get; set; }
        public string LeadName { get; set; }
        public string ManagerName { get; set; }
        public string ProjectName { get; set; }
        public string PrimarySkill { get; set; }
        public string SecondarySkill { get; set; }       
        public decimal Allocationpercentage { get; set; }
    }

    //public class SkillSearch
    //{
    //    public bool IsBillable { get; set; }
    //    public bool IsCritical { get; set; }
    //    public bool IsPrimary { get; set; }
    //    public bool IsnonBillable { get; set; }
    //    public bool IsnonCritical { get; set; }
    //    public bool IsSecondary { get; set; }
    //    public decimal? Experience { get; set; }
    //    public int EmployeeId { get; set; }
    //    public int ProjectId { get; set; }
    //    public int SkillId { get; set; }
    //    public string Designation { get; set; }
    //    public string EmployeeName { get; set; }
    //    public string Grade { get; set; }
    //    public string LeadName { get; set; }
    //    public string ManagerName { get; set; }
    //    public string ProjectName { get; set; }
    //    public string PrimarySkill { get; set; }
    //    public string SecondarySkill { get; set; }
    //    public string SkillIds { get; set; }
    //    public string SkillNames { get; set; }
    //    public string SkillName { get; set; }
    //    public decimal? Allocationpercentage { get; set; }
    //}

    public class SkillSearchEmployee
    {
        public int EmployeeId { get; set; }
        public int SkillId { get; set; }
        public bool IsBillable { get; set; }
        public bool IsCritical { get; set; }
        public decimal Experience { get; set; }
        public int DesignationId { get; set; }
        public string EmployeeName { get; set; }
        public int GradeId { get; set; }
        public string LeadName { get; set; }
        public string ManagerName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool IsSkillPrimary { get; set; }
        public string PrimarySkill { get; set; }
        public string SecondarySkill { get; set; }
        public decimal Allocationpercentage { get; set; }
    }
}
