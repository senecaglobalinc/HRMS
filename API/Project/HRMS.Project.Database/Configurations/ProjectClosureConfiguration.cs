using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectClosureConfiguration : IEntityTypeConfiguration<Entities.ProjectClosure>
    {
        public void Configure(EntityTypeBuilder<Entities.ProjectClosure> builder)
        {
            builder.ToTable("ProjectClosure");

            builder
                .HasKey(e => e.ProjectClosureId);

            builder
                .Property(e => e.ProjectId)
                .HasColumnName("ProjectId")
                .HasColumnType("int");
            
            builder.Property(e => e.Remarks)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)")
                .IsUnicode(false);

            builder
                .Property(e => e.ExpectedClosureDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ActualClosureDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(e => e.IsTransitionRequired)
               .HasColumnType("boolean");

            builder
               .Property(e => e.StatusId)
               .HasColumnType("int");

            EntityConfiguration.Add(builder);
        }
    }

}
