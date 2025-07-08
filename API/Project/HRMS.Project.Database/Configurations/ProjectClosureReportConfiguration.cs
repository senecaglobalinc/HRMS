using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Project.Database.Configurations
{
    class ProjectClosureReportConfiguration : IEntityTypeConfiguration<ProjectClosureReport>
    {
        public void Configure(EntityTypeBuilder<ProjectClosureReport> builder)
        {
            builder.ToTable("ProjectClosureReport");

            builder
               .HasKey(e => e.ProjectClosureReportId);

            builder
                .Property(e => e.ProjectClosureId);

            builder
                .Property(e => e.ClientFeedback)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder
                .Property(e => e.DeliveryPerformance)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder
                .Property(e => e.ValueDelivered)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder
               .Property(e => e.ManagementChallenges)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.TechnologyChallenges)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.EngineeringChallenges)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.BestPractices)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.LessonsLearned)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.ReusableArtifacts)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.ProcessImprovements)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.Awards)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.NewTechnicalSkills)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.NewTools)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.Remarks)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
               .Property(e => e.CaseStudy)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
                .Property(d => d.StatusId)
                .HasColumnType("int");
            builder
              .Property(e => e.ClientFeedbackFile)
              .HasMaxLength(256)
              .HasColumnType("varchar(256)");
            builder
              .Property(e => e.DeliveryPerformanceFile)
              .HasMaxLength(256)
              .HasColumnType("varchar(256)");
            builder
                .Property(e => e.RejectRemarks);

            EntityConfiguration.Add(builder);
        }
    }
}