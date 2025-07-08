using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class SGRoleConfiguration : IEntityTypeConfiguration<SGRole>
    {
        public void Configure(EntityTypeBuilder<SGRole> builder)
        {
            builder.ToTable("SGRole");

            builder
               .HasKey(s => s.SGRoleID);

            builder
               .Property(s => s.SGRoleName)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)")
               .IsRequired();

            builder
                .HasOne(d => d.Department)
                .WithMany(dt => dt.SGRoles)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

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
