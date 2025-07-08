using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class ReasonTypeConfiguration : IEntityTypeConfiguration<ReasonType>
    {
        public void Configure(EntityTypeBuilder<ReasonType> builder)
        {
            builder.ToTable("ReasonType");

            builder
               .HasKey(pt => pt.ReasonTypeId);            

            builder
                .Property(pt => pt.Description)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder
               .Property(cm => cm.ParentId)
               .HasColumnType("int")
               .IsRequired();

            builder.Ignore(cm => cm.ParentReasonType);

            EntityConfiguration.Add(builder);
        }
    }
}
