using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class GradesConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Grade");

            builder
               .HasKey(grd => grd.GradeId);

            builder
                .Property(grd => grd.GradeCode)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder
                .Property(grd => grd.GradeName)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)")
                .IsRequired();

            builder
                .Property(grd => grd.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(grd => grd.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(grd => grd.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(grd => grd.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(grd => grd.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(grd => grd.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(grd => grd.CurrentUser);
        }
    }
}
