using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class Activities
    {

        public int ProjectId { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string Remarks { get; set; }

        public int StatusId { get; set; }

        public string StatusDescription { get; set; }
        public List<GetActivityChecklist> ActivityDetails { get; set; }

    }
}
