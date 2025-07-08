//using HRMS.Common.Redis;
using HRMS.Employee.Database.Configurations;
using HRMS.Employee.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ValueType = HRMS.Employee.Entities.ValueType;

namespace HRMS.Employee.Database
{
    public class EmployeeDBContext : DbContext
    {
        private readonly IHttpContextAccessor m_HttpContextAccessor;
        //private readonly ICacheService m_CacheService;

        public virtual DbSet<AssociateCertification> AssociateCertifications { get; set; }
        public virtual DbSet<AssociateCertificationsHistory> AssociateCertificationsHistory { get; set; }
        public virtual DbSet<AssociateDesignation> AssociateDesignations { get; set; }
        public virtual DbSet<AssociateHistory> AssociateHistory { get; set; }
        public virtual DbSet<AssociateSkillGap> AssociateSkillGaps { get; set; }
        public virtual DbSet<AssociateMembership> AssociateMemberships { get; set; }
        public virtual DbSet<AssociateResignation> AssociateResignations { get; set; }
        public virtual DbSet<AssociateLongLeave> AssociateLongLeaves { get; set; }
        public virtual DbSet<AssociatesMembershipHistory> AssociateMembershipHistory { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<EducationDetails> EducationDetails { get; set; }
        public virtual DbSet<EmergencyContactDetails> EmergencyContactDetails { get; set; }
        public virtual DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public virtual DbSet<Employee.Entities.Employee> Employees { get; set; }
        public virtual DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public virtual DbSet<EmployeeSkillsHistory> EmployeeSkillsHistory { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<FamilyDetails> FamilyDetails { get; set; }
        public virtual DbSet<lkValue> lkValue { get; set; }
        public virtual DbSet<PreviousEmploymentDetails> PreviousEmploymentDetails { get; set; }
        public virtual DbSet<ProfessionalDetail> ProfessionalDetails { get; set; }
        public virtual DbSet<ProfessionalReferences> ProfessionalReferences { get; set; }
        public virtual DbSet<ProspectiveAssociate> ProspectiveAssociates { get; set; }
        public virtual DbSet<SkillSearch> SkillSearch { get; set; }
        public virtual DbSet<TagAssociate> TagAssociates { get; set; }
        public virtual DbSet<ValueType> ValueType { get; set; }
        public virtual DbSet<UploadFile> UploadFiles { get; set; }
        public virtual DbSet<ServiceTypeToEmployee> ServiceTypeToEmployee { get; set; }
        public virtual DbSet<EmployeeSkillWorkFlow> EmployeeSkillWorkFlow { get; set; }
        public virtual DbSet<AssociateExit> AssociateExit { get; set; }
        public virtual DbSet<AssociateExitWorkflow> AssociateExitWorkflow { get; set; }
        public virtual DbSet<AssociateExitInterview> AssociateExitInterview { get; set; }
        public virtual DbSet<AssociateExitActivity> AssociateExitActivity { get; set; }
        public virtual DbSet<AssociateExitActivityDetail> AssociateExitActivityDetail { get; set; }
        public virtual DbSet<TransitionPlan> TransitionPlan { get; set; }
        public virtual DbSet<TransitionPlanDetail> TransitionPlanDetail { get; set; }
        public virtual DbSet<AssociateExitAnalysis> AssociateExitAnalysis { get; set; }
        public virtual DbSet<Remarks> Remarks { get; set; }
        public virtual DbSet<EmployeeKRARoleTypeHistory> EmployeeKRARoleTypeHistory { get; set; }
        public virtual DbSet<UtilizationReport> UtilizationReport { get; set; }
        public virtual DbSet<EmployeeHistory> EmployeeHistory { get; set; }
        public virtual DbSet<WelcomeEmail> WelcomeEmail { get; set; }
        public virtual DbSet<AssociateExitRevokeWorkflow> AssociateExitRevokeWorkflow { get; set; }
        public virtual DbSet<AssociateExitInterviewReview> AssociateExitInterviewReview { get; set; }
        public virtual DbSet<AssociateExitReport> AssociateExitReport { get; set; }
        public virtual DbSet<AssociateAbscond> AssociateAbscond { get; set; }
        public virtual DbSet<AttendanceDetail> AttendanceDetail { get; set; }
        public virtual DbSet<LeadershipAssociates> LeadershipAssociates { get; set; }
        public virtual DbSet<BookedParkingSlots> BookedParkingSlots { get; set; }
        public virtual DbSet<AttendanceRegularizationWorkFlow> AttendanceRegularizationWorkFlow { get; set; }
        public virtual DbSet<AssociateLeave> AssociateLeave { get; set; }
        public virtual DbSet<SkillVersion> SkillVersion { get; set; }
        public virtual DbSet<BioMetricAttendance> BioMetricAttendenceDetail { get; set; }
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options)
        {
        }

        /// <summary>
        /// EmployeeContext
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContextAccessor"></param>
        /// 
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options,
                                 IHttpContextAccessor httpContextAccessor
                                 //,ICacheService cacheService
            ) : base(options)
        {
            m_HttpContextAccessor = httpContextAccessor;
            //m_CacheService = cacheService;
        }

