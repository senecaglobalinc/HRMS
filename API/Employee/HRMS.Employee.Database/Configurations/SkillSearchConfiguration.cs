using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class SkillSearchConfiguration : IEntityTypeConfiguration<SkillSearch>
    {
        public void Configure(EntityTypeBuilder<SkillSearch> builder)
        {
            builder
                .Property(e => e.Id)
                .HasColumnName("ID");

            builder
                .Property(e => e.CompetencyAreaCode)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder
                .Property(e => e.CompetencyAreaId)
                .HasColumnName("CompetencyAreaID");

            builder
                .Property(e => e.DesignationCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder
                .Property(e => e.DesignationId)
                .HasColumnName("DesignationID");

            builder
                .Property(e => e.DesignationName)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder
                .Property(e => e.EmployeeCode)
                .HasMaxLength(10)
                .HasColumnType("varchar(10)")
                .IsUnicode(false);

            builder
                .Property(e => e.EmployeeId)
                .HasColumnName("EmployeeID");

            builder
                .Property(e => e.Experience)
                .HasColumnType("decimal(5, 2)");

            builder
                .Property(e => e.FirstName)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder
                .Property(e => e.IsSkillPrimary)
                .HasColumnType("boolean")
                .HasDefaultValueSql("(('t'))");

            builder
                .Property(e => e.LastName)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder
                .Property(e => e.ProficiencyLevelCode)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder
                .Property(e => e.ProficiencyLevelId)
                .HasColumnName("ProficiencyLevelID");

            builder
                .Property(e => e.ProjectCode)
                .HasMaxLength(15)
                .IsUnicode(false);

            builder
                .Property(e => e.ProjectName)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder
                .Property(e => e.RoleDescription)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder
                .Property(e => e.SkillGroupName)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder
                .Property(e => e.SkillId)
                .HasColumnName("SkillID");

            builder
                .Property(e => e.SkillIgroupId)
                .HasColumnName("SkillIGroupID");

            builder
                .Property(e => e.SkillName)
                .HasMaxLength(150)
                .IsUnicode(false);

        }
    }
}
