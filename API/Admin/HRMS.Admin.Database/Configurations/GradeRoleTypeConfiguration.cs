using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class GradeRoleTypeConfiguration : IEntityTypeConfiguration<GradeRoleType>
    {
        public void Configure(EntityTypeBuilder<GradeRoleType> builder)
        {
            builder.ToTable("GradeRoleType");

            builder.HasKey(e => e.GradeRoleTypeId);
            builder
                .Property(e => e.RoleTypeId)
                .HasColumnType("int")
                .IsRequired();
            builder
                .Property(e => e.GradeId)
                .HasColumnType("int")
                .IsRequired();
            builder
                .HasOne(e => e.Grade)
                .WithMany(e => e.GradeRoleTypes)
                .HasForeignKey(e => e.GradeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder
                .HasOne(e => e.RoleType)
                .WithMany(e => e.GradeRoleTypes)
                .HasForeignKey(e => e.RoleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder
                .HasIndex(e => new { e.RoleTypeId, e.GradeId }).IsUnique();
            builder
                .Property(pt => pt.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(pt => pt.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(pt => pt.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(pt => pt.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Property(pt => pt.IsActive);
            builder.Ignore(pt => pt.CurrentUser);
            builder.Ignore(pt => pt.SystemInfo);
        }
    }
}
