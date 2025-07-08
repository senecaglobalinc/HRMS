using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.KRA.Database.Configurations
{
    public class OperatorConfiguration : IEntityTypeConfiguration<Operator>
    {
        public void Configure(EntityTypeBuilder<Operator> builder)
        {
            builder.ToTable("Operator");
            builder.HasKey(e => e.OperatorId);

            builder
                .Property(e => e.OperatorValue)
                .HasMaxLength(10)
                .HasColumnType("varchar(10)")
                .IsRequired();

            builder
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(e => e.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(e => e.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(e => e.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
                .Property(e => e.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(e => e.CurrentUser);
        }
    }
}
