using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class ClientHistoryConfiguration : IEntityTypeConfiguration<ClientsHistory>
    {
        public void Configure(EntityTypeBuilder<ClientsHistory> builder)
        {
            builder.ToTable("ClientsHistory");

            builder
               .HasKey(m => m.ClientHistoryId);

            builder
               .Property(m => m.ClientId)
               .IsRequired()
               .HasColumnType("int");

            builder
              .Property(c => c.ClientCode)
              .HasMaxLength(100)
              .IsRequired();

            builder
              .Property(c => c.ClientName)
              .HasMaxLength(50)
              .IsRequired();

            builder
                .Property(c => c.ClientRegisterName)
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");

            builder
                .Property(c => c.CreatedDate)
                .HasColumnType("timestamp with time zone"); 

            builder
               .Property(c => c.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(c => c.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(c => c.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder.Ignore(c => c.ModifiedBy);
            builder.Ignore(c => c.ModifiedDate);
            builder.Ignore(c => c.CurrentUser);

        }
    }
}
