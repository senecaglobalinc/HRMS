using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class EmergencyContactsConfiguration : IEntityTypeConfiguration<EmergencyContactDetails>
    {
        public void Configure(EntityTypeBuilder<EmergencyContactDetails> builder)
        {
            builder.ToTable("EmergencyContactDetails");

            builder
               .Property(e => e.Id)
               .HasColumnName("Id");

            builder
               .Property(e => e.EmployeeId)
               .IsRequired()
               .HasColumnName("EmployeeId");

            builder
                .Property(e => e.ContactType)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder
               .Property(e => e.ContactName)
               .HasMaxLength(100)
               .IsUnicode(false);

            builder
               .Property(e => e.Relationship)
               .HasMaxLength(50)
               .IsUnicode(false);

            builder
               .Property(e => e.AddressLine1)
               .HasMaxLength(500)
               .IsUnicode(false);

            builder
              .Property(e => e.AddressLine2)
              .HasMaxLength(500)
              .IsUnicode(false);

            builder
               .Property(e => e.City)
               .HasMaxLength(150)
               .IsUnicode(false);

            builder
               .Property(e => e.State)
               .HasMaxLength(150)
               .IsUnicode(false);

            builder
               .Property(e => e.PostalCode)
               .HasMaxLength(50)
               .IsUnicode(false);

            builder
              .Property(e => e.Country)
              .HasMaxLength(150)
              .IsUnicode(false);

            builder
              .Property(e => e.TelePhoneNo)
              .HasMaxLength(30)
              .IsUnicode(false);

            builder
               .Property(e => e.MobileNo)
               .HasMaxLength(30)
               .IsUnicode(false);

            builder
              .Property(e => e.EmailAddress)
              .HasMaxLength(100)
              .IsUnicode(false);


            builder
                .Property(e => e.IsActive)
                .HasColumnType("boolean");

            builder
            .Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

            builder
               .Property(e => e.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(e => e.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(e => e.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(e => e.IsPrimary)
               .HasColumnType("boolean");

            builder.Ignore(e => e.CurrentUser);
        }
    }
}
