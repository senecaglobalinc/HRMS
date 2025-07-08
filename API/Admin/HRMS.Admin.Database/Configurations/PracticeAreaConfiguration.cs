using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class PracticeAreaConfiguration : IEntityTypeConfiguration<PracticeArea>
    {
        public void Configure(EntityTypeBuilder<PracticeArea> builder)
        {
            builder.ToTable("PracticeArea");

            builder
                .HasKey(pa => pa.PracticeAreaId);

            builder
                .Property(pa => pa.PracticeAreaCode)
                .HasMaxLength(20)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder
                .Property(pa => pa.PracticeAreaDescription)
                .HasMaxLength(512)
                .HasColumnType("varchar(512)")
                .IsRequired();

            builder
                .Property(pa => pa.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(pa => pa.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(pa => pa.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(pa => pa.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(pa => pa.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(pa => pa.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
                .Property(d => d.PracticeAreaHeadId)
                .HasColumnType("Integer");

            builder.Ignore(pa => pa.CurrentUser);
        }
    }
}