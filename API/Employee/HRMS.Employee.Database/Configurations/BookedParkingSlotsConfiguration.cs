using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    class BookedParkingSlotsConfiguration : IEntityTypeConfiguration<BookedParkingSlots>
    {
        public void Configure(EntityTypeBuilder<BookedParkingSlots> builder)
        {
            builder.ToTable("BookedParkingSlots");
            builder.Property(c => c.ID);
            builder.Property(c => c.EmailID);
            builder.Property(c => c.BookedDate);
            builder.Property(c => c.BookedTime);
            builder.Property(c => c.ReleaseDate);
            builder.Property(c => c.ReleaseTime);
            builder.Property(c => c.IsActive).HasDefaultValue(true);
            builder.Ignore(c => c.CreatedBy);
            builder.Ignore(c => c.CreatedDate);
            builder.Ignore(c => c.ModifiedBy);
            builder.Ignore(c => c.ModifiedDate);
            builder.Ignore(c => c.SystemInfo);
            builder.Ignore(c => c.CurrentUser);
        }
   }
    
}
