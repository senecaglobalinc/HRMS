using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class SGRoleSuffixConfiguration : IEntityTypeConfiguration<SGRoleSuffix>
    {
        public void Configure(EntityTypeBuilder<SGRoleSuffix> builder)
        {
            builder.ToTable("SGRoleSuffix");

            builder
               .HasKey(s => s.SuffixID);

            builder
               .Property(s => s.SuffixName)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)")
               .IsRequired();

            builder
                .Property(s => s.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(s => s.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(s => s.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(s => s.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(s => s.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(s => s.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(s => s.CurrentUser);
        }
    }
}
