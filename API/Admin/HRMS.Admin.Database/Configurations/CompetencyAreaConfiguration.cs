using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class CompetencyAreaConfiguration : IEntityTypeConfiguration<CompetencyArea>
    {
        public void Configure(EntityTypeBuilder<CompetencyArea> builder)
        {
            builder.ToTable("CompetencyArea");

            builder
                .HasKey(ca => ca.CompetencyAreaId);

            builder
                .Property(ca => ca.CompetencyAreaCode)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder
                .Property(ca => ca.CompetencyAreaDescription)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder
                .Property(ca => ca.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(ca => ca.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(ca => ca.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(ca => ca.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(ca => ca.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(ca => ca.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(ca => ca.CurrentUser);
        }
    }
}