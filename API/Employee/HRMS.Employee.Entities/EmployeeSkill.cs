using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class EmployeeSkill : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// SkillID
        /// </summary>
        public int SkillId { get; set; }

        /// <summary>
        /// CompetencyAreaId
        /// </summary>
        public int CompetencyAreaId { get; set; }

        /// <summary>
        /// ProficiencyLevelId
        /// </summary>
        public int ProficiencyLevelId { get; set; }

        /// <summary>
        /// Experience
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// LastUsed
        /// </summary>
        public int LastUsed { get; set; }

        /// <summary>
        /// IsPrimary
        /// </summary>
        public bool? IsPrimary { get; set; }

       
        /// <summary>
        /// SkillGroupID
        /// </summary>
        public int SkillGroupId { get; set; }

       
    }
}
