using HRMS.Admin.Database.Configurations;
using HRMS.Admin.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Admin.Database
{
    public class AdminContext : DbContext
    {
        private readonly IHttpContextAccessor m_HttpContextAccessor;
        public DbSet<AllMenus> AllMenus { get; set; }
        public DbSet<CategoryMaster> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientsHistory> ClientsHistory { get; set; }
        public DbSet<CompetencyArea> CompetencyAreas { get; set; }
        public DbSet<CompetencySkill> CompetencySkills { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentType> DepartmentTypes { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<FinancialYear> FinancialYears { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<MenuMaster> MenuMaster { get; set; }
        public DbSet<MenuRole> MenuRoles { get; set; }
        public DbSet<NotificationConfiguration> NotificationConfigurations { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<PracticeArea> PracticeAreas { get; set; }
        public DbSet<ProficiencyLevel> ProficiencyLevels { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<SGRole> SGRoles { get; set; }
        public DbSet<SGRolePrefix> SGRolePrefixes { get; set; }
        public DbSet<SGRoleSuffix> SGRoleSuffixes { get; set; }
        public DbSet<ServiceDepartmentRole> ServiceDepartmentRoles { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillGroup> SkillGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<RoleType> RoleTypes { get; set; }
        public DbSet<GradeRoleType> GradeRoleTypes { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<ExitType> ExitTypes { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<ReasonType> ReasonTypes { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<DepartmentDL> DepartmentDL { get; set; }

        public AdminContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// AdminContext
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContextAccessor"></param>
        public AdminContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
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
                else if (entity.State == EntityState.Modified)
                {
                    ((BaseEntity)entity.Entity).ModifiedDate = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).ModifiedBy = currentUsername;
                }
                ((BaseEntity)entity.Entity).CurrentUser = currentUsername;
            }
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Unique Key for skill group table
            //modelBuilder.Entity<SkillGroup>()
            //    .HasAlternateKey(sg => new { sg.SkillGroupName, sg.CompetencyAreaId });

            modelBuilder.ApplyConfiguration(new AllMenusConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new ClientHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryMasterConfiguration());
            modelBuilder.ApplyConfiguration(new CompetencyAreaConfiguration());
            modelBuilder.ApplyConfiguration(new CompetencySkillConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DesignationConfiguration());
            modelBuilder.ApplyConfiguration(new DomainConfiguration());
            modelBuilder.ApplyConfiguration(new FinancialYearConfiguration());
            modelBuilder.ApplyConfiguration(new GradesConfiguration());
            modelBuilder.ApplyConfiguration(new MenuMasterConfiguration());
            modelBuilder.ApplyConfiguration(new MenuRoleConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfigurationConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PracticeAreaConfiguration());
            modelBuilder.ApplyConfiguration(new ProficiencyLevelConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleMasterConfiguration());
            modelBuilder.ApplyConfiguration(new SGRoleConfiguration());
            modelBuilder.ApplyConfiguration(new SGRolePrefixConfiguration());
            modelBuilder.ApplyConfiguration(new SGRoleSuffixConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceDepartmentRoleConfiguration());
            modelBuilder.ApplyConfiguration(new SkillConfiguration());
            modelBuilder.ApplyConfiguration(new SkillGroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new RoleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GradeRoleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReasonTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ExitTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReasonConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentDLConfiguration());
            modelBuilder.ApplyConfiguration(new HolidayConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
        }
    }

}
