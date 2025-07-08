using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder) 
        {
            builder.ToTable("Activity");

            builder
               .HasKey(at => at.ActivityId);

            builder
                .HasOne(at => at.Department)
                .WithMany(dt => dt.Activities)
                .HasForeignKey(at => at.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);           

            builder
                .Property(at => at.Description)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");
          
            builder
                .HasOne(at => at.ActivityType)
                .WithMany(av => av.Activities)
                .HasForeignKey(at => at.ActivityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
               .Property(at => at.IsRequired);              

            EntityConfiguration.Add(builder);
        }
    }
}
