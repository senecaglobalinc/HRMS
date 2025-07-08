using System;
using HRMS.Cache.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRMS.Cache.Database
{
    public partial class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext()
        {
        }

        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<SkillSearch> SkillSearch { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => new { e.EmployeeId, e.Userid, e.IsActive })
                    .HasName("IX_EMployee_UserId_IsActive");

                entity.Property(e => e.AadharNumber).HasMaxLength(50);

                entity.Property(e => e.AccessCardNo).HasMaxLength(100);

                entity.Property(e => e.AlternateMobileNo).HasMaxLength(15);

                entity.Property(e => e.BgvcompletionDate)
                    .HasColumnName("BGVCompletionDate")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.BgvinitiatedDate)
                    .HasColumnName("BGVInitiatedDate")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.Bgvstatus)
                    .HasColumnName("BGVStatus")
                    .HasMaxLength(50);

                entity.Property(e => e.BgvstatusId).HasColumnName("BGVStatusId");

                entity.Property(e => e.BgvtargetDate)
                    .HasColumnName("BGVTargetDate")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.BloodGroup).HasMaxLength(50);

                entity.Property(e => e.ConfirmationDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CubicalNumber).HasMaxLength(50);

                entity.Property(e => e.DateofBirth).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EmployeeCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EmploymentStartDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Experience).HasColumnType("numeric(18,2)");

                entity.Property(e => e.ExperienceExcludingCareerBreak).HasColumnType("numeric(18,2)");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.Hradvisor)
                    .HasColumnName("HRAdvisor")
                    .HasMaxLength(100);

                entity.Property(e => e.JoinDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.MaritalStatus).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(100);

                entity.Property(e => e.MobileNo).HasMaxLength(30);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Nationality).HasMaxLength(50);

                entity.Property(e => e.Paid).HasColumnName("PAID");

                entity.Property(e => e.Pannumber)
                    .HasColumnName("PANNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.PassportDateValidUpto).HasMaxLength(50);

                entity.Property(e => e.PassportIssuingOffice).HasMaxLength(50);

                entity.Property(e => e.PassportNumber).HasMaxLength(50);

                entity.Property(e => e.PersonalEmailAddress).HasMaxLength(100);

                entity.Property(e => e.Pfnumber)
                    .HasColumnName("PFNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.Qualification).HasMaxLength(100);

                entity.Property(e => e.RelievingDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ResignationDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");

                entity.Property(e => e.TelephoneNo).HasMaxLength(30);

                entity.Property(e => e.TotalExperience).HasColumnType("numeric(18,2)");

                entity.Property(e => e.Uannumber)
                    .HasColumnName("UANNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.WorkEmailAddress).HasMaxLength(100);
            });

            modelBuilder.Entity<SkillSearch>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetencyAreaCode).HasMaxLength(150);

                entity.Property(e => e.CompetencyAreaId).HasColumnName("CompetencyAreaID");

                entity.Property(e => e.DesignationCode).HasMaxLength(50);

                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.Property(e => e.DesignationName).HasMaxLength(150);

                entity.Property(e => e.EmployeeCode).HasMaxLength(10);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Experience).HasColumnType("numeric(5,2)");

                entity.Property(e => e.FirstName).HasMaxLength(150);

                entity.Property(e => e.IsSkillPrimary).HasDefaultValueSql("true");

                entity.Property(e => e.LastName).HasMaxLength(150);

                entity.Property(e => e.ProficiencyLevelCode).HasMaxLength(150);

                entity.Property(e => e.ProficiencyLevelId).HasColumnName("ProficiencyLevelID");

                entity.Property(e => e.ProjectCode).HasMaxLength(15);

                entity.Property(e => e.ProjectName).HasMaxLength(150);

                entity.Property(e => e.RoleDescription).HasMaxLength(50);

                entity.Property(e => e.SkillGroupName).HasMaxLength(150);

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.SkillIgroupId).HasColumnName("SkillIGroupID");

                entity.Property(e => e.SkillName).HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
