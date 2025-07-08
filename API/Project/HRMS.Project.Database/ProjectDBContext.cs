using HRMS.Project.Database.Configurations;
using HRMS.Project.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Project.Database
{
    public class ProjectDBContext : DbContext
    {
        private readonly IHttpContextAccessor m_HttpContextAccessor;

        public virtual DbSet<Entities.Project> Projects { get; set; }
        public virtual DbSet<ProjectManager> ProjectManagers { get; set; }
        public virtual DbSet<Addendum> Addendum { get; set; }
        public virtual DbSet<AllocationPercentage> AllocationPercentage { get; set; }
        public virtual DbSet<AssociateAllocation> AssociateAllocation { get; set; }
        public virtual DbSet<ClientBillingRoles> ClientBillingRoles { get; set; }
        public virtual DbSet<ProjectRoleDetails> ProjectRoleDetails { get; set; }
        public virtual DbSet<ProjectRoles> ProjectRoles { get; set; }
        public virtual DbSet<ProjectsHistory> ProjectsHistory { get; set; }
        public virtual DbSet<ProjectWorkFlow> ProjectWorkFlow { get; set; }
        public virtual DbSet<SOW> SOW { get; set; }
        public virtual DbSet<TalentPool> TalentPool { get; set; }
        public virtual DbSet<TalentRequisition> TalentRequisition { get; set; }
        public virtual DbSet<ClientBillingRolesHistory> ClientBillingRoleHistory { get; set; }
        public virtual DbSet<ProjectClosure> ProjectClosure { get; set; }
        public virtual DbSet<ProjectClosureWorkflow> ProjectClosureWorkflow { get; set; }
        public virtual DbSet<ProjectClosureActivity> ProjectClosureActivity { get; set; }
        public virtual DbSet<ProjectClosureReport> ProjectClosureReport { get; set; }
        public virtual DbSet<ProjectClosureActivityDetail> ProjectClosureActivityDetail { get; set; }
        public virtual DbSet<AssociateInformationReport> AssociateInformationReport { get; set; }
        public virtual DbSet<AssociateFutureProjectAllocation> AssociateFutureProjectAllocation { get; set; }
        public virtual DbSet<TrainingMode> TrainingMode { get; set; }
        public virtual DbSet<ProjectRole> ProjectRole { get; set; }
        public virtual DbSet<ProjectSkillsRequired> ProjectSkillsRequired { get; set; }
        public virtual DbSet<ProjectRoleAssociateMapping> ProjectRoleAssociateMapping { get; set; }
        public virtual DbSet<ProjectTrainingPlan> ProjectTrainingPlan { get; set; }
        public ProjectDBContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// EmployeeContext
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContextAccessor"></param>
        public ProjectDBContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            m_HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Saves all changes made in the context
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        ///  Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains  the number of state entries written to the database.</returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(true, cancellationToken);
        }

        /// <summary>
        /// Gets the current time stamp and current user
        /// </summary>
        private void AddTimestamps()
        {

            //var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = m_HttpContextAccessor != null && m_HttpContextAccessor.HttpContext.Request.Headers["UserName"].ToString() != "" ? m_HttpContextAccessor.HttpContext.Request.Headers["UserName"].ToString() : "Anonymous";

            foreach (var entity in ChangeTracker.Entries())
            {
                //Update CreatedDate, CreatedBy when adding else update ModifiedDate, ModifiedBy
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedDate = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).CreatedBy = currentUsername;
                }
                else if(entity.State==EntityState.Modified)
                {
                    ((BaseEntity)entity.Entity).ModifiedDate = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).ModifiedBy = currentUsername;
                }

                ((BaseEntity)entity.Entity).CurrentUser = currentUsername;
            }
        }

        /// <summary>
        /// override  OnConfiguring 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }


        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectsConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectManagersConfiguration());
            modelBuilder.ApplyConfiguration(new AddendumConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new AllocationPercentageConfiguration());
            modelBuilder.ApplyConfiguration(new ClientBillingRolesConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectRolesConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectRoleDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new TalentRequisitionConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectWorkFlowConfiguration());
            modelBuilder.ApplyConfiguration(new SOWConfiguration());
            modelBuilder.ApplyConfiguration(new TalentPoolConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectsHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new ClientBillingRolesConfiguration());
            modelBuilder.ApplyConfiguration(new ClientBillingRolesHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectClosureConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectClosureWorkflowConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectClosureActivityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectClosureReportConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectClosureActivityDetailConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateInformationReportConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateFutureProjectAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new TrainingModeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectSkillsRequiredConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectRoleAssociateMappingConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTrainingPlanConfiguration());
        }
    }
}
