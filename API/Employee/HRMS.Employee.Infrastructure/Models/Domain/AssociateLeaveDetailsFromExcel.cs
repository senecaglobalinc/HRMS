using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
   public class AssociateLeaveDetailsFromExcel
    {
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string ManagerNo { get; set; }
        public string ManagerName { get; set; }
        //public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Session1 { get; set; }
        public string Session2 { get; set; }
        public string Days { get; set; }
      

    }
}
