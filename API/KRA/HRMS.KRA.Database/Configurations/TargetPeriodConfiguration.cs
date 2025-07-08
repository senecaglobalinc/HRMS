using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.KRA.Database.Configurations
{
    public class TargetPeriodConfiguration : IEntityTypeConfiguration<TargetPeriod>
    {
        public void Configure(EntityTypeBuilder<TargetPeriod> builder)
        {
            builder.ToTable("TargetPeriod");
            builder.HasKey(tp => tp.TargetPeriodId);

            builder
                .Property(tp => tp.TargetPeriodValue)
                .HasMaxLength(30)
                .HasColumnType("varchar(30)")
                .IsRequired();

            builder
                .Property(tp => tp.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(tp => tp.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(tp => tp.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(tp => tp.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(tp => tp.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
                .Property(tp => tp.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(tp => tp.CurrentUser);
        }
    }
}
