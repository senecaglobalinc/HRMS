using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class AssociateFutureProjectAllocationConfiguration : IEntityTypeConfiguration<AssociateFutureProjectAllocation>
    {
        public void Configure(EntityTypeBuilder<AssociateFutureProjectAllocation> builder)
        {

            builder
                .HasKey(e => e.ID);

            builder
                .Property(e => e.EmployeeId);
          
            builder
               .Property(e => e.ProjectId);
            builder
               .Property(e => e.ProjectName);
            builder
              .Property(e => e.TentativeDate);
            builder
               .Property(e => e.Remarks);
            builder
               .Property(e => e.IsActive)
               .HasDefaultValue(true);
            builder
               .Property(e => e.CreatedBy);
            builder
              .Property(e => e.ModifiedBy);
            builder
              .Property(e => e.CreatedDate);
            builder
              .Property(e => e.ModifiedDate);
            builder
              .Property(e => e.SystemInfo);
            builder
              .Ignore(e => e.CurrentUser);


        }
    }
}
