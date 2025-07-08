using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class EmployeeProjectsConfiguration : IEntityTypeConfiguration<EmployeeProject>
    {
        public void Configure(EntityTypeBuilder<EmployeeProject> builder)
        {
            builder.ToTable("EmployeeProjects");

            builder
               .Property(e => e.Id)
               .HasColumnName("ID");

            builder
               .Property(e => e.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
                .Property(e => e.OrganizationName)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder
                .Property(e => e.ProjectName)
                .IsUnicode(false);

            builder
              .Property(e => e.DomainId)
              .HasColumnName("DomainId");

            builder
             .Property(e => e.Duration)
             .HasColumnName("Duration");

            builder
             .Property(e => e.RoleMasterId)
             .HasColumnName("RoleMasterId");

            builder
               .Property(e => e.KeyAchievements)
               .IsUnicode(false);

            builder
              .Property(e => e.IsActive)
              .HasColumnType("boolean");

            builder
            .Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

            builder
               .Property(e => e.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(e => e.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(e => e.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(e => e.CurrentUser);
        }
    }
}
