using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociatesMembershipHistoryConfiguration : IEntityTypeConfiguration<AssociatesMembershipHistory>
    {
        public void Configure(EntityTypeBuilder<AssociatesMembershipHistory> builder)
        {
            builder.ToTable("AssociatesMembershipsHistory");

            builder
               .Property(a => a.ID)
               .HasColumnName("ID");

            builder
               .Property(a => a.EmployeeId)
               .HasColumnName("EmployeeId");
               
            builder
                .Property(a => a.ProgramTitle)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder
               .Property(a => a.ValidFrom)
               .HasMaxLength(4)
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
        }
    }
}
