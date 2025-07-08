using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class CategoryMasterConfiguration : IEntityTypeConfiguration<CategoryMaster>
    {
        public void Configure(EntityTypeBuilder<CategoryMaster> builder)
        {
            builder
                .HasKey(cm => cm.CategoryMasterId);

            builder
                .Property(cm => cm.CategoryName)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(cm => cm.ParentId)
                .HasColumnType("int")
                .IsRequired();

            builder.Ignore(cm => cm.ParentCategoryName);

            builder
                .Property(cm => cm.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(cm => cm.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(cm => cm.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(cm => cm.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(cm => cm.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(cm => cm.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(cm => cm.CurrentUser);
        }
    }
}