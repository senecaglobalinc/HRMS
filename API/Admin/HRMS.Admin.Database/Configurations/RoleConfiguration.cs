using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder
               .HasKey(rs => rs.RoleId);

            builder
              .Property(rs => rs.RoleName)
              .HasMaxLength(256)
              .HasColumnType("varchar(256)");

            builder
                .Property(rs => rs.RoleDescription)
                .HasMaxLength(256)
                .HasColumnType("text");

            builder
           .Property(rs => rs.EducationQualification)
           .HasColumnType("text");

            builder
                .Property(rs => rs.KeyResponsibilities)
                .HasColumnType("text");

            builder
               .HasOne(rs => rs.Department)
               .WithMany(dp => dp.Roles)
               .HasForeignKey(rs => rs.DepartmentId)
               .OnDelete(DeleteBehavior.ClientSetNull);
          

            builder
                .Property(rs => rs.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(rs => rs.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(rs => rs.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(rs => rs.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(rs => rs.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(rs => rs.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(rs => rs.CurrentUser);
        }
    }
}
