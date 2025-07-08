using HRMS.Cache.Redis;
using Microsoft.Extensions.Logging;
using System;
using HRMS.Cache.Database;
using HRMS.Cache.Database.Entities;
using System.Collections.Generic;
using AutoMapper;
using HRMS.Cache.Linq.Models;
using System.Linq;
//using System.Text.Json;

namespace HRMS.Cache.Linq
{
    public class SpToLinq
    {
        private readonly ICacheService m_CacheService;
        private ILogger<SpToLinq> m_Logger;
        private IMapper m_mapper;

        /// <summary>
        /// SpToLinq Constructor
        /// </summary>
        /// <param name="cacheService"></param>
        /// <param name="logger"></param>
        public SpToLinq(ICacheService cacheService,
            ILogger<SpToLinq> logger)
        {
            m_Logger = logger;
            m_CacheService = cacheService;
        }

        /// <summary>
        /// GetEmployeesBySkillUsingCache
        /// </summary>
        public void GetEmployeesBySkillUsingCache()
        {
            var skills = new List<EmployeeSkills>();
            bool isAdded = false;

            //Get the data for all the tables from redis
            var projects = m_CacheService.GetCacheValue<List<Project>>("Projects");
            var skillSearch = m_CacheService.GetCacheValue<List<SkillSearch>>("SkillSearch");
            var associateAllocation = m_CacheService.GetCacheValue<List<AssociateAllocation>>("AssociateAllocation");
            var allocationPercentage = m_CacheService.GetCacheValue<List<AllocationPercentage>>("AllocationPercentage");
            var employee = m_CacheService.GetCacheValue<List<Employee>>("Employee");
            var grades = m_CacheService.GetCacheValue<List<Grade>>("Grades");
            var projectManagers = m_CacheService.GetCacheValue<List<ProjectManagers>>("ProjectManagers");

            ////CreateMapper
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<List<SkillSearch>, List<EmployeeSkills>>();
            //});
            //m_mapper = config.CreateMapper();

            //Get the employee skills who are actively working on a project
            var employeeSkills = (from ss in skillSearch
                                  from aa in associateAllocation
                                  where ss.EmployeeId == aa.EmployeeId &&
                                        ss.ProjectId == aa.ProjectId &&
                                        aa.ReleaseDate == null
                                  select
                                     new EmployeeSkills
                                     {
                                         EmployeeId = ss.EmployeeId.Value,
                                         EmployeeName = ss.FirstName + " " + ss.LastName,
                                         Designation = ss.DesignationName,
                                         Experience = ss.Experience,
                                         ProjectName = ss.ProjectName,
                                         ProjectId = ss.ProjectId,
                                         IsBillable = ss.IsBillable,
                                         IsCritical = ss.IsCritical,
                                         IsSkillPrimary = ss.IsSkillPrimary,
                                         AllocationPercentage = aa.AllocationPercentage.Value,
                                         SkillName = ss.SkillName
                                     }).ToList();

            //Get all the distinct employee ids
            var empids = (
                                from es in employeeSkills
                                select es.EmployeeId
                            )
                            .Distinct()
                            .ToList<int>();

            //Get active employee records
            var employees = (from ss in skillSearch
                             from e in employee
                             where ss.EmployeeId == e.EmployeeId &&
                                   e.IsActive == true
                             select new
                             {
                                 EmployeeId = ss.EmployeeId,
                                 EmployeeName = ss.FirstName + " " + ss.LastName,
                                 GradeId = e.GradeId,
                                 ProjectId = ss.ProjectId
                             })
                             .Distinct()
                             .ToList();

            
            var emps = (from e in employees
                        from aa in associateAllocation
                        where empids.Contains(e.EmployeeId.Value) &&
                              e.EmployeeId == aa.EmployeeId &&
                              e.ProjectId == aa.ProjectId
                        select new
                        {
                            EmployeeId = e.EmployeeId,
                            GradeId = e.GradeId,
                            ProjectId = e.ProjectId,
                            EmployeeName = e.EmployeeName,
                            ReleaseDate = aa.ReleaseDate
                        })
                         .Where(e => e.ReleaseDate == null)
                         .Distinct()
                         .ToList();

            //loop through the distinct employees and fill the information
            emps.ForEach(e =>
            {
                isAdded = false;
                employeeSkills.ForEach(es =>
                {
                    if (es.EmployeeId == e.EmployeeId && !isAdded)
                    {
                        //Get the grade name
                        var gradeName = from g in grades
                                        where e.GradeId == g.GradeId
                                        select g.GradeName;

                        // Get the lead name
                        var leadName = from pm in projectManagers
                                       where pm.ProjectId == es.ProjectId &&
                                             pm.LeadId == e.EmployeeId
                                       select e.EmployeeName;

                        //Get the manager name
                        var managerName = from pm in projectManagers
                                          where pm.ProjectId == es.ProjectId &&
                                                pm.ReportingManagerId == e.EmployeeId
                                          select e.EmployeeName;

                        // Get the allocation percentage 
                        var percentage = from ap in allocationPercentage
                                         where ap.AllocationPercentageId == es.AllocationPercentage
                                         select ap.Percentage;

                        // Get the primary skills
                        var primarySkills = string.Join(",",
                                                    (
                                                        from skillsprimary in employeeSkills
                                                        where skillsprimary.EmployeeId == e.EmployeeId &&
                                                              skillsprimary.IsSkillPrimary == true
                                                        orderby skillsprimary.SkillName
                                                        select skillsprimary.SkillName                                                       
                                                    )
                                                 );

                        // Get the secondary skills
                        var secondarySkills = string.Join(",",
                                                    (
                                                        from skillssecondary in employeeSkills
                                                        where skillssecondary.EmployeeId == e.EmployeeId &&
                                                              skillssecondary.IsSkillPrimary == false
                                                        orderby skillssecondary.SkillName
                                                        select skillssecondary.SkillName)
                                                    );

                        es.Grade = gradeName.ToString();
                        es.LeadName = leadName.ToString();
                        es.ManagerName = managerName.ToString();
                        es.AllocationPercentage = decimal.Parse(percentage.FirstOrDefault().ToString());
                        es.PrimarySkills = primarySkills;
                        es.SecondarySkills = secondarySkills;

                        //Add to the list
                        skills.Add(es);
                        isAdded = true;
                    }
                });
            });
        }

