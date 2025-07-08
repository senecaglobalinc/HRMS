using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Database.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Status");
            builder
                .HasKey(sm => sm.StatusId);

            builder
                .Property(sm => sm.StatusText)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
              .Property(sm => sm.StatusDescription)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder
               .Property(sm => sm.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(sm => sm.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(sm => sm.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(sm => sm.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(sm => sm.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(sm => sm.IsActive);

            builder.Ignore(sm => sm.CurrentUser);
        }
    }
}
