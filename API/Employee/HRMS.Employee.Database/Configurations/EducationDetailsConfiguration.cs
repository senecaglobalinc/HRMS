using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class EducationDetailsConfiguration : IEntityTypeConfiguration<EducationDetails>
    {
        public void Configure(EntityTypeBuilder<EducationDetails> builder)
        {
            builder.ToTable("EducationDetails");

            builder
               .Property(e => e.Id)
               .HasColumnName("ID");

            builder
               .Property(e => e.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
                .Property(e => e.EducationalQualification)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder
               .Property(e => e.AcademicCompletedYear)
               .HasColumnName("AcademicCompletedYear")
               .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.Institution)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder
               .Property(e => e.Specialization)
               .HasMaxLength(100)
               .IsUnicode(false);

            builder
              .Property(e => e.ProgramType)
              .HasMaxLength(100)
              .IsUnicode(false);

            builder
              .Property(e => e.Grade)
              .HasMaxLength(10)
              .IsUnicode(false);

            builder
              .Property(e => e.Marks)
              .HasMaxLength(10)
              .IsUnicode(false);

            builder
             .Property(e => e.AcademicYearFrom)
             .HasMaxLength(100)
             .IsUnicode(false);

            builder
             .Property(e => e.AcademicYearTo)
             .HasMaxLength(100)
             .IsUnicode(false);

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

            builder
                .Property(e => e.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(e => e.ProgramTypeId);
            builder.Ignore(e => e.CurrentUser);
        }
    }
}
