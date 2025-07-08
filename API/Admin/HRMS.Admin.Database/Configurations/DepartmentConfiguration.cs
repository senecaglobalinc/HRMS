using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");

            builder
               .HasKey(d => d.DepartmentId);

            builder
               .Property(d => d.Description)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)")
               .IsRequired();

            builder
               .Property(d => d.DepartmentCode)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)")
               .IsRequired();

            builder
              .Property(d => d.DepartmentHeadId);

            builder
                .HasOne(d => d.DepartmentType)
                .WithMany(dt => dt.Departments)
                .HasForeignKey(d => d.DepartmentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(d => d.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(d => d.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(d => d.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(d => d.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(d => d.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(d => d.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(d => d.CurrentUser);
            builder.Ignore(d => d.Projects);
        }
    }
}
