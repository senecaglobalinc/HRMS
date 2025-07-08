using HRMS.Project.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class SOWRequest:SOW 
    {
        public bool SOW { get; set; }
        public bool? IsActive { get; set; }
        public string RoleName { get; set; }
        public string CurrentUserName { get; set; }
    }
}
