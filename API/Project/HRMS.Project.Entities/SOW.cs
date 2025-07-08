using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class SOW : BaseEntity
    {
        public SOW()
        {
            Addendum = new HashSet<Addendum>();
        }

        public int Id { get; set; }
        public string SOWId { get; set; }
        public string SOWFileName { get; set; }
        public int ProjectId { get; set; }
        public DateTime? SOWSignedDate { get; set; }
        
        public virtual Project Project { get; set; }
        public virtual ICollection<Addendum> Addendum { get; set; }
    }
}
