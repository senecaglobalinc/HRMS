using System;
using HRMS.Cache.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRMS.Cache.Database
{
    public partial class OrgCoreDBContext : DbContext
    {
        public OrgCoreDBContext()
        {
        }

        public OrgCoreDBContext(DbContextOptions<OrgCoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<CompetencyArea> CompetencyArea { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DepartmentType> DepartmentType { get; set; }
        public virtual DbSet<Designation> Designation { get; set; }
        public virtual DbSet<Domain> Domain { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<NotificationConfiguration> NotificationConfiguration { get; set; }
        public virtual DbSet<NotificationType> NotificationType { get; set; }
        public virtual DbSet<PracticeArea> PracticeArea { get; set; }
        public virtual DbSet<ProficiencyLevel> ProficiencyLevel { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectManager> ProjectManager { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<SkillGroup> SkillGroup { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryMasterId);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ClientNameHash)
                    .HasMaxLength(450);

                entity.Property(e => e.ClientRegisterName)
                    .HasMaxLength(150);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<CompetencyArea>(entity =>
            {
                entity.Property(e => e.CompetencyAreaCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CompetencyAreaDescription).HasMaxLength(256);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.DepartmentTypeId);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DepartmentCode)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);

                entity.HasOne(d => d.DepartmentType)
                    .WithMany(p => p.Department)
                    .HasForeignKey(d => d.DepartmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DepartmentType>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DepartmentTypeDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.HasIndex(e => e.GradeId);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DesignationCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DesignationName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.Designation)
                    .HasForeignKey(d => d.GradeId);
            });

            modelBuilder.Entity<Domain>(entity =>
            {
                entity.Property(e => e.DomainId).HasColumnName("DomainID");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.DomainName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.GradeCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.GradeName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<NotificationConfiguration>(entity =>
            {
                entity.HasIndex(e => e.CategoryMasterId);

                entity.HasIndex(e => e.NotificationTypeId);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EmailCc)
                    .HasColumnName("EmailCC")
                    .HasMaxLength(512);

                entity.Property(e => e.EmailFrom).HasMaxLength(512);

                entity.Property(e => e.EmailSubject).HasMaxLength(150);

                entity.Property(e => e.EmailTo).HasMaxLength(512);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Sla).HasColumnName("SLA");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);

                entity.HasOne(d => d.CategoryMaster)
                    .WithMany(p => p.NotificationConfiguration)
                    .HasForeignKey(d => d.CategoryMasterId);

                entity.HasOne(d => d.NotificationType)
                    .WithMany(p => p.NotificationConfiguration)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .HasConstraintName("FK_NotificationConfiguration_NotificationType_NotificationType~");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.HasIndex(e => e.CategoryMasterId);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.NotificationCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NotificationDescription).HasMaxLength(150);

                entity.Property(e => e.SystemInfo).HasMaxLength(50);

                entity.HasOne(d => d.CategoryMaster)
                    .WithMany(p => p.NotificationType)
                    .HasForeignKey(d => d.CategoryMasterId);
            });

            modelBuilder.Entity<PracticeArea>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.PracticeAreaCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PracticeAreaDescription)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<ProficiencyLevel>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ProficiencyLevelCode)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ProficiencyLevelDescription)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ActualEndDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ActualStartDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.PlannedEndDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.PlannedStartDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ProjectCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<ProjectManager>(entity =>
            {
                entity.HasIndex(e => e.ProgramManagerId);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(50);

                entity.HasOne(d => d.ProgramManager)
                    .WithMany(p => p.ProjectManager)
                    .HasForeignKey(d => d.ProgramManagerId);
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ProjectTypeCode)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.SystemInfo).HasMaxLength(50);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasIndex(e => e.CompetencyAreaId);

                entity.HasIndex(e => e.SkillGroupId);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SkillCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SkillDescription)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.SkillName).HasMaxLength(256);

                entity.Property(e => e.SystemInfo).HasMaxLength(50);

                entity.HasOne(d => d.CompetencyArea)
                    .WithMany(p => p.Skill)
                    .HasForeignKey(d => d.CompetencyAreaId);

                entity.HasOne(d => d.SkillGroup)
                    .WithMany(p => p.Skill)
                    .HasForeignKey(d => d.SkillGroupId);
            });

            modelBuilder.Entity<SkillGroup>(entity =>
            {
                entity.HasIndex(e => e.CompetencyAreaId);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SkillGroupName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SystemInfo).HasMaxLength(50);

                entity.HasOne(d => d.CompetencyArea)
                    .WithMany(p => p.SkillGroup)
                    .HasForeignKey(d => d.CompetencyAreaId);
            });

            modelBuilder.Entity<User>(entity =>
            {

                entity.ToTable("Users");

                entity.HasKey(us => us.UserId);

                entity.Property(us => us.UserName)
                   .HasMaxLength(256)
                   .HasColumnType("text");

                entity.Property(us => us.Password)
                   .HasColumnType("text");

                entity
                  .Property(us => us.EmailAddress)
                  .HasMaxLength(254)
                  .HasColumnType("text");

                entity
                 .Property(us => us.IsSuperAdmin)
                 .HasColumnType("boolean");

                

                
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
