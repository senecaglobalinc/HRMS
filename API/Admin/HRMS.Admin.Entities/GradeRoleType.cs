namespace HRMS.Admin.Entities
{
    public class GradeRoleType : BaseEntity
    {
        public int GradeRoleTypeId { get; set; }
        public int GradeId { get; set; }
        public int RoleTypeId { get; set; }
        public virtual Grade Grade { set; get; }
        public virtual RoleType RoleType { get; set; }
    }
}
