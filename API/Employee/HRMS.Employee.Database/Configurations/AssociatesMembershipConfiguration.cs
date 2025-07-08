using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociatesMembershipConfiguration : IEntityTypeConfiguration<AssociateMembership>
    {
        public void Configure(EntityTypeBuilder<AssociateMembership> builder)
        {
            builder.ToTable("AssociatesMemberships");

            builder
               .Property(a => a.Id)
               .HasColumnName("ID");

            builder
               .Property(a => a.EmployeeId)
               .HasColumnName("EmployeeId")
               .IsRequired();

            builder
                .Property(a => a.ProgramTitle)
                .HasMaxLength(150)
                .IsRequired()
                .IsUnicode(false);

            builder
               .Property(a => a.ValidFrom)
               .HasMaxLength(4)
               .IsRequired()
               .IsUnicode(false);

            builder
               .Property(a => a.Institution)
               .HasMaxLength(150)
               .IsUnicode(false);

            builder
               .Property(a => a.Specialization)
               .HasMaxLength(150)
               .IsUnicode(false);

            builder
               .Property(a => a.ValidUpto)
               .HasMaxLength(4)
               .IsUnicode(false);

            
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

            builder
               .Property(a => a.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(a => a.CurrentUser);
            builder.Ignore(a => a.IsActive);
        }
    }
}
