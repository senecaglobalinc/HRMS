using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class AssociateAllocationConfiguration : IEntityTypeConfiguration<AssociateAllocation>
    {
        public void Configure(EntityTypeBuilder<AssociateAllocation> builder)
        {

            builder
                .HasKey(e => e.AssociateAllocationId);

            builder
                .HasIndex(e => e.ProjectId);

            builder
                .Property(e => e.AllocationDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ClientBillingPercentage)
                .HasColumnType("decimal(18, 0)")
                .HasDefaultValueSql("((0))");

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder
                .Property(e => e.EffectiveDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.InternalBillingPercentage)
                .HasColumnType("decimal(18, 0)");

            builder
                .Property(e => e.IsPrimary)
                .HasColumnType("boolean")
                .HasDefaultValueSql("(('f'))");

            builder
                .Property(e => e.LeadId)
                .HasColumnName("LeadID");

            builder
                .Property(e => e.ModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ProgramManagerId)
                .HasColumnName("ProgramManagerID");

            builder
                .Property(e => e.ReleaseDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.SystemInfo)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .HasDefaultValueSql("inet_client_addr()")
                .IsUnicode(false);

            builder
                .Property(e => e.Trid)
                .HasColumnName("TRId")
                .HasColumnType("Integer");

            builder
                .HasOne(d => d.AllocationPercentageNavigation)
                .WithMany(p => p.AssociateAllocation)
                .HasForeignKey(d => d.AllocationPercentage);

            builder
                .Property(d => d.EmployeeId)
                .HasColumnType("Integer");

            builder
                .Property(d => d.LeadId)
                .HasColumnType("Integer"); ;

            builder
                .Property(d => d.ProgramManagerId)
                .HasColumnType("Integer"); ;

            builder
                .Property(d => d.ProjectId)
                .HasColumnType("Integer");

            builder
                .Property(d => d.ReportingManagerId)
                .HasColumnType("Integer");

            builder
                .Property(d => d.RoleMasterId)
                .HasColumnType("Integer");

            builder
                .Property(d => d.Trid)
                .HasColumnType("Integer");

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
