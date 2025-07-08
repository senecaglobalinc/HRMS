using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class AssociateSkillGapConfiguration : IEntityTypeConfiguration<AssociateSkillGap>
    {
        public void Configure(EntityTypeBuilder<AssociateSkillGap> builder)
        {
            builder.ToTable("AssociateSkillGap");

            builder
               .Property(c => c.Id)
               .HasColumnName("ID");

            builder
               .Property(c => c.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
               .Property(c => c.ProjectSkillId)          
               .HasColumnName("ProjectSkillId");

            builder
               .Property(c => c.CompetencyAreaId)
               .HasColumnName("CompetencyAreaId");

            builder
             .Property(c => c.StatusId)
             .HasColumnName("StatusId");

            builder
               .Property(c => c.CurrentProficiencyLevelId)
               .HasColumnName("CurrentProficiencyLevelId");

            builder
               .Property(c => c.RequiredProficiencyLevelId)
               .HasColumnName("RequiredProficiencyLevelId");

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

            builder.Ignore(c => c.CurrentUser);
        }
    }
}
