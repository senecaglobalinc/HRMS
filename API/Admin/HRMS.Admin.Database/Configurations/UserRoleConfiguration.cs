using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder
               .HasKey(us => us.UserRoleID);


            builder
               .HasOne(us => us.Role)
               .WithMany(rs => rs.UserRoles)
               .HasForeignKey(us => us.RoleId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
              .HasOne(us => us.User)
              .WithMany(rs => rs.UserRoles)
              .HasForeignKey(us => us.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(us => us.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(us => us.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(us => us.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(us => us.IsActive)
               .HasColumnType("boolean");

            builder
              .Property(us => us.IsPrimary)
              .HasColumnType("boolean");

            builder
               .Property(us => us.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(us => us.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(us => us.CurrentUser);
        }
    }
}
