using System;
using System.Collections.Generic;

namespace HRMS.Cache.Database.Entities
{
    public partial class Projects
    {
        public Projects()
        {
            Addendum = new HashSet<Addendum>();
            AssociateAllocation = new HashSet<AssociateAllocation>();
            ClientBillingRoles = new HashSet<ClientBillingRoles>();
            ProjectManagers = new HashSet<ProjectManagers>();
            ProjectRoles = new HashSet<ProjectRoles>();
            ProjectWorkFlow = new HashSet<ProjectWorkFlow>();
            Sow = new HashSet<Sow>();
            TalentRequisition = new HashSet<TalentRequisition>();
        }

        public int ProjectId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SystemInfo { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int ClientId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int ProjectStateId { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int DepartmentId { get; set; }
        public int PracticeAreaId { get; set; }
        public int? DomainId { get; set; }

        public virtual ICollection<Addendum> Addendum { get; set; }
        public virtual ICollection<AssociateAllocation> AssociateAllocation { get; set; }
        public virtual ICollection<ClientBillingRoles> ClientBillingRoles { get; set; }
        public virtual ICollection<ProjectManagers> ProjectManagers { get; set; }
        public virtual ICollection<ProjectRoles> ProjectRoles { get; set; }
        public virtual ICollection<ProjectWorkFlow> ProjectWorkFlow { get; set; }
        public virtual ICollection<Sow> Sow { get; set; }
        public virtual ICollection<TalentRequisition> TalentRequisition { get; set; }
    }
}
