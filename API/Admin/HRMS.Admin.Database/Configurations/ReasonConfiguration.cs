using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class ReasonConfiguration : IEntityTypeConfiguration<Reason>
    {
        public void Configure(EntityTypeBuilder<Reason> builder) 
        {
            builder.ToTable("Reason");

            builder
               .HasKey(rn => rn.ReasonId);

            builder
                .Property(rn => rn.ReasonCode)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder
                .Property(rn => rn.Description)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");
          
            builder
                .HasOne(rn => rn.ReasonType)
                .WithMany(rt => rt.Reasons)
                .HasForeignKey(rn => rn.ReasonTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            EntityConfiguration.Add(builder);
        }
    }
}
