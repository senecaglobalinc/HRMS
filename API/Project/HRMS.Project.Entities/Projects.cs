using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Entities
{
    public class Project:BaseEntity
    {
        public Project()
        {
            AssociateAllocation = new HashSet<AssociateAllocation>();
            ProjectManagers = new HashSet<ProjectManager>();
            TalentRequisition = new HashSet<TalentRequisition>();
        }

        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public int ClientId { get; set; }
        public int? StatusId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? ProjectStateId { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int DepartmentId { get; set; }
        public int PracticeAreaId { get; set; }
        public int? DomainId { get; set; }

        public virtual ICollection<Addendum> Addendum { get; set; }
        public virtual ICollection<AssociateAllocation> AssociateAllocation { get; set; }
        public virtual ICollection<ClientBillingRoles> ClientBillingRoles { get; set; }
        public virtual ICollection<ProjectManager> ProjectManagers { get; set; }
        public virtual ICollection<ProjectRoles> ProjectRoles { get; set; }
        public virtual ICollection<ProjectWorkFlow> ProjectWorkFlow { get; set; }
        public virtual ICollection<SOW> SOW { get; set; }
        public virtual ICollection<TalentRequisition> TalentRequisition { get; set; }
        public virtual ICollection<ProjectTrainingPlan> ProjectTrainingPlan { get; set; }
        public virtual ICollection<ProjectRole> ProjectRole { get; set; }
        public virtual ICollection<ProjectRoleAssociateMapping> ProjectRoleAssociateMapping { get; set; }
        public ProjectClosure ProjectClosure { get; set; }
    }
}