        /// <summary>
        /// GetEmployeesBySkillUsingCache
        /// </summary>
        public async void GetProjectDetailUsingCache(string userRole, int employeeId, string dashboard)
        {
            var projectDetails = new List<ProjectDetail>();

            var projects = m_CacheService.GetCacheValue<List<Project>>("Projects");
            var projectManagers = m_CacheService.GetCacheValue<List<ProjectManagers>>("ProjectManagers");

            var projectCategoryId = m_CacheService.GetCacheValue<List<Categories>>("CategoryMasters")
                .Where(cm => "ppc".Equals(cm.CategoryName.ToLower().Trim())).Select(cm => cm.CategoryMasterId).FirstOrDefault();

            var statuses = m_CacheService.GetCacheValue<List<Status>>("Statuses")
                        .Where(st => projectCategoryId == st.CategoryMasterId);
            var projectTypes = m_CacheService.GetCacheValue<List<ProjectType>>("ProjectTypes");
            var clients = m_CacheService.GetCacheValue<List<Client>>("Clients");
            var practiceAreas = m_CacheService.GetCacheValue<List<PracticeArea>>("PracticeAreas");
            var employees = m_CacheService.GetCacheValue<List<Employee>>("Employee");

            var userId = employees.Where(emp => emp.EmployeeId == employeeId).Select(emp => emp.Userid).FirstOrDefault();

            var user = m_CacheService.GetCacheValue<List<User>>("Users")
                .Where(us => us.UserId == userId)
                .FirstOrDefault();

            List<string> unallowedProjectTypeCodes = new List<string>() { "Talent Pool", "Training" };
            List<string> unallowedStatusCodes = new List<string>() { "drafted", "closed" };

            if (dashboard == "ProjectDashboard")
                unallowedStatusCodes.Add("submittedforapproval");

            projectDetails = (from proj in projects
                              join pm in projectManagers on proj.ProjectId equals pm.ProjectId into projpm
                              from programManager in projpm.DefaultIfEmpty()
                              join emp in employees on programManager.ProgramManagerId equals emp.EmployeeId into projemp
                              from e in projemp.DefaultIfEmpty()
                              join st in statuses on proj.ProjectStateId equals st.StatusId into projst
                              from status in projst.DefaultIfEmpty()
                              join pt in projectTypes on proj.ProjectTypeId equals pt.ProjectTypeId
                              join cl in clients on proj.ClientId equals cl.ClientId
                              join pa in practiceAreas on proj.PracticeAreaId equals pa.PracticeAreaId
                              where programManager.IsActive == true
                                   && (!unallowedStatusCodes.Contains(status.StatusCode.ToLower().Trim())
                                       || ("drafted".Equals(status.StatusCode.ToLower().Trim())
                                              && proj.CreatedBy.Equals(user.EmailAddress.ToLower().Trim())))
                                   && !unallowedProjectTypeCodes.Contains(pt.ProjectTypeCode)
                              orderby proj.ProjectName
                              select new ProjectDetail
                              {
                                  ProjectId = proj.ProjectId,
                                  ProjectCode = proj.ProjectCode,
                                  ProjectName = proj.ProjectName,
                                  ActualStartDate = proj.ActualStartDate,
                                  ActualEndDate = proj.ActualEndDate,
                                  ProjectState = status != null ? status.StatusCode : string.Empty,
                                  PracticeAreaCode = pa.PracticeAreaCode,
                                  ProjectTypeDescription = pt.Description,
                                  ClientName = cl.ClientName,
                                  ManagerName = e != null ? $"{e.FirstName} {e.LastName}" : string.Empty,
                                  ProjectManagerId = programManager != null ? programManager.ProgramManagerId.Value : 0
                              }).ToList();

            if ("Program Manager".ToLower().Trim().Equals(userRole.ToLower().Trim()))
                projectDetails = projectDetails.Where(pd => pd.ProjectManagerId == employeeId).ToList();
            else if (!"Department Head".ToLower().Trim().Equals(userRole.ToLower().Trim()))
                projectDetails = null;

            if (projectDetails != null && projectDetails.Count() > 0 && (dashboard == "PMDashboard" || dashboard == "DHDashboard"))
                projectDetails = projectDetails.Where(i => "submittedforapproval".Equals(i.ProjectState.ToLower().Trim())).ToList();

            //var json = JsonSerializer.Serialize(projectDetails);
        }
    }
}
