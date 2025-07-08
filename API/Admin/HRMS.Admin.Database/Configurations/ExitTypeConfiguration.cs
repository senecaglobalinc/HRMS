using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class ExitTypeConfiguration : IEntityTypeConfiguration<ExitType>
    {
        public void Configure(EntityTypeBuilder<ExitType> builder)
        {
            builder.ToTable("ExitType");

            builder
               .HasKey(et => et.ExitTypeId);
           
            builder
                .Property(et => et.Description)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            EntityConfiguration.Add(builder);            
        }
    }
}
