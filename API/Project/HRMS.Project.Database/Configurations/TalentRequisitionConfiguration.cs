using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class TalentRequisitionConfiguration : IEntityTypeConfiguration<TalentRequisition>
    {
        public void Configure(EntityTypeBuilder<TalentRequisition> builder)
        {
            builder
                .HasKey(e => e.TrId);

            builder
                .Property(e => e.ClientId)
                .HasColumnType("Integer");

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.CreatedBy)
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
                .Property(e => e.Remarks)
                .HasMaxLength(1500)
                .HasColumnType("varchar(1500)");

            builder
                .Property(e => e.RequestedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.RequiredDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .HasDefaultValueSql("inet_client_addr()");

            builder
                .Property(e => e.TargetFulfillmentDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.Trcode)
                .HasColumnName("TRCode")
                .HasMaxLength(20)
                .HasColumnType("varchar(20)")
                .IsUnicode(false);

            builder
                .Property(d => d.DepartmentId)
                .HasColumnType("Integer");

            builder
                .Property(d => d.PracticeAreaId)
                .HasColumnType("Integer"); 

            builder
                .HasOne(d => d.Project)
                .WithMany(p => p.TalentRequisition)
                .HasForeignKey(d => d.ProjectId);

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
