using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class lkValueConfiguration : IEntityTypeConfiguration<lkValue>
    {
        public void Configure(EntityTypeBuilder<lkValue> builder)
        {
            builder.ToTable("lkValue");

            builder
              .HasKey(lk => lk.ValueKey);

            builder
              .Property(lk => lk.ValueId)
              .HasMaxLength(255)
              .HasColumnType("varchar(255)")
              .IsRequired();

            builder
                .Property(lk => lk.ValueName)
                .HasMaxLength(255)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder
               .HasOne(lk => lk.ValueType)
               .WithMany(v => v.lkValue)
               .HasForeignKey(lk => lk.ValueTypeKey)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
            .Property(lk => lk.CreatedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

            builder
               .Property(lk => lk.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(lk => lk.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(lk => lk.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(lk => lk.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(lk => lk.SystemInfo);
            builder.Ignore(lk => lk.CurrentUser);
        }
    }
}
