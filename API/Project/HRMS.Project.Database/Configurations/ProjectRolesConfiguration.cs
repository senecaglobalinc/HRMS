using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectRolesConfiguration : IEntityTypeConfiguration<ProjectRoles>
    {
        public void Configure(EntityTypeBuilder<ProjectRoles> builder)
        {
            builder
                .HasKey(e => e.ProjectRoleId);

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .HasDefaultValueSql("inet_client_addr()");

            builder
                .HasOne(d => d.Project)
                .WithMany(p => p.ProjectRoles)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(d => d.RoleMasterId)
                .HasColumnType("Integer");

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
