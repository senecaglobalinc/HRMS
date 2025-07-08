using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.KRA.Database.Configurations
{
    public class DefinitionTransactionConfiguration : IEntityTypeConfiguration<DefinitionTransaction>
    {
        public void Configure(EntityTypeBuilder<DefinitionTransaction> builder)
        {
            builder.ToTable("DefinitionTransaction");
            builder
                .HasKey(dt => dt.DefinitionTransactionId);

            builder
                .Property(dt => dt.DefinitionId)
                .HasColumnType("uuid")
                .IsRequired();

            builder
              .Property(dt => dt.FinancialYearId)
                .HasColumnType("int")
                .IsRequired();

            builder
              .Property(dt => dt.RoleTypeId)
                .HasColumnType("int")
                .IsRequired();

            builder
              .Property(dt => dt.AspectId)
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
                .Property(dt => dt.IsAccepted)
                .HasColumnType("boolean");

            builder
               .Property(dt => dt.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(dt => dt.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(dt => dt.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(dt => dt.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(dt => dt.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(dt => dt.CurrentUser);

            builder
               .HasOne(dt => dt.Operator)
               .WithMany(ko => ko.DefinitionTransactions)
               .HasForeignKey(dt => dt.OperatorId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .HasOne(dt => dt.MeasurementType)
               .WithMany(mt => mt.DefinitionTransactions)
               .HasForeignKey(dt => dt.MeasurementTypeId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .HasOne(dt => dt.Scale)
               .WithMany(ksm => ksm.DefinitionTransactions)
               .HasForeignKey(dt => dt.ScaleId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .HasOne(dt => dt.TargetPeriod)
               .WithMany(tp => tp.DefinitionTransactions)
               .HasForeignKey(dt => dt.TargetPeriodId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Ignore(wf => wf.IsActive);
        }
    }
}
