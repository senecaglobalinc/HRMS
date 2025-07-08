using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class ProfessionalDetailsConfiguration : IEntityTypeConfiguration<ProfessionalDetail>
    {
        public void Configure(EntityTypeBuilder<ProfessionalDetail> builder)
        {
            builder.ToTable("ProfessionalDetails");

            builder
               .Property(p => p.Id)
               .HasColumnName("ID");

            builder
               .Property(p => p.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
            .Property(p => p.ProgramTitle)
            .HasMaxLength(100)
            .IsUnicode(false);

            builder
            .Property(p => p.ProgramType)
            .HasMaxLength(30)
            .IsUnicode(false);

            builder
              .Property(p => p.Year)
              .HasColumnName("Year");

            builder
            .Property(p => p.institution)
            .HasMaxLength(100)
            .IsUnicode(false);

            builder
            .Property(p => p.specialization)
            .HasMaxLength(100)
            .IsUnicode(false);

            builder
            .Property(p => p.CurrentValidity)
            .HasMaxLength(50)
            .IsUnicode(false);

            builder
          .Property(p => p.IsActive)
          .HasColumnType("boolean");

            builder
            .Property(p => p.CreatedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

            builder
               .Property(p => p.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(p => p.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(p => p.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(p => p.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(p => p.CurrentUser);
        }
    }
}
