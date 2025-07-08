using HRMS.Cache.Database;
using HRMS.Cache.Database.Entities;
using HRMS.Cache.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRMS.Cache.DataService
{
    public class DataCacheService
    {
        private readonly ICacheService m_CacheService;
        private readonly ProjectDBContext m_ProjectDBContext;
        private readonly EmployeeDBContext m_EmployeeDBContext;
        private readonly OrgCoreDBContext m_OrgCoreDBContext;
        private ILogger<DataCacheService> m_Logger;

        public DataCacheService(ICacheService cacheService,
            ILogger<DataCacheService> logger,
            ProjectDBContext projectDBContext,
            EmployeeDBContext employeeDBContext,
            OrgCoreDBContext orgCoreDBContext)
        {
            m_Logger = logger;
            m_CacheService = cacheService;
            m_ProjectDBContext = projectDBContext;
            m_EmployeeDBContext = employeeDBContext;
            m_OrgCoreDBContext = orgCoreDBContext;
        }

        public void Run()
        {

            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            //Projects
            var projects =  m_ProjectDBContext.Projects.ToListAsync();
            if (projects.Result.Count > 0)
            {
                var projectsJson = JsonConvert.SerializeObject(projects.Result, settings);
                m_CacheService.SetCacheValueAsync("Projects", projectsJson);
            }

            //Projects
            var projectRoles = m_ProjectDBContext.ProjectRoles.ToListAsync();
            if (projectRoles.Result.Count > 0)
            {
                var projectRolesJson = JsonConvert.SerializeObject(projectRoles, settings);
                m_CacheService.SetCacheValueAsync("projectRoles", projectRolesJson);
            }

            //Projects
            var projectManagers = m_ProjectDBContext.ProjectManagers.ToListAsync();
            if (projectManagers.Result.Count > 0)
            {
                var projectManagersJson = JsonConvert.SerializeObject(projectManagers.Result, settings);
                m_CacheService.SetCacheValueAsync("ProjectManagers", projectManagersJson);
            }

            //ProjectRoleDetails
            var projectRoleDetails = m_ProjectDBContext.ProjectRoleDetails.ToListAsync();
            if (projectRoleDetails.Result.Count > 0)
            {
                var projectRoleDetailsJson = JsonConvert.SerializeObject(projectRoleDetails.Result, settings);
                m_CacheService.SetCacheValueAsync("ProjectRoleDetails", projectRoleDetailsJson);
            }

            //AllocationPercentage
            var allocationPercentage = m_ProjectDBContext.AllocationPercentage.ToListAsync();
            if (allocationPercentage.Result.Count > 0)
            {
                var allocationPercentageJson = JsonConvert.SerializeObject(allocationPercentage.Result, settings);
                m_CacheService.SetCacheValueAsync("AllocationPercentage", allocationPercentageJson);
            }

            //AssociateAllocation
            var associateAllocation = m_ProjectDBContext.AssociateAllocation.ToListAsync();
            if (associateAllocation.Result.Count > 0)
            {
                var associateAllocationJson = JsonConvert.SerializeObject(associateAllocation.Result,settings);
                m_CacheService.SetCacheValueAsync("AssociateAllocation", associateAllocationJson);
            }

            //Projects
            var clientBillingRoles = m_ProjectDBContext.ClientBillingRoles.ToListAsync();
            if (clientBillingRoles.Result.Count > 0)
            {
                var clientBillingRolesJson = JsonConvert.SerializeObject(clientBillingRoles.Result, settings);
                m_CacheService.SetCacheValueAsync("ClientBillingRoles", clientBillingRolesJson);
            }
            //TalentPool
            var talentPool = m_ProjectDBContext.TalentPool.ToListAsync();
            if (talentPool.Result.Count > 0)
            {
                var talentPoolJson = JsonConvert.SerializeObject(talentPool.Result, settings);
                m_CacheService.SetCacheValueAsync("Projects", talentPoolJson);
            }

            //TalentRequisition
            var talentRequisition = m_ProjectDBContext.TalentRequisition.ToListAsync();
            if (talentRequisition.Result.Count > 0)
            {
                var talentRequisitionJson = JsonConvert.SerializeObject(talentRequisition.Result, settings);
                m_CacheService.SetCacheValueAsync("TalentRequisition", talentRequisitionJson);
            }

            //Employee
            var employee = m_EmployeeDBContext.Employee.ToListAsync();
            if (employee.Result.Count > 0)
            {
                var employeeJson = JsonConvert.SerializeObject(employee.Result, settings);
                m_CacheService.SetCacheValueAsync("Employee", employeeJson);
            }

            //SkillSearch
            var skillSearch = m_EmployeeDBContext.SkillSearch.ToListAsync();
            if (skillSearch.Result.Count > 0)
            {
                var skillSearchJson = JsonConvert.SerializeObject(skillSearch.Result, settings);
                m_CacheService.SetCacheValueAsync("SkillSearch", skillSearchJson);
            }

        }
    }
}
