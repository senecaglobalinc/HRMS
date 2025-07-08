using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateExitRevokeWorkflowConfiguration : IEntityTypeConfiguration<AssociateExitRevokeWorkflow>
    {
        public void Configure(EntityTypeBuilder<AssociateExitRevokeWorkflow> builder)
        {
            builder.ToTable("AssociateExitRevokeWorkflow");

            builder
              .HasKey(v => v.Id);

            builder
                .Property(v => v.AssociateExitId);

            builder
                .Property(v => v.RevokeStatusId);

            builder
               .Property(v => v.RevokeComment);

            builder
               .Property(v => v.IsActive);

            builder
             .Property(v => v.CreatedBy);

            builder
               .Property(v => v.CreatedDate);

            builder
               .Property(v => v.ModifiedBy);

            builder
                .Property(v => v.ModifiedDate);

            builder.Ignore(v => v.SystemInfo);
            builder.Ignore(v => v.CurrentUser);
        }
    }
}
