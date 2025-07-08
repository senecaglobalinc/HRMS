using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectSkillsRequiredConfiguration : IEntityTypeConfiguration<ProjectSkillsRequired>
    {
        public void Configure(EntityTypeBuilder<ProjectSkillsRequired> builder)
        {
            builder.ToTable("ProjectSkillsRequired");

            builder
               .Property(c => c.ProjectSkillsRequiredId)
               .HasColumnName("ProjectSkillsRequiredId");

            builder
               .Property(c => c.ProjectRoleId)
               .HasColumnName("ProjectRoleId");

            builder
               .Property(c => c.SkillId)
               .HasColumnName("SkillId");

            builder
               .Property(c => c.ProficiencyId)
               .HasColumnName("ProficiencyId");

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

            builder.HasOne(c => c.ProjectRole)
                .WithMany(p => p.ProjectSkillsRequired)
                .HasForeignKey(c => c.ProjectRoleId);
        }
    }
}
