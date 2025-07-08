using HRMS.Employee.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociateResignationData : BaseEntity
    {
        public int EmployeeId { get; set; }
        public int ReasonId { get; set; }
        public string Reason { get; set; }
        public string ReasonDescription { get; set; }
        public int StatusId { get; set; }  
        public string ProgramManagerName { get; set; }
        public string ReportingManagerName { get; set; }
        public string ProjectName { get; set; }
        public string ResignationDate { get; set; }
        public bool? IsBillable { get; set; }
        public bool? IsCritical { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string LastWorkingDate { get; set; }
        public bool? IsResigned { get; set; }
        public bool? IsLongLeave { get; set; }
        public string LongLeaveStartDate { get; set; }
        public string TentativeJoinDate { get; set; }

    }
}
