using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class DomainConfiguration : IEntityTypeConfiguration<Domain>
    {
        public void Configure(EntityTypeBuilder<Domain> builder)
        {
            builder.ToTable("Domain");

            builder
               .HasKey(dm => dm.DomainID);

            builder
                .Property(dm => dm.DomainName)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(dm => dm.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(dm => dm.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(dm => dm.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(dm => dm.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(dm => dm.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(dm => dm.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(dm => dm.CurrentUser);
        }
    }
}
