using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Project;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class SOWGetByIdAndProjectIdRequest
    {
        public int Id { get; set; }
        public int projectId { get; set; }
        public string roleName { get; set; }
    }
}
