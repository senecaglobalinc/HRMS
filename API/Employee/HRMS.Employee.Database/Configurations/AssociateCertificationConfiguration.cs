using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateCertificationConfiguration : IEntityTypeConfiguration<AssociateCertification>
    {
        public void Configure(EntityTypeBuilder<AssociateCertification> builder)
        {
            builder.ToTable("AssociateCertifications");

            builder
               .Property(c => c.Id)
               .HasColumnName("ID");

            builder
               .Property(c => c.EmployeeId)
               .IsRequired()
               .HasColumnName("EmployeeId");

            builder
              .Property(c => c.CertificationId)
              .IsRequired()
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
            builder.Ignore(c => c.IsActive);

        }
    }
}
