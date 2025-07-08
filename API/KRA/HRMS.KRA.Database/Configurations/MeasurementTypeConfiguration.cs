using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.KRA.Database.Configurations
{
    class MeasurementTypeConfiguration : IEntityTypeConfiguration<MeasurementType>
    {
        public void Configure(EntityTypeBuilder<MeasurementType> builder)
        {
            builder.ToTable("MeasurementType");
            builder.HasKey(e => e.MeasurementTypeId);

            builder
                .Property(e => e.MeasurementTypeName)
                .HasMaxLength(140)
                .HasColumnType("varchar(140)")
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

            builder.Ignore(asp => asp.CurrentUser);
        }
    }
}
