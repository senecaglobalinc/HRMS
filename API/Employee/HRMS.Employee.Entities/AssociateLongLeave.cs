using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateLongLeave : BaseEntity
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }   
        public DateTime LongLeaveStartDate { get; set; }
        public DateTime TentativeJoinDate { get; set; }
        public int StatusId { get; set; }
        public bool? IsMaternity { get; set; }
        public string Reason { get; set; }
        public virtual Employee employee { get; set; }

    }
}
