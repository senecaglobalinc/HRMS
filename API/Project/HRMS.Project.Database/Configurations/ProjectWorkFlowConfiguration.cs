using HRMS.Project.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Database.Configurations
{
    public class ProjectWorkFlowConfiguration : IEntityTypeConfiguration<ProjectWorkFlow>
    {
        public void Configure(EntityTypeBuilder<ProjectWorkFlow> builder)
        {
            builder.HasKey(e => e.WorkFlowId);

            builder.Property(e => e.Comments)
                .HasMaxLength(250)
                .HasColumnType("varchar(250)")
                .IsUnicode(false);

            builder.Property(e => e.SubmittedDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.HasOne(d => d.Project)
                .WithMany(p => p.ProjectWorkFlow)
                .HasForeignKey(d => d.ProjectId);
            
            builder
                .Ignore(d => d.CurrentUser);
        }
    }
}
