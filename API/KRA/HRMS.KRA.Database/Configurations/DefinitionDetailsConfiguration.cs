using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Database.Configurations
{
    public class DefinitionDetailsConfiguration : IEntityTypeConfiguration<DefinitionDetails>
    {
        public void Configure(EntityTypeBuilder<DefinitionDetails> builder)
        {
            builder.ToTable("DefinitionDetails");
            builder
                .HasKey(dd => dd.DefinitionDetailsId);

            builder
                .Property(dd => dd.DefinitionId)
                .HasColumnType("int")
                .IsRequired();

            builder
               .Property(dd => dd.Metric)
               .HasMaxLength(255)
               .HasColumnType("varchar(255)");

            builder
              .Property(dd => dd.OperatorId)
                .HasColumnType("int")
                .IsRequired();

            builder
              .Property(dd => dd.MeasurementTypeId)
                .HasColumnType("int")
                .IsRequired();

            builder
              .Property(dd => dd.ScaleId)
                .HasColumnType("int")
                .IsRequired(false);

            builder
               .Property(dd => dd.TargetValue)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
              .Property(dd => dd.TargetPeriodId)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(dd => dd.IsDeleted)
                .HasColumnType("boolean");

            builder
               .Property(dd => dd.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(dd => dd.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(dd => dd.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(dd => dd.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(dd => dd.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(dd => dd.IsActive);

            builder.Ignore(dd => dd.CurrentUser);

            //builder
            //   .HasOne(dd => dd.Definition)
            //   .WithMany(d => d.DefinitionDetails)
            //   .HasForeignKey(dd => dd.DefinitionId)
            //   .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .HasOne(dd => dd.Operator)
               .WithMany(ko => ko.DefinitionDetails)
               .HasForeignKey(dd => dd.OperatorId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .HasOne(dd => dd.MeasurementType)
               .WithMany(mt => mt.DefinitionDetails)
               .HasForeignKey(dd => dd.MeasurementTypeId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .HasOne(dd => dd.Scale)
               .WithMany(ksm => ksm.DefinitionDetails)
               .HasForeignKey(dd => dd.ScaleId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .HasOne(dd => dd.TargetPeriod)
               .WithMany(tp => tp.DefinitionDetails)
               .HasForeignKey(dd => dd.TargetPeriodId)
               .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
