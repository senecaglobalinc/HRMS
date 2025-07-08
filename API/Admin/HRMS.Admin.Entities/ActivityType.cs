using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class ActivityType : BaseEntity
    {        
        public int ActivityTypeId  {get; set;}        
        public string Description  {get; set;}
        public int ParentId { get; set; }
        public string ParentActivityType { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }

    }
}
