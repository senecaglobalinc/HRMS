using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateHistoryConfiguration : IEntityTypeConfiguration<AssociateHistory>
    {
        public void Configure(EntityTypeBuilder<AssociateHistory> builder)
        {
            builder.ToTable("AssociateHistory");
            builder
              .Property(c => c.Id)
              .HasColumnName("ID");

            builder
               .Property(c => c.EmployeeId)
               .IsRequired()
               .HasColumnName("EmployeeId");

            builder
              .Property(c => c.Designation)
              .HasMaxLength(100)
              .IsUnicode(false);

            builder
              .Property(c => c.GradeId)
              .HasColumnName("GradeId");

            builder
              .Property(c => c.Remarks)
              .HasMaxLength(100)
              .IsUnicode(false);

            builder
             .Property(c => c.DepartmentId)
             .HasColumnName("DepartmentId");

            builder
             .Property(c => c.PracticeAreaId)
             .HasColumnName("PracticeAreaId");

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


            builder.Ignore(c => c.CurrentUser);
            builder.Ignore(c => c.IsActive);
        }
    }
}
