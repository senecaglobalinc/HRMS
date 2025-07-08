using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class ProficiencyLevelConfiguration : IEntityTypeConfiguration<ProficiencyLevel>
    {
        public void Configure(EntityTypeBuilder<ProficiencyLevel> builder)
        {
            builder.ToTable("ProficiencyLevel");

            builder
               .HasKey(pl => pl.ProficiencyLevelId);

            builder
                .Property(pl => pl.ProficiencyLevelCode)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)")
                .IsRequired();

            builder
                .Property(pl => pl.ProficiencyLevelDescription)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)")
                .IsRequired();

            builder
                .Property(pl => pl.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(pl => pl.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(pl => pl.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(pl => pl.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(pl => pl.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(pl => pl.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(pl => pl.CurrentUser);
        }
    }
}
