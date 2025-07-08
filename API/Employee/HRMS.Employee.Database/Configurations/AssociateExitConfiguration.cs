using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateExitConfiguration : IEntityTypeConfiguration<AssociateExit>
    {
        public void Configure(EntityTypeBuilder<AssociateExit> builder)
        {
            builder.ToTable("AssociateExit");
            builder
              .Property(c => c.AssociateExitId)
              .HasColumnName("AssociateExitId");

            builder
               .Property(c => c.EmployeeId)
               .IsRequired()
               .HasColumnName("EmployeeId");

            builder
              .Property(c => c.ExitTypeId)
              .IsRequired()
              .HasColumnName("ExitTypeId");

            builder
             .Property(c => c.ProjectId)
             .HasColumnName("ProjectId");

            builder
            .Property(c => c.AssociateAllocationId)
            .HasColumnName("AssociateAllocationId");

            builder
              .Property(c => c.ActualExitReasonId)
              .HasColumnName("ActualExitReasonId");

            builder
              .Property(c => c.ActualExitReasonDetail)
              .HasColumnName("ActualExitReasonDetail")
              .HasMaxLength(256)
              .IsUnicode(false);

            builder
             .Property(c => c.ExitReasonId)
             .HasColumnName("ExitReasonId");

            builder
              .Property(c => c.ExitReasonDetail)
              .HasColumnName("ExitReasonDetail")
              .HasMaxLength(256)
              .IsUnicode(false);

            builder
             .Property(c => c.ResignationRecommendation)
             .HasColumnName("ResignationRecomendation")
             .HasMaxLength(256)
             .IsUnicode(false);

            builder
            .Property(c => c.ResignationDate)
            .HasColumnName("ResignationDate")
            .HasColumnType("timestamp with time zone");

            builder
             .Property(c => c.ExitDate)
             .HasColumnName("ExitDate")
             .HasColumnType("timestamp with time zone");

            builder
             .Property(c => c.CalculatedExitDate)
             .HasColumnName("CalculatedExitDate")
             .HasColumnType("timestamp with time zone");

            builder
             .Property(c => c.ActualExitDate)
             .HasColumnName("ActualExitDate")
             .HasColumnType("timestamp with time zone");

            builder
            .Property(c => c.RehireEligibility)
            .HasColumnName("RehireEligibility");

            builder
           .Property(c => c.RehireEligibilityDetail)
           .HasColumnName("RehireEligibilityDetail");

            builder
             .Property(c => c.ResignationWithdrawn)
             .HasColumnName("ResignationWithdrawn");

            builder
             .Property(c => c.WithdrawReason)
             .HasColumnName("WithdrawReason");

            builder
             .Property(c => c.WithdrawRemarks)
             .HasColumnName("WithdrawRemarks");

            builder
             .Property(c => c.TransitionRequired)
             .HasColumnName("TransitionRequired")
             .HasDefaultValue(true);

            builder
           .Property(c => c.TransitionRemarks)
           .HasColumnName("TransitionRemarks");

            builder
             .Property(c => c.ImpactOnClientDelivery)
             .HasColumnName("ImpactOnClientDelivery");

            builder
             .Property(c => c.ImpactOnClientDeliveryDetail)
             .HasColumnName("ImpactOnClientDeliveryDetail")
             .HasMaxLength(256)
             .IsUnicode(false);

            builder
             .Property(c => c.Tenure)
             .HasColumnName("Tenure");

            builder
            .Property(c => c.Retained)
            .HasColumnName("Retained");

            builder
            .Property(c => c.RetainedDetail)
            .HasColumnName("RetainedDetail")
            .HasMaxLength(256)
            .IsUnicode(false);

            builder
          .Property(c => c.LegalExit)
          .HasColumnName("LegalExit");

            builder
          .Property(c => c.StatusId)
          .HasColumnName("StatusId");

            builder
           .Property(c => c.AssociateRemarks)
           .HasColumnName("AssociateRemarks")
           .HasMaxLength(256)
           .IsUnicode(false);

            EntityConfiguration.Add(builder);

        }
    }
}
