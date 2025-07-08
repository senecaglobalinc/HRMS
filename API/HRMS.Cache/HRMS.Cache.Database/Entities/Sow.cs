using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Sow
    {
        public Sow()
        {
            Addendum = new HashSet<Addendum>();
        }

        public string Sowid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Id { get; set; }
        public string SowfileName { get; set; }
        public int ProjectId { get; set; }
        public DateTime? SowsignedDate { get; set; }

        public virtual Projects Project { get; set; }
        public virtual ICollection<Addendum> Addendum { get; set; }
    }
}
