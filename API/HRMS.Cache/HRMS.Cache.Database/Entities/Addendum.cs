using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Addendum
    {
        public int AddendumId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int ProjectId { get; set; }
        public string Sowid { get; set; }
        public string AddendumNo { get; set; }
        public string RecipientName { get; set; }
        public DateTime AddendumDate { get; set; }
        public string Note { get; set; }
        public int Id { get; set; }

        public virtual Projects Project { get; set; }
        public virtual Sow Sow { get; set; }
    }
}
