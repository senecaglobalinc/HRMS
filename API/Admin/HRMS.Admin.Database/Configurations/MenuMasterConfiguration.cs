using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class MenuMasterConfiguration : IEntityTypeConfiguration<MenuMaster>
    {
        public void Configure(EntityTypeBuilder<MenuMaster> builder)
        {
            builder.ToTable("MenuMaster");

            builder
               .HasKey(m => m.MenuId);

            builder
               .Property(m => m.Title)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
              .Property(m => m.Path)
              .HasMaxLength(250)
              .HasColumnType("varchar(250)");

            builder
              .Property(m => m.DisplayOrder)
              .IsRequired()
              .HasColumnType("int");

            builder
              .Property(m => m.ParentId)
              .IsRequired()
              .HasColumnType("int");

            builder
              .Property(m => m.Parameter)
              .HasMaxLength(50)
              .HasColumnType("varchar(50)");

            builder
            .Property(m => m.NodeId)
            .HasMaxLength(50)
            .HasColumnType("varchar(50)");

            builder
           .Property(m => m.Style)
           .HasMaxLength(50)
           .HasColumnType("varchar(50)");

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
