using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Project.Database.Configurations
{
    class ProjectClosureActivityDetailConfiguration : IEntityTypeConfiguration<Entities.ProjectClosureActivityDetail>
    {
        public void Configure(EntityTypeBuilder<Entities.ProjectClosureActivityDetail> builder)
        {
            builder.ToTable("ProjectClosureActivityDetail");

            builder
                .HasKey(e => e.ProjectClosureActivityDetailId);

            builder
                .Property(e => e.ProjectClosureActivityId)
                .HasColumnName("ProjectClosureActivityId")
                .HasColumnType("int");

            builder.Property(e => e.ActivityId)
                .HasColumnType("int");

            builder
                .Property(e => e.Remarks)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");
            builder
               .Property(e => e.Value)
               .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            EntityConfiguration.Add(builder);

        }
    }
}

