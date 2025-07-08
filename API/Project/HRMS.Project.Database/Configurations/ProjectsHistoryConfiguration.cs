using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectsHistoryConfiguration : IEntityTypeConfiguration<ProjectsHistory>
    {
        public void Configure(EntityTypeBuilder<ProjectsHistory> builder)
        {
            builder.HasKey(e => e.ProjectHistoryId);

            builder.Property(e => e.ActualEndDate).HasColumnType("timestamp with time zone");

            builder.Property(e => e.ActualStartDate).HasColumnType("timestamp with time zone");

            builder.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder.Property(e => e.ProjectCode)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("varchar(30)")
                .IsUnicode(false);

            builder.Property(e => e.ProjectName)
                .IsRequired()
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
                .Property(d => d.ClientId)
                .HasColumnType("integer");

            builder
                .Property(d => d.DepartmentId)
                .HasColumnType("integer");

            builder
                .Property(d => d.PracticeAreaId)
                .HasColumnType("integer");

            builder
                .Property(d => d.ProjectTypeId)
                .HasColumnType("integer");

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
