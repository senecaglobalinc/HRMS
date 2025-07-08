using System;
using System.Collections.Generic;

namespace HRMS.Employee.Entities
{
    public partial class AssociateExitRevokeWorkflow : BaseEntity
    {
        public int Id { get; set; }
        public int AssociateExitId { get; set; }
        public int RevokeStatusId { get; set; }
        public string RevokeComment { get; set; }
    }
}
