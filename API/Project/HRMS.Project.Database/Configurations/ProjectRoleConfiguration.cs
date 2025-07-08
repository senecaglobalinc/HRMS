using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectRoleConfiguration : IEntityTypeConfiguration<ProjectRole>
    {
        public void Configure(EntityTypeBuilder<ProjectRole> builder)
        {
            builder.ToTable("ProjectRole");

            builder
               .Property(c => c.ProjectRoleId)
               .HasColumnName("ProjectRoleId");

            builder
               .Property(c => c.ProjectId)
               .HasColumnName("ProjectId");

            builder
               .Property(c => c.RoleId)
               .HasColumnName("RoleId");

            builder
               .Property(c => c.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(c => c.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
                .Property(c => c.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(c => c.CurrentUser);

            builder.HasOne(c => c.Project)
                .WithMany(p => p.ProjectRole)
                .HasForeignKey(c => c.ProjectId);
        }
    }
}
