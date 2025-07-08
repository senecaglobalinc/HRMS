using System;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }

        public string Password { get; set; }

        public string EmailAddress { get; set; }

        public Nullable<bool> IsSuperAdmin { get; set; }
    }
}
