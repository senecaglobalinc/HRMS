using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class SOWConfiguration : IEntityTypeConfiguration<SOW>
    {
        public void Configure(EntityTypeBuilder<SOW> builder)
        {
            builder
                .HasIndex(e => new { e.SOWId, e.ProjectId })
                .IsUnique();

            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.Id)
                .HasColumnName("Id")
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
                .Property(e => e.SOWFileName)
                .HasColumnName("SOWFileName")
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder.Property(e => e.SOWId)
                .IsRequired()
                .HasColumnName("SOWId")
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder.Property(e => e.SOWSignedDate)
                .HasColumnName("SOWSignedDate")
                .HasColumnType("timestamp with time zone");

            builder.Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .HasDefaultValueSql("inet_client_addr()");

            builder.HasOne(d => d.Project)
                .WithMany(p => p.SOW)
                .HasForeignKey(d => d.ProjectId);

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
