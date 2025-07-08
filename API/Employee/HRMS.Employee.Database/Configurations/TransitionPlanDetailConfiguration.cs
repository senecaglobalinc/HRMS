using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class TransitionPlanDetailConfiguration : IEntityTypeConfiguration<TransitionPlanDetail>
    {

        public void Configure(EntityTypeBuilder<TransitionPlanDetail> builder)
        {
            builder.ToTable("TransitionPlanDetail");

            builder.Property(c => c.TransitionPlanDetailId)
              .HasColumnName("TransitionPlanDetailId");

            builder
              .Property(c => c.TransitionPlanId)
              .HasColumnName("TransitionPlanId");

            builder
              .Property(c => c.ActivityId)
              .HasColumnName("ActivityId");

            builder
              .Property(c => c.StartDate)
              .HasColumnName("StartDate")
              .HasColumnType("timestamp with time zone");

            builder
              .Property(c => c.EndDate)
              .HasColumnName("EndDate")
              .HasColumnType("timestamp with time zone");

            builder
              .Property(c => c.Remarks)
              .HasColumnName("Remarks");

            builder
              .Property(c => c.Status)
              .HasColumnName("Status")
              .HasMaxLength(256);

            builder
              .Property(c => c.ActivityDescription)
              .HasColumnName("ActivityDescription");

            builder.HasOne(d => d.TransitionPlan)
                   .WithMany(p => p.TransitionPlanDetail)
                   .HasForeignKey(d => d.TransitionPlanId);

            builder.HasIndex(e => new { e.TransitionPlanId, e.ActivityId })
                   .HasName("UNIQUE_TPID_ACTIVITYID")
                   .IsUnique();

            EntityConfiguration.Add(builder);
        }
    }

}

