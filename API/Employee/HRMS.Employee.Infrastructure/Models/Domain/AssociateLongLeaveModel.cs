using System;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociateLongLeaveModel
    {
        public int AssociateId { get; set; }
        public string AssociateCode { get; set; }
        public string AssociateName { get; set; }
        public string AssociateEmail { get; set; }
        public DateTime LongLeaveStartDate { get; set; }
        public DateTime TentativeJoinDate { get; set; }
        public string Reason { get; set; }
        public int NoOfDaysLeft { get; set; }
    }
}
