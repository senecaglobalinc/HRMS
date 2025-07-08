using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class NotificationConfigurationConfiguration : IEntityTypeConfiguration<NotificationConfiguration>
    {
        public void Configure(EntityTypeBuilder<NotificationConfiguration> builder)
        {
            builder.ToTable("NotificationConfiguration");

            builder
                .HasKey(nc => nc.NotificationConfigurationId);

            builder
               .HasOne(nc => nc.NotificationType)
               .WithMany(nt => nt.NotificationConfigurations)
               .HasForeignKey(nc => nc.NotificationTypeId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(nc => nc.EmailFrom)
                .HasMaxLength(512)
                .HasColumnType("varchar(512)");

            builder
                .Property(nc => nc.EmailTo)
                .HasMaxLength(512)
                .HasColumnType("varchar(512)");

            builder
                .Property(nc => nc.EmailCC)
                .HasMaxLength(512)
                .HasColumnType("varchar(512)");

            builder
                .Property(nc => nc.EmailSubject)
                .HasMaxLength(150)
                .HasColumnType("varchar(150)");

            builder
                .Property(nc => nc.EmailContent)
                .HasColumnType("text");

            builder
                .Property(nc => nc.SLA)
                .HasColumnType("int");

            builder
               .HasOne(nc => nc.CategoryMaster)
               .WithMany(ct => ct.NotificationConfigurations)
               .HasForeignKey(nc => nc.CategoryMasterId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(nc => nc.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(nc => nc.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(nc => nc.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(nc => nc.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(nc => nc.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(nc => nc.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(nc => nc.CurrentUser);
        }
    }
}