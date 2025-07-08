using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMS.Employee.Entities
{
   public class EmployeeKRARoleTypeHistory: BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// RoleTypeId
        /// </summary>
        public int RoleTypeId { get; set; }
       
        /// <summary>
        /// RoleTypeValidFrom
        /// </summary>
        public DateTime? RoleTypeValidFrom { get; set; }

        /// <summary>
        /// RoleTypeValidTo
        /// </summary>
        public DateTime? RoleTypeValidTo { get; set; }

    }
}
