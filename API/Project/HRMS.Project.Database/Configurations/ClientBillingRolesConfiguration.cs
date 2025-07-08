using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ClientBillingRolesConfiguration : IEntityTypeConfiguration<ClientBillingRoles>
    {
        public void Configure(EntityTypeBuilder<ClientBillingRoles> builder)
        {
            builder
                .HasKey(e => e.ClientBillingRoleId);

            builder
                .Property(e => e.ClientBillingRoleCode)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder
                .Property(e => e.ClientBillingRoleName)
                .HasMaxLength(60)
                .HasColumnType("varchar(60)")
                .IsUnicode(false);

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.EndDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.IsActive)
                .HasColumnType("boolean");

            builder
                .Property(e => e.StartDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .HasDefaultValueSql("inet_client_addr()");

            builder
                .HasOne(d => d.Project)
                .WithMany(p => p.ClientBillingRoles)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_ClientBillingRoles_Projects");

            builder
                .Property(d => d.ClientId)
                .HasColumnType("integer");

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
