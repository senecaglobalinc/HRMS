using HRMS.Admin.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Database.Configurations
{
    public class CompetencySkillConfiguration : IEntityTypeConfiguration<CompetencySkill>
    {
        public void Configure(EntityTypeBuilder<CompetencySkill> builder)
        {
            
            builder
                .HasKey(ca => ca.CompetencySkillId);

            builder
                 .HasOne(cs => cs.CompetencyArea)
                 .WithMany(ca => ca.CompetencySkills)
                 .HasForeignKey(cs => cs.CompetencyAreaId)
                 .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                 .HasOne(cs => cs.ProficiencyLevel)
                 .WithMany(pf => pf.CompetencySkills)
                 .HasForeignKey(cs => cs.ProficiencyLevelId)
                 .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                 .HasOne(cs => cs.RoleMaster)
                 .WithMany(rm => rm.CompetencySkills)
                 .HasForeignKey(cs => cs.RoleMasterId)
                 .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(cs => cs.Skill)
                .WithMany(s => s.CompetencySkills)
                .HasForeignKey(cs => cs.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(cs => cs.SkillGroup)
                .WithMany(sg => sg.CompetencySkills)
                .HasForeignKey(cs => cs.SkillGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
              .Property(cs => cs.IsPrimary)
              .HasColumnType("boolean");

            builder
             .Property(cs => cs.CreatedDate)
             .HasColumnType("timestamp with time zone");

            builder
                .Property(cs => cs.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(cs => cs.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(cs => cs.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(cs => cs.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(cs => cs.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(cs => cs.CurrentUser);
        }
    }
}
