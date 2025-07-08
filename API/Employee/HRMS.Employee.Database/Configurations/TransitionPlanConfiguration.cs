using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Employee.Entities;

namespace HRMS.Employee.Database.Configurations
{
    public class TransitionPlanConfiguration : IEntityTypeConfiguration<TransitionPlan>
    {
        
            public void Configure(EntityTypeBuilder<TransitionPlan> builder)
            {
                builder.ToTable("TransitionPlan");
                builder.Property(c => c.TransitionPlanId)
                    .HasColumnName("TransitionPlanId");

                builder
                  .Property(c => c.AssociateExitId)
                  .HasColumnName("AssociateExitId");

                builder
                   .Property(c => c.ProjectClosureId)
                   .HasColumnName("ProjectClosureId");

                builder
                  .Property(c => c.AssociateReleaseId)
                  .HasColumnName("AssociateReleaseId");

                builder
                  .Property(c => c.TransitionFrom)
                  .HasColumnName("TransitionFrom");

                builder
                 .Property(c => c.TransitionTo)
                 .HasColumnName("TransitionTo");

                builder
                  .Property(c => c.StartDate)
                  .HasColumnName("StartDate")
                  .HasColumnType("timestamp with time zone");

                builder
                 .Property(c => c.EndDate)
                 .HasColumnName("EndDate")
                 .HasColumnType("timestamp with time zone");

                builder
                .Property(c => c.KnowledgeTransferred)
                .HasColumnName("KnowledgeTransferred");


            builder
             .Property(c => c.KnowledgeTransaferredRemarks)
             .HasColumnName("KnowledgeTransaferredRemarks");

                builder
                 .Property(c => c.Others)
                 .HasColumnName("Others")
                 .HasMaxLength(56);

                builder
              .Property(c => c.StatusId)
              .HasColumnName("StatusId");

            builder.HasOne(d => d.AssociateExit)
               .WithMany(p => p.TransitionPlan)
               .HasForeignKey(d => d.AssociateExitId);

            EntityConfiguration.Add(builder);

            }
        }
    
}
