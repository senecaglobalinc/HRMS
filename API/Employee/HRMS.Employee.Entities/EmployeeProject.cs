using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class EmployeeProject : BaseEntity
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
        /// OrganizationName
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// DomainId
        /// </summary>
        public int? DomainId { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// RoleMasterId
        /// </summary>
        public int? RoleMasterId { get; set; }

        /// <summary>
        /// KeyAchievements
        /// </summary>
        public string KeyAchievements { get; set; }
      
    }
}
