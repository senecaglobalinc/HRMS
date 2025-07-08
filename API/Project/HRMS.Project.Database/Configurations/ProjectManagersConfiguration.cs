using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectManagersConfiguration : IEntityTypeConfiguration<ProjectManager>
    {
        public void Configure(EntityTypeBuilder<ProjectManager> builder)
        {
            builder.ToTable("ProjectManagers");

            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.Id)
                .HasColumnName("ID")
                .HasColumnType("Integer");

            builder
                .Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.LeadId)
                .HasColumnName("LeadID")
                .HasColumnType("Integer");

            builder
                .Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ProgramManagerId)
                .HasColumnName("ProgramManagerID")
                .HasColumnType("Integer");

            builder
                .Property(e => e.ProjectId)
                .HasColumnName("ProjectID")
                .HasColumnType("Integer");

            builder
                .Property(e => e.ReportingManagerId)
                .HasColumnName("ReportingManagerID")
                .HasColumnType("Integer");

            builder
               .Property(e => e.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)")
               .IsUnicode(false)
               .HasDefaultValueSql("inet_client_addr()");

            builder
                .Property(d => d.ProgramManagerId)
                .HasColumnType("Integer");

            builder
                .Property(d => d.ProjectId)
                .HasColumnType("Integer");

            builder
                .Property(d => d.ReportingManagerId)
                .HasColumnType("Integer");

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
