using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class ServiceDepartmentRoleConfiguration : IEntityTypeConfiguration<ServiceDepartmentRole>
    {
        public void Configure(EntityTypeBuilder<ServiceDepartmentRole> builder)
        {
            builder.ToTable("ServiceDepartmentRoles");

            builder
               .HasKey(sd => sd.ServiceDepartmentRoleId);

            builder
             .Property(sd => sd.RoleMasterId)
             .IsRequired()
             .HasColumnType("int");

            builder
             .Property(sd => sd.EmployeeId)
             .IsRequired()
             .HasColumnType("int");

            builder
             .Property(sd => sd.DepartmentId)
             .IsRequired()
             .HasColumnType("int");

            builder
           .Property(sd => sd.CreatedDate)
           .HasColumnType("timestamp with time zone");

            builder
                .Property(sd => sd.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(sd => sd.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(sd => sd.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(sd => sd.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(sd => sd.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(sd => sd.CurrentUser);
        }
    }
}
