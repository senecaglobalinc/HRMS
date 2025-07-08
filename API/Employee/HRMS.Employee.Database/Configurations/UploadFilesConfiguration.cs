using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class UploadFilesConfiguration : IEntityTypeConfiguration<UploadFile>
    {
        public void Configure(EntityTypeBuilder<UploadFile> builder)
        {

            builder.ToTable("UploadFiles");

            builder
               .Property(u => u.Id)
               .HasColumnName("ID");

            builder
               .Property(u => u.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
               .Property(u => u.FileName)
               .IsUnicode(false);

            builder
           .Property(u => u.IsActive)
           .HasColumnType("boolean");

            builder
            .Property(u => u.CreatedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

            builder
               .Property(u => u.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(u => u.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(u => u.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(u => u.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(u => u.CurrentUser);
        }
    }
}
