using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class AssociateAbscondWFStatus
    {
        public int AssociateId { get; set; }
        public int AssociateExitId { get; set; }
        public string AssociateExitStatusCode { get; set; }
        public string AssociateExitStatusDesc { get; set; }
        public List<ActivityStatus> ActivitiesSubStatus { get; set; }
    }
}
