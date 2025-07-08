using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ValueType = HRMS.Employee.Entities.ValueType;

namespace HRMS.Employee.Database.Configurations
{
    public class ValueTypeConfiguration : IEntityTypeConfiguration<ValueType>
    {
        public void Configure(EntityTypeBuilder<ValueType> builder)
        {
            builder.ToTable("ValueType");

            builder
              .HasKey(v => v.ValueTypeKey);

            builder
                .Property(v => v.ValueTypeId)
                .HasMaxLength(255)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder
                .Property(v => v.ValueTypeName)
                .HasMaxLength(255)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder
             .Property(v => v.CreatedBy)
             .HasMaxLength(100)
             .HasColumnType("varchar(100)");

            builder
               .Property(v => v.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(v => v.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(v => v.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(v => v.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(v => v.SystemInfo);
            builder.Ignore(v => v.CurrentUser);
        }
    }
}
