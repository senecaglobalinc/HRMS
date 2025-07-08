using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociateExitWFStatus
    {
        public int EmployeeId { get; set; }
        public int AssociateExitId { get; set; }
        public string AssociateExitStatusCode { get; set; }
        public string AssociateExitStatusDesc { get; set; }
        public List<ActivityStatus> ActivitiesSubStatus { get; set; }
        public ResignationSubStatus ExitInterviewSubStatus { get; set; }
        public ResignationSubStatus KTPlanSubStatus { get; set; }
    }

    public class ActivityStatus
    {
        public int DepartmentId { get; set; }
        public string ActivityStatusCode { get; set; }
        public string ActivityStatusDesc { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class ResignationSubStatus
    {
        public string SubStatusCode { get; set; }
        public string SubStatusDesc { get; set; }
        public bool IsCompleted { get; set; }
    }
}
