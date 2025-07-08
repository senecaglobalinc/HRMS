using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class EmployeeSkillWorkFlowConfiguration : IEntityTypeConfiguration<EmployeeSkillWorkFlow>
    {
        public void Configure(EntityTypeBuilder<EmployeeSkillWorkFlow> builder)
        {
            builder.ToTable("EmployeeSkillWorkFlow");
            builder.Property(x => x.Id)
                .HasColumnName("Id");

            builder
             .Property(e => e.EmployeeSkillId)
             .HasColumnName("EmployeeSkillId");

            builder
                .Property(e => e.SubmittedRating)
                .HasColumnName("SubmittedRating");

            builder
              .Property(e => e.ReportingManagerRating)
              .HasColumnName("ReportingManagerRating");


            builder.Property(x => x.Status)
              .HasColumnName("Status")
              .IsRequired();

            builder
              .Property(e => e.Remarks)
              .HasMaxLength(1000)
              .HasColumnType("varchar(1000)")
              .HasColumnName("Remarks");
              

            builder.Property(x => x.SubmittedBy)
             .HasColumnName("SubmittedBy")
             .IsRequired();

            builder.Property(x => x.SubmittedTo)
              .HasColumnName("SubmittedTo")
              .IsRequired();

            builder.Property(x => x.ApprovedDate)
               .HasColumnName("ApprovedDate")
               .HasColumnType("DATE");

              builder
                .Property(e => e.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("boolean")                
                .HasDefaultValue(true);

             builder
              .Property(e => e.CreatedBy)
              .HasMaxLength(100)
              .HasColumnType("varchar(100)");

            builder
               .Property(e => e.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(e => e.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(e => e.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
              .Property(e => e.Experience)
              .HasColumnName("Experience");

            builder
              .Property(e => e.LastUsed)
              .HasColumnName("LastUsed");

            builder.Ignore(e => e.CurrentUser);


        }
    }
}
