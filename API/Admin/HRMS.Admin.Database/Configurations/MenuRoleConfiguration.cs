using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class MenuRoleConfiguration : IEntityTypeConfiguration<MenuRole>
    {
        public void Configure(EntityTypeBuilder<MenuRole> builder)
        {
            builder.ToTable("MenuRoles");

            builder
               .HasKey(m => m.MenuRoleId);

            builder
             .Property(m => m.MenuId)
             .IsRequired()
             .HasColumnType("int");

            builder
             .Property(m => m.RoleId)
             .HasColumnType("int");

            builder
            .Property(m => m.CreatedDate)
            .HasColumnType("timestamp with time zone");

            builder
                .Property(m => m.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(m => m.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(m => m.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(m => m.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(m => m.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(m => m.CurrentUser);
        }
    }
}
