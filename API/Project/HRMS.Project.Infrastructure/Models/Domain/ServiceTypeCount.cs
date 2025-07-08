using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ServiceTypeCount
    {
        public int ServiceTypeId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
