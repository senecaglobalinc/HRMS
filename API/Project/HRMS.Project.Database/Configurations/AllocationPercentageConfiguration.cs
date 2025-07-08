using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class AllocationPercentageConfiguration : IEntityTypeConfiguration<AllocationPercentage>
    {
        public void Configure(EntityTypeBuilder<AllocationPercentage> builder)
        {
            builder
                .HasKey(e => e.AllocationPercentageId);

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.Percentage)
                .HasColumnType("decimal(18, 0)");

            builder
               .Property(e => e.IsActive)
               .HasColumnType("boolean");

            builder
                .Property(e => e.SystemInfo)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .HasDefaultValueSql("inet_client_addr()");

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
