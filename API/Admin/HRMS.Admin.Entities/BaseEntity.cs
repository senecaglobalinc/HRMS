namespace HRMS.Admin.Entities
{
    public class BaseEntity
    {
        public string CurrentUser { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
