using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Request
{
    public class ProjectCloseRequest
    {
        public int ProjectId { get; set; }
        public DateTime? EndDate { get; set; }
        public int? StatusId { get; set; }

    }
}
