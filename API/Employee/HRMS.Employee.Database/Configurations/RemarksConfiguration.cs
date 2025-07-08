using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class RemarksConfiguration : IEntityTypeConfiguration<Remarks>
    {
        public void Configure(EntityTypeBuilder<Remarks> builder)
        {
            builder.ToTable("Remarks");

            builder
               .Property(c => c.Id)
               .HasColumnName("Id");

            builder
               .Property(c => c.AssociateExitId)
               .IsRequired()
               .HasColumnName("AssociateExitId");

            builder
               .Property(c => c.RoleId)
               .HasColumnName("RoleId");
            builder
               .Property(c => c.Comment)
               .HasColumnName("Comment");
     
            builder
                .Property(c => c.IsActive)
                .HasColumnType("boolean");

            builder
              .Property(c => c.CreatedBy)
              .HasMaxLength(100)
              .HasColumnType("varchar(100)");

            builder
               .Property(c => c.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(c => c.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(c => c.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(c => c.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");


            builder.Ignore(c => c.CurrentUser);


        }
    }
}
