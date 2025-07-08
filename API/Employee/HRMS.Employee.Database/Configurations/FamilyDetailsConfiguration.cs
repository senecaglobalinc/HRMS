using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class FamilyDetailsConfiguration : IEntityTypeConfiguration<FamilyDetails>
    {
        public void Configure(EntityTypeBuilder<FamilyDetails> builder)
        {
            builder.ToTable("FamilyDetails");

            builder
               .Property(f => f.Id)
               .HasColumnName("ID");

            builder
               .Property(f => f.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
               .Property(f => f.Name)
               .HasMaxLength(100)
               .IsUnicode(false);

            builder
                .Property(f => f.DateOfBirth)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(f => f.RelationShip)
               .HasMaxLength(50)
               .IsUnicode(false);

            builder
              .Property(f => f.Occupation)
              .HasMaxLength(100)
              .IsUnicode(false);

            builder
             .Property(f => f.IsActive)
             .HasColumnType("boolean");

            builder
            .Property(f => f.CreatedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

            builder
               .Property(f => f.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(f => f.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(f => f.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(f => f.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(f => f.CurrentUser);

        }
    }
}
