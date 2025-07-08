using System;
using System.Collections.Generic;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociateAbscondModel
    {
        public int AssociateAbscondId { get; set; }
        public int AssociateId { get; set; }
        public string AssociateName { get; set; }
        public DateTime AbsentFromDate { get; set; }
        public DateTime AbsentToDate { get; set; }
        public bool IsAbscond { get; set; }
        public int StatusId { get; set; }
        public int? TLId { get; set; }
        public string RemarksByTL { get; set; }
        public int? HRAId { get; set; }
        public string RemarksByHRA { get; set; }
        public int? HRMId { get; set; }
        public string RemarksByHRM { get; set; }
    }
}
