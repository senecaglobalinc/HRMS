using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Database.Configurations
{
    public class DefinitionConfiguration : IEntityTypeConfiguration<Definition>
    {
        public void Configure(EntityTypeBuilder<Definition> builder)
        {
            builder.ToTable("Definition");
            builder
                .HasKey(d => d.DefinitionId);

            builder
                .Property(dt => dt.FinancialYearId)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(dt => dt.RoleTypeId)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(d => d.AspectId)
                .HasColumnType("int")
                .IsRequired();

            builder
               .Property(dt => dt.Metric)
               .HasMaxLength(255)
               .HasColumnType("varchar(255)");

            builder
               .Property(dt => dt.OperatorId)
               .HasColumnType("int")
               .IsRequired();

            builder
               .Property(dt => dt.MeasurementTypeId)
               .HasColumnType("int")
               .IsRequired();

            builder
               .Property(dt => dt.ScaleId)
               .HasColumnType("int")
               .IsRequired(false);

            builder
               .Property(dt => dt.TargetValue)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
                .Property(dt => dt.TargetPeriodId)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(d => d.IsActive)
                .HasColumnType("boolean")
                .IsRequired();

            builder
               .Property(d => d.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(d => d.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(d => d.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(d => d.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(d => d.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(d => d.CurrentUser);

            builder
               .HasOne(d => d.Aspect)
               .WithMany(a => a.Definitions)
               .HasForeignKey(d => d.AspectId)
               .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
