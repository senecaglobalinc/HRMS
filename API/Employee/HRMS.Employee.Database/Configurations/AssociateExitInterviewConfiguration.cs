using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateExitInterviewConfiguration : IEntityTypeConfiguration<AssociateExitInterview>
    {
        public void Configure(EntityTypeBuilder<AssociateExitInterview> builder)
        {
            builder.HasKey(e => e.AssociateExitInterviewId);

            builder
                .Property(c => c.AssociateExitId)
                .HasColumnName("AssociateExitId");

            builder
                .Property(c => c.ReasonId)
                .HasColumnName("ReasonId");

            builder
                .Property(c => c.AlternateMobileNo)
                .HasColumnName("AlternateMobileNo");

            builder
                .Property(c => c.AlternateEmail)
                .HasColumnName("AlternateEmail");

            builder
                .Property(c => c.ShareEmploymentInfo)
                .HasColumnName("ShareEmploymentInfo");

            builder
                .Property(c => c.IncludeInExAssociateGroup)
                .HasColumnName("IncludeInExAssociateGroup");

            builder.Property(e => e.AlternateAddress)
                .HasMaxLength(250)
                .HasColumnType("varchar(250)")
                .IsUnicode(false);

            builder.Property(e => e.Remarks)
                .HasMaxLength(250)
                .HasColumnType("varchar(250)")
                .IsUnicode(false);

            builder.Property(e => e.ReasonDetail)
                .HasMaxLength(250)
                .HasColumnType("varchar(250)")
                .IsUnicode(false);

            builder
               .Property(c => c.IsNotified)
               .HasColumnName("IsNotified");

            builder.HasOne(d => d.AssociateExit)
                .WithMany(p => p.AssociateExitInterview)
                .HasForeignKey(d => d.AssociateExitId);

            EntityConfiguration.Add(builder);
        }
    }
}
