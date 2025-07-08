using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class TalentRequisition : BaseEntity
    {
        public TalentRequisition()
        {
            AssociateAllocation = new HashSet<AssociateAllocation>();
        }

        public int TrId { get; set; }
        public int DepartmentId { get; set; }
        public int? PracticeAreaId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? TargetFulfillmentDate { get; set; }
        public int StatusId { get; set; }
        public int? ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string Trcode { get; set; }
        public int? RequisitionType { get; set; }
        public int? RaisedBy { get; set; }
        public int? DraftedBy { get; set; }
        public int? ClientId { get; set; }
        public int? ProjectDuration { get; set; }
        
        public virtual Project Project { get; set; }
        public virtual ICollection<AssociateAllocation> AssociateAllocation { get; set; }
    }
}
