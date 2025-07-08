using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
   public class ActiveAllocationDetails
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? ClientId { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public int? ProgramManagerId { get; set; }
        public bool? IsPrimary { get; set; }
        public int EmployeeId { get; set; }
        public string ClientName { get; set; }
        public int? RoleMasterId { get; set; }
    }

}
