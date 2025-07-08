using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateExitActivityDetailConfiguration : IEntityTypeConfiguration<AssociateExitActivityDetail>
    {
        public void Configure(EntityTypeBuilder<AssociateExitActivityDetail> builder)
        {
            builder.HasKey(e => e.AssociateExitActivityDetailId);
            
            builder
       .Property(c => c.AssociateExitActivityId)
       .HasColumnName("AssociateExitActivityId");

            builder
     .Property(c => c.ActivityId)
     .HasColumnName("ActivityId");

            builder
    .Property(c => c.ActivityValue)
    .HasColumnName("ActivityValue");      

            builder.Property(e => e.Remarks)
               .HasMaxLength(250)
               .HasColumnType("varchar(250)")
               .IsUnicode(false);          

            builder.HasOne(d => d.AssociateExitActivity)
                .WithMany(p => p.AssociateExitActivityDetail)
                .HasForeignKey(d => d.AssociateExitActivityId);

            EntityConfiguration.Add(builder);
        }
    }
}
