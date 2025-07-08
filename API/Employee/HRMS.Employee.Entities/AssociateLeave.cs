using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMS.Employee.Entities
{
   public class AssociateLeave:BaseEntity
    {
        public Guid AssociateLeaveId { get; set; }
        public string EmployeeCode { get; set; }
        public string AssociateName { get; set; }
        public string ManagerCode { get; set; }
        public string ManagerName { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Session1Id { get; set; }
        public string Session1Name { get; set; }
        public int Session2Id { get; set; }
        public string Session2Name { get; set; }
        public decimal NumberOfDays { get; set; }

    }
}
