using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ProjectDTO
    {
        public List<ProjectDetails> Projects { get; set; }
    }
    public class ProjectsData
    {
        public List<Projects> Projects { get; set; }
    }
    public class Projects
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
    }
    public class ProjectDetails:Projects
    {
        public int ProjectId { get; set; }
    }

    

}
