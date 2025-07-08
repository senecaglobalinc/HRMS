using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class NotificationTypeConfiguration : IEntityTypeConfiguration<NotificationType>
    {
        public void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            builder.ToTable("NotificationType");

            builder
               .HasKey(nt => nt.NotificationTypeId);

            builder
             .Property(nt => nt.NotificationTypeId)
             .HasColumnType("int");

            builder
                .Property(nt => nt.NotificationCode)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder
                .Property(nt => nt.NotificationDescription)
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");

            builder
               .HasOne(nt => nt.CategoryMaster)
               .WithMany(ct => ct.NotificationTypes)
               .HasForeignKey(nt => nt.CategoryMasterId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(nt => nt.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(nt => nt.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(nt => nt.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(nt => nt.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(nt => nt.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(nt => nt.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(nt => nt.CurrentUser);
        }
    }
}