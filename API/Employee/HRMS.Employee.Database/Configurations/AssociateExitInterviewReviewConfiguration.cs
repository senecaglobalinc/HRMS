using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateExitInterviewReviewConfiguration : IEntityTypeConfiguration<AssociateExitInterviewReview>
    {
        public void Configure(EntityTypeBuilder<AssociateExitInterviewReview> builder)
        {
            builder.HasKey(e => e.AssociateExitInterviewReviewId);

            builder
                .Property(c => c.AssociateExitInterviewId)
                .HasColumnName("AssociateExitInterviewId");

            builder
                .Property(c => c.ShowInitialRemarks)
                .HasColumnName("ShowInitialRemarks");

            builder.Property(e => e.FinalRemarks)
               .HasMaxLength(250)
               .HasColumnType("varchar(250)")
               .IsUnicode(false);

            EntityConfiguration.Add(builder);
        }
    }

}
