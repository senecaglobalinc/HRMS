using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
  public  class UserRoleDetails
    {
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
