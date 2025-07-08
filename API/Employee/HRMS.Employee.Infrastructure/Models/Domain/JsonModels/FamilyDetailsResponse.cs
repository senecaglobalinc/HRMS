using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class FamilyDetailsResponse 
    {        
        /// <summary>
        /// PersonalInfoId
        /// </summary>
        public Guid PersonalInfoId { get; set; }

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
        public int RelationShip { get; set; }

        /// <summary>
        /// Occupation
        /// </summary>
        public string Occupation { get; set; }

        public bool IsActive { get; set; }

    }
}
