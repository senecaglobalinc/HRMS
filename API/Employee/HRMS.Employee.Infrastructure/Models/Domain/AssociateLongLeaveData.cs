using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociateLongLeaveData : BaseEntity
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public string LongLeaveStartDate { get; set; }
        public string TentativeJoinDate { get; set; }
        public int StatusId { get; set; }
        public string Reason { get; set; }
        public Nullable<bool> IsMaternity { get; set; }
    }
}
