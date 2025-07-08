using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
   public class AssociateFutureProjectAllocationDetails
    {
        public int EmployeeId { get; set; }
        public string ProjectName { get; set; }
        public int? ProjectId { get; set; }
        public DateTime TentativeDate { get; set; }
        public string Remarks { get; set; }
    }
}
