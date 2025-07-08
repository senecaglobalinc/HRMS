using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class TrainingModeConfiguration : IEntityTypeConfiguration<TrainingMode>
    {
        public void Configure(EntityTypeBuilder<TrainingMode> builder)
        {
            builder.ToTable("TrainingMode");

            builder
               .Property(c => c.TrainingModeId)
               .HasColumnName("TrainingModeId");

            builder
                .Property(c => c.TrainingModeCode)
                .HasMaxLength(256)
                .IsUnicode(false);

            builder
               .Property(c => c.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(c => c.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
                .Property(c => c.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(c => c.CurrentUser);
        }
    }
}
