using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.KRA.Database.Configurations
{
    public class AspectConfiguration : IEntityTypeConfiguration<Aspect>
    {
        public void Configure(EntityTypeBuilder<Aspect> builder)
        {
            builder.ToTable("Aspect");
            builder
                .HasKey(asp => asp.AspectId);

            builder
                .Property(asp => asp.AspectName)
                .HasMaxLength(70)
                .HasColumnType("varchar(70)")
                .IsRequired();

            builder
                .Property(asp => asp.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(asp => asp.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(asp => asp.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(asp => asp.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(asp => asp.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
                .Property(asp => asp.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(asp => asp.CurrentUser);
        }
    }
}
