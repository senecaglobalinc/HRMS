using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class DesignationConfiguration : IEntityTypeConfiguration<Designation>
    {
        public void Configure(EntityTypeBuilder<Designation> builder)
        {
            builder.ToTable("Designation");

            builder
               .HasKey(des => des.DesignationId);

            builder
                .Property(des => des.DesignationCode)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder
                .Property(des => des.DesignationName)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)")
                .IsRequired();

            builder
                .HasOne(des => des.Grade)
                .WithMany(grd => grd.Designations)
                .HasForeignKey(des => des.GradeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(des => des.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(des => des.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(des => des.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(des => des.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(des => des.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(des => des.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(des => des.CurrentUser);
        }
    }
}
