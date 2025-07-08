using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
   public class AssociateFutureProjectAllocation:BaseEntity
    {
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public string ProjectName { get; set; }
        public int? ProjectId { get; set; }
        public DateTime TentativeDate { get; set; }
        public string Remarks { get; set; }
    }
}
