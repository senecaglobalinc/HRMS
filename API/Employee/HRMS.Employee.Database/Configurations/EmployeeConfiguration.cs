using HRMS.Employee.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Database.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee.Entities.Employee>
    {
        public void Configure(EntityTypeBuilder<Employee.Entities.Employee> builder)
        {
            builder.ToTable("Employee");

            builder 
                .HasIndex(e => new { e.EmployeeId, e.UserId, e.IsActive })
                    .HasName("IX_EMployee_UserId_IsActive");

            builder 
                .Property(e => e.AadharNumber)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder 
                .Property(e => e.AccessCardNo)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder 
                .Property(e => e.AlternateMobileNo)
                .HasMaxLength(15)
                .HasColumnType("varchar(15)")
                .IsUnicode(false);

            builder 
                .Property(e => e.BgvcompletionDate)
                .HasColumnName("BGVCompletionDate")
                .HasColumnType("timestamp with time zone");

            builder 
                .Property(e => e.BgvinitiatedDate)
                .HasColumnName("BGVInitiatedDate")
                .HasColumnType("timestamp with time zone");

            builder 
                .Property(e => e.Bgvstatus)
                .HasColumnName("BGVStatus")
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder
                .Property(e => e.BgvstatusId)
                .HasColumnName("BGVStatusId")
                .HasColumnType("integer");

            builder
                .Property(e => e.UserId)
                .HasColumnName("userid")
                .HasColumnType("integer");

            builder 
                .Property(e => e.BgvtargetDate)
                .HasColumnName("BGVTargetDate")
                .HasColumnType("timestamp with time zone");

            builder 
                .Property(e => e.BloodGroup)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder
               .Property(e => e.CareerBreak)
               .HasColumnType("integer");

            builder
               .Property(e => e.CompetencyGroup)
               .HasColumnType("integer");

            builder 
                .Property(e => e.ConfirmationDate)
                .HasColumnType("timestamp with time zone");
         
            builder 
                .Property(e => e.CubicalNumber)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder 
                .Property(e => e.DateofBirth)
                .HasColumnType("timestamp with time zone");

            builder 
                .Property(e => e.EmployeeCode)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);


            //builder
            //    .Property(e => e.EmployeeCode) 
            //    .HasColumnType("integer");

            builder 
                .Property(e => e.EmploymentStartDate)
                .HasColumnType("timestamp with time zone");

            builder 
                .Property(e => e.Experience)
                .HasColumnType("decimal(18, 2)");

            builder 
                .Property(e => e.ExperienceExcludingCareerBreak)
                .HasColumnType("decimal(18, 2)");

            builder 
                .Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder 
                .Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnType("varchar(10)")
                .IsUnicode(false);

            builder 
                .Property(e => e.Hradvisor)
                .HasColumnName("HRAdvisor")
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder 
                .Property(e => e.JoinDate)
                .HasColumnType("timestamp with time zone");

            builder 
                .Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
               .Property(e => e.Photograph)
               .HasColumnType("BYTEA");

            builder
              .Property(e => e.GradeId)
              .HasColumnType("integer");

            builder
              .Property(e => e.DesignationId)
              .HasColumnType("integer");

            builder
            .Property(e => e.ReportingManager)
            .HasColumnType("integer");

            builder
            .Property(e => e.ProgramManager)
            .HasColumnType("integer");

            builder
            .Property(e => e.DepartmentId)
            .HasColumnType("integer");

            builder
            .Property(e => e.DesignationId)
            .HasColumnType("integer");

            builder
            .Property(e => e.DesignationId)
            .HasColumnType("integer");

            builder
            .Property(e => e.DocumentsUploadFlag)
            .HasColumnType("boolean");

            builder
            .Property(e => e.StatusId)
            .HasColumnType("integer");
            
            builder
                .Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder 
                .Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder 
                .Property(e => e.MobileNo)
                .HasMaxLength(30)
                .HasColumnType("varchar(30)")
                .IsUnicode(false);

            builder 
                .Property(e => e.Nationality)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder 
                .Property(e => e.Pannumber)
                .HasColumnName("PANNumber")
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder 
                .Property(e => e.PassportDateValidUpto)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder 
                .Property(e => e.PassportIssuingOffice)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder 
                .Property(e => e.PassportNumber)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder 
                .Property(e => e.PersonalEmailAddress)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder 
                .Property(e => e.Pfnumber)
                .HasColumnName("PFNumber")
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder 
                .Property(e => e.Qualification)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder
                .Property(e => e.Remarks)
                .HasColumnType("BYTEA")
                .IsUnicode(false);

            builder 
                .Property(e => e.RelievingDate)
                .HasColumnType("timestamp with time zone");

            builder 
                .Property(e => e.ResignationDate)
                .HasColumnType("timestamp with time zone");

            builder 
                .Property(e => e.SystemInfo)
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .HasDefaultValueSql("inet_client_addr()");

            builder 
                .Property(e => e.TelephoneNo)
                .HasMaxLength(30)
                .HasColumnType("varchar(30)")
                .IsUnicode(false);

            builder 
                .Property(e => e.TotalExperience)
                .HasColumnType("decimal(18, 2)");

            builder 
                .Property(e => e.Uannumber)
                .HasColumnName("UANNumber")
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsUnicode(false);

            builder.Property(e => e.WorkEmailAddress)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .IsUnicode(false);

            builder.Property(e => e.RoleTypeId);            
               

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
               .HasMaxLength(100)
               .HasColumnType("varchar(100)");

            builder.Ignore(c => c.CurrentUser);
        }
    }
}
