using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class DepartmentTypeConfiguration : IEntityTypeConfiguration<DepartmentType>
    {
        public void Configure(EntityTypeBuilder<DepartmentType> builder)
        {
            builder.ToTable("DepartmentType");

            builder
               .HasKey(dt => dt.DepartmentTypeId);

            builder
                .Property(dt => dt.DepartmentTypeDescription)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(dt => dt.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(dt => dt.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(dt => dt.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(dt => dt.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(dt => dt.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(dt => dt.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(dt => dt.CurrentUser);
        }
    }
}
