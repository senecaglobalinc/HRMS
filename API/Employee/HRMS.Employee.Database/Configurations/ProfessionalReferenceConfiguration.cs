using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class ProfessionalReferenceConfiguration : IEntityTypeConfiguration<ProfessionalReferences>
    {
        public void Configure(EntityTypeBuilder<ProfessionalReferences> builder)
        {
            builder.ToTable("ProfessionalReferences");

            builder
               .Property(p => p.Id)
               .HasColumnName("ID");

            builder
               .Property(p => p.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
            .Property(p => p.Name)
            .HasMaxLength(100)
            .IsUnicode(false);

            builder
            .Property(p => p.Designation)
            .HasMaxLength(100)
            .IsUnicode(false);

            builder
            .Property(p => p.CompanyName)
            .HasMaxLength(100)
            .IsUnicode(false);

            builder
            .Property(p => p.CompanyAddress)
            .IsUnicode(false);

            builder
            .Property(p => p.OfficeEmailAddress)
            .HasMaxLength(100)
            .IsUnicode(false);

            builder
            .Property(p => p.MobileNo)
            .HasMaxLength(20)
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
