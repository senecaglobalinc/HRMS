using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateAbscondConfiguration : IEntityTypeConfiguration<AssociateAbscond>
    {
        public void Configure(EntityTypeBuilder<AssociateAbscond> builder)
        {
            builder.ToTable("AssociateAbscond");

            builder.HasKey(e => e.AssociateAbscondId);

            builder
               .Property(c => c.AssociateId)
               .HasColumnName("AssociateId");

            builder
               .Property(c => c.AbsentFromDate)
               .HasColumnName("AbsentFromDate")
               .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.AbsentToDate)
               .HasColumnName("AbsentToDate")
               .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.RemarksByTL)
               .HasColumnName("RemarksByTL");

            builder
               .Property(c => c.RemarksByHRA)
               .HasColumnName("RemarksByHRA");

            builder
              .Property(c => c.RemarksByHRM)
              .HasColumnName("RemarksByHRM");

            builder
               .Property(c => c.TLId)
               .HasColumnName("TLId");

            builder
               .Property(c => c.HRAId)
               .HasColumnName("HRAId");

            builder
               .Property(c => c.HRMId)
               .HasColumnName("HRMId");

            builder
               .Property(v => v.CreatedBy);

            builder
               .Property(v => v.CreatedDate);

            builder
               .Property(v => v.ModifiedBy);

            builder
               .Property(v => v.ModifiedDate);

            builder
                .Property(v => v.IsActive);

            builder.Ignore(v => v.SystemInfo);
            builder.Ignore(v => v.CurrentUser);
        }
    }
}
