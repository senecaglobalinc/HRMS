using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
    {
        public void Configure(EntityTypeBuilder<ServiceType> builder)
        {
            builder.ToTable("ServiceType");
            builder
                .HasKey(cm => cm.ServiceTypeId);

            builder
                .Property(cm => cm.ServiceTypeName)
                .HasMaxLength(128)
                .HasColumnType("varchar(128)")
                .IsRequired();

            builder
                .Property(cm => cm.ServiceDescription)
                .HasMaxLength(128)
                .HasColumnType("varchar(128)")
                .IsRequired();


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

