using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> builder)
        {
            builder.ToTable("Holiday");

            builder
               .HasKey(fy => fy.Id);

            builder
               .Property(fy => fy.Occasion)
               .IsRequired()
               .HasColumnName("Occasion");

            builder
               .Property(fy => fy.HolidayDate)
               .IsRequired()
               .HasColumnName("HolidayDate");
        }
    }
}
