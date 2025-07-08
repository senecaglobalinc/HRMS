namespace HRMS.Project.Entities
{
    public class ProjectClosureActivityDetail : BaseEntity
    {
        public int ProjectClosureActivityDetailId { get; set; }
        public int ProjectClosureActivityId { get; set; }
        public int ActivityId { get; set; }
        public string Value { get; set; }
        public string Remarks { get; set; }

        public ProjectClosureActivity ProjectClosureActivity;
    }
}
