using AutoMapper;
//using HRMS.Common.Redis;
using HRMS.Employee.Database;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to get the SkillSearch details
    /// </summary>
    public class SkillSearchService : ISkillSearchService
    {
        #region Global Varibles      

        private readonly ILogger<SkillSearchService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        //private readonly ICacheService m_CacheService;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;

        #endregion

        #region Constructor
        public SkillSearchService(EmployeeDBContext employeeDBContext,
                                ILogger<SkillSearchService> logger,
                                IHttpClientFactory clientFactory,
                                IOptions<APIEndPoints> apiEndPoints,
                                //ICacheService cacheService,
                                IProjectService projectService,
                                IOrganizationService orgService
                                 )
        {
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee.Entities.Employee, Employee.Entities.Employee>();
            });
            m_mapper = config.CreateMapper();
            //m_CacheService = cacheService;
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
            m_ProjectService = projectService;
            m_OrgService = orgService;
        }

        #endregion

        #region Bulk Insert
        /// <summary>
        /// Bulk Insert
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceResponse<dynamic>> BulkInsert()
        {
            //1. Delete all records from SkillSearch table
            //2. Reset the Identity seed to 1
            //3. Prepare SkillSearch data from Employee, EmployeeSkills and allocations
            //4. Insert prepared data in SkillSerach table.

            List<Employee.Entities.SkillSearch> skillsToDelete = await m_EmployeeContext.SkillSearch.ToListAsync();
            m_EmployeeContext.RemoveRange(skillsToDelete);
            int deletededRows = await m_EmployeeContext.SaveChangesAsync();
            int createdRows = 0;
            var response = new ServiceResponse<dynamic>();
            m_Logger.LogInformation("SkillSearch: Calling \"Create\" method.");
            try
            {
                var designations = await m_OrgService.GetAllDesignations();
                var proficiencyLevels = await m_OrgService.GetAllProficiencyLevels(true);
                var skills = await m_OrgService.GetAllSkills(true);
                var competencyAreas = await m_OrgService.GetCompetencyAreas(true);
                var roles = await m_OrgService.GetAllRoles();
                var employees = (from em in m_EmployeeContext.Employees
                                 join es in m_EmployeeContext.EmployeeSkills
                                 on em.EmployeeId equals es.EmployeeId
                                 where em.IsActive == true
                                 select
                                    new
                                    {
                                        EmployeeId = em.EmployeeId,
                                        FirstName = em.FirstName,
                                        LastName = em.LastName,
                                        DesignationId = em.DesignationId,
                                        Experience = em.TotalExperience,
                                        ExperienceExcludingCareerBreak = em.ExperienceExcludingCareerBreak,
                                        SkillId = es.SkillId,
                                        SkillGroupId = es.SkillGroupId,
                                        ProficiencyLevelId = es.ProficiencyLevelId,
                                        IsPrimary = es.IsPrimary,
                                        LastUsed = es.LastUsed ,
                                        SkillExperience = es.Experience 
                                    }).ToList();

                string employeeIds = string.Join(",", employees.Select(c => c.EmployeeId).Distinct().ToList());

                var allocations = m_ProjectService.GetSkillSearchAssociateAllocations(employeeIds).Result?.Items;
                List<Employee.Entities.SkillSearch> list =
                   (from em in employees
                    join al in allocations on em.EmployeeId equals al.EmployeeId
                    join ds in designations.Items on em.DesignationId equals ds.DesignationId
                    join sk in skills.Items on em.SkillId equals sk.SkillId
                    join pl in proficiencyLevels.Items on em.ProficiencyLevelId equals pl.ProficiencyLevelId
                    join ca in competencyAreas.Items on sk.CompetencyAreaId equals ca.CompetencyAreaId
                    join rl in roles.Items on al.RoleMasterId equals rl.RoleId
                    select new Employee.Entities.SkillSearch()
                    {
                        Id = 0
                        ,
                        EmployeeId = em.EmployeeId
                        ,
                        FirstName = em.FirstName
  ,
                        LastName = em.LastName
  ,
                        RoleMasterId = al.RoleMasterId
  ,
                        RoleDescription = rl.RoleDescription
  ,
                        DesignationId = em.DesignationId
  ,
                        DesignationCode = ds.DesignationCode
  ,
                        ProjectCode = al.ProjectCode
  ,
                        ProjectName = al.ProjectName
  ,
                        IsPrimary = al.IsPrimary
  ,
                        IsCritical = al.IsCritical
  ,
                        IsBillable = al.IsBillable
  ,
                        CompetencyAreaId = sk.CompetencyAreaId
  ,
                        CompetencyAreaCode = ca.CompetencyAreaCode
  ,
                        SkillIgroupId = sk.SkillGroupId
  ,
                        SkillGroupName = sk.SkillGroup.SkillGroupName
  ,
                        SkillId = sk.SkillId
  ,
                        SkillName = sk.SkillName
  ,
                        ProficiencyLevelId = em.ProficiencyLevelId
  ,
                        ProficiencyLevelCode = pl.ProficiencyLevelCode
  ,
                        EmployeeCode = em.FirstName
  ,
                        ProjectId = al.ProjectId
  ,
                        IsSkillPrimary = em.IsPrimary ?? false
  ,
                        DesignationName = em.FirstName
  ,
                        LastUsed = em.LastUsed
  ,
                        Experience = em.Experience
  ,
                        SkillExperience = em.SkillExperience                        
                    }
                     ).ToList();

                string sql = @"ALTER SEQUENCE ""SkillSearch_ID_seq"" RESTART WITH 1;";               
                m_EmployeeContext.Database.ExecuteSqlRaw(sql);
                m_EmployeeContext.SkillSearch.AddRange(list);
                m_Logger.LogInformation("Calling SaveChanges method on DB Context in SkillSearchService");
                createdRows = await m_EmployeeContext.SaveChangesAsync();
                if (createdRows > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = null;
                    response.Message = "Successfully created SkillSearch records: " + createdRows.ToString();
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while creating skillsearch";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while creating employee skill details";
                m_Logger.LogError("Error occurred in \"Create\" method of EmployeeSkillService" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the SkillSearch
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Employee.Entities.SkillSearch>> GetAll()
        {
            var response = new ServiceListResponse<Employee.Entities.SkillSearch>();
            try
            {
                var skillList = await m_EmployeeContext.SkillSearch.ToListAsync();
                response.IsSuccessful = true;
                response.Items = skillList;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching skills";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
                        

        #endregion

        #region GetById
        /// <summary>
        /// Get SkillSearch by id
        /// </summary>
        /// <param name="id">SkillSearch Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<Employee.Entities.SkillSearch>> GetById(int id)
        {
            var response = new ServiceResponse<Employee.Entities.SkillSearch>();
            try
            {
                var skillList = await m_EmployeeContext.SkillSearch.FindAsync(id);
                response.IsSuccessful = true;
                response.Item = skillList;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching skills";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }


        #endregion

        #region GetAllSkillDetails
        /// <summary>
        /// 
        /// </summary>
        public async Task<ServiceListResponse<EmployeeSkillSearch>> GetAllSkillDetails(int empID)
        {
            var response = new ServiceListResponse<EmployeeSkillSearch>();
            try
            {
              //  var designations = await m_OrgService.GetAllDesignations();
              //  var proficiencyLevels = await m_OrgService.GetAllProficiencyLevels(true);
              //  var skills = await m_OrgService.GetAllSkills(true);
              //  var competencyAreas = await m_OrgService.GetCompetencyAreas(true);
                var employees = await (m_EmployeeContext.SkillSearch.Where(sw => sw.EmployeeId == empID)                          
                          .Select(sk => new EmployeeSkillSearch
                          {
                              EmployeeId = sk.EmployeeId ?? 0,
                              EmployeeName = sk.FirstName + " " + sk.LastName,
                              SkillId = sk.SkillId ?? 0,
                              SkillGroupId = sk.SkillIgroupId ?? 0,
                              SkillGroupName = sk.SkillGroupName,
                              SkillName = sk.SkillName,
                              IsPrimary = sk.IsSkillPrimary ?? false,
                              LastUsed = sk.LastUsed ?? 0,
                              CompetencyAreaId = sk.CompetencyAreaId ?? 0,
                              CompetencyAreaCode = sk.CompetencyAreaCode,
                              ProficiencyLevelId = sk.ProficiencyLevelId ?? 0,
                              ProficiencyLevelCode = sk.ProficiencyLevelCode,
                              SkillExperience = sk.SkillExperience ?? 0
                          }).Distinct().OrderBy(x => x.SkillName).ToListAsync<EmployeeSkillSearch>());


                response.IsSuccessful = true;
                response.Items = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching skills";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion
    }
}
