using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class ProspectiveAssociateConfiguration : IEntityTypeConfiguration<ProspectiveAssociate>
    {
        public void Configure(EntityTypeBuilder<ProspectiveAssociate> builder)
        {
            builder
                .Property(p => p.Id)
                .HasColumnName("Id");

            builder
                .Property(p => p.FirstName)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsRequired()
                .IsUnicode(false);

            builder
               .Property(p => p.MiddleName)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)")
               .IsUnicode(false);

            builder
               .Property(p => p.LastName)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)")
               .IsRequired()
               .IsUnicode(false);

            builder
                .Property(p => p.Gender)
                .HasMaxLength(10)
                .HasColumnType("varchar(10)")
                .IsUnicode(false);

            builder
             .Property(p => p.GradeId)
             .HasColumnType("integer");

            builder
              .Property(e => e.DesignationId)
              .IsRequired()
              .HasColumnType("integer");

            builder
              .Property(e => e.DepartmentId)
              .IsRequired()
              .HasColumnType("integer");

            builder
             .Property(p => p.Technology)
             .HasMaxLength(100)
             .HasColumnType("varchar(100)")
             .IsUnicode(false);

            builder
              .Property(p => p.HRAdvisorName)
              .HasMaxLength(100)
              .IsRequired()
              .HasColumnType("varchar(100)")
              .IsUnicode(false);

            builder
             .Property(p => p.JoiningStatusId)
             .HasColumnType("integer");

            builder
               .Property(p => p.JoinDate)
               .IsRequired()
               .HasColumnType("timestamp with time zone");

            builder
             .Property(p => p.EmploymentType)
             .HasMaxLength(100)
             .IsRequired()
             .HasColumnType("varchar(100)")
             .IsUnicode(false);

            builder
             .Property(p => p.MaritalStatus)
             .HasMaxLength(50)
             .HasColumnType("varchar(50)")
             .IsUnicode(false);

            builder
            .Property(p => p.BGVStatusId)
            .HasColumnType("integer");

            builder
           .Property(p => p.TechnologyID)
           .HasColumnType("integer");

            builder
           .Property(p => p.EmployeeID)
           .HasColumnType("integer");

            builder
            .Property(p => p.RecruitedBy)
            .HasMaxLength(100)
            .HasColumnType("varchar(100)")
            .IsUnicode(false);

            builder
              .Property(p => p.StatusID)
              .HasColumnType("integer");

            builder
            .Property(p => p.ManagerId)
            .IsRequired()
            .HasColumnType("integer");

            builder
              .Property(p => p.ReasonForDropOut)
              .HasMaxLength(150)
              .HasColumnType("varchar(150)")
              .IsUnicode(false);

            builder
            .Property(p => p.PersonalEmailAddress)
            .HasMaxLength(50)
            .IsRequired()
            .HasColumnType("varchar(50)")
            .IsUnicode(false);

            builder
            .Property(p => p.MobileNo)
            .HasMaxLength(30)
            .IsRequired()
            .HasColumnType("varchar(30)")
            .IsUnicode(false);

            builder
             .Property(p => p.CreatedDate)
             .HasColumnType("timestamp with time zone");

            builder
                .Property(p => p.ModifiedDate)
                .HasColumnType("timestamp with time zone");

            builder
               .Property(p => p.SystemInfo)
               .HasMaxLength(50)
               .HasColumnType("varchar(50)");

            builder
               .Property(p => p.IsActive)
               .HasColumnType("boolean");

            builder
               .Property(p => p.CreatedBy)
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder
               .Property(p => p.ModifiedBy)
               .HasMaxLength(50)
               .HasColumnType("varchar(100)");

            builder.Ignore(p => p.CurrentUser);
        }
    }
}
