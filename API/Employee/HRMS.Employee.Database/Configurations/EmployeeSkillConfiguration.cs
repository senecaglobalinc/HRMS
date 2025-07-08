using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class EmployeeSkillConfiguration : IEntityTypeConfiguration<EmployeeSkill>
    {
        public void Configure(EntityTypeBuilder<EmployeeSkill> builder)
        {
            builder.ToTable("EmployeeSkills");

            builder
               .Property(e => e.Id)
               .HasColumnName("ID");

            builder
               .Property(e => e.EmployeeId)
               .IsRequired()
               .HasColumnName("EmployeeId");

            builder
              .Property(e => e.CompetencyAreaId)
              .HasColumnName("CompetencyAreaId");

            builder
              .Property(e => e.SkillId)
              .HasColumnName("SkillID");

            builder
              .Property(e => e.ProficiencyLevelId)
              .HasColumnName("ProficiencyLevelId");

            builder
              .Property(e => e.Experience)
              .HasColumnName("Experience");

            builder
              .Property(e => e.LastUsed)
              .HasColumnName("LastUsed");

            builder
            .Property(e => e.IsPrimary)
            .HasDefaultValue(false)
            .HasColumnType("boolean");

            builder
              .Ignore(e => e.IsActive);             

            builder
            .Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

            builder
               .Property(e => e.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder
               .Property(e => e.CreatedDate)
               .HasColumnType("timestamp with time zone");

            builder
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(e => e.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder.Ignore(e => e.CurrentUser);

            builder
              .Property(e => e.SkillGroupId)
              .HasColumnName("SkillGroupID");

                
        }
    }
}
