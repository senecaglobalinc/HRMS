using System;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class UserRole
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
    }
}
