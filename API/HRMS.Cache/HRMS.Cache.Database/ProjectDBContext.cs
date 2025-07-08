using System;
using HRMS.Cache.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HRMS.Cache.Database
{
    public partial class ProjectDBContext : DbContext
    {
        public ProjectDBContext()
        {
        }

        public ProjectDBContext(DbContextOptions<ProjectDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Addendum> Addendum { get; set; }
        public virtual DbSet<AllocationPercentage> AllocationPercentage { get; set; }
        public virtual DbSet<AssociateAllocation> AssociateAllocation { get; set; }
        public virtual DbSet<ClientBillingRoles> ClientBillingRoles { get; set; }
        public virtual DbSet<ProjectManagers> ProjectManagers { get; set; }
        public virtual DbSet<ProjectRoleDetails> ProjectRoleDetails { get; set; }
        public virtual DbSet<ProjectRoles> ProjectRoles { get; set; }
        public virtual DbSet<ProjectWorkFlow> ProjectWorkFlow { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<ProjectsHistory> ProjectsHistory { get; set; }
        public virtual DbSet<Sow> Sow { get; set; }
        public virtual DbSet<TalentPool> TalentPool { get; set; }
        public virtual DbSet<TalentRequisition> TalentRequisition { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Addendum>(entity =>
            {
                entity.HasKey(e => e.AddendumId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.Sowid);

                entity.Property(e => e.AddendumDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.AddendumNo).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Note).HasMaxLength(250);

                entity.Property(e => e.RecipientName).HasMaxLength(30);

                entity.Property(e => e.Sowid)
                    .IsRequired()
                    .HasColumnName("SOWId")
                    .HasMaxLength(15);

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Addendum)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Sow)
                    .WithMany(p => p.Addendum)
                    .HasForeignKey(d => d.Sowid);
            });

            modelBuilder.Entity<AllocationPercentage>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Percentage).HasColumnType("numeric(18,0)");

                entity.Property(e => e.SystemInfo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");
            });

            modelBuilder.Entity<AssociateAllocation>(entity =>
            {
                entity.HasIndex(e => e.AllocationPercentage);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.Trid);

                entity.Property(e => e.AllocationDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ClientBillingPercentage)
                    .HasColumnType("numeric(18,0)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CreatedBy).HasMaxLength(200);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EffectiveDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.InternalBillingPercentage).HasColumnType("numeric(18,0)");

                entity.Property(e => e.IsPrimary).HasDefaultValueSql("false");

                entity.Property(e => e.LeadId).HasColumnName("LeadID");

                entity.Property(e => e.ModifiedBy).HasMaxLength(200);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ProgramManagerId).HasColumnName("ProgramManagerID");

                entity.Property(e => e.ReleaseDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("inet_client_addr()");

                entity.Property(e => e.Trid).HasColumnName("TRId");

                entity.HasOne(d => d.AllocationPercentageNavigation)
                    .WithMany(p => p.AssociateAllocation)
                    .HasForeignKey(d => d.AllocationPercentage)
                    .HasConstraintName("FK_AssociateAllocation_AllocationPercentage_AllocationPercenta~");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.AssociateAllocation)
                    .HasForeignKey(d => d.ProjectId);

                entity.HasOne(d => d.Tr)
                    .WithMany(p => p.AssociateAllocation)
                    .HasForeignKey(d => d.Trid);
            });

            modelBuilder.Entity<ClientBillingRoles>(entity =>
            {
                entity.HasKey(e => e.ClientBillingRoleId);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.ClientBillingRoleCode).HasMaxLength(50);

                entity.Property(e => e.ClientBillingRoleName).HasMaxLength(60);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.EndDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.StartDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ClientBillingRoles)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ClientBillingRoles_Projects");
            });

            modelBuilder.Entity<ProjectManagers>(entity =>
            {
                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.LeadId).HasColumnName("LeadID");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ProgramManagerId).HasColumnName("ProgramManagerID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.ReportingManagerId).HasColumnName("ReportingManagerID");

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectManagers)
                    .HasForeignKey(d => d.ProjectId);
            });

            modelBuilder.Entity<ProjectRoleDetails>(entity =>
            {
                entity.HasKey(e => e.RoleAssignmentId)
                    .HasName("PK_EmployeeRoleDetails");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.FromDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo).HasMaxLength(100);

                entity.Property(e => e.ToDate).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<ProjectRoles>(entity =>
            {
                entity.HasKey(e => e.ProjectRoleId);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectRoles)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProjectWorkFlow>(entity =>
            {
                entity.HasKey(e => e.WorkFlowId);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.Comments).HasMaxLength(250);

                entity.Property(e => e.SubmittedDate).HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectWorkFlow)
                    .HasForeignKey(d => d.ProjectId);
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.HasIndex(e => e.ProjectCode)
                    .IsUnique();

                entity.HasIndex(e => e.ProjectName)
                    .IsUnique();

                entity.Property(e => e.ActualEndDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ActualStartDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ProjectCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");
            });

            modelBuilder.Entity<ProjectsHistory>(entity =>
            {
                entity.HasKey(e => e.ProjectHistoryId);

                entity.Property(e => e.ActualEndDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ActualStartDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ProjectCode)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");
            });

            modelBuilder.Entity<Sow>(entity =>
            {
                entity.HasKey(e => e.Sowid);

                entity.ToTable("SOW");

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => new { e.Sowid, e.ProjectId })
                    .IsUnique();

                entity.Property(e => e.Sowid)
                    .HasColumnName("SOWId")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SowfileName)
                    .HasColumnName("SOWFileName")
                    .HasMaxLength(50);

                entity.Property(e => e.SowsignedDate)
                    .HasColumnName("SOWSignedDate")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Sow)
                    .HasForeignKey(d => d.ProjectId);
            });

            modelBuilder.Entity<TalentPool>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("inet_client_addr()");
            });

            modelBuilder.Entity<TalentRequisition>(entity =>
            {
                entity.HasKey(e => e.TrId);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Remarks).HasMaxLength(1500);

                entity.Property(e => e.RequestedDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.RequiredDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.SystemInfo)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("inet_client_addr()");

                entity.Property(e => e.TargetFulfillmentDate).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Trcode)
                    .HasColumnName("TRCode")
                    .HasMaxLength(20);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TalentRequisition)
                    .HasForeignKey(d => d.ProjectId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
