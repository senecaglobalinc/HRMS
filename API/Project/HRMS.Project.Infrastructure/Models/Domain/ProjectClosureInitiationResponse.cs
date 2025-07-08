using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ProjectClosureInitiationResponse : Entities.ProjectClosure
    {
        public DateTime? ActualEndDate { get; set; }
    }
}
