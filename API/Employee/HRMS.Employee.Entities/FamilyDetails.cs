using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class FamilyDetails : BaseEntity
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
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// DateOfBirth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// RelationShip
        /// </summary>
        public string RelationShip { get; set; }

        /// <summary>
        /// Occupation
        /// </summary>
        public string Occupation { get; set; }
    }
}
