using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class TalentPoolConfiguration : IEntityTypeConfiguration<TalentPool>
    {
        public void Configure(EntityTypeBuilder<TalentPool> builder)
        {
            builder
                .HasKey(e => e.TalentPoolId);

            builder
                .Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnType("varchar(100)");

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .HasColumnType("varchar(100)")
                .HasDefaultValueSql("inet_client_addr()");

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
