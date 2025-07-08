using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociateAdvanceAttendanceDetails
    {
        public string Date { get; set; }
        public string DayOfWeek { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeMail { get; set; }
        public string ProjectName {  get; set; }
        public string ProgramManager {  get; set; }
        public string ReportingManager { get; set; }
        public string RmEmail { get; set; }
        public string AttendanceStatus { get; set; }
    }
}
