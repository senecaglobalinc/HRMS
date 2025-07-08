using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class ProjectTypeConfiguration : IEntityTypeConfiguration<ProjectType>
    {
        public void Configure(EntityTypeBuilder<ProjectType> builder)
        {
            builder.ToTable("ProjectType");

            builder
               .HasKey(pt => pt.ProjectTypeId);

            builder
                .Property(pt => pt.ProjectTypeCode)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)")
                .IsRequired();

            builder
                .Property(pt => pt.Description)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder
                .Property(pt => pt.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(pt => pt.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(pt => pt.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(pt => pt.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(pt => pt.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(pt => pt.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(pt => pt.CurrentUser);
        }
    }
}
