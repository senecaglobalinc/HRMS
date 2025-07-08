using HRMS.KRA.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Database.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {

            builder.ToTable("Comment");
            builder
                .HasKey(c => c.CommentID);

            builder
                .Property(c => c.FinancialYearId)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(c => c.DepartmentId)
                .HasColumnType("int")
                .IsRequired();

            //builder
            //    .Property(c => c.ApplicableRoleTypeId)
            //    .HasColumnType("int")
            //    .IsRequired(false);

            builder
                .Property(c => c.CommentText)
                .HasMaxLength(255)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder
               .Property(c => c.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(c => c.CreatedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.ModifiedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
                .Property(c => c.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(sm => sm.IsActive);

            builder.Ignore(sm => sm.CurrentUser);

            //builder
            //   .HasOne(c => c.ApplicableRoleType)
            //   .WithMany(rt => rt.Comments)
            //   .HasForeignKey(c => c.ApplicableRoleTypeId)
            //   .OnDelete(DeleteBehavior.ClientSetNull);
        }

    }
}
