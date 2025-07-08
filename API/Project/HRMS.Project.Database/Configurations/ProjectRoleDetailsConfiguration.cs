using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectRoleDetailsConfiguration : IEntityTypeConfiguration<ProjectRoleDetails>
    {
        public void Configure(EntityTypeBuilder<ProjectRoleDetails> builder)
        {
            builder.HasKey(e => e.RoleAssignmentId)
                    .HasName("PK_EmployeeRoleDetails");

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

            builder.Property(e => e.FromDate).HasColumnType("timestamp with time zone");

            builder.Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

            builder.Property(e => e.RejectReason).IsUnicode(false);

            builder.Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder.Property(e => e.ToDate).HasColumnType("timestamp with time zone");

            builder.Property(d => d.EmployeeId)
                .HasColumnType("Integer");

            builder.Property(d => d.RoleMasterId)
                .HasColumnType("Integer");

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
