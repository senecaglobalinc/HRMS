using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class LeadershipAssociatesConfiguration : IEntityTypeConfiguration<LeadershipAssociates>
    {
        public void Configure(EntityTypeBuilder<LeadershipAssociates> builder)
        {
            builder.ToTable("LeadershipAssociates");

            builder.HasKey(e => e.LeadershipAssociatesId);

            builder.HasIndex(e => e.AssociateId)
                    .HasName("UC_AssociateId")
                    .IsUnique();

            builder
              .Property(v => v.CreatedBy);

            builder
               .Property(v => v.CreatedDate);

            builder
               .Property(v => v.ModifiedBy);

            builder
               .Property(v => v.ModifiedDate);

            builder
                .Property(v => v.IsActive);
        }
    }
}