        /// <summary>
        /// Saves all changes made in the context
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            AddTimestamps();
            CacheEntity();
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
            CacheEntity();
            return await base.SaveChangesAsync(true, cancellationToken);
        }

        /// <summary>
        /// Gets the current time stamp and current user
        /// </summary>
        private void AddTimestamps()
        {

            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity);
            if (entities == null || entities.Count() == 0)
                return;
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

                //Commented by kalyan.Penumutchu on 13-July-2023. Not using it.
                //((BaseEntity)entity.Entity).CurrentUser = currentUsername;
            }
        }

        /// <summary>
        /// Cache the modified / inserted data
        /// </summary>
        private void CacheEntity()
        {
            var employees = ChangeTracker.Entries<Entities.Employee>()
                                         .Select(e => e.Entity)
                                         .ToList();

            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (employees != null)
            {
                foreach (Entities.Employee employee in employees)
                {
                    var employeeJson = JsonConvert.SerializeObject(employee, settings);
                    //m_CacheService.SetCacheValueAsync(employee.EmployeeId.ToString(), employeeJson);
                }
            }
        }

        /// <summary>
        /// override  OnConfiguring 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AssociateCertificationConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateCertificationHistoryConfig());
            modelBuilder.ApplyConfiguration(new AssociateDesignationConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateSkillGapConfiguration());
            modelBuilder.ApplyConfiguration(new AssociatesMembershipConfiguration());
            modelBuilder.ApplyConfiguration(new AssociatesMembershipHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateResignationConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateLongLeaveConfiguration());
            modelBuilder.ApplyConfiguration(new ContactsConfiguration());
            modelBuilder.ApplyConfiguration(new EmergencyContactsConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EducationDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeProjectsConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeSkillConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeSkillHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FamilyDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new lkValueConfiguration());
            modelBuilder.ApplyConfiguration(new PreviousEmploymentConfiguration());
            modelBuilder.ApplyConfiguration(new ProfessionalDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new ProfessionalReferenceConfiguration());
            modelBuilder.ApplyConfiguration(new ProspectiveAssociateConfiguration());
            modelBuilder.ApplyConfiguration(new SkillSearchConfiguration());
            modelBuilder.ApplyConfiguration(new TagAssociateConfiguration());
            modelBuilder.ApplyConfiguration(new ValueTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UploadFilesConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceTypeToEmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeSkillWorkFlowConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateExitConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateExitWorkflowConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateExitActivityConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateExitActivityDetailConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateExitInterviewConfiguration());
            modelBuilder.ApplyConfiguration(new TransitionPlanConfiguration());
            modelBuilder.ApplyConfiguration(new TransitionPlanDetailConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateExitAnalysisConfiguration());
            modelBuilder.ApplyConfiguration(new RemarksConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeKRARoleTypeHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new UtilizationReportConfiguration());            
            modelBuilder.ApplyConfiguration(new EmployeeHistoryConfigurations());
            modelBuilder.ApplyConfiguration(new WelcomeEmailConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateExitRevokeWorkflowConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateExitInterviewReviewConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateExitReportConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateAbscondConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceDetailConfiguration());
            modelBuilder.ApplyConfiguration(new LeadershipAssociatesConfiguration());
            modelBuilder.ApplyConfiguration(new BookedParkingSlotsConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceRegularizationWorkFlowConfiguration());
            modelBuilder.ApplyConfiguration(new AssociateLeaveConfiguration());
            modelBuilder.ApplyConfiguration(new SkillVersionConfiguration());
            modelBuilder.ApplyConfiguration(new BioMetricAttendanceDetailConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
