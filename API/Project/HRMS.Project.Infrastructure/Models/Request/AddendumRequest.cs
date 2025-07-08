using HRMS.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class AddendumRequest:Addendum    {
        public string RoleName { get; set; }
        public string CurrentUserName { get; set; }
    }
}
