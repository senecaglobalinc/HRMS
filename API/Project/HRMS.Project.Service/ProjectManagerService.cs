using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class ProjectManagerService : IProjectManagerService
    {
        #region Global Varibles

        private readonly ILogger<ProjectManagerService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private IOrganizationService m_OrgService;
        private IEmployeeService m_EmpService;
        private IAssociateAllocationService m_AllocationService;

        #endregion

        #region ProjectManagerService
        public ProjectManagerService(ILogger<ProjectManagerService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IOrganizationService orgService,
            IEmployeeService empService,
            IAssociateAllocationService allocationService)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectManager, ProjectManager>();
            });
            m_mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            //m_apiEndPoints = apiEndPoints.Value;
            m_apiEndPoints = apiEndPoints != null ? apiEndPoints.Value : null;
            m_OrgService = orgService;
            m_EmpService = empService;
            m_AllocationService = allocationService;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates project manager
        /// </summary>
        /// <param name="projectManagerIn">Project manager information</param>
        /// <returns>ProjectManager</returns>
        public async Task<ServiceResponse<ProjectManager>> Create(ProjectManager projectManagerIn)
        {
            ServiceResponse<ProjectManager> response;
            try
            {
                int isCreated;
                m_Logger.LogInformation("Calling \"Create\" method in ProjectManagerService");

                ProjectManager projectManager = new ProjectManager();

                if (!projectManagerIn.IsActive.HasValue)
                    projectManagerIn.IsActive = true;

                m_Logger.LogInformation("Assigning to automapper.");

                m_mapper.Map<ProjectManager, ProjectManager>(projectManagerIn, projectManager);

                m_ProjectContext.ProjectManagers.Add(projectManager);
                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProjectManagerService");
                isCreated = await m_ProjectContext.SaveChangesAsync();

                if (isCreated > 0)
                {
                    m_Logger.LogInformation("Project manager created successfully.");
                    return response = new ServiceResponse<ProjectManager>()
                    {
                        Item = projectManager,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("No project manager created.");
                    return response = new ServiceResponse<ProjectManager>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = string.Empty
                    }; ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the ProjectManagers
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectManager>> GetAll(bool ?isActive)
        {
            ServiceListResponse<ProjectManager> response;
            var obj = await m_ProjectContext.ProjectManagers.ToListAsync();
            if (isActive.HasValue)
                    obj = obj.Where(x => x.IsActive == isActive).ToList();
            if (obj == null || obj.Count == 0)
            {
                response = new ServiceListResponse<ProjectManager>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Project Managers found.."
                };
            }
            else
            {
                response = new ServiceListResponse<ProjectManager>()
                {
                    Items = obj,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            return response;
        }

        #endregion

        #region GetById
        /// <summary>
        /// Get ProjectManagers by id
        /// </summary>
        /// <param name="id">ProjectManagers Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<ProjectManager>> GetById(int id)
        {
            ServiceResponse<ProjectManager> response;
            if (id == 0)
            {
                response = new ServiceResponse<ProjectManager>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            var obj = await m_ProjectContext.ProjectManagers.FindAsync(id);
            if (obj == null)
            {
                response = new ServiceResponse<ProjectManager>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "No Project Managers found with this Id."
                };
            }
            else
            {
                response = new ServiceResponse<ProjectManager>()
                {
                    Item = obj,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            return response;
        }

        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// GetProjectManagers by employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectManager>> GetByEmployeeId(int employeeId)
        {
            ServiceListResponse<ProjectManager> response = new ServiceListResponse<ProjectManager>();
            try
            {
                if (employeeId == 0)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Invalid Request..";
                }
                else
                {
                    var programManagers = await m_ProjectContext.ProjectManagers
                                                .Where(pm => (pm.ProgramManagerId == employeeId
                                                    || pm.LeadId == employeeId) && pm.IsActive == true).ToListAsync();
                    if (programManagers == null || programManagers.Count == 0)
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "No Program Manager count found for this employeeid.";
                    }
                    else
                    {
                        response.Items = programManagers;
                        response.IsSuccessful = true;
                        response.Message = "";
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in Project Manager Service - GetByEmployeeId method.");
                throw ex;
            }
        }
        #endregion

        #region GetProjectManagerByEmployeeId
        /// <summary>
        /// Get ProjectManagers by EmployeeId
        /// </summary>
        /// <param name="id">ProjectManagers Id</param>
        /// <returns></returns>
        public async Task<List<ProjectManager>> GetProjectManagerByEmployeeId(string employeeIds)
        {
            m_Logger.LogInformation("Calling \"GetProjectManagerByEmployeeId\" method in ProjectManagerService");
            List<ProjectManager> projectManagers = new List<ProjectManager>();


            try
            {
                List<int> ids = employeeIds.Split(",".ToCharArray()).Select(Int32.Parse).ToList();

                projectManagers = await m_ProjectContext.ProjectManagers.Where(
                                                                    p => ids.Contains(p.LeadId.Value) ||
                                                                    ids.Contains(p.ReportingManagerId.Value)
                                                                    )
                                                               .ToListAsync();

            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Exception occured in \"GetProjectManagerByEmployeeId\" method in ProjectManagerService" + ex.Message);
                throw ex;
            }

            return projectManagers;
        }
        #endregion

        #region GetActiveProjectManagers
        /// <summary>
        /// Get the Active project managers
        /// <param name="isActive"></param>
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectManager>> GetActiveProjectManagers(bool? isActive = true)
        {
            ServiceListResponse<ProjectManager> response;
            var obj = await m_ProjectContext.ProjectManagers.Where(pm => pm.IsActive == isActive).ToListAsync();
            if (obj == null || obj.Count == 0)
            {
                response = new ServiceListResponse<ProjectManager>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Active Project Managers found.."
                };
            }
            else
            {
                response = new ServiceListResponse<ProjectManager>()
                {
                    Items = obj,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            return response;
        }

        #endregion

        #region GetReportingDetailsByProjectId
        /// <summary>
        /// Get Reporting Details By ProjectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<ProjectManagersData>> GetReportingDetailsByProjectId(int projectId)
        {
            ServiceResponse<ProjectManagersData> response;
            var employees = await m_EmpService.GetAll(true);
            var query = await (from p in m_ProjectContext.Projects
                               join projectmanagers in m_ProjectContext.ProjectManagers on p.ProjectId equals projectmanagers.ProjectId into projects
                               from pm in projects.Where(i => i.IsActive == true).DefaultIfEmpty()
                               where p.ProjectId == projectId
                               select pm).ToListAsync();
            var managersList = (from projectmgrs in query
                                join programmanager in employees.Items on projectmgrs.ProgramManagerId equals programmanager.EmployeeId
                                join reportingmanager in employees.Items on projectmgrs.ReportingManagerId equals reportingmanager.EmployeeId into managers
                                from manager in managers.DefaultIfEmpty()
                                join lead in employees.Items on projectmgrs.LeadId equals lead.EmployeeId into leads
                                from lead in leads.DefaultIfEmpty()
                                select new ProjectManagersData
                                {
                                    ProgramManagerName = programmanager != null ? programmanager.FirstName + " " + programmanager.LastName : " ",
                                    ReportingManagerName = manager != null ? manager.FirstName + " " + manager.LastName : " ",
                                    LeadName = lead != null ? lead.FirstName + " " + lead.LastName : " ",
                                    ProgramManagerId = programmanager != null ? programmanager.EmployeeId : 0,
                                    ReportingManagerId = manager != null ? manager.EmployeeId : 0,
                                    LeadId = lead != null ? lead.EmployeeId : 0
                                }).Distinct().OrderBy(x => x.ReportingManagerName).FirstOrDefault();
            if (managersList == null)
            {
                response = new ServiceResponse<ProjectManagersData>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "No Active Project Managers found.."
                };
            }
            else
            {
                response = new ServiceResponse<ProjectManagersData>()
                {
                    Item = managersList,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            return response;
        }

        #endregion

        #region GetLeadsManagersBySearchString
        /// <summary>
        ///  Gets the Leads and managers information by searchString
        /// </summary>
        /// <param name="searchString">userName</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetLeadsManagersBySearchString(string searchString)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                List<Employee> employees = m_EmpService.GetAll(true).Result.Items;

                var projectManagers = GetActiveProjectManagers().Result.Items;

                //Gets the Leads, program managers and Reporting managers

                var obj = (from pm in m_ProjectContext.ProjectManagers
                           select new ProjectManager
                           {
                               IsActive = pm.IsActive,
                               ReportingManagerId = pm.ReportingManagerId
                           }).Distinct().ToList()

                    .Union(from rm in m_ProjectContext.ProjectManagers
                           select new ProjectManager
                           {
                               IsActive = rm.IsActive,
                               ProgramManagerId = rm.ProgramManagerId
                           }).Distinct().ToList()

                    .Union(from lead in m_ProjectContext.ProjectManagers
                           select new ProjectManager
                           {
                               IsActive = lead.IsActive,
                               LeadId = lead.ProgramManagerId
                           }).Distinct().ToList();


                var managersList = (from emp in employees
                                    join projmanager in obj
                                    on emp.EmployeeId equals projmanager.ReportingManagerId
                                    where emp.IsActive == true && projmanager.IsActive == true && emp.FirstName.ToLower().Contains(searchString.ToLower()) || emp.LastName.ToLower().Contains(searchString.ToLower())

                                    select new EmployeeDetails
                                    {
                                        EmployeeId = emp.EmployeeId,
                                        EmployeeName = emp.EmpName
                                    }).Distinct().ToList();

                response.Items = managersList;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Leads and managers data with searchString";
                m_Logger.LogError("Error occured in GetLeadsManagersBySearchString() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetLeadsManagersForDropdown
        /// <summary>
        ///  Gets the Leads and managers 
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetLeadsManagersForDropdown()
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> employees = m_EmpService.GetAssociatesForDropdown().Result.Items;

                //Gets the Leads, program managers and Reporting managers

                var managers = (from pm in m_ProjectContext.ProjectManagers
                                select new ProjectManager
                                {
                                    IsActive = pm.IsActive,
                                    ReportingManagerId = pm.ReportingManagerId
                                }).Distinct().ToList()

                    .Union(from rm in m_ProjectContext.ProjectManagers
                           select new ProjectManager
                           {
                               IsActive = rm.IsActive,
                               ReportingManagerId = rm.ProgramManagerId
                           }).Distinct().ToList()

                    .Union(from lead in m_ProjectContext.ProjectManagers
                           select new ProjectManager
                           {
                               IsActive = lead.IsActive,
                               ReportingManagerId = lead.LeadId
                           }).Distinct().ToList();

                List<GenericType> managersList = (from emp in employees
                                                  join projmanager in managers
                                                  on emp.Id equals projmanager.ReportingManagerId
                                                  where projmanager.IsActive == true
                                                  group emp by new { emp.Id, emp.Name } into g
                                                  select new GenericType
                                                  {
                                                      Id = g.Key.Id,
                                                      Name = g.Key.Name
                                                  }).OrderBy(c => c.Name).ToList();

                response.Items = managersList;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Leads and managers data";
                m_Logger.LogError("Error occured in GetLeadsManagersForDropdown() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetManagerandLeadByProjectIDandEmpId
        /// <summary>
        ///  Gets the Leads and managers information by projectId and EmployeeId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ManagersData>> GetManagerandLeadByProjectIdandEmpId(int projectId, int employeeId)
        {
            var response = new ServiceListResponse<ManagersData>();
            try
            {
                List<Employee> employees = m_EmpService.GetAll(true).Result.Items;
                List<AssociateAllocation> allocations = m_AllocationService.GetAll().Result.Items;

                var getNames = (from allocation in allocations
                                join programmanager in employees on allocation.ProgramManagerId equals programmanager.EmployeeId into pm
                                from programmanager in pm.DefaultIfEmpty()
                                join reportingmanager in employees on allocation.ReportingManagerId equals reportingmanager.EmployeeId into rm
                                from reportingmanager in rm.DefaultIfEmpty()
                                join lead in employees on allocation.LeadId equals lead.EmployeeId into leads
                                from lead in leads.DefaultIfEmpty()
                                where allocation.ProjectId == projectId && allocation.IsActive == true && allocation.EmployeeId == employeeId
                                select new ManagersData
                                {
                                    ProgramManagerName = (programmanager?.FirstName + " " + programmanager?.LastName) ?? "",
                                    ReportingManagerName = (reportingmanager?.FirstName + " " + reportingmanager?.LastName) ?? "",
                                    LeadName = (lead?.FirstName + " " + lead?.LastName) ?? "",
                                    ProgramManagerId = programmanager?.EmployeeId ?? 0,
                                    ReportingManagerId = reportingmanager?.EmployeeId ?? 0,
                                    LeadId = lead?.EmployeeId ?? 0
                                }).Distinct().ToList();

                response.Items = getNames;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the data";
                m_Logger.LogError("Error occured in GetManagerandLeadByProjectIdandEmpId() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region UpdateReportingManagerToAssociate
        /// <summary>
        ///  UpdateReportingManagerToAssociate
        /// </summary>
        /// <param name="projectData"></param>
        /// <param name="isDelivery"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> UpdateReportingManagerToAssociate(ProjectRequest projectData, bool isDelivery)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                bool status = false;

                using (var trans = m_ProjectContext.Database.BeginTransaction())
                {
                    if (isDelivery == true)
                    {
                        AssociateAllocation allocationData = await m_ProjectContext.AssociateAllocation.Where(allocation => allocation.EmployeeId == projectData.EmployeeId && allocation.ProjectId == projectData.ProjectId && allocation.IsActive == true).FirstOrDefaultAsync();
                        var existingAllocationEffectiveDate = Convert.ToDateTime(allocationData.EffectiveDate).Date;
                        if (existingAllocationEffectiveDate > Convert.ToDateTime(projectData.EffectiveDate).Date)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Invalid date. Effective date should be greater than are eqaul to " + string.Format("{0:dd-MM-yyyy}", existingAllocationEffectiveDate);
                            return response;
                        }
                        if (allocationData != null)
                        {
                            if (existingAllocationEffectiveDate < projectData.EffectiveDate)
                            {
                                int allocationId = allocationData.AssociateAllocationId;
                                allocationData.AssociateAllocationId = 0;
                                allocationData.ReportingManagerId = projectData.ReportingManagerId;
                                allocationData.ProgramManagerId = projectData.ManagerId;
                                allocationData.LeadId = projectData.LeadId;
                                allocationData.EffectiveDate = projectData.EffectiveDate;

                                //adding new allocation record after reporting manager changes
                                m_ProjectContext.AssociateAllocation.Add(allocationData);
                                response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                                if (response.IsSuccessful)
                                {
                                    //updating existing allocation as inactive
                                    AssociateAllocation existingAllocation = m_ProjectContext.AssociateAllocation.Find(allocationId);
                                    existingAllocation.IsActive = false;
                                    existingAllocation.ReleaseDate = Convert.ToDateTime(projectData.EffectiveDate).AddDays(-1);
                                    response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;

                                }
                            }
                            else
                            {
                                allocationData.ReportingManagerId = projectData.ReportingManagerId;
                                allocationData.ProgramManagerId = projectData.ManagerId;
                                allocationData.LeadId = projectData.LeadId;
                                response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;

                            }


                            status = (UpdateReportingManagerToEmployee(projectData.EmployeeId, projectData.ReportingManagerId)).Result.Item;
                        }
                    }
                    else
                    {
                        status = (UpdateReportingManagerToEmployee(projectData.EmployeeId, projectData.ReportingManagerId)).Result.Item;
                    }

                    trans.Commit();
                    if (status == true)
                    {
                        return response = new ServiceResponse<bool>()
                        {
                            Item = status,
                            IsSuccessful = true,
                            Message = "Updated ReportingManager To Associate.."
                        };
                    }
                    else
                    {
                        return response = new ServiceResponse<bool>()
                        {
                            Item = status,
                            IsSuccessful = false,
                            Message = "Failed updating ReportingManager To Associate.."
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while UpdateReportingManagerToAssociate";
                m_Logger.LogError("Error occured in UpdateReportingManagerToAssociate() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region UpdateReportingManagerToEmployee
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="reportingManagerID"></param>
        /// <returns></returns>
        private async Task<ServiceResponse<bool>> UpdateReportingManagerToEmployee(int? employeeID, int? reportingManagerID)
        {
            var response = new ServiceResponse<bool>();
            int employeeId = (int)employeeID;
            int RMId = (int)reportingManagerID;
            var employees = m_EmpService.GetAll(true).Result.Items;

            Employee employeeData = employees.Where(employee => employee.EmployeeId == employeeID && employee.IsActive == true).FirstOrDefault();

            if (employeeData != null)
            {
                employeeData.ReportingManager = reportingManagerID;
                await m_EmpService.UpdateReportingManagerId(employeeId, RMId);

                return response = new ServiceResponse<bool>()
                {
                    Item = true,
                    IsSuccessful = true,
                    Message = "Updated ReportingManager To Associate.."
                };
            }

            else
            {
                return response = new ServiceResponse<bool>()
                {
                    Item = false,
                    IsSuccessful = false,
                    Message = "Failed in UpdateReportingManagerToEmployee method.."
                };
            }
        }
        #endregion

        #region SaveManagersToProject
        /// <summary>
        /// Add and updates managers for a project
        /// </summary>
        /// <param name="projectManagerIn"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> SaveManagersToProject(ProjectManagersData projectManagerIn)
        {
            var response = new BaseServiceResponse();
            response.IsSuccessful = false;

            using (var dbContext = m_ProjectContext.Database.BeginTransaction())
            {
                try
                {
                    var IsAllocation = m_ProjectContext.AssociateAllocation.Where(allocation => allocation.ProjectId == projectManagerIn.ProjectId && allocation.IsActive == true).ToList().Count() > 0 ? true : false;
                    if (!IsAllocation)
                    {
                        DateTime projectActulaStartDate = Convert.ToDateTime(m_ProjectContext.Projects.Where(project => project.ProjectId == projectManagerIn.ProjectId && project.IsActive == true)
                            .Select(project => project.ActualStartDate).FirstOrDefault()).Date;

                        if (projectActulaStartDate > Convert.ToDateTime(projectManagerIn.EffectiveDate).Date)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Invalid date. Effective date should be greater than are eqaul to " + string.Format("{0:dd-MM-yyyy}", projectActulaStartDate);
                            return response;
                        }
                    }
                    ProjectManager projectManagerDetails = m_ProjectContext.ProjectManagers.Where(projects => projects.ProjectId == projectManagerIn.ProjectId && projects.IsActive == true).FirstOrDefault();

                    if (projectManagerDetails != null)
                    {

                        if (projectManagerDetails.ProjectId == projectManagerIn.ProjectId && projectManagerDetails.ProgramManagerId == projectManagerIn.ManagerId && projectManagerDetails.ReportingManagerId == projectManagerIn.ReportingManagerId && projectManagerDetails.LeadId == projectManagerIn.LeadId)
                        {
                            response.Message = "Record already exists.";
                            return response;
                        }

                        else
                        {
                            projectManagerDetails.IsActive = false;
                            response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                        }

                    }
                    ProjectManager projectManagerData = new ProjectManager()
                    {
                        ProjectId = projectManagerIn.ProjectId,
                        IsActive = true,
                        ProgramManagerId = projectManagerIn.ManagerId,
                        ReportingManagerId = projectManagerIn.ReportingManagerId,
                        LeadId = projectManagerIn.LeadId
                    };

                    m_ProjectContext.ProjectManagers.Add(projectManagerData);
                    response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;

                    List<AssociateAllocation> allocations = m_ProjectContext.AssociateAllocation.Where(allocation => allocation.ProjectId == projectManagerIn.ProjectId && allocation.EmployeeId != projectManagerIn.ManagerId && allocation.IsActive == true).ToList();
                    var minEffectiveDate = Convert.ToDateTime(allocations.Min(allocation => allocation.EffectiveDate)).Date;

                    if (minEffectiveDate > Convert.ToDateTime(projectManagerIn.EffectiveDate).Date)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Invalid date. Effective date should be greater than are eqaul to " + string.Format("{0:dd-MM-yyyy}", minEffectiveDate);
                        return response;
                    }
                    if (allocations.Count > 0)
                    {
                        foreach (var allocation in allocations)
                        {
                            AssociateAllocation associateAllocation;
                            if (Convert.ToDateTime(allocation.EffectiveDate).Date < projectManagerIn.EffectiveDate)
                            {
                                int allocationID = allocation.AssociateAllocationId;
                                associateAllocation = new AssociateAllocation();
                                associateAllocation = allocation;

                                associateAllocation.AssociateAllocationId = 0;
                                associateAllocation.ReportingManagerId = projectManagerIn.ReportingManagerId;
                                associateAllocation.ProgramManagerId = projectManagerIn.ManagerId;
                                associateAllocation.LeadId = projectManagerIn.LeadId;
                                associateAllocation.EffectiveDate = projectManagerIn.EffectiveDate;
                                //adding new record in alllocation when ProjectManagers change
                                m_ProjectContext.AssociateAllocation.Add(associateAllocation);
                                response.IsSuccessful = m_ProjectContext.SaveChanges() > 0 ? true : false;
                                if (response.IsSuccessful)
                                {

                                    //updating existing allocation as inactive (because projectmanager got changed)
                                    AssociateAllocation existingAllocation = m_ProjectContext.AssociateAllocation.Find(allocationID);
                                    existingAllocation.IsActive = false;
                                    existingAllocation.ReleaseDate = Convert.ToDateTime(projectManagerIn.EffectiveDate).AddDays(-1);
                                    response.IsSuccessful = m_ProjectContext.SaveChanges() > 0 ? true : false;
                                }
                            }
                            else
                            {

                                allocation.ReportingManagerId = projectManagerIn.ReportingManagerId;
                                allocation.ProgramManagerId = projectManagerIn.ManagerId;
                                allocation.LeadId = projectManagerIn.LeadId;
                                response.IsSuccessful = m_ProjectContext.SaveChanges() > 0 ? true : false;
                            }


                            //update into employee table
                            //var emp = new EmployeeDetails();
                            //emp.ReportingManagerId = allocation.ReportingManagerId;
                            //emp.EmployeeId = allocation.EmployeeId ?? 0;
                            //emp.External = "ReportingManager";
                            var emp_response = await m_EmpService.UpdateReportingManagerId(allocation.EmployeeId ?? 0, projectManagerIn.ReportingManagerId ?? 0);
                            //Employee employees = m_ProjectContext.Employees.Where(employee => employee.EmployeeId == allocation.EmployeeId && employee.IsActive == true).FirstOrDefault();

                            //if (employees != null)
                            //{
                            //    employees.ReportingManager = allocation.ReportingManagerId;
                            //    employees.ModifiedDate = DateTime.Now;
                            //    employees.ModifiedUser = HttpContext.Current.User.Identity.Name;
                            //    employees.SystemInfo = Commons.GetClientIPAddress();
                            //    m_ProjectContext.Entry(employees).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    dbContext.Commit();
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    m_Logger.LogError("Error occured while Assigning Manager To Project :" + ex.StackTrace);
                }
                return response;
            }

        }
        #endregion

        #region GetProgramManagersForDropdown
        /// <summary>
        /// Get ProgramManagers ForDropdown
        /// </summary>        
        /// <returns></returns>       
        public async Task<ServiceListResponse<GenericType>> GetProgramManagersForDropdown()
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> employees = m_EmpService.GetAssociatesForDropdown().Result.Items;
                List<int> managers = await m_ProjectContext.ProjectManagers.Where(pm => pm.IsActive == true && pm.ProgramManagerId.HasValue)
                                                 .Select(c => c.ProgramManagerId.Value).Distinct().ToListAsync();


                var getNames = (from pm in managers
                                join em in employees on pm equals em.Id
                                select new GenericType
                                {
                                    Id = em.Id,
                                    Name = em.Name
                                }).OrderBy(c => c.Name).Distinct().ToList();

                response.Items = getNames;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the data";
                m_Logger.LogError("Error occured in GetProgramManagersForDropdown() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetProjectLeadData
        /// <summary>
        /// GetProjectManagersData
        /// </summary>        
        /// <returns></returns>       
        public async Task<ServiceListResponse<ProjectManagersData>> GetProjectLeadData(int employeeId)
        {
            var response = new ServiceListResponse<ProjectManagersData>();
            try
            {
                var projectManagers = await m_ProjectContext.ProjectManagers.Where(pm => pm.LeadId == employeeId && pm.IsActive == true).ToListAsync();
                var projectManagersData = (from pm in projectManagers
                                           join prj in m_ProjectContext.Projects on pm.ProjectId equals prj.ProjectId
                                           where prj.IsActive == true
                                           select new ProjectManagersData
                                           {
                                               ProjectName = prj.ProjectName,
                                               ProgramManagerId = pm.ProgramManagerId
                                           }).Distinct().ToList();

                response.Items = projectManagersData;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the data";
                m_Logger.LogError("Error occured in GetProjectLeadData() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetProjectRMData
        /// <summary>
        /// GetProjectRMData
        /// </summary>        
        /// <returns></returns>       
        public async Task<ServiceListResponse<ProjectManagersData>> GetProjectRMData(int employeeId)
        {
            var response = new ServiceListResponse<ProjectManagersData>();
            try
            {
                var projectManagers = await m_ProjectContext.ProjectManagers.Where(pm => pm.ReportingManagerId == employeeId && pm.IsActive == true).ToListAsync();
                var projectManagersData = (from pm in projectManagers
                                           join prj in m_ProjectContext.Projects on pm.ProjectId equals prj.ProjectId
                                           where prj.IsActive == true
                                           select new ProjectManagersData
                                           {
                                               ProjectName = prj.ProjectName,
                                               ProgramManagerId = pm.ProgramManagerId
                                           }
                           ).Distinct().ToList();

                response.Items = projectManagersData;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the data";
                m_Logger.LogError("Error occured in GetProjectRMData() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetProjectManagerFromAllocations
        /// <summary>
        /// GetProjectManagerFromAllocations
        /// </summary>        
        /// <returns></returns>       
        public async Task<bool> GetProjectManagerFromAllocations(int employeeId)
        {
            try
            {
                var projectManagersOfAllocations = await m_ProjectContext.AssociateAllocation.Where(pm => (pm.ReportingManagerId == employeeId || pm.LeadId == employeeId || pm.ProgramManagerId == employeeId) && pm.IsActive == true).ToListAsync();

                return projectManagersOfAllocations.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in GetProjectManagerFromAllocations() method" + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region GetPMByPracticeAreaID
        /// <summary>
        /// Get  ProjectManagers by practiceAreaId
        /// </summary>
        /// <param name="practiceAreaId">practiceAreaId</param>
        /// <returns></returns>
        public async Task<ProjectManager> GetPMByPracticeAreaId(int practiceAreaId)
        {
            m_Logger.LogInformation("Calling \"GetPMByPracticeAreaId\" method in ProjectManagerService");
            ProjectManager projectManagers = new ProjectManager();

            try
            {
                var projectManagersData = await (from pm in m_ProjectContext.ProjectManagers
                                                 join tp in m_ProjectContext.TalentPool on pm.ProjectId equals tp.ProjectId
                                                 where pm.IsActive == true && tp.IsActive == true && tp.PracticeAreaId == practiceAreaId
                                                 select new ProjectManager
                                                 {
                                                     ProjectId = pm.ProjectId,
                                                     ReportingManagerId = pm.ReportingManagerId,
                                                     ProgramManagerId = pm.ProgramManagerId,
                                                     LeadId = pm.LeadId,
                                                     IsActive = pm.IsActive
                                                 }).Distinct().ToListAsync();

                projectManagers = projectManagersData.FirstOrDefault();

            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Exception occured in \"GetPMByPracticeAreaId\" method in ProjectManagerService" + ex.Message);
                throw ex;
            }

            return projectManagers;
        }
        #endregion
    }
}
