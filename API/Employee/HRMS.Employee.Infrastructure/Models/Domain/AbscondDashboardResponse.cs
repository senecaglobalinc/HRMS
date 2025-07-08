using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AbscondDashboardResponse
    {
        public int AssociateId { get; set; }
        public string AssociateCode { get; set; }
        public string AssociateName { get; set; }
        public int AssociateAbscondId { get; set; }
        public int StatusId { get; set; }
        public string StatusDesc { get; set; }
        public bool EditAction { get; set; }
        public bool ViewAction { get; set; }
        public bool EditActivity { get; set; }
        public bool ViewActivity { get; set; }
        public bool EditClearance { get; set; }
    }
}
