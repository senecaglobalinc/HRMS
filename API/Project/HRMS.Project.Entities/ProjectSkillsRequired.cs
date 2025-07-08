using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class ProjectSkillsRequired : BaseEntity
    {
        /// <summary>
        /// ProjectSkillsRequiredId
        /// </summary>
        public int ProjectSkillsRequiredId { get; set; }

        /// <summary>
        /// ProjectRoleId
        /// </summary>
        public int ProjectRoleId { get; set; }

        /// <summary>
        /// SkillId
        /// </summary>
        public int SkillId { get; set; }

        /// <summary>
        /// ProficiencyId
        /// </summary>
        public int ProficiencyId { get; set; }

        public virtual ProjectRole ProjectRole { get; set; }
    }
}
