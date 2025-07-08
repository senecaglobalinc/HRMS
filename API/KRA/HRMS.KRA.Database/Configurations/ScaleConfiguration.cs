using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.KRA.Database.Configurations
{
    public class ScaleConfiguration : IEntityTypeConfiguration<Scale>
    {
        public void Configure(EntityTypeBuilder<Scale> builder)
        {
            builder.ToTable("Scale");
            builder
                .HasKey(sm => sm.ScaleId);

            builder
              .Property(sm => sm.MinimumScale)
              .HasColumnType("integer");
            builder
                .Property(sm => sm.MaximumScale)
                .HasColumnType("integer");

            builder
               .Property(sm => sm.ScaleTitle)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(sm => sm.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(sm => sm.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");
            
            builder
                .Property(sm => sm.ModifiedDate)
                .HasColumnType("timestamp with time zone");
            
            builder
               .Property(sm => sm.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");
            
            builder
               .Property(sm => sm.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");
            
            builder
                .Property(sm => sm.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(sm => sm.CurrentUser);
        }
    }
}
