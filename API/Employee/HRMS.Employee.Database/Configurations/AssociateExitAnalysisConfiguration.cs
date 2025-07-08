using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateExitAnalysisConfiguration : IEntityTypeConfiguration<AssociateExitAnalysis>
    {
        public void Configure(EntityTypeBuilder<AssociateExitAnalysis> builder)
        {
            builder.ToTable("AssociateExitAnalysis");

            builder
               .Property(c => c.Id)
               .HasColumnName("Id");

            builder
               .Property(c => c.AssociateExitId)
               .IsRequired()
               .HasColumnName("AssociateExitId");

            builder
               .Property(c => c.RootCause)
               .HasColumnName("RootCause");
            builder
               .Property(c => c.ActionItem)
               .HasColumnName("ActionItem");
            builder
               .Property(c => c.Responsibility)
               .HasColumnName("Responsibility");
            builder
               .Property(c => c.Remarks)
               .HasColumnName("Remarks");

            builder
               .Property(c => c.ActualDate)
               .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.TagretDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(c => c.IsActive)
                .HasColumnType("boolean");

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


            builder.Ignore(c => c.CurrentUser);
            

        }
    }

}
