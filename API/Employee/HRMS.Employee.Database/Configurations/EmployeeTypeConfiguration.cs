using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class EmployeeTypeConfiguration : IEntityTypeConfiguration<EmployeeType>
    {
        public void Configure(EntityTypeBuilder<EmployeeType> builder)
        {
            builder
               .Property(et => et.EmployeeTypeId)
               .HasColumnName("EmployeeTypeId");

            builder
                .Property(et => et.EmployeeTypeCode)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder
                .Property(et => et.EmpType)
                .HasColumnName("EmpType");

            builder
                .Property(et => et.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(et => et.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(et => et.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(et => et.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(et => et.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(et => et.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(et => et.CurrentUser);
        }
    }
}
