using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class RoleMasterConfiguration : IEntityTypeConfiguration<RoleMaster>
    {
        public void Configure(EntityTypeBuilder<RoleMaster> builder)
        {
            builder.ToTable("RoleMaster");

            builder
               .HasKey(rs => rs.RoleMasterID);

            builder
              .Property(rs => rs.RoleDescription)
              .HasMaxLength(500)
              .HasColumnType("varchar(500)");

            builder
               .Property(rs => rs.KeyResponsibilities)
               .HasColumnType("text");

            builder
                .Property(rs => rs.EducationQualification)
                .HasColumnType("text");

            builder
               .HasOne(rs => rs.SGRole)
               .WithMany(r => r.RoleMasters)
               .HasForeignKey(rs => rs.SGRoleID)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .HasOne(rs => rs.SGRolePrefix)
               .WithMany(p => p.RoleMasters)
               .HasForeignKey(rs => rs.PrefixID)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
              .HasOne(rs => rs.SGRoleSuffix)
              .WithMany(s => s.RoleMasters)
              .HasForeignKey(rs => rs.SuffixID)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .HasOne(rs => rs.Department)
               .WithMany(dp => dp.RoleMasters)
               .HasForeignKey(rs => rs.DepartmentId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
              .Property(rs => rs.KRAGroupId)
              .HasColumnName("KRAGroupId");

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
