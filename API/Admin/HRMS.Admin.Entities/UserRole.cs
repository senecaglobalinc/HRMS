using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class UserRole : BaseEntity
    {
        /// <summary>
        /// UserRoleID
        /// </summary>
        public int UserRoleID { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public Nullable<int> RoleId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public Nullable<int> UserId { get; set; }

        /// <summary>
        /// IsPrimary
        /// </summary>
        public Nullable<bool> IsPrimary { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public virtual User User { get; set; }
    }
}
