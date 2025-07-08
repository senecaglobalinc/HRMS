using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectsConfiguration : IEntityTypeConfiguration<Entities.Project>
    {
        public void Configure(EntityTypeBuilder<Entities.Project> builder)
        {
            builder.ToTable("Projects");

            builder
                .HasKey(e => e.ProjectId);

            builder
                .HasIndex(e => e.ProjectCode)
                .IsUnique();

            builder
                .HasIndex(e => e.ProjectName)
                .IsUnique();

            builder
                .Property(e => e.PlannedStartDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.PlannedEndDate)
                .HasColumnType("timestamp with time zone"); 

            builder
                .Property(e => e.ActualEndDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ActualStartDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedBy)
                .HasMaxLength(100)
               .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.ProjectCode)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.ProjectName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("inet_client_addr()");

            builder
                .Property(d => d.ClientId)
                .HasColumnType("integer");

            builder
                .Property(d => d.StatusId)
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
