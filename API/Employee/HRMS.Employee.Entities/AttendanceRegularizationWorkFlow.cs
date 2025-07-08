using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
   public class AttendanceRegularizationWorkFlow: BaseEntity
    {
        public int WorkFlowId { get; set; }
        public DateTime RegularizationAppliedDate { get; set; }
        public string SubmittedBy { get; set; }
        public  string Location { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public  int SubmittedTo { get; set; }
        public int Status { get; set; }
        public  int? ApprovedBy { get; set; }
        public  DateTime? ApprovedDate { get; set; }
        public  string Remarks { get; set; }
        public  string RemarksByRM { get; set; }
        public int? RejectedBy { get; set; }
        public DateTime? RejectedDate { get; set; }

    }
}
