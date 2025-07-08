using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ClientBillingRolesHistoryConfiguration : IEntityTypeConfiguration<ClientBillingRolesHistory>
    {
        public void Configure(EntityTypeBuilder<ClientBillingRolesHistory> builder)
        {
            builder
                .ToTable("ClientBillingRoleHistory");

            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.ClientBillingRoleId)
                .HasColumnType("integer")
                .IsRequired();

            builder
                .Property(e => e.ClientBillingRoleCode)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder
                .Property(e => e.ClientBillingRoleName)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(c => c.ProjectId)
                .HasColumnType("integer")
                .IsRequired();

            builder
                .Property(c => c.NoOfPositions)
                .HasColumnType("integer")
                .IsRequired();

            builder
                .Property(e => e.StartDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.EndDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder.Property(e => e.CreatedBy)
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
                .Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .HasDefaultValueSql("inet_client_addr()");

            builder
                .Ignore(d => d.CurrentUser);
            builder
                .Ignore(d => d.IsActive);
        }
    }
}
