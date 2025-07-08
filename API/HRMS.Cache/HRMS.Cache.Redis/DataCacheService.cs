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

namespace HRMS.Cache.Redis
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
                foreach (var project in projects.Result)
                {
                    var projectsJson = JsonConvert.SerializeObject(project, settings);
                    m_CacheService.SetCacheValueAsync("Projects:ID:" + project.ProjectId.ToString(), projectsJson);
                }
            }

            ////ProjectRoles
            //var projectRoles = m_ProjectDBContext.ProjectRoles.ToListAsync();
            //if (projectRoles.Result.Count > 0)
            //{
            //    var projectRolesJson = JsonConvert.SerializeObject(projectRoles, settings);
            //    m_CacheService.SetCacheValueAsync("ProjectRoles", projectRolesJson);
            //}

            ////ProjectManagers
            //var projectManagers = m_ProjectDBContext.ProjectManagers.ToListAsync();
            //if (projectManagers.Result.Count > 0)
            //{
            //    var projectManagersJson = JsonConvert.SerializeObject(projectManagers.Result, settings);
            //    m_CacheService.SetCacheValueAsync("ProjectManagers", projectManagersJson);
            //}

            ////ProjectRoleDetails
            //var projectRoleDetails = m_ProjectDBContext.ProjectRoleDetails.ToListAsync();
            //if (projectRoleDetails.Result.Count > 0)
            //{
            //    var projectRoleDetailsJson = JsonConvert.SerializeObject(projectRoleDetails.Result, settings);
            //    m_CacheService.SetCacheValueAsync("ProjectRoleDetails", projectRoleDetailsJson);
            //}

            ////AllocationPercentage
            //var allocationPercentage = m_ProjectDBContext.AllocationPercentage.ToListAsync();
            //if (allocationPercentage.Result.Count > 0)
            //{
            //    var allocationPercentageJson = JsonConvert.SerializeObject(allocationPercentage.Result, settings);
            //    m_CacheService.SetCacheValueAsync("AllocationPercentage", allocationPercentageJson);
            //}

            //AssociateAllocation
            var associateAllocations = m_ProjectDBContext.AssociateAllocation.ToListAsync();
            if (associateAllocations.Result.Count > 0)
            {
                foreach (var associateAllocation in associateAllocations.Result)
                {
                    var associateAllocationJson = JsonConvert.SerializeObject(associateAllocation, settings);
                    m_CacheService.HashSet(associateAllocation,"AssociateAllocation:ID:" + associateAllocation.AssociateAllocationId.ToString());
                    m_CacheService.SetAdd("AssociateAllocation:Emp:ID:" + associateAllocation.EmployeeId, "AssociateAllocation:ID:" + associateAllocation.AssociateAllocationId.ToString());

                }
            }

            ////Projects
            //var clientBillingRoles = m_ProjectDBContext.ClientBillingRoles.ToListAsync();
            //if (clientBillingRoles.Result.Count > 0)
            //{
            //    var clientBillingRolesJson = JsonConvert.SerializeObject(clientBillingRoles.Result, settings);
            //    m_CacheService.SetCacheValueAsync("ClientBillingRoles", clientBillingRolesJson);
            //}
            ////TalentPool
            //var talentPool = m_ProjectDBContext.TalentPool.ToListAsync();
            //if (talentPool.Result.Count > 0)
            //{
            //    var talentPoolJson = JsonConvert.SerializeObject(talentPool.Result, settings);
            //    m_CacheService.SetCacheValueAsync("Projects", talentPoolJson);
            //}

            ////TalentRequisition
            //var talentRequisition = m_ProjectDBContext.TalentRequisition.ToListAsync();
            //if (talentRequisition.Result.Count > 0)
            //{
            //    var talentRequisitionJson = JsonConvert.SerializeObject(talentRequisition.Result, settings);
            //    m_CacheService.SetCacheValueAsync("TalentRequisition", talentRequisitionJson);
            //}

            //Employee
            var employee = m_EmployeeDBContext.Employee.ToListAsync();
            if (employee.Result.Count > 0)
            {
                foreach (var emp in employee.Result)
                {
                    var employeeJson = JsonConvert.SerializeObject(emp, settings);
                    m_CacheService.SetCacheValueAsync("Employee_ID_"+ emp.EmployeeId.ToString(), employeeJson);
                }
            }

            ////SkillSearch
            //var skillSearch = m_EmployeeDBContext.SkillSearch.ToListAsync();
            //if (skillSearch.Result.Count > 0)
            //{
            //    var skillSearchJson = JsonConvert.SerializeObject(skillSearch.Result, settings);
            //    m_CacheService.SetCacheValueAsync("SkillSearch", skillSearchJson);
            //}

            ////Grades
            //var grades = m_OrgCoreDBContext.Grade.ToListAsync();
            //if (grades.Result.Count > 0)
            //{
            //    var gradesJson = JsonConvert.SerializeObject(grades.Result, settings);
            //    m_CacheService.SetCacheValueAsync("Grades", gradesJson);
            //}

            ////statuses
            //var statuses = m_OrgCoreDBContext.Status.ToListAsync();
            //if (statuses.Result.Count > 0)
            //{
            //    var statusesJson = JsonConvert.SerializeObject(statuses.Result, settings);
            //    m_CacheService.SetCacheValueAsync("Statuses", statusesJson);
            //}

            ////ProjectTypes
            //var projectTypes = m_OrgCoreDBContext.ProjectType.ToListAsync();
            //if (projectTypes.Result.Count > 0)
            //{
            //    var projectTypesJson = JsonConvert.SerializeObject(projectTypes.Result, settings);
            //    m_CacheService.SetCacheValueAsync("ProjectTypes", projectTypesJson);
            //}

            ////Clients
            //var clients = m_OrgCoreDBContext.Client.ToListAsync();
            //if (clients.Result.Count > 0)
            //{
            //    var clientsJson = JsonConvert.SerializeObject(clients.Result, settings);
            //    m_CacheService.SetCacheValueAsync("Clients", clientsJson);
            //}

            ////PracticeAreas
            //var practiceAreas = m_OrgCoreDBContext.PracticeArea.ToListAsync();
            //if (practiceAreas.Result.Count > 0)
            //{
            //    var practiceAreasJson = JsonConvert.SerializeObject(practiceAreas.Result, settings);
            //    m_CacheService.SetCacheValueAsync("PracticeAreas", practiceAreasJson);
            //}

            ////Users
            //var users = m_OrgCoreDBContext.User.ToListAsync();
            //if (users.Result.Count > 0)
            //{
            //    var usersJson = JsonConvert.SerializeObject(users.Result, settings);
            //    m_CacheService.SetCacheValueAsync("Users", usersJson);
            //}

            ////CategoryMasters
            //var categoryMaster = m_OrgCoreDBContext.Categories.ToListAsync();
            //if (categoryMaster.Result.Count > 0)
            //{
            //    var categoryMasterJson = JsonConvert.SerializeObject(categoryMaster.Result, settings);
            //    m_CacheService.SetCacheValueAsync("CategoryMasters", categoryMasterJson);
            //}


        }
    }
}
