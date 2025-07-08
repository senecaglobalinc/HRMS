using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.KRA.Database.Configurations
{
    public class ScaleDetailsConfiguration : IEntityTypeConfiguration<ScaleDetails>
    {
        public void Configure(EntityTypeBuilder<ScaleDetails> builder)
        {
            builder.ToTable("ScaleDetails");
            builder
                .HasKey(sd => sd.ScaleDetailId);
            
            builder
               .HasOne(sd => sd.Scale)
               .WithMany(sm => sm.ScaleDetails)
               .HasForeignKey(sd => sd.ScaleId)
               .OnDelete(DeleteBehavior.ClientSetNull);
            
            builder
                .Property(sd => sd.ScaleValue)
                .HasColumnType("integer");
            
            builder
               .Property(sd => sd.ScaleDescription)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");
            
            builder
                .Property(sd => sd.CreatedDate)
                .HasColumnType("timestamp with time zone");
            
            builder
               .Property(sd => sd.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");
            
            builder
                .Property(sd => sd.ModifiedDate)
                .HasColumnType("timestamp with time zone");
            
            builder
               .Property(sd => sd.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");
            
            builder
               .Property(sd => sd.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");
            
            builder
                .Property(sd => sd.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(sd => sd.CurrentUser);
        }
    }
}
