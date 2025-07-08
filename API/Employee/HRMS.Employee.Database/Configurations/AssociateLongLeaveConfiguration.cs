using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateLongLeaveConfiguration : IEntityTypeConfiguration<AssociateLongLeave>
    {
        public void Configure(EntityTypeBuilder<AssociateLongLeave> builder)
        {
            builder.ToTable("AssociateLongLeave");

            builder
               .HasKey(a => a.LeaveId);

            builder
               .HasOne(a => a.employee)
               .WithMany(e => e.AssociateLongLeaves)
               .HasForeignKey(a => a.EmployeeId);
               //.IsRequired()
               //.OnDelete(DeleteBehavior.ClientSetNull);

            builder
              .Property(a => a.LongLeaveStartDate)
              .HasColumnName("LongLeaveStartDate")
              .HasColumnType("timestamp with time zone")
              .IsRequired();

            builder
            .Property(a => a.TentativeJoinDate)
            .HasColumnName("TentativeJoinDate")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

            builder
             .Property(a => a.StatusId)
             .HasColumnName("StatusId")
             .IsRequired();

            builder
             .Property(a => a.Reason)
             .HasMaxLength(1000)
             .HasColumnType("varchar(1000)");

            builder
              .Property(a => a.IsMaternity)
              .HasColumnType("boolean");

            builder
               .Property(a => a.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(a => a.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(a => a.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(a => a.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(a => a.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder.Ignore(a => a.CurrentUser);
            builder.Ignore(a => a.SystemInfo);

        }
    }
}
