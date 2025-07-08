using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class ReasonType : BaseEntity
    {        
        public int ReasonTypeId  {get; set;}        
        public string Description  {get; set;}
        public int ParentId { get; set; }
        public string ParentReasonType { get; set; }
        public virtual ICollection<Reason> Reasons { get; set; }

    }
}
