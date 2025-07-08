using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder
               .HasKey(us => us.UserId);

            builder
               .Property(us => us.UserName)
               .HasMaxLength(256)
               .HasColumnType("text");

            builder
               .Property(us => us.Password)
               .HasColumnType("text");

            builder
              .Property(us => us.EmailAddress)
              .HasMaxLength(254)
              .HasColumnType("text");

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
             .Property(us => us.IsSuperAdmin)
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
