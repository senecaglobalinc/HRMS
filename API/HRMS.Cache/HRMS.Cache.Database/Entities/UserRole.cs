using System;

namespace HRMS.Cache.Database.Entities
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

        /// <summary>
        /// User
        /// </summary>
        public virtual User User { get; set; }
    }
}