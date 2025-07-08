using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmployeeSkillDetails
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal? Experience { get; set; }
        public int? NewExperience { get; set; }
        public int LastUsed { get; set; }
        public int? NewLastUsed { get; set; }
        public bool? IsPrimary { get; set; }
        public int? SkillId { get; set; }
        public string SkillName { get; set; }
        public string SkillCode { get; set; }
        public int SkillGroupId { get; set; }
        public string SkillGroupName { get; set; }
        public int CompetencyAreaId { get; set; }
        public string CompetencyAreaCode { get; set; }
        public int? ProficiencyLevelId { get; set; }
        public string ProficiencyLevelCode { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public int RoleId { get; set; }
        public int? ProficiencyIDByRM { get; set; }
        public string ProficiencyByRM { get; set; }
        public string Remarks { get; set; }
        public string CurrentProficiencyLevelCode { get; set; }
        public string RoleName { get; set; }
        public string Version { get; set; }
        public bool IsAlreadyExist { get; set; } = false;
    }

    public class EmployeeSkillSearch
    {
        public int SkillsSubmittedForApprovalId { get; set; }
        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int Experience { get; set; }
        public bool IsPrimary { get; set; }
        public int LastUsed { get; set; }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }        
        public string SkillCode { get; set; }
        public int SkillGroupId { get; set; }
        public string SkillGroupName { get; set; }
        public int CompetencyAreaId { get; set; }
        public string CompetencyAreaCode { get; set; }
        public int ProficiencyLevelId { get; set; }
        public string ProficiencyLevelCode { get; set; }
        public int RequistionId { get; set; }
        public int RoleId { get; set; }
        public int SkillExperience { get; set; }
    }
}
