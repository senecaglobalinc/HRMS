using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class SGRolePrefixConfiguration : IEntityTypeConfiguration<SGRolePrefix>
    {
        public void Configure(EntityTypeBuilder<SGRolePrefix> builder)
        {
            builder.ToTable("SGRolePrefix");

            builder
               .HasKey(p => p.PrefixID);

            builder
               .Property(p => p.PrefixName)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)")
               .IsRequired();

            builder
                .Property(p => p.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(p => p.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(p =>  p.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(p => p.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(p => p.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(p => p.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(p => p.CurrentUser);
        }
    }
}
