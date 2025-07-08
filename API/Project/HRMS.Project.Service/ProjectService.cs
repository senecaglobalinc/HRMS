using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Constants;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HRMS.Project.Service
{
    public class ProjectService : IProjectService
    {
        #region Global Varibles

        private readonly ILogger<ProjectService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private readonly IMapper m_closemapper;
        private IHttpClientFactory m_clientFactory;
        private IProjectManagerService m_projectManagerService;
        private IAssociateAllocationService m_associateAllocationService;
        private IOrganizationService m_OrgService;
        private APIEndPoints m_apiEndPoints;
        private IClientBillingRoleService m_clientBillingRoleService;
        private IEmployeeService m_EmployeeService;
        private readonly IMapper m_Mapper;
        private readonly IMapper m_CloseMapper;
        private readonly IConfiguration m_config;
        IMapper mapper;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;

        #endregion

        #region ProjectService
        public ProjectService(ILogger<ProjectService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IProjectManagerService projectManagerService,
            IAssociateAllocationService associateAllocationService,
            IOrganizationService orgService,
            IClientBillingRoleService clientBillingRoleService,
            IEmployeeService employeeService,
            IMapper mapper,
            IMapper closemapper,
            IConfiguration configuration,
            IOptions<MiscellaneousSettings> miscellaneousSettings)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            m_clientFactory = clientFactory;
            //m_apiEndPoints = apiEndPoints.Value;
            m_apiEndPoints = apiEndPoints != null ? apiEndPoints.Value : null;
            m_projectManagerService = projectManagerService;
            m_associateAllocationService = associateAllocationService;
            m_OrgService = orgService;
            m_clientBillingRoleService = clientBillingRoleService;
            m_EmployeeService = employeeService;
            m_config = configuration;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectRequest, Entities.Project>();
            });
            m_Mapper = config.CreateMapper();
            m_mapper = mapper;
            var configure = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectClosureInitiationResponse, Entities.ProjectClosure>();
            });
            m_CloseMapper = configure.CreateMapper();
            m_closemapper = closemapper;
            m_MiscellaneousSettings = miscellaneousSettings?.Value;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates project based on sent info.
        /// </summary>
        /// <param name="projectIn">Project information</param>
        /// <returns>Project</returns>
        public async Task<ServiceResponse<int>> Create(ProjectRequest projectIn)
        {
            ServiceResponse<int> response;
            try
            {
                int isCreated;
                m_Logger.LogInformation("Calling \"Create\" method in ProjectService");

                ServiceResponse<Entities.Project> projectCodeAlreadyExits =
                    await GetByProjectCode(projectIn.ProjectCode);

                if (projectCodeAlreadyExits.Item != null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project code already exists."
                    };
                }
                ServiceResponse<Status> draftedStatus = await m_OrgService.GetStatusByCategoryAndStatusCode("PPC", "Drafted");
                ServiceResponse<Status> executionStatus = await m_OrgService.GetStatusByCategoryAndStatusCode("PPC", "Execution");

                if (!projectIn.IsActive.HasValue)
                    projectIn.IsActive = true;

                if (projectIn.ProjectStateId == -1)
                    projectIn.ProjectStateId = executionStatus.Item.StatusId;

                else if (projectIn.ProjectStateId != 0)
                    projectIn.ProjectStateId = draftedStatus.Item.StatusId;



                Entities.Project project = new Entities.Project();
                project = m_mapper.Map<ProjectRequest, Entities.Project>(projectIn);
                m_ProjectContext.Projects.Add(project);

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProjectService");
                isCreated = await m_ProjectContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    var project_id = await m_ProjectContext.Projects.Where(q => q.ProjectCode == project.ProjectCode).Select(p => p.ProjectId).FirstOrDefaultAsync();
                    var projectManager = new ProjectManager()
                    {
                        ProjectId = project_id,
                        ProgramManagerId = projectIn.ManagerId,
                        ReportingManagerId = projectIn.ManagerId,
                        LeadId = projectIn.ManagerId,
                        IsActive = true
                    };
                    
                    var pm_created = await m_projectManagerService.Create(projectManager);
                    if (pm_created == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Error occurred while creatig project manager."
                        };
                    }
                    m_Logger.LogInformation("Project created successfully.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = project_id,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("No project created.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No project created."
                    };
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
        /// Get the all the projects
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Entities.Project>> GetAll()
        {
            ServiceListResponse<Entities.Project> response;
            var obj = await m_ProjectContext.Projects.OrderBy(x => x.ProjectName).ToListAsync();
            if (obj == null || obj.Count == 0)
            {
                return response = new ServiceListResponse<Entities.Project>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Projects found.."
                };
            }
            else
            {
                return response = new ServiceListResponse<Entities.Project>()
                {
                    Items = obj,
                    IsSuccessful = true,
                    Message = ""
                };
            }
        }
        #endregion

        #region GetByProjectCode
        /// <summary>
        /// Gets the project
        /// TODO- Need to move this service to Project Microservice
        /// </summary>
        /// <param name="projectCode">project code</param>
        /// <returns>Project</returns>
        public async Task<ServiceResponse<Entities.Project>> GetByProjectCode(string projectCode)
        {
            ServiceResponse<Entities.Project> response;
            if (string.IsNullOrEmpty(projectCode))
            {
                return response = new ServiceResponse<Entities.Project>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            var obj = await m_ProjectContext.Projects.Where(prj => prj.ProjectCode == projectCode)
.FirstOrDefaultAsync();
            if (obj == null)
            {
                return response = new ServiceResponse<Entities.Project>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "No Project found with this project code.."
                };
            }
            else
            {
                return response = new ServiceResponse<Entities.Project>()
                {
                    Item = obj,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
        }
        #endregion

        #region GetById
        /// <summary>
        /// Get Project by id
        /// </summary>
        /// <param name="id">project Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<Entities.Project>> GetById(int id)
        {
            ServiceResponse<Entities.Project> response;
            if (id == 0)
            {
                return response = new ServiceResponse<Entities.Project>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            var obj = await m_ProjectContext.Projects.FindAsync(id);
            if (obj == null)
            {
                return response = new ServiceResponse<Entities.Project>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "No Project found with this Id.."
                };
            }
            else
            {
                return response = new ServiceResponse<Entities.Project>()
                {
                    Item = obj,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
        }

        #endregion

        #region Update
        /// <summary>
        /// Update project based on sent info.
        /// </summary>
        /// <param name="projectIn">Project information</param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> Update(ProjectRequest projectIn)
        {
            ServiceResponse<int> response;
            try
            {
                int isUpdated;
                m_Logger.LogInformation("Calling \"Update\" method in ProjectService");

                ServiceResponse<Status> closedStatus = await m_OrgService.GetStatusByCategoryAndStatusCode("PPC", "Closed");

                var project = m_ProjectContext.Projects.Find(projectIn.ProjectId);

                if (project == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project not found for update."
                    };
                }
                else
                    m_Logger.LogInformation("Project found for update.");

                ServiceResponse<Entities.Project> projectAlreadyExits =
                   await GetByProjectCode(projectIn.ProjectCode);

                if (projectAlreadyExits != null &&
                    projectAlreadyExits.Item.ProjectId != project.ProjectId)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = -1,
                        IsSuccessful = false,
                        Message = "Project code already exists."
                    };
                }
                else
                    m_Logger.LogInformation("Project code not already exists.");

                ProjectManager projectManager = await m_ProjectContext.ProjectManagers
                                .Where(pm => pm.ProjectId == project.ProjectId && pm.IsActive == true).FirstOrDefaultAsync();

                if (projectManager == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = -3,
                        IsSuccessful = false,
                        Message = "Project manager not found."
                    };
                }

                if (projectIn.ProjectStateId == closedStatus.Item.StatusId && projectIn.ActualEndDate.HasValue)
                {
                    var allocations = await m_associateAllocationService.GetByProjectId(projectIn.ProjectId);

                    if (allocations.Items.Where(aa => !aa.ReleaseDate.HasValue).Count() > 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = -2,
                            IsSuccessful = false,
                            Message = "Project has allocations can't close."
                        };
                    }

                    project.ActualEndDate = projectIn.ActualEndDate;
                    project.ProjectStateId = projectIn.ProjectStateId;

                    //Update Project Manager                   
                    projectManager.IsActive = false;
                }
                else
                {
                    if (Roles.DepartmentHead.GetEnumDescription().TrimLowerCase().Equals(projectIn.UserRole.TrimLowerCase()) ||
                        Roles.ProgramManager.GetEnumDescription().TrimLowerCase().Equals(projectIn.UserRole.TrimLowerCase()))
                    {
                        project.ProjectName = projectIn.ProjectName;
                        project.ProjectStateId = projectIn.ProjectStateId;
                        project.ActualStartDate = projectIn.ActualStartDate;
                        project.ActualEndDate = projectIn.ActualEndDate;
                        project.ClientId = projectIn.ClientId;
                        project.DomainId = projectIn.DomainId;
                        project.PracticeAreaId = projectIn.PracticeAreaId;
                        project.ProjectTypeId = projectIn.ProjectTypeId;
                        project.DepartmentId = projectIn.DepartmentId;
                    }
                    //     m_mapper.Map<ProjectRequest, Entities.Project>(projectIn, project);

                    //Update Project Manager   
                    if (Roles.DepartmentHead.GetEnumDescription().TrimLowerCase().Equals(projectIn.UserRole.TrimLowerCase()))
                        projectManager.ProgramManagerId = projectIn.ManagerId;
                }

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProjectService");
                isUpdated = await m_ProjectContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    m_Logger.LogInformation("Project Updated successfully.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 1,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("No project created.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No project updated."
                    }; ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region HasActiveClientBillingRoles
        /// <summary>
        /// HasActiveClientBillingRoles
        /// </summary>
        /// <param name="projectId">Project information</param>
        /// <returns>Project</returns>
        public async Task<ServiceResponse<int>> HasActiveClientBillingRoles(int projectId)
        {
            ServiceResponse<int> response;
            try
            {
                m_Logger.LogInformation("Calling \"HasActiveClientBillingRoles\" method in ProjectService");
                if (projectId == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Invalid Request.."
                    };
                }
                int clientBillingRoleCount = 0;
                var project = await m_ProjectContext.Projects.FindAsync(projectId);

                if (project == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "HasActiveClientBillingRoles - No Project found with this projectId.."
                    };
                }
                else
                {
                    var Lead = await m_ProjectContext.ProjectManagers.Where(st => st.ProjectId == projectId && st.IsActive == true).FirstOrDefaultAsync();
                    int? LeadId = Lead.LeadId;
                    if (!LeadId.HasValue)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = -1,
                            IsSuccessful = true,
                            Message = "No Lead is Assigned to this Project - Please Assign Lead"
                        };
                    }
                    var clientBillingRole = await m_clientBillingRoleService.GetAllByProjectId(projectId);
                    clientBillingRoleCount = clientBillingRole.Items.Count();
                    if (clientBillingRoleCount > 0)
                    {
                        m_Logger.LogInformation("HasActiveClientBillingRoles - Client billing roles active.");
                    }
                    else
                    {
                        m_Logger.LogError("HasActiveClientBillingRoles - No client billing roles.");
                    }
                    return response = new ServiceResponse<int>()
                    {
                        Item = clientBillingRoleCount,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in HasActiveClientBillingRoles.");
                throw ex;
            }

        }
        #endregion

        #region GetProjectsList
        /// <summary>
        /// Gets Projects List by user role, employeeId and dashboard
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="employeeId"></param>
        /// <param name="dashboard"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectResponse>> GetProjectsList(string userRole, int employeeId, string dashboard)
        {
            ServiceListResponse<ProjectResponse> response;
            response = new ServiceListResponse<ProjectResponse>();

            List<Entities.Project> projects = null;
            ServiceListResponse<Status> statusesforPPC = null;
            ServiceResponse<Employee> employee = null;
            ServiceResponse<User> user = null;
            int closedStatusId;
            int closureInitiationStatusId;
            int draftedStatusId;
            int inProgressStatusId;

            if (string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(dashboard) || employeeId == 0)
            {
                return response = new ServiceListResponse<ProjectResponse>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "Invalid Request."
                };
            }
            statusesforPPC = await m_OrgService.GetStatusesByCategoryName(HRMS.Common.Enums.CategoryMaster.PPC.ToString());

            if (statusesforPPC.Items == null || (statusesforPPC.Items != null && statusesforPPC.Items.Count <= 0))
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Status for project category not found.";
            }


            closedStatusId = statusesforPPC.Items.Where(st => "closed".Equals(st.StatusCode.ToLower().Trim()))
                .Select(st => st.StatusId).FirstOrDefault();

            if (closedStatusId == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Closed status for project category not found.";
            }

            draftedStatusId = statusesforPPC.Items.Where(st => "drafted".Equals(st.StatusCode.ToLower().Trim()))
                .Select(st => st.StatusId).FirstOrDefault();



            if (draftedStatusId == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Drafted status for project category not found.";
            }

            closureInitiationStatusId = statusesforPPC.Items.Where(st => "closureinitiated".Equals(st.StatusCode.ToLower().Trim())).
            Select(st => st.StatusId).FirstOrDefault();
            if (closureInitiationStatusId == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Closure Initiation status for project category not found.";
            }
            inProgressStatusId = statusesforPPC.Items.Where(st => "inprogress".Equals(st.StatusCode.ToLower().Trim())).
            Select(st => st.StatusId).FirstOrDefault();
            if (inProgressStatusId == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Closure Initiation status for project category not found.";
            }
            employee = await m_EmployeeService.GetEmployeeById(employeeId);

            if (employee.Item == null)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Employee not found.";
            }

            if (!employee.Item.UserId.HasValue)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "User not found.";
            }

            user = await m_OrgService.GetUserById(employee.Item.UserId.Value);

            if (user.Item == null)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "User not found.";
            }
            var projType = await m_OrgService.GetAllProjectTypes(true);
            int talentpoolProjectTypeId = projType.Items.Where(x => x.ProjectTypeCode.ToLower().Contains("talent pool")).Select(x => x.ProjectTypeId).FirstOrDefault();
            int trainingProjectTypeId = projType.Items.Where(x => x.ProjectTypeCode.ToLower().Contains("training")).Select(x => x.ProjectTypeId).FirstOrDefault();
            //Fetch projects where he/she is project manager
            if (Roles.ProgramManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
                projects = await (from proj in m_ProjectContext.Projects
                                  join pm in m_ProjectContext.ProjectManagers on proj.ProjectId equals pm.ProjectId
                                  where (pm.IsActive.Value == true && pm.ProgramManagerId.Value == employeeId
                                  && proj.ProjectTypeId != talentpoolProjectTypeId && proj.ProjectTypeId != trainingProjectTypeId)
                                  orderby proj.ProjectName
                                  select proj).Distinct().ToListAsync();
            //Fetch all projects
            else if (Roles.DepartmentHead.GetEnumDescription().TrimLowerCase().Equals(userRole.TrimLowerCase()))
                projects = await (from proj in m_ProjectContext.Projects
                                  join pm in m_ProjectContext.ProjectManagers.Where(q => q.IsActive == true) on proj.ProjectId equals pm.ProjectId
                                  where ((proj.ProjectStateId != closedStatusId && proj.ProjectStateId != draftedStatusId) ||
                                  (proj.ProjectStateId == draftedStatusId && (proj.CreatedBy == user.Item.EmailAddress || proj.CreatedBy == "Anonymous"))
                                  && proj.ProjectTypeId != talentpoolProjectTypeId && proj.ProjectTypeId != trainingProjectTypeId)
                                  orderby proj.ProjectName
                                  select proj).Distinct().ToListAsync();

            else if (Roles.TeamLead.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
                projects = await (from proj in m_ProjectContext.Projects
                                  join pm in m_ProjectContext.ProjectManagers on proj.ProjectId equals pm.ProjectId
                                  join pc in m_ProjectContext.ProjectClosure on proj.ProjectId equals pc.ProjectId
                                  join pcr in m_ProjectContext.ProjectClosureReport
                                  on pc.ProjectClosureId equals pcr.ProjectClosureId
                                  where (pm.IsActive.Value == true && pm.LeadId.Value == employeeId) &&
                                  (proj.ProjectStateId == closureInitiationStatusId && pcr.StatusId == inProgressStatusId)
                                  && (proj.ProjectTypeId != 6 && proj.ProjectTypeId != 8)
                                  orderby proj.ProjectName
                                  select proj).Distinct().ToListAsync();

            else if (Roles.ITManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                ServiceResponse<Department> department = await m_OrgService.GetDepartmentByCode("IT");
                projects = await (from proj in m_ProjectContext.Projects
                                  join pc in m_ProjectContext.ProjectClosure on proj.ProjectId equals pc.ProjectId
                                  join pa in m_ProjectContext.ProjectClosureActivity on pc.ProjectClosureId equals pa.ProjectClosureId
                                  where (proj.ProjectStateId == closureInitiationStatusId && pa.DepartmentId == department.Item.DepartmentId
                                  && pa.StatusId == inProgressStatusId)
                                  orderby proj.ProjectName
                                  select proj).Distinct().ToListAsync();
            }
            else if (Roles.FinanceManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                ServiceResponse<Department> department = await m_OrgService.GetDepartmentByCode("FD");
                projects = await (from proj in m_ProjectContext.Projects
                                  join pc in m_ProjectContext.ProjectClosure on proj.ProjectId equals pc.ProjectId
                                  join pa in m_ProjectContext.ProjectClosureActivity on pc.ProjectClosureId equals pa.ProjectClosureId
                                  where (proj.ProjectStateId == closureInitiationStatusId && pa.DepartmentId == department.Item.DepartmentId
                                  && pa.StatusId == inProgressStatusId)
                                  orderby proj.ProjectName
                                  select proj).Distinct().ToListAsync();
            }
            else if (Roles.QualityandInformationSecurityManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                ServiceResponse<Department> department = await m_OrgService.GetDepartmentByCode("QA");
                projects = await (from proj in m_ProjectContext.Projects
                                  join pc in m_ProjectContext.ProjectClosure on proj.ProjectId equals pc.ProjectId
                                  join pa in m_ProjectContext.ProjectClosureActivity on pc.ProjectClosureId equals pa.ProjectClosureId
                                  where (proj.ProjectStateId == closureInitiationStatusId && pa.DepartmentId == department.Item.DepartmentId
                                  && pa.StatusId == inProgressStatusId)
                                  orderby proj.ProjectName
                                  select proj).Distinct().ToListAsync();
            }
            else if (Roles.AdminManager.GetEnumDescription().TrimLowerCase().Equals(userRole.ToLower().Trim()))
            {
                ServiceResponse<Department> department = await m_OrgService.GetDepartmentByCode("Admin");
                projects = await (from proj in m_ProjectContext.Projects
                                  join pc in m_ProjectContext.ProjectClosure on proj.ProjectId equals pc.ProjectId
                                  join pa in m_ProjectContext.ProjectClosureActivity on pc.ProjectClosureId equals pa.ProjectClosureId
                                  where (proj.ProjectStateId == closureInitiationStatusId && pa.DepartmentId == department.Item.DepartmentId
                                  && pa.StatusId == inProgressStatusId)
                                  orderby proj.ProjectName
                                  select proj).Distinct().ToListAsync();
            }

            List<ProjectResponse> projRes = new List<ProjectResponse>();
            if (projects != null && projects.Count > 0)
            {
                ServiceListResponse<Client> clients = null;
                ServiceListResponse<PracticeArea> practiceAreas = null;
                ServiceListResponse<ProjectType> projectTypes = null;

                List<int> clientIds = projects.Where(proj => proj.ClientId != 0).Select(proj => proj.ClientId).Distinct().ToList();
                if (clientIds != null && clientIds.Count > 0)
                    clients = await m_OrgService.GetClientsByIds(clientIds);

                List<int> practiceAreaIds = projects
                    .Where(proj => proj.PracticeAreaId != 0).Select(proj => proj.PracticeAreaId).Distinct().ToList();
                if (practiceAreaIds != null && practiceAreaIds.Count > 0)
                    practiceAreas = await m_OrgService.GetPracticeAreasByIds(practiceAreaIds);

                List<int> projectTypeIds = projects
                    .Where(proj => proj.ProjectTypeId != 0).Select(proj => proj.PracticeAreaId).Distinct().ToList();
                if (projectTypeIds != null && projectTypeIds.Count > 0)
                    projectTypes = await m_OrgService.GetProjectTypesByIds(projectTypeIds);

                foreach (var project in projects)
                {
                    Status projStatus = null;
                    if (project.ProjectStateId != null)
                    {
                        projStatus = statusesforPPC.Items.Where(st => st.StatusId == project.ProjectStateId).FirstOrDefault();
                        if (((dashboard == "PMDashboard") && projStatus.StatusCode != "SubmittedForApproval" && projStatus.StatusCode != "ClosureInitiated" && projStatus.StatusCode != "SubmittedForClosureApproval"
                            && projStatus.StatusCode != "RejectedClosureByDH"))
                        {
                            continue;
                        }
                        else if (((dashboard == "ProjectDashboard") && (projStatus.StatusCode == "SubmittedForApproval" || projStatus.StatusCode == "ClosureInitiated" || projStatus.StatusCode == "SubmittedForClosureApproval"
                            || projStatus.StatusCode == "RejectedClosureByDH" || projStatus.StatusCode == "AprrovedClosureByDH")))
                        {
                            continue;
                        }
                        else
                        {
                            if (((dashboard == "DHDashboard") && projStatus.StatusCode != "SubmittedForApproval" && projStatus.StatusCode != "SubmittedForClosureApproval"))
                            {
                                continue;
                            }
                        }
                        ProjectResponse pr = new ProjectResponse();
                        pr.ProjectId = project.ProjectId;
                        pr.ProjectCode = project.ProjectCode;
                        pr.ProjectName = project.ProjectName;
                        pr.ActualStartDate = project.ActualStartDate;
                        pr.ActualEndDate = project.ActualEndDate;
                        if (projStatus.StatusCode != null)
                        {
                            pr.StatusCode = projStatus.StatusCode;
                            pr.StatusId = projStatus.StatusId;
                            pr.ProjectStateId = projStatus.StatusId;
                            pr.ProjectState = projStatus.StatusCode;
                        }
                        if (clients.Items != null && clients.Items.Count > 0)
                        {
                            var client = clients.Items.Where(q => q.ClientId == project.ClientId).Select(q => q).FirstOrDefault();
                            if (client != null)
                            {
                                pr.ClientName = client.ClientName;
                                pr.ClientId = client.ClientId;
                            }
                        }
                        if (practiceAreas.Items != null && practiceAreas.Items.Count > 0)
                        {
                            var practiceArea = practiceAreas.Items.Where(q => q.PracticeAreaId == project.PracticeAreaId).FirstOrDefault();
                            if (practiceArea != null)
                            {
                                pr.PracticeAreaCode = practiceArea.PracticeAreaCode;
                                pr.PracticeAreaId = practiceArea.PracticeAreaId;
                            }
                        }
                        if (projectTypes.Items != null && projectTypes.Items.Count > 0)
                        {
                            var projectType = projectTypes.Items.Where(q => q.ProjectTypeId == project.ProjectTypeId.Value).FirstOrDefault();
                            if (projectType != null)
                            {
                                pr.ProjectTypeDescription = projectType.Description;
                                pr.ProjectTypeId = projectType.ProjectTypeId;
                            }
                        }

                        pr.IsActive = project.IsActive;
                        pr.PlannedStartDate = project.PlannedStartDate;
                        pr.PlannedEndDate = project.PlannedEndDate;

                        var progManager = m_ProjectContext.ProjectManagers.Where(pm => pm.ProjectId == project.ProjectId && pm.IsActive == true).FirstOrDefault();
                        if (progManager != null)
                        {
                            var emp = await m_EmployeeService.GetEmployeeById(progManager.ProgramManagerId.Value);
                            if (emp.IsSuccessful)
                            {
                                pr.ManagerName = emp.Item.FirstName + " " + emp.Item.LastName;
                                pr.ManagerId = emp.Item.EmployeeId;
                            }
                        }
                        if (project.DomainId.HasValue)
                        {
                            pr.DomainId = project.DomainId.Value;
                            var domain = await m_OrgService.GetDomainById(project.DomainId.Value);
                            if (domain.Item != null)
                            {
                                pr.DomainName = domain.Item.DomainName;
                            }
                        }
                        var department = await (m_OrgService.GetDepartmentById(project.DepartmentId));
                        if (department.Item != null)
                        {
                            pr.DepartmentCode = department.Item.DepartmentCode;
                            pr.DepartmentId = department.Item.DepartmentId;
                        }
                        projRes.Add(pr);
                    }
                };

                response.Items = projRes;
                response.IsSuccessful = true;
                response.Message = string.Empty;
            }
            else
            {

                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Projects not found..";
            }
            return response;
        }
        #endregion

        #region GetProjectById
        /// <summary>
        /// Get the project info by Id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<ProjectResponse>> GetProjectById(int projectId)
        {

            ServiceResponse<ProjectResponse> response = new ServiceResponse<ProjectResponse>();
            Entities.Project project = null;
            ServiceListResponse<Status> statuses = null;
            int closedStatusId;
            int draftedStatusId;
            if (projectId == 0)
            {
                return response = new ServiceResponse<ProjectResponse>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request."
                };
            }
            statuses = await m_OrgService.GetStatusesByCategoryName(HRMS.Common.Enums.CategoryMaster.PPC.ToString());

            if (statuses.Items == null || (statuses.Items != null && statuses.Items.Count <= 0))
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = "Status for project category not found.";
            }

            closedStatusId = statuses.Items.Where(st => "closed".Equals(st.StatusCode.ToLower().Trim()))
                .Select(st => st.StatusId).FirstOrDefault();

            if (closedStatusId == 0)
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = "Closed status for project category not found.";
            }

            draftedStatusId = statuses.Items.Where(st => "drafted".Equals(st.StatusCode.ToLower().Trim()))
                .Select(st => st.StatusId).FirstOrDefault();

            if (draftedStatusId == 0)
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = "Drafted status for project category not found.";
            }

            project = await (from proj in m_ProjectContext.Projects
                             join pm in m_ProjectContext.ProjectManagers on proj.ProjectId equals pm.ProjectId
                             where (pm.IsActive.Value == true &&
                                    proj.ProjectId == projectId)
                             orderby proj.ProjectName
                             select proj).FirstOrDefaultAsync();


            ProjectResponse pr = new ProjectResponse();
            if (project != null)
            {
                ServiceResponse<Client> client = null;
                ServiceResponse<PracticeArea> practiceArea = null;
                ServiceResponse<ProjectType> projectType = null;
                ServiceResponse<Status> status = null;
                ServiceResponse<Domain> domain = null;
                ServiceResponse<Department> dept = null;

                client = await m_OrgService.GetClientById(project.ClientId);
                practiceArea = await m_OrgService.GetPracticeAreaById(project.PracticeAreaId);
                projectType = await m_OrgService.GetProjectTypeById(project.ProjectTypeId.Value);
                var st = statuses.Items.Where(st_inner => st_inner.StatusId == project.ProjectStateId).FirstOrDefault();
                if (project.DomainId != null)
                    domain = await m_OrgService.GetDomainById(project.DomainId.Value);
                dept = await m_OrgService.GetDepartmentById(project.DepartmentId);
                if (st != null)
                {
                    status = new ServiceResponse<Status>();
                    status.Item = st;
                }

                pr.ProjectId = project.ProjectId;
                pr.ProjectCode = project.ProjectCode;
                pr.ProjectName = project.ProjectName;
                pr.ActualStartDate = project.ActualStartDate;
                pr.ActualEndDate = project.ActualEndDate;

                if (domain != null && domain.Item != null)
                {
                    pr.DomainId = domain.Item.DomainID;
                    pr.DomainName = domain.Item.DomainName;
                }
                if (dept != null && dept.Item != null)
                {
                    pr.DepartmentId = dept.Item.DepartmentId;
                    pr.DepartmentCode = dept.Item.DepartmentCode;
                }
                if (status != null && status.Item != null)
                {
                    pr.StatusCode = status.Item.StatusCode;
                    pr.StatusId = status.Item.StatusId;
                    pr.ProjectStateId = status.Item.StatusId;
                    pr.ProjectState = status.Item.StatusCode;
                }
                if (client != null && client.Item != null)
                {
                    pr.ClientName = client.Item.ClientName;
                    pr.ClientId = client.Item.ClientId;
                }

                if (practiceArea != null && practiceArea.Item != null)
                {
                    pr.PracticeAreaCode = practiceArea.Item.PracticeAreaCode;
                    pr.PracticeAreaId = practiceArea.Item.PracticeAreaId;
                }

                if (projectType != null && projectType.Item != null)
                {
                    pr.ProjectTypeDescription = projectType.Item.Description;
                    pr.ProjectTypeId = projectType.Item.ProjectTypeId;
                    pr.ProjectTypeCode = projectType.Item.ProjectTypeCode;
                }
                pr.IsActive = project.IsActive;
                pr.PlannedStartDate = project.PlannedStartDate;
                pr.PlannedEndDate = project.PlannedEndDate;
                var progManager = m_ProjectContext.ProjectManagers.Where(pm => pm.ProjectId == project.ProjectId && pm.IsActive == true).FirstOrDefault();
                if (progManager != null)
                {
                    var emp = await m_EmployeeService.GetEmployeeById(progManager.ProgramManagerId.Value);
                    if (emp.IsSuccessful)
                    {
                        pr.ManagerName = emp.Item.FirstName + " " + emp.Item.LastName;
                        pr.ManagerId = emp.Item.EmployeeId;
                    }
                }

                response.Item = pr;
                response.IsSuccessful = true;
                response.Message = string.Empty;
            }
            else
            {

                response.Item = null;
                response.IsSuccessful = false;
                response.Message = "Projects not found..";
            }
            return response;

        }
        #endregion

        #region SubmitForApproval
        /// <summary>
        /// Submits a project for approval by DH
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userRole"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> SubmitForApproval(int projectId, string userRole, int employeeId)
        {
            ServiceResponse<int> response;

            if (projectId == 0 || string.IsNullOrEmpty(userRole) || employeeId == 0)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Invalid Request"
                };
            }
            if (userRole == "Program Manager")
            {
                //GetByCategoryName... "PPC"
                var category = await m_OrgService.GetCategoryByName("PPC");

                if (category == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Category not found."
                    };
                }

                //GetNotificationTypeByCategoryId.. "PPC" (New Method)
                var notificationType = await m_OrgService.GetNotificationTypeByCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString());

                if (notificationType == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Notification Type not found."
                    };
                }

                //GetEmailTo from NotificationConfiguration by CategoryId and NotificationTypeId
                var notificationConfig = await m_OrgService.GetByNotificationTypeAndCategoryId(notificationType.Item.NotificationTypeId, category.Item.CategoryMasterId);

                if (notificationConfig == null || (!notificationConfig.IsSuccessful && string.IsNullOrEmpty(notificationConfig.Message)))
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Notification Config not found."
                    };
                }
                //Get userId by email
                var user = await m_OrgService.GetUserByEmail(notificationConfig.Item.EmailTo); //Need to pass the email from the above get.

                if (user == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "User not found."
                    };
                }
                //Get employeeId by userId
                var emp = await m_EmployeeService.GetEmployeeByUserId(user.Item.UserId); //Need to pass the userId from the above user object.
                if (emp == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Employee not found."
                    };
                }
                //Get Workflow status by CategoryId && statusCode ="SubmittedForApproval"
                //Need to pass the categoryId from above category object.
                var status = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, "SubmittedForApproval");
                if (status == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Status not found."
                    };
                }

                using (var trans = m_ProjectContext.Database.BeginTransaction())
                {
                    ProjectWorkFlow p_workflow = new ProjectWorkFlow()
                    {
                        SubmittedBy = employeeId,
                        // should be used an employee id from emp object.
                        SubmittedTo = emp.Item.EmployeeId,
                        SubmittedDate = DateTime.Now,
                        // should be used the status id from the above status object.
                        WorkFlowStatus = status.Item.StatusId,
                        ProjectId = projectId,
                        Comments = null
                    };

                    m_ProjectContext.ProjectWorkFlow.Add(p_workflow);

                    //Get Project by project Id

                    var project = await (m_ProjectContext.Projects.Where(q => q.ProjectId == projectId)).FirstOrDefaultAsync();

                    project.ProjectStateId = status.Item.StatusId;
                    //project.ModifiedDate = DateTime.Now;

                    var created = m_ProjectContext.SaveChanges();

                    trans.Commit();
                    // var created = m_ProjectContext.SaveChangesAsync();

                    response = new ServiceResponse<int>();
                    response.Item = 1;
                    response.IsSuccessful = true;
                    response.Message = "Project submitted for Approval successfully.";

                    await NotificationConfiguration(projectId);

                    return response;
                }
            }
            else
            {
                var approveOrRejectByDH = await ApproveOrRejectProjectByDH(projectId, employeeId, true);
                if (approveOrRejectByDH.Item == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = approveOrRejectByDH.Message
                    };
                }
                else
                {
                    //NotificationConfiguration(projectId);
                    return response = new ServiceResponse<int>()
                    {
                        Item = approveOrRejectByDH.Item,
                        IsSuccessful = approveOrRejectByDH.IsSuccessful,
                        Message = approveOrRejectByDH.Message
                    };
                }
            }
        }
        #endregion

        #region ApproveOrRejectByDH
        /// <summary>
        /// Approves or Rejects a project by DH
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="status"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> ApproveOrRejectByDH(int projectId, string status, int employeeId)
        {
            ServiceResponse<int> response;
            if (projectId == 0 || string.IsNullOrEmpty(status) || employeeId == 0)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Invalid Request"
                };
            }
            if (status == "Approve")
            {
                var approveOrRejectByDH = await ApproveOrRejectProjectByDH(projectId, employeeId, false);
                if (approveOrRejectByDH.Item == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = approveOrRejectByDH.Message
                    };
                }
                else
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = approveOrRejectByDH.Item,
                        IsSuccessful = approveOrRejectByDH.IsSuccessful,
                        Message = approveOrRejectByDH.Message
                    };
                }
            }
            else
            {

                //GetByCategoryName... "PPC"
                var category = await m_OrgService.GetCategoryByName("PPC");

                if (category == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Category not found."
                    };
                }

                //GetNotificationTypeByCategoryId.. "PPC" (New Method)
                var notificationType = await m_OrgService.GetNotificationTypeByCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString());
                if (notificationType == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Notification Type not found."
                    };
                }

                //Get EmailTo from NotificationConfiguration by CategoryId and NotificationTypeId
                var notificationConfig = await m_OrgService.GetByNotificationTypeAndCategoryId(notificationType.Item.NotificationTypeId, category.Item.CategoryMasterId);

                if (notificationConfig == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Notification Config not found."
                    };
                }

                //Get userId using EmailTo.
                var user = await m_OrgService.GetUserByEmail(notificationConfig.Item.EmailTo);

                if (user == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "User not found."
                    };
                }

                //Get submittedTo employee by userId
                var emp = await m_EmployeeService.GetEmployeeByUserId(user.Item.UserId); //Need to pass the userId from the above user object.
                if (emp == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Employee not found."
                    };
                }

                //Get Workflow status by CategoryId && statusCode ="RejectedByDH"
                var workFlowByDH = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, "RejectedByDH");
                if (workFlowByDH == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Workflow not found."
                    };
                }

                //Get projectstate status from Status by CategoryId AND StatusCode = 'Drafted' 
                var projectState = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, "Drafted");
                if (projectState == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project State not found."
                    };
                }

                using (var trans = m_ProjectContext.Database.BeginTransaction())
                {
                    //Update project workflow
                    ProjectWorkFlow p_workflow = new ProjectWorkFlow()
                    {
                        SubmittedBy = employeeId,
                        SubmittedTo = emp.Item.EmployeeId,
                        SubmittedDate = DateTime.Now,
                        WorkFlowStatus = workFlowByDH.Item.StatusId,
                        ProjectId = projectId,
                        Comments = null
                    };

                    m_ProjectContext.ProjectWorkFlow.Add(p_workflow);

                    //Get Project by project Id
                    var project = await (m_ProjectContext.Projects.Where(q => q.ProjectId == projectId)).FirstOrDefaultAsync();
                    //Update project state
                    //workflow status id.;
                    project.ProjectStateId = projectState.Item.StatusId;
                    //project.ModifiedBy = HttpContext.Current.User.Identity.Name;
                    //project.ModifiedDate = DateTime.Now;
                    //project.SystemInfo = 

                    var created = m_ProjectContext.SaveChanges();

                    trans.Commit();
                    if (created == 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Failed to approve a project.."
                        };
                    }
                    else
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = created,
                            IsSuccessful = true,
                            Message = "Project Rejected successfully.",
                        };
                    }
                }
            }
        }
        #endregion

        #region ApproveOrRejectProjectByDH
        private async Task<ServiceResponse<int>> ApproveOrRejectProjectByDH(int projectId, int employeeId, bool approveFlag)
        {
            ServiceResponse<int> response;

            if (projectId == 0 || employeeId == 0)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Invalid Request"
                };
            }

            //GetByCategoryName... "PPC"
            var category = await m_OrgService.GetCategoryByName("PPC");

            if (category == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Category not found."
                };
            }

            //Get Workflow status by CategoryId && statusCode = "SubmittedForApproval"
            //Need to pass the categoryId from above category object.
            var workFlowstatus = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, "SubmittedForApproval");
            if (workFlowstatus == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "WorkFlow Status by CategoryId not found."
                };
            }

            //Get Workflow status from Status by CategoryId && statusCode ="SubmittedForApproval"
            var wfStatusByDH = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, "ApprovedByDH");
            if (wfStatusByDH == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "WorkFlow Status not found."
                };
            }

            //Get projectstate status from Status by CategoryId AND StatusCode = 'Created' 
            var projectState = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, "Created");
            if (projectState == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Project Status not found."
                };
            }

            var projectWorkFlow = new ProjectWorkFlow();
            var toUserEmp = new ServiceResponse<Employee>();
            var user = new ServiceResponse<User>();
            var submittedToEmp = new ServiceResponse<Employee>();

            //Check if direct DH save the project
            if (!approveFlag)
            {
                //Get submitted by info from ProjectWorkFlow by projectId and workflowstatus
                projectWorkFlow = await m_ProjectContext.ProjectWorkFlow
                                           .Where(q => q.ProjectId == projectId && q.WorkFlowStatus == workFlowstatus.Item.StatusId)
                                           .FirstOrDefaultAsync();
                if (projectWorkFlow == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project Workflow not found"
                    };
                }

                //Get userId who submitted the project using submittedBy info
                toUserEmp = await m_EmployeeService.GetActiveEmployeeById(projectWorkFlow.SubmittedBy);
                if (toUserEmp == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "To User not found"
                    };
                }

                //Get EmailTo from Users by userId
                user = await m_OrgService.GetUserById(toUserEmp.Item.UserId.Value);
                if (user == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "User not found."
                    };
                }

                //Get SubmittedTo from employee using userId
                submittedToEmp = await m_EmployeeService.GetEmployeeByUserId(toUserEmp.Item.UserId.Value);
                if (submittedToEmp == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "SubmittedToEmp not found."
                    };
                }
            }

            using (var trans = m_ProjectContext.Database.BeginTransaction())
            {
                ProjectWorkFlow p_workflow = new ProjectWorkFlow()
                {
                    SubmittedBy = employeeId,
                    SubmittedTo = approveFlag ? employeeId : submittedToEmp.Item.EmployeeId,
                    SubmittedDate = DateTime.Now,
                    WorkFlowStatus = wfStatusByDH.Item.StatusId,
                    ProjectId = projectId,
                    Comments = null
                };

                m_ProjectContext.ProjectWorkFlow.Add(p_workflow);

                //Get Project by project Id

                var project = await (m_ProjectContext.Projects.Where(q => q.ProjectId == projectId)).FirstOrDefaultAsync();

                //workflow status id.;
                project.ProjectStateId = projectState.Item.StatusId;
                //project.ModifiedBy = HttpContext.Current.User.Identity.Name;
                //project.ModifiedDate = DateTime.Now;
                //project.SystemInfo = 

                var created = m_ProjectContext.SaveChanges();
                trans.Commit();
                if (created == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Failed to approve a project.."
                    };
                }
                else
                {
                    await ApprovedNotificationConfiguration(projectId);
                    return response = new ServiceResponse<int>()
                    {
                        Item = created,
                        IsSuccessful = true,
                        Message = "Project Approved successfully.",
                    };
                }
            }
        }
        #endregion

        #region DeleteProjectDetails
        public async Task<ServiceResponse<bool>> DeleteProjectDetails(int projectId)
        {
            ServiceResponse<bool> response;
            try
            {
                var category = await m_OrgService.GetCategoryByName("PPC");

                if (category == null)
                {
                    return response = new ServiceResponse<bool>()
                    {
                        Item = false,
                        IsSuccessful = false,
                        Message = "Category not found."
                    };
                }
                var status = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, "Drafted");
                if (status == null)
                {
                    return response = new ServiceResponse<bool>()
                    {
                        Item = false,
                        IsSuccessful = false,
                        Message = "Status not found."
                    };
                }
                var project = await m_ProjectContext.Projects.Where(q => q.ProjectId == projectId &&
                                                        q.ProjectStateId == status.Item.StatusId).FirstOrDefaultAsync();
                if (project == null)
                {
                    return response = new ServiceResponse<bool>()
                    {
                        Item = false,
                        IsSuccessful = false,
                        Message = "Project not found to delete."
                    };
                }
                else
                {
                    //remove any project managers associated with project.
                    var p_manager = await m_ProjectContext.ProjectManagers
                                            .Where(q => q.ProjectId == projectId && q.IsActive == true).FirstOrDefaultAsync();
                    if (p_manager != null)
                    {
                        m_ProjectContext.Remove(p_manager);
                    }

                    //remove any sows attached with project.
                    var p_sow = await m_ProjectContext.SOW
                                        .Where(q => q.ProjectId == projectId && q.IsActive == true).ToListAsync();
                    if (p_sow != null && p_sow.Count > 0)
                    {
                        m_ProjectContext.SOW.RemoveRange(p_sow);
                    }
                    //remove any Client Billing Roles attached with project.
                    var p_cbr = await m_ProjectContext.ClientBillingRoles
                                        .Where(q => q.ProjectId == projectId && q.IsActive == true).ToListAsync();
                    if (p_cbr != null && p_cbr.Count > 0)
                    {
                        m_ProjectContext.ClientBillingRoles.RemoveRange(p_cbr);
                    }

                    //Remove project
                    m_ProjectContext.Projects.Remove(project);

                    var deleted = await m_ProjectContext.SaveChangesAsync();
                    if (deleted == 0)
                    {
                        return response = new ServiceResponse<bool>()
                        {
                            Item = false,
                            IsSuccessful = false,
                            Message = "Could not delete the project.."
                        };
                    }
                    return response = new ServiceResponse<bool>()
                    {
                        Item = true,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
            }

            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in DeleteProjectDetails.: " + ex.Message + "\n" + ex.StackTrace);
                return response = new ServiceResponse<bool>()
                {
                    Item = false,
                    IsSuccessful = false,
                    Message = "An error occurred.."
                };
            }
        }
        #endregion

        #region NotificationConfiguration
        /// <summary>
        /// NotificationConfiguration
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns></returns>
        private async Task<ServiceResponse<int>> NotificationConfiguration(int projectId)
        {
            ServiceResponse<int> response;
            try
            {
                m_Logger.LogInformation("Calling \"NotificationConfiguration\" method in ProjectService");

                //Send notification email to Delivery Head for approval
                int notificationTypeId = Convert.ToInt32(HRMS.Common.Enums.NotificationType.PPC);
                // notificationTypeId = 12;
                int categoryId = Convert.ToInt32(HRMS.Common.Enums.CategoryMaster.PPC);
                // categoryId = 9;
                ServiceResponse<NotificationConfiguration> notificationConfiguration = await m_OrgService.GetNotificationConfiguration(notificationTypeId, categoryId);

                if (notificationConfiguration.Item != null)
                {
                    var project = await GetProjectById(projectId);

                    if (project != null)
                    {
                        NotificationDetail notificationDetail = new NotificationDetail();
                        StringBuilder emailContent = new StringBuilder(System.Net.WebUtility.HtmlDecode(notificationConfiguration.Item.EmailContent));

                        emailContent = emailContent.Replace("{ProjectCode}", project.Item.ProjectCode)
                                                   .Replace("{ProjectName}", project.Item.ProjectName)
                                                   .Replace("{ManagerName}", project.Item.ManagerName)
                                                   .Replace("{ProjectState}", project.Item.ProjectState);

                        if (string.IsNullOrEmpty(notificationConfiguration.Item.EmailFrom))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email From cannot be blank"
                            };
                        }
                        notificationDetail.FromEmail = notificationConfiguration.Item.EmailFrom;

                        if (string.IsNullOrEmpty(notificationConfiguration.Item.EmailTo))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email To cannot be blank"
                            };
                        }
                        notificationDetail.ToEmail = notificationConfiguration.Item.EmailTo;

                        notificationDetail.CcEmail = notificationConfiguration.Item.EmailCC;

                        notificationDetail.Subject = project.Item.ProjectName + " " + notificationConfiguration.Item.EmailSubject;
                        notificationDetail.EmailBody = emailContent.ToString();
                        m_OrgService.SendEmail(notificationDetail);
                    }
                }
                return response = new ServiceResponse<int>()
                {
                    Item = 1,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in NotificationConfiguration.");

                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Exception occured in NotificationConfiguration."
                };
            }

        }
        #endregion

        #region ApprovedNotificationConfiguration
        /// <summary>
        /// ApprovedNotificationConfiguration
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns></returns>
        private async Task<ServiceResponse<int>> ApprovedNotificationConfiguration(int projectId)
        {
            ServiceResponse<int> response;
            try
            {
                m_Logger.LogInformation("Calling \"ApprovedNotificationConfiguration\" method in ProjectService");

                //Get Program Manager email address
                var PMEmail = (await GetPMEmail(projectId)).Item;

                //Send notification email from Delivery Head to Program Manager

                int notificationTypeId = Convert.ToInt32(Common.Enums.NotificationType.PPCApproved);
                //notificationTypeId = 17;
                int categoryId = Convert.ToInt32(Common.Enums.CategoryMaster.PPC);
                //categoryId = 10;
                ServiceResponse<NotificationConfiguration> notificationConfiguration = await m_OrgService.GetNotificationConfiguration(notificationTypeId, categoryId);

                if (notificationConfiguration.Item != null)
                {
                    var project = await GetProjectById(projectId);

                    if (project != null)
                    {
                        NotificationDetail notificationDetail = new NotificationDetail();
                        StringBuilder emailContent = new StringBuilder(System.Net.WebUtility.HtmlDecode(notificationConfiguration.Item.EmailContent));

                        emailContent = emailContent.Replace("{ProjectCode}", project.Item.ProjectCode)
                                                   .Replace("{ProjectName}", project.Item.ProjectName)
                                                   .Replace("{ManagerName}", project.Item.ManagerName)
                                                   .Replace("{PragramManager}", PMEmail);

                        if (string.IsNullOrEmpty(notificationConfiguration.Item.EmailFrom))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email From cannot be blank"
                            };
                        }
                        notificationDetail.FromEmail = notificationConfiguration.Item.EmailFrom;

                        if (string.IsNullOrEmpty(PMEmail))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email To cannot be blank"
                            };
                        }
                        //Program Manager Email Address
                        notificationDetail.ToEmail = PMEmail;

                        string HigherOfficialEmail = m_MiscellaneousSettings.HigherOfficialEmail;
                        List<DepartmentWithDLAddress> DepartmentDLs = m_OrgService.GetAllDepartmentsWithDLs().Result.Items;
                        DepartmentWithDLAddress departmenthead = DepartmentDLs.Where(department => department.DepartmentCode == "IT").FirstOrDefault();
                        notificationDetail.CcEmail = notificationConfiguration.Item.EmailCC + "," + HigherOfficialEmail + "," + departmenthead.DepartmentDLAddress;
                        notificationDetail.Subject = project.Item.ProjectName + " " + notificationConfiguration.Item.EmailSubject;
                        notificationDetail.EmailBody = emailContent.ToString();
                        m_OrgService.SendEmail(notificationDetail);
                    }
                }
                return response = new ServiceResponse<int>()
                {
                    Item = 1,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in ApprovedNotificationConfiguration.");
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Exception occured in ApprovedNotificationConfiguration."
                };
            }

        }
        #endregion

        #region GetPMEmail
        /// <summary>
        /// GetPMEmail
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Program Manager Email</returns>
        private async Task<ServiceResponse<string>> GetPMEmail(int projectId)
        {
            ServiceResponse<string> response;
            try
            {
                m_Logger.LogInformation("Calling \"ApprovedNotificationConfiguration\" method in ProjectService");

                //ServiceResponse<Status> status = await m_OrgService.GetStatusByCategoryAndStatusCode("PPC", "SubmittedForApproval");

                //var projectWorkflow =
                //        await m_ProjectContext.ProjectWorkFlow.Where(pw => pw.ProjectId == projectId
                //                                                               && pw.WorkFlowStatus == status.Item.StatusId
                //                                                        ).ToListAsync();
                //int submittedBy = projectWorkflow.First().SubmittedBy;

                //var employee = await m_EmployeeService.GetEmployeeById(submittedBy);

                var projectDtls = await m_ProjectContext.ProjectManagers.Where(pw => pw.ProjectId == projectId && pw.IsActive == true).ToListAsync();

                int pmId = projectDtls.First().ProgramManagerId ?? 0;

                var employee = await m_EmployeeService.GetEmployeeById(pmId);

                int userId = employee.Item.UserId.Value;

                var users = await m_OrgService.GetUserById(userId);
                string emailAddress = users.Item.EmailAddress;
                // string emailAddress = "jayanth.chincholi@senecaglobal.com"; //  to-do: works only for DEV (Replace with the above line)

                return response = new ServiceResponse<string>()
                {
                    Item = emailAddress,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in getting PM Email.");

                return response = new ServiceResponse<string>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Exception occured in getting PM Email."
                };
            }

        }
        #endregion

        #region GetProjectsForAllocation
        /// <summary>
        /// Get Projects ForAllocation
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectResponse>> GetProjectsForAllocation()
        {
            var response = new ServiceListResponse<ProjectResponse>();

            ServiceListResponse<ReportDetails> masters = await m_OrgService.GetAssociateAllocationMasters();

            if (masters.Items == null || masters.Items.Count == 0)
            {
                m_Logger.LogInformation("No Masters found");
                response = new ServiceListResponse<ProjectResponse>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Masters found"
                };
                return response;
            }

            List<ReportDetails> clients = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Client).ToList();
            List<ReportDetails> departments = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Department).ToList();
            List<ReportDetails> projectStatuses = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.ProjectStatus).ToList();
            List<ReportDetails> projectTypes = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.ProjectType).ToList();
            List<ReportDetails> practiceAreas = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.PracticeArea).ToList();
            var createdProjectStatus = projectStatuses.Where(c => c.Name == "Created").Select(x => x.Id).FirstOrDefault();
            var executedProjectStatus = projectStatuses.Where(c => c.Name == "Execution").Select(x => x.Id).FirstOrDefault();

            var projects = await (from p in m_ProjectContext.Projects
                                  join pt in m_ProjectContext.ProjectManagers on p.ProjectId equals pt.ProjectId
                                  where p.IsActive == true && pt.IsActive == true
                                  select new ProjectResponse
                                  {
                                      ProjectId = p.ProjectId,
                                      ClientId = p.ClientId,
                                      ProjectCode = p.ProjectCode,
                                      ActualStartDate = p.ActualStartDate,
                                      ActualEndDate = p.ActualEndDate,
                                      ProjectName = p.ProjectName,
                                      ProjectTypeId = p.ProjectTypeId ?? 0,
                                      PracticeAreaId = p.PracticeAreaId,
                                      DepartmentId = p.DepartmentId,
                                      ProjectStateId = p.ProjectStateId ?? 0,
                                      ProgramManagerId = pt.ProgramManagerId ?? 0,
                                      StatusId = p.StatusId ?? 0
                                  }).ToListAsync();

            List<int> programManagerIdList = projects.Select(c => c.ProgramManagerId).Distinct().ToList();
            var programManagers = await m_EmployeeService.GetEmployeesByIds(programManagerIdList);
            var projRes = (from p in projects
                           join cl in clients on p.ClientId equals cl.Id
                           join pt in projectTypes on p.ProjectTypeId equals pt.Id
                           join pa in practiceAreas on p.PracticeAreaId equals pa.Id
                           join e in programManagers.Items on p.ProgramManagerId equals e.EmpId into empgr
                           from emp in empgr.DefaultIfEmpty()
                           join d in departments on p.DepartmentId equals d.Id into depgr
                           from dept in depgr.DefaultIfEmpty()
                           where p.ProjectStateId == createdProjectStatus || p.ProjectStateId == executedProjectStatus
                           select new ProjectResponse
                           {
                               ProjectId = p.ProjectId,
                               ProjectCode = p.ProjectCode,
                               ActualStartDate = p.ActualStartDate,
                               ActualEndDate = p.ActualEndDate,
                               ProjectName = p.ProjectName,
                               ClientName = cl.Name,
                               ProjectTypeId = pt.Id,
                               ProjectTypeDescription = pt.Name,
                               PracticeAreaId = pa.Id,
                               PracticeAreaCode = pa.Name,
                               ProgramManager = emp == null ? string.Empty : emp.EmpName,
                               ProgramManagerId = p.ProgramManagerId,
                               ProjectStateId = p.ProjectStateId,
                               StatusId = p.StatusId
                           }).OrderBy(x => x.ProjectName).ToList();
            if (projRes != null && projRes.Count > 0)
            {
                response.Items = projRes;
                response.IsSuccessful = true;
                response.Message = string.Empty;
            }
            else
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Projects not found..";
            }
            return response;
        }
        #endregion

        #region GetAssociateProjectsForRelease
        /// <summary>
        /// GetAssociateProjectsForRelease
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectDetails>> GetAssociateProjectsForRelease(int employeeId)
        {
            var response = new ServiceListResponse<ProjectDetails>();
            try
            {
                int projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower() == "talent pool").Select(x => x.ProjectTypeId).FirstOrDefault();

                IQueryable<ProjectDetails> allocations = (from allocation in m_ProjectContext.AssociateAllocation
                                                          join project in m_ProjectContext.Projects on allocation.ProjectId equals project.ProjectId
                                                          where allocation.EmployeeId == employeeId && allocation.IsActive == true
                                                          && allocation.ReleaseDate == null && project.ProjectTypeId != projectTypeId
                                                          select new ProjectDetails
                                                          {
                                                              ProjectId = project.ProjectId,
                                                              ProjectName = project.ProjectName,
                                                              EffectiveDate = allocation.EffectiveDate,
                                                          });

                if (allocations != null && allocations.Count() > 0)
                {
                    response.Items = allocations.ToList();
                    response.IsSuccessful = true;
                }
                else
                {

                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Projects not found..";
                }
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
                m_Logger.LogError("Error occured while fetching project details", e.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetProjectsForDropdown
        /// <summary>
        /// GetProjectsForDropdown
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetProjectsForDropdown()
        {
            var response = new ServiceListResponse<GenericType>();
            var executedProjectStatus = await m_OrgService.GetStatusByCategoryAndStatusCode("PPC", "Execution");
            var projRes = await (from p in m_ProjectContext.Projects
                                 where p.ProjectStateId == executedProjectStatus.Item.StatusId
                                 select new GenericType
                                 {
                                     Id = p.ProjectId,
                                     Name = p.ProjectName
                                 }).OrderBy(x => x.Name).ToListAsync();

            if (projRes != null && projRes.Count > 0)
            {
                response.Items = projRes;
                response.IsSuccessful = true;
                response.Message = string.Empty;
            }
            else
            {

                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Projects not found..";
            }
            return response;
        }
        #endregion

        #region GetProjectById
        public async Task<ServiceResponse<ProjectResponse>> GetEmpTalentPool(int employeeId)
        {
            ServiceResponse<ProjectResponse> response = new ServiceResponse<ProjectResponse>();
            try
            {
                var employee = await m_EmployeeService.GetActiveEmployeeById(employeeId);
                var practiceArea = await m_OrgService.GetPracticeAreaById((int)employee.Item.CompetencyGroup);
                var result = await (from p in m_ProjectContext.Projects
                                    join projectmgrs in m_ProjectContext.ProjectManagers on p.ProjectId equals projectmgrs.ProjectId
                                    join tp in m_ProjectContext.TalentPool on p.ProjectId equals tp.ProjectId
                                    where (tp.IsActive == true && projectmgrs.IsActive == true && tp.PracticeAreaId == employee.Item.CompetencyGroup)
                                    select new ProjectResponse
                                    {
                                        ProjectId = p.ProjectId,
                                        ProjectName = p.ProjectName,
                                        ReportingManagerId = projectmgrs.ReportingManagerId,
                                        EmployeeCode = employee.Item.EmployeeCode,
                                        EmployeeId = employeeId,
                                        FirstName = employee.Item.FirstName,
                                        LastName = employee.Item.LastName,
                                        PracticeArea = practiceArea.Item.PracticeAreaCode
                                    }).FirstOrDefaultAsync();
                if (result == null)
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "Projects not found..";
                }
                else
                {
                    response.Item = result;
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in NotificationConfiguration.", ex.StackTrace);
                response.IsSuccessful = false;
                response.Message = "Exception occured in NotificationConfiguration.";

            }
            return response;

        }

        public async Task<ServiceListResponse<ProjectResponse>> GetEmpTalentPool(int employeeId, int projectId, string roleName)
        {
            ServiceListResponse<ProjectResponse> response = new ServiceListResponse<ProjectResponse>();
            try
            {
                var emp = await m_EmployeeService.GetActiveEmployeeById(employeeId);
                List<Employee> employees = new List<Employee>();
                employees.Add(emp.Item);

                int projectTypeId = m_ProjectContext.Projects.Where(c => c.ProjectId == projectId)
                                    .Select(d => d.ProjectTypeId.Value).FirstOrDefault();

                var projectType = await m_OrgService.GetProjectTypeById(projectTypeId);

                var practiceArea = await m_OrgService.GetAllPracticeArea(true);
                var projects = await (from p in m_ProjectContext.Projects
                                      join projectmgrs in m_ProjectContext.ProjectManagers on p.ProjectId equals projectmgrs.ProjectId
                                      join tp in m_ProjectContext.TalentPool on p.ProjectId equals tp.ProjectId
                                      where (tp.IsActive == true && projectmgrs.IsActive == true
                                      //&& p.IsActive == true
                                      )
                                      select new ProjectResponse
                                      {
                                          ProjectId = p.ProjectId,
                                          ProjectName = p.ProjectName,
                                          ReportingManagerId = projectmgrs.ReportingManagerId,
                                          PracticeAreaId = tp.PracticeAreaId ?? 0
                                      }).ToListAsync();

                if (roleName.ToLower() == StringConstants.TRANING_DEPT_HEAD_ROLE.ToLower() || projectType.Item.ProjectTypeCode == "Training")
                {
                    var result = (from p in projects
                                  join pa in practiceArea.Items on p.PracticeAreaId equals pa.PracticeAreaId
                                  select new ProjectResponse
                                  {
                                      EmployeeId = emp.Item.EmployeeId,
                                      EmployeeCode = emp.Item.EmployeeCode,
                                      FirstName = emp.Item.FirstName,
                                      LastName = emp.Item.LastName,
                                      ProjectId = p.ProjectId,
                                      ProjectName = p.ProjectName,
                                      ReportingManagerId = p.ReportingManagerId,
                                      PracticeArea = pa.PracticeAreaCode
                                  }).ToList();
                    if (result == null)
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Projects not found..";
                    }
                    else
                    {
                        response.Items = result;
                        response.IsSuccessful = true;
                    }
                }
                else
                {
                    var result = (from e in employees
                                  join pa in practiceArea.Items on e.CompetencyGroup equals pa.PracticeAreaId
                                  join p in projects on e.CompetencyGroup equals p.PracticeAreaId into gp
                                  from tp in gp.DefaultIfEmpty()
                                  where e.EmployeeId == employeeId
                                  select new ProjectResponse
                                  {
                                      EmployeeId = e.EmployeeId,
                                      EmployeeCode = e.EmployeeCode,
                                      FirstName = e.FirstName,
                                      LastName = e.LastName,
                                      ProjectId = tp.ProjectId,
                                      ProjectName = tp.ProjectName,
                                      ReportingManagerId = tp.ReportingManagerId,
                                      PracticeArea = pa.PracticeAreaCode
                                  }).ToList();
                    if (result == null)
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Projects not found..";
                    }
                    else
                    {
                        response.Items = result;
                        response.IsSuccessful = true;
                    }
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in GetEmpTalentPool.");
                response.IsSuccessful = false;
                response.Message = "Exception occured in GetEmpTalentPool.";

            }
            return response;

        }
        #endregion

        #region GetProjectsByEmpId
        /// <summary>
        /// Get Project by empid
        /// </summary>
        /// <param name="employeeId">project Id</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectDetails>> GetProjectsByEmpId(int employeeId)
        {
            ServiceListResponse<ProjectDetails> response;
            if (employeeId == 0)
            {
                return response = new ServiceListResponse<ProjectDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }

            // int projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower().Contains("talent pool")).Select(x => x.ProjectTypeId).FirstOrDefault();

            var practiceArea = m_OrgService.GetAllPracticeArea(true).Result.Items;

            var obj = await (from p in m_ProjectContext.Projects
                             join aut in m_ProjectContext.AssociateAllocation on p.ProjectId equals aut.ProjectId
                             where aut.EmployeeId == employeeId && aut.IsActive == true /*&& p.ProjectTypeId != projectTypeId*/
                             select new ProjectDetails
                             {
                                 ProjectId = p.ProjectId,
                                 ProjectCode = p.ProjectCode,
                                 ProjectName = p.ProjectName,
                                 PracticeAreaId = p.PracticeAreaId,
                                 AllocationPercentage = aut.AllocationPercentage,
                                 EffectiveDate = aut.EffectiveDate
                             }).Distinct().ToListAsync();

            var project = (from p in obj
                           join pt in practiceArea on p.PracticeAreaId equals pt.PracticeAreaId into pa
                           from PracticeArea in pa.DefaultIfEmpty()
                           select new ProjectDetails
                           {
                               ProjectId = p.ProjectId,
                               ProjectCode = p.ProjectCode,
                               ProjectName = p.ProjectName,
                               PracticeAreaCode = PracticeArea.PracticeAreaCode,
                               AllocationPercentage = p.AllocationPercentage,
                               EffectiveDate = p.EffectiveDate
                           }).Distinct().OrderBy(x => x.ProjectName).ToList();

            if (obj == null)
            {
                return response = new ServiceListResponse<ProjectDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Project found with this Id.."
                };
            }
            else
            {
                return response = new ServiceListResponse<ProjectDetails>()
                {
                    Items = project,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
        }

        #endregion

        #region GetProjectsByIds
        /// <summary>
        /// Get the projects by Ids
        /// </summary>
        /// <param name="projectIds">project Ids</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<Entities.Project>> GetProjectsByIds(string projectIds)
        {
            ServiceListResponse<Entities.Project> response = new ServiceListResponse<Entities.Project>();

            List<int> ids = projectIds.Split(",".ToCharArray()).Select(Int32.Parse).ToList();
            response.Items = await m_ProjectContext.Projects.Where(p => ids.Contains(p.ProjectId)).ToListAsync();

            if (response.Items == null || response.Items.Count == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "No Projects found..";
            }
            else
            {
                response.IsSuccessful = true;
                response.Message = "";
            }

            return response;
        }
        #endregion
    }
}
