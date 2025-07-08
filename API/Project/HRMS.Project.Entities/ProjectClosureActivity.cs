using System.Collections.Generic;

namespace HRMS.Project.Entities
{
    public class ProjectClosureActivity : BaseEntity
    {
        public int ProjectClosureActivityId { get; set; }      
        public int ProjectClosureId { get; set; }
        public int DepartmentId { get; set; }
        public string Remarks { get; set; }
        public int StatusId { get; set; }
        public virtual ProjectClosure ProjectClosure { get; set; }
        public ICollection<ProjectClosureActivityDetail> ProjectClosureActivityDetails;
    }
    
}
