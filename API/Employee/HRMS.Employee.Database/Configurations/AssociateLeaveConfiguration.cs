using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateLeaveConfiguration : IEntityTypeConfiguration<AssociateLeave>
    {
        public void Configure(EntityTypeBuilder<AssociateLeave> builder)
        {
            builder.ToTable("AssociateLeave");

            builder.HasKey(e => e.AssociateLeaveId);

            builder.Property(e => e.EmployeeCode)
                .HasColumnName("employeecode")
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(e => e.AssociateName)
                .HasColumnName("associatename")
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(e => e.ManagerCode)
                .HasColumnName("managercode")
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(e => e.ManagerName)
                .HasColumnName("managername")
                .HasMaxLength(250);

            builder.Property(e => e.LeaveTypeId)
                .HasColumnName("leavetypeid")
                .IsRequired();

            builder.Property(e => e.LeaveType)
                .HasColumnName("leavetype")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.FromDate)
                .HasColumnName("fromdate")
                .IsRequired();

            builder.Property(e => e.ToDate)
                .HasColumnName("todate")
                .IsRequired();

            builder.Property(e => e.Session1Id)
                .HasColumnName("session1id")
                .IsRequired();

            builder.Property(e => e.Session1Name)
                .HasColumnName("session1name")
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("Session 1");

            builder.Property(e => e.Session2Id)
                .HasColumnName("session2id")
                .IsRequired();

            builder.Property(e => e.Session2Name)
                .HasColumnName("session2name")
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("Session 2");

            builder.Property(e => e.NumberOfDays)
                .HasColumnName("numberofdays")
                .IsRequired();

            builder.Property(e => e.AssociateLeaveId)
                .HasColumnName("associateleaveid")
                .HasDefaultValueSql("uuid_generate_v1()")
                .IsRequired();


            builder.Ignore(c => c.IsActive);
            builder.Ignore(c => c.CreatedBy);
            builder.Ignore(c => c.CreatedDate);
            builder.Ignore(c => c.ModifiedBy);
            builder.Ignore(c => c.ModifiedDate);

            builder.Ignore(c => c.SystemInfo);

            builder.Ignore(c => c.CurrentUser);
        }
    }
}
