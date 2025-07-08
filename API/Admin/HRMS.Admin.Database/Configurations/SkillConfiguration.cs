using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Admin.Database.Configurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skill");

            builder
               .HasKey(sk => sk.SkillId);

            builder
               .Property(sk => sk.SkillCode)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)")
               .IsRequired();

            builder
                .Property(sk => sk.SkillName)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");


            builder
                .Property(sk => sk.SkillDescription)
                .HasMaxLength(512)
                .HasColumnType("varchar(512)");

            builder
               .Property(sk => sk.IsApproved)
               .HasColumnType("boolean");
         
            builder
               .HasOne(sk => sk.CompetencyArea)
               .WithMany(ca => ca.Skills)
               .HasForeignKey(sk => sk.CompetencyAreaId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder
              .HasOne(sk => sk.SkillGroup)
              .WithMany(sg => sg.Skills)
              .HasForeignKey(sk => sk.SkillGroupId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Property(sk => sk.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(sk => sk.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(sk => sk.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(sk => sk.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(sk => sk.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(sk => sk.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(sk => sk.CurrentUser);
        }
    }
}
