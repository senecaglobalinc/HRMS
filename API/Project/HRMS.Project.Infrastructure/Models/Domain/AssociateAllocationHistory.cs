using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
   public class AssociateAllocationHistory
    {
      public    int  ProjectId { get; set; }
      public    string ProjectName { get; set; }
      public    int ClientId { get; set; }
      public    string ClientName { get; set; }
      public    int MasterRoleId { get; set; }
      public    string RoleName { get; set; }
      public    decimal AllocationPercentage { get; set; }
      public    string Critical { get; set; }
      public    int ReportingManagerId { get; set; }
      public    string ReportingManager { get; set; }
      public    int LeadId { get; set; }
      public    string Lead { get; set; }
      public    string EffectiveDate { get; set; }
      public    string ReleaseDate { get; set; }
      public    string Billable { get; set; }
      public    bool? IsActive { get; set; }
                                    
    }
}
