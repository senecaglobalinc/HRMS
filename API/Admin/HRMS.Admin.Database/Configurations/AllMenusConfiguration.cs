using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class AllMenusConfiguration : IEntityTypeConfiguration<AllMenus>
    {
        public void Configure(EntityTypeBuilder<AllMenus> builder)
        {
            builder.ToTable("AllMenus");

            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.MenuId)
                .HasColumnType("int")
                .IsRequired();

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
               .Property(m => m.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(m => m.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(m => m.IsActive);
            builder.Ignore(m => m.CurrentUser);
        }
    }
}
