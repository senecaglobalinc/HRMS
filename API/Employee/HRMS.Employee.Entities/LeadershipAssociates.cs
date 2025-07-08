using System;
using System.Collections.Generic;

namespace HRMS.Employee.Entities
{
    public class LeadershipAssociates
    {
        public int LeadershipAssociatesId { get; set; }
        public int AssociateId { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
