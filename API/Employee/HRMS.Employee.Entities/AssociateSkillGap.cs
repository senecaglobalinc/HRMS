using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateSkillGap : BaseEntity
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
        /// ProjectSkillId
        /// </summary>
        public int? ProjectSkillId { get; set; }

        /// <summary>
        /// CompetencyAreaId
        /// </summary>
        public int? CompetencyAreaId { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        public int? StatusId { get; set; }

        /// <summary>
        /// CurrentProficiencyLevelId
        /// </summary>
        public int? CurrentProficiencyLevelId { get; set; }

        /// <summary>
        /// RequiredProficiencyLevelId
        /// </summary>
        public int? RequiredProficiencyLevelId { get; set; }
    }

}
