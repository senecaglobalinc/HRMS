using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class ActivityTypeConfiguration : IEntityTypeConfiguration<ActivityType>
    {
        public void Configure(EntityTypeBuilder<ActivityType> builder)
        {
            builder.ToTable("ActivityType");

            builder
               .HasKey(pt => pt.ActivityTypeId);            

            builder
                .Property(pt => pt.Description)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder
               .Property(cm => cm.ParentId)
               .HasColumnType("int")
               .IsRequired();

            builder.Ignore(cm => cm.ParentActivityType);

            EntityConfiguration.Add(builder);
        }
    }
}
