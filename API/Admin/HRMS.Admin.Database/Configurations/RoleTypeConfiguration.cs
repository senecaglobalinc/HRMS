using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class RoleTypeConfiguration : IEntityTypeConfiguration<RoleType>
    {
        public void Configure(EntityTypeBuilder<RoleType> builder)
        {
            builder.ToTable("RoleType");

            builder.HasKey(e => e.RoleTypeId);

            builder
              .Property(e => e.RoleTypeName)
              .HasMaxLength(30)
              .HasColumnType("varchar(30)")
              .IsRequired();

            builder
              .Property(e => e.RoleTypeDescription)
              .HasMaxLength(500)
              .HasColumnType("varchar(500)");

            builder
               .Property(e => e.FinancialYearId)
               .HasColumnType("int")
               .IsRequired();

            builder
               .Property(e => e.DepartmentId)
               .HasColumnType("int")
               .IsRequired();

            builder
                .Property(rs => rs.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(rs => rs.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(rs => rs.IsDeliveryDepartment)
               .HasColumnType("boolean");

            builder
               .Property(rs => rs.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(rs => rs.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(rs => rs.IsActive)
               .HasColumnType("boolean");

            builder.Ignore(rs => rs.SystemInfo);
            builder.Ignore(rs => rs.CurrentUser);
        }
    }
}
