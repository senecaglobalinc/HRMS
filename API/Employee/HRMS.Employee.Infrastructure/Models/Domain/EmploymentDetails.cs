using System;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmploymentDetails
    {
        public int ID { get; set; }
        public int? empID { get; set; }
        public int? ReportingManagerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string designation { get; set; }
        public DateTime? serviceFrom { get; set; }
        public DateTime? serviceTo { get; set; }
        public string leavingReason { get; set; }
        public string fromYear { get; set; }
        public string toYear { get; set; }

    }
}
