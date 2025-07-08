using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class UserDetails 
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        public int EmployeeId { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// EmailAddress
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// IsSuperAdmin
        /// </summary>
        public Nullable<bool> IsSuperAdmin { get; set; }

        /// <summary>
        /// IsActive
        /// </summary>
        public bool IsActive { get; set; }
        public int? DepartmentId { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
    }
}
