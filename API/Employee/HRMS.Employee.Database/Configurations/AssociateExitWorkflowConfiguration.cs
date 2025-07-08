using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateExitWorkflowConfiguration : IEntityTypeConfiguration<AssociateExitWorkflow>
    {
        public void Configure(EntityTypeBuilder<AssociateExitWorkflow> builder)
        {
            builder.HasKey(e => e.AssociateExitWorkflowId);

            builder.Property(e => e.Comments)
                .HasMaxLength(250)
                .HasColumnType("varchar(250)")
                .IsUnicode(false);

            builder.Property(e => e.SubmittedDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.HasOne(d => d.AssociateExit)
                .WithMany(p => p.AssociateExitWorkflow)
                .HasForeignKey(d => d.AssociateExitId);

            EntityConfiguration.Add(builder);
        }
    }
}
