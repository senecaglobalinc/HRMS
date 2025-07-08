using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectClosureWorkflowConfiguration : IEntityTypeConfiguration<ProjectClosureWorkflow>
    {
        public void Configure(EntityTypeBuilder<ProjectClosureWorkflow> builder)
        {
            builder.HasKey(e => e.ProjectClosureWorkflowId);

            builder.Property(e => e.Comments)
                .HasMaxLength(250)
                .HasColumnType("varchar(250)")
                .IsUnicode(false);

            builder.Property(e => e.SubmittedDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.HasOne(d => d.ProjectClosure)
                .WithMany(p => p.ProjectClosureWorkflows)
                .HasForeignKey(d => d.ProjectClosureId);

            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
