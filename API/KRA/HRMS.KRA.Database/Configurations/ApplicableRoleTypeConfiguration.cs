using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Database.Configurations
{
    public class ApplicableRoleTypeConfiguration : IEntityTypeConfiguration<ApplicableRoleType>
    {
        public void Configure(EntityTypeBuilder<ApplicableRoleType> builder)
        {
            builder.ToTable("ApplicableRoleType");
            builder
                .HasKey(rt => rt.ApplicableRoleTypeId);

            builder
                .Property(rt => rt.FinancialYearId)
                .HasColumnType("int")
                .IsRequired();

            builder
              .Property(rt => rt.DepartmentId)
                .HasColumnType("int")
                .IsRequired();

            builder
              .Property(rt => rt.GradeRoleTypeId)
                .HasColumnType("int")
                .IsRequired();

            builder
              .Property(rt => rt.StatusId)
                .HasColumnType("int")
                .IsRequired();

            builder
               .Property(rt => rt.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(rt => rt.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(rt => rt.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(rt => rt.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(rt => rt.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(rt => rt.IsActive);

            builder.Ignore(rt => rt.CurrentUser);

            builder
               .HasOne(rt => rt.Status)
               .WithMany(sm => sm.ApplicableRoleTypes)
               .HasForeignKey(rt => rt.StatusId)
               .OnDelete(DeleteBehavior.ClientSetNull);               
        }
    }
}
