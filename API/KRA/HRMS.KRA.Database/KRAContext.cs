using HRMS.KRA.Database.Configurations;
using HRMS.KRA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.KRA.Database
{
    public class KRAContext : DbContext
    {
        private readonly IHttpContextAccessor m_HttpContextAccessor;
        public DbSet<Aspect> Aspects { get; set; }
        public DbSet<Scale> Scales { get; set; }
        public DbSet<ScaleDetails> ScaleDetails { get; set; }
        public DbSet<MeasurementType> MeasurementTypes { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<TargetPeriod> TargetPeriods { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<DefinitionDetails> DefinitionDetails { get; set; }
        public DbSet<DefinitionTransaction> DefinitionTransactions { get; set; }
        public DbSet<KRAWorkFlow> KRAWorkFlows { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<KRAPdf> KRAPdfs { get; set; }

        public KRAContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// KRAContext
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContextAccessor"></param>
        public KRAContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            m_HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Saves all changes made in the context
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            AddAuditColumns();
            return base.SaveChanges();
        }

        /// <summary>
        ///  Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains  the number of state entries written to the database.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditColumns();
            return await base.SaveChangesAsync(true, cancellationToken);
        }

        /// <summary>
        /// Add audit columns data like crated and modified date, creaednad modified user
        /// </summary>
        private void AddAuditColumns()
        {
            //var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var currentUsername = m_HttpContextAccessor != null && m_HttpContextAccessor.HttpContext.Request.Headers["UserName"].ToString() != "" ? m_HttpContextAccessor.HttpContext.Request.Headers["UserName"].ToString() : "Anonymous";

            foreach (var entity in ChangeTracker.Entries())
            {
                //Update CreatedDate, CreatedBy when adding else update ModifiedDate, ModifiedBy
                if (entity.Entity.GetType().Name == "DefinitionTransaction" || entity.Entity.GetType().Name == "Comment")
                {
                    if (entity.State == EntityState.Added)
                    {
                        if (((BaseEntity)entity.Entity).CreatedDate == null)
                            ((BaseEntity)entity.Entity).CreatedDate = DateTime.UtcNow;
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        if (((BaseEntity)entity.Entity).ModifiedDate == null)
                            ((BaseEntity)entity.Entity).ModifiedDate = DateTime.UtcNow;
                    }
                }
                else
                {
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
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AspectConfiguration());
            modelBuilder.ApplyConfiguration(new ScaleConfiguration());
            modelBuilder.ApplyConfiguration(new ScaleDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new MeasurementTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperatorConfiguration());
            modelBuilder.ApplyConfiguration(new TargetPeriodConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new DefinitionConfiguration());
            modelBuilder.ApplyConfiguration(new DefinitionDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new DefinitionTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new KRAWorkFlowConfiguration());
            modelBuilder.ApplyConfiguration(new KRAPdfConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
