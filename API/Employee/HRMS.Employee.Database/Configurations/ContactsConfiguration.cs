using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class ContactsConfiguration : IEntityTypeConfiguration<Contacts>
    {
        public void Configure(EntityTypeBuilder<Contacts> builder)
        {
            builder.ToTable("Contacts");

            builder
               .Property(c => c.ID)
               .HasColumnName("ID");

            builder
               .Property(c => c.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
                .Property(c => c.AddressType)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder
               .Property(c => c.AddressLine1)
               .HasMaxLength(256)
               .IsUnicode(false);

            builder
               .Property(c => c.AddressLine2)
               .HasMaxLength(256)
               .IsUnicode(false);

            builder
               .Property(c => c.City)
               .HasMaxLength(100)
               .IsUnicode(false);

            builder
               .Property(c => c.State)
               .HasMaxLength(100)
               .IsUnicode(false);

            builder
               .Property(c => c.PostalCode)
               .HasMaxLength(20)
               .IsUnicode(false);

            builder
              .Property(c => c.Country)
              .HasMaxLength(100)
              .IsUnicode(false);         

            builder
               .Property(c => c.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(c => c.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
                .Property(c => c.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(c => c.CurrentUser);
        }
    }
}
