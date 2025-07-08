using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
   public class EmployeeRoleDetails
    {
        /// <summary>
        /// EmployeeId
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// EmpName
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        public string RoleName { get; set; }
    }
}
