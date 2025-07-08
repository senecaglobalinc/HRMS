using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Grade
    {
        public Grade()
        {
            Designation = new HashSet<Designation>();
        }

        public int GradeId { get; set; }
        public string GradeCode { get; set; }
        public string GradeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<Designation> Designation { get; set; }
    }
}
