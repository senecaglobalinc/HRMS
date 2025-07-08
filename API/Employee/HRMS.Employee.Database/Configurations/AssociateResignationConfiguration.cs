using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateResignationConfiguration : IEntityTypeConfiguration<AssociateResignation>
    {
        public void Configure(EntityTypeBuilder<AssociateResignation> builder)
        {
            builder.ToTable("AssociateResignation");

            builder
               .HasKey(a => a.ResignationId);

            builder
                .HasOne(a => a.employee)
                .WithMany(e => e.AssociateResignations)
                .HasForeignKey(a => a.EmployeeId);
                //.IsRequired()
                //.OnDelete(DeleteBehavior.ClientSetNull);

            builder
              .Property(a => a.ReasonId)
              .HasColumnName("ReasonId");

            builder
              .Property(a => a.ReasonDescription)
              .HasMaxLength(1000)
              .HasColumnType("varchar(1000)");

            builder
              .Property(a => a.ResignationDate)
              .HasColumnName("DateOfResignation")
              .HasColumnType("timestamp with time zone")
              .IsRequired();

            builder
            .Property(a => a.LastWorkingDate)
            .HasColumnName("LastWorkingDate")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

            builder
             .Property(a => a.StatusId)
             .HasColumnName("StatusId")
             .IsRequired();

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
