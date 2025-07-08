using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateResignation : BaseEntity
    {
        public int ResignationId { get; set; }
        public int EmployeeId { get; set; }
        public int? ReasonId { get; set; }
        public string ReasonDescription { get; set; }
        public DateTime ResignationDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public int StatusId { get; set; }
        public virtual Employee employee { get; set; }
    }
}
