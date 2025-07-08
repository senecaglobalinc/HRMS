using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class FinancialYearConfiguration : IEntityTypeConfiguration<FinancialYear>
    {
        public void Configure(EntityTypeBuilder<FinancialYear> builder)
        {
            builder.ToTable("FinancialYear");

            builder
               .HasKey(fy => fy.Id);

            builder
               .Property(fy => fy.FromYear)
               .IsRequired()
               .HasColumnName("FromYear");

            builder
               .Property(fy => fy.ToYear)
               .IsRequired()
               .HasColumnName("ToYear");

            builder
                .Ignore(fy => fy.CurrentUser);

            builder
                .Ignore(pt => pt.SystemInfo);
        }
    }
}
