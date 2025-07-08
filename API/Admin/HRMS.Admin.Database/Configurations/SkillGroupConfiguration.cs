using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class SkillGroupConfiguration : IEntityTypeConfiguration<SkillGroup>
    {
        public void Configure(EntityTypeBuilder<SkillGroup> builder)
        {
            builder.ToTable("SkillGroup");

            builder
               .HasKey(sg => sg.SkillGroupId);

            builder
                .Property(sg => sg.SkillGroupName)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder
                .Property(sg => sg.Description)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder
               .HasOne(sg => sg.CompetencyArea)
               .WithMany(ca => ca.SkillGroups)
               .HasForeignKey(sg => sg.CompetencyAreaId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(sg => sg.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(sg => sg.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(sg => sg.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(sg => sg.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(sg => sg.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(sg => sg.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(sg => sg.CurrentUser);
        }
    }
}
