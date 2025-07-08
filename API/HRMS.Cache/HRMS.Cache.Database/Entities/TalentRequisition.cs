using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class TalentRequisition
    {
        public TalentRequisition()
        {
            AssociateAllocation = new HashSet<AssociateAllocation>();
        }

        public int TrId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
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

        public virtual Projects Project { get; set; }
        public virtual ICollection<AssociateAllocation> AssociateAllocation { get; set; }
    }
}
