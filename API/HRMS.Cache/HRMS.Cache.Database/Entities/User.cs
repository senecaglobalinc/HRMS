using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Cache.Database.Entities
{
    public class User
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }

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

    }
}
