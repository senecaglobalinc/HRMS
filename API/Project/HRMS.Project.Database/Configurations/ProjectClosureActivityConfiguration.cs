using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Project.Database.Configurations
{
    class ProjectClosureActivityConfiguration : IEntityTypeConfiguration<ProjectClosureActivity>
    {
        public void Configure(EntityTypeBuilder<ProjectClosureActivity> builder)
        {
            builder.ToTable("ProjectClosureActivity");

            builder
               .HasKey(pt => pt.ProjectClosureActivityId);

            builder
                .HasOne(pt => pt.ProjectClosure)
                .WithMany(pt => pt.ProjectClosureActivities)
                .HasForeignKey(pt => pt.ProjectClosureId);

            builder
                .Property(pt => pt.DepartmentId)
                .HasColumnType("int");            

            builder
              .Property(pt => pt.Remarks)
              .HasMaxLength(256)
              .HasColumnType("varchar(256)");

            EntityConfiguration.Add(builder);
        }

        
   
    }

}
