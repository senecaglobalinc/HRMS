using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
   public class AttendanceMusterDetails
    {
        public string AssociateName { get; set; }
        public string AssociateId { get; set; }
        public string Designation { get; set; }
    }

    public class AttendanceDetailsWithDates
    {
       
        public string AssociateId { get; set; }     
        public Dictionary<string, string> Day { get; set; } = new Dictionary<string, string>();
    }

    public class AttendanceTotalDaysDetails
    {
        public string AssociateId { get; set; }
        public decimal Present { get; set; }
        public decimal Leave   { get; set; }
        public int Holiday { get; set; }
        public decimal Absent { get; set; }
        public decimal Total { get; set; }
    }

}
