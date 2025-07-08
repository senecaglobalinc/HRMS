using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class SkillSearch
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? Experience { get; set; }
        public int? RoleMasterId { get; set; }
        public string RoleDescription { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationCode { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public bool? IsPrimary { get; set; }
        public bool? IsCritical { get; set; }
        public bool? IsBillable { get; set; }
        public int? CompetencyAreaId { get; set; }
        public string CompetencyAreaCode { get; set; }
        public int? SkillIgroupId { get; set; }
        public string SkillGroupName { get; set; }
        public int? SkillId { get; set; }
        public string SkillName { get; set; }
        public int? ProficiencyLevelId { get; set; }
        public string ProficiencyLevelCode { get; set; }
        public string EmployeeCode { get; set; }
        public int? ProjectId { get; set; }
        public bool? IsSkillPrimary { get; set; }
        public string DesignationName { get; set; }
        public int? LastUsed { get; set; }
        public int? SkillExperience { get; set; }
    }
}
