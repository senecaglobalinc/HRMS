using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class ServiceTypeToEmployeeConfiguration : IEntityTypeConfiguration<ServiceTypeToEmployee>
    {
        public void Configure(EntityTypeBuilder<ServiceTypeToEmployee> builder)
        {
            builder.ToTable("ServiceTypeToEmployee");

            builder
               .Property(c => c.Id)
               .HasColumnName("ID");

            builder
               .Property(c => c.EmployeeId)
               .IsRequired()
               .HasColumnName("EmployeeId");

            builder
              .Property(c => c.ServiceTypeId)
              .IsRequired()
              .HasColumnName("ServiceTypeId");

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
            builder
              .Property(c => c.IsActive)
              .HasColumnType("boolean");


            builder.Ignore(c => c.CurrentUser);
          
        }
    }
}
