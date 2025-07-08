using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectTrainingPlanConfiguration : IEntityTypeConfiguration<ProjectTrainingPlan>
    {
        public void Configure(EntityTypeBuilder<ProjectTrainingPlan> builder)
        {
            builder.ToTable("ProjectTrainingPlan");

            builder
               .Property(c => c.ProjectTrainingPlanId)
               .HasColumnName("ProjectTrainingPlanId");

            builder
               .Property(c => c.ProjectId)
               .HasColumnName("ProjectId");

            builder
               .Property(c => c.AssociateId)
               .HasColumnName("AssociateId");

            builder
               .Property(c => c.SkillId)
               .HasColumnName("SkillId");

            builder
               .Property(c => c.ProjectTrainingPlanned)
               .HasMaxLength(256)
               .HasColumnType("varchar(256)");

            builder
                .Property(c => c.TrainingFromDate)
                .HasColumnType("timestamp with time zone");

            builder
                .Property(c => c.TrainingToDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.FinancialYearId)
               .HasColumnName("FinancialYearId");

            builder
               .Property(c => c.CycleId)
               .HasColumnName("CycleId");

            builder
               .Property(c => c.TrainingModeId)
               .HasColumnName("TrainingModeId");

            builder
               .Property(c => c.IsTrainingCompleted)
               .HasColumnType("boolean");

            builder
               .Property(c => c.SkillAssessedBy)
               .HasColumnName("KnowledgeAssessedBy");

            builder
                .Property(c => c.SkillAssessmentDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.SkillApplied)
               .HasColumnType("boolean");

            builder
               .Property(c => c.ProficiencyLevelAchieved)
               .HasColumnName("ProficiencyLevelAchieved");

            builder
               .Property(c => c.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(c => c.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
                .Property(c => c.IsActive)
                .HasColumnType("boolean");

            builder.Ignore(c => c.CurrentUser);

            builder.HasOne(c => c.Project)
                .WithMany(p => p.ProjectTrainingPlan)
                .HasForeignKey(c => c.ProjectId);

            builder.HasOne(c => c.TrainingMode)
                .WithMany(p => p.ProjectTrainingPlan)
                .HasForeignKey(c => c.TrainingModeId);
        }
    }
}
