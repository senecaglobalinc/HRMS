using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class EmployeeSkillHistoryConfiguration : IEntityTypeConfiguration<EmployeeSkillsHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeSkillsHistory> builder)
        {
            builder.ToTable("EmployeeSkillsHistory");

            builder
               .Property(e => e.Id)
               .HasColumnName("ID");

            builder
               .Property(e => e.EmployeeId)
               .HasColumnName("EmployeeId");

            builder
             .Property(e => e.CompetencyAreaId)
             .HasColumnName("CompetencyAreaId");

            builder
              .Property(e => e.SkillID)
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
            .HasColumnType("boolean");

            builder
            .Property(e => e.SkillGroupID)
            .HasColumnName("SkillGroupID");
        }
    }
}
