using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure
{
   public class AttendanceRegularizationWorkFlowDetails
    {
        public  int? ApprovedBy { get; set; }
        public  bool IsApproved { get; set; }
        public string RemarksByRM  { get; set; }
        public DateTime RegularizationAppliedDate { get; set; }
        public string SubmittedBy { get; set; }
        public string Location { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string AssociateName { get; set;}
        public string ProjectName { get; set; }
        public int EmployeeId { get; set; }
        public int RegularizationCount { get; set; }
        public List<DateTime> RegularizationDates { get; set; }
        public int? RejectedBy { get; set; }
        public DateTime? RejectedDate { get; set; }


    }

}
