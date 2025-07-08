using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateDesignationConfiguration : IEntityTypeConfiguration<AssociateDesignation>
    {
        public void Configure(EntityTypeBuilder<AssociateDesignation> builder)
        {
            builder.ToTable("AssociateDesignations");
            builder
                 .Property(c => c.Id)
                 .HasColumnName("ID");

            builder
               .Property(c => c.EmployeeId)
               .HasColumnName("EmployeeId");
            builder
               .Property(c => c.DesignationId)
               .HasColumnName("DesignationId");
            builder
               .Property(c => c.GradeId)
               .HasColumnName("GradeId");
            builder
               .Property(e => e.FromDate)
               .HasColumnType("timestamp with time zone");
            builder
               .Property(e => e.ToDate)
               .HasColumnType("timestamp with time zone");
            builder
             .Property(c => c.CreatedBy)
             .HasMaxLength(100)
             .HasColumnType("varchar(100)");

            builder
               .Property(c => c.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(c => c.ModifiedBy);
            builder.Ignore(c => c.ModifiedDate);
            builder.Ignore(c => c.CurrentUser);
            builder.Ignore(c => c.IsActive);
        }
    }
}
