using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    class AssociateCertificationHistoryConfig : IEntityTypeConfiguration<AssociateCertificationsHistory>
    {
        public void Configure(EntityTypeBuilder<AssociateCertificationsHistory> builder)
        {
            builder.ToTable("AssociateCertificationsHistory");
            builder
              .Property(c => c.Id)
              .HasColumnName("ID");

            builder
               .Property(c => c.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
              .Property(c => c.CertificationId)
              .HasColumnName("CertificationId");

            builder
               .Property(c => c.ValidFrom)
               .HasMaxLength(4)
               .IsUnicode(false);
            builder
               .Property(c => c.Institution)
               .HasMaxLength(150)
               .IsUnicode(false);
            builder
               .Property(c => c.Specialization)
               .HasMaxLength(150)
               .IsUnicode(false);
            builder
               .Property(c => c.ValidUpto)
               .HasMaxLength(4)
               .IsUnicode(false);

            builder
               .Property(c => c.SkillGroupId)
               .IsRequired()
               .HasColumnName("SkillGroupId");
        }
    }
}
