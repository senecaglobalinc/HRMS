using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public  class SkillSearch
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Experience
        /// </summary>
        public decimal? Experience { get; set; }

        /// <summary>
        /// RoleMasterId
        /// </summary>
        public int? RoleMasterId { get; set; }

        /// <summary>
        /// RoleDescription
        /// </summary>
        public string RoleDescription { get; set; }

        /// <summary>
        /// DesignationId
        /// </summary>
        public int? DesignationId { get; set; }

        /// <summary>
        /// DesignationCode
        /// </summary>
        public string DesignationCode { get; set; }

        /// <summary>
        /// ProjectCode
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// IsPrimary
        /// </summary>
        public bool? IsPrimary { get; set; }

        /// <summary>
        /// IsCritical
        /// </summary>
        public bool? IsCritical { get; set; }

        /// <summary>
        /// IsBillable
        /// </summary>
        public bool? IsBillable { get; set; }

        /// <summary>
        /// CompetencyAreaId
        /// </summary>
        public int? CompetencyAreaId { get; set; }

        /// <summary>
        /// CompetencyAreaCode
        /// </summary>
        public string CompetencyAreaCode { get; set; }

        /// <summary>
        /// SkillIgroupId
        /// </summary>
        public int? SkillIgroupId { get; set; }

        /// <summary>
        /// SkillGroupName
        /// </summary>
        public string SkillGroupName { get; set; }

        /// <summary>
        /// SkillId
        /// </summary>
        public int? SkillId { get; set; }

        /// <summary>
        /// SkillName
        /// </summary>
        public string SkillName { get; set; }

        /// <summary>
        /// ProficiencyLevelId
        /// </summary>
        public int? ProficiencyLevelId { get; set; }

        /// <summary>
        /// ProficiencyLevelCode
        /// </summary>
        public string ProficiencyLevelCode { get; set; }

        /// <summary>
        /// EmployeeCode
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// ProjectId
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// IsSkillPrimary
        /// </summary>
        public bool? IsSkillPrimary { get; set; }

        /// <summary>
        /// DesignationName
        /// </summary>
        public string DesignationName { get; set; }

        /// <summary>
        /// LastUsed
        /// </summary>
        public int? LastUsed { get; set; }

        /// <summary>
        /// SkillExperience
        /// </summary>
        public int? SkillExperience { get; set; }
    }
}
