using AutoMapper;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class AssociateAllocationService : IAssociateAllocationService
    {
        #region Global Varibles
        private readonly ILogger<AssociateAllocationService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private readonly IHttpClientFactory m_clientFactory;
        private readonly APIEndPoints m_apiEndPoints;
        private readonly IEmployeeService m_EmployeeService;
        private readonly IOrganizationService m_OrgService;
        private readonly IConfiguration _configuration;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;
        private readonly EmailConfigurations m_EmailConfigurations;
        #endregion

        #region AssociateAllocationService
        public AssociateAllocationService(ILogger<AssociateAllocationService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IEmployeeService employeeService,
            IOrganizationService orgService,
            IConfiguration configuration,
            IOptions<MiscellaneousSettings> miscellaneousSettings,
            IOptions<EmailConfigurations> emailConfiguration)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssociateAllocationDetails, AssociateAllocation>();
                cfg.CreateMap<AssociateFutureProjectAllocationDetails, AssociateFutureProjectAllocation>();
            });
            m_mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            //m_apiEndPoints = apiEndPoints.Value;
            m_apiEndPoints = apiEndPoints != null ? apiEndPoints.Value : null;
            m_EmployeeService = employeeService;
            m_OrgService = orgService;
            _configuration = configuration;
            m_MiscellaneousSettings = miscellaneousSettings?.Value;
            m_EmailConfigurations = emailConfiguration?.Value;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the AssociateAllocation
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateAllocation>> GetAll()
        {
            ServiceListResponse<AssociateAllocation> response = new ServiceListResponse<AssociateAllocation>();
            var obj = await m_ProjectContext.AssociateAllocation.ToListAsync();
            if (obj == null || obj.Count == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "No Associate Allocations found.";
            }
            else
            {
                response.Items = obj;
                response.IsSuccessful = true;
                response.Message = "";
            }
            return response;
        }

        #endregion

        #region GetById
        /// <summary>
        /// Get AssociateAllocation by id
        /// </summary>
        /// <param name="id">AssociateAllocation Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateAllocation>> GetById(int id)
        {
            ServiceResponse<AssociateAllocation> response = new ServiceResponse<AssociateAllocation>();
            if (id == 0)
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = "Invalid Request..";
            }
            else
            {
                var obj = await m_ProjectContext.AssociateAllocation.FindAsync(id);
                if (obj == null)
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "No Associate Allocatins found";
                }
                else
                {
                    response.Item = obj;
                    response.IsSuccessful = true;
                    response.Message = "";
                }
            }
            return response;
        }

        #endregion

        #region GetByProjectId
        /// <summary>
        /// Get AssociateAllocation by id
        /// </summary>
        /// <param name="projectId">AssociateAllocation Id</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateAllocation>> GetByProjectId(int projectId)
        {
            ServiceListResponse<AssociateAllocation> response = new ServiceListResponse<AssociateAllocation>();
            if (projectId == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Invalid Request..";
            }
            else
            {
                var obj = await m_ProjectContext.AssociateAllocation.Where(aa => aa.ProjectId == projectId).ToListAsync();
                if (obj == null || obj.Count == 0)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "No Associate Allocatins found for this project.";
                }
                else
                {
                    response.Items = obj;
                    response.IsSuccessful = true;
                    response.Message = "";
                }
            }
            return response;
        }

        #endregion

        #region GetByClientBillingRoleId
        /// <summary>
        /// Get AssociateAllocation by id
        /// </summary>
        /// <param name="clientBillingRoleId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateAllocation>> GetByClientBillingRoleId(int clientBillingRoleId)
        {
            ServiceListResponse<AssociateAllocation> response = new ServiceListResponse<AssociateAllocation>();
            if (clientBillingRoleId == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "Invalid Request..";
            }
            else
            {
                var obj = await m_ProjectContext.AssociateAllocation
                                         .Where(aa => aa.ClientBillingRoleId == clientBillingRoleId
                                                    && aa.ReleaseDate == null).ToListAsync();
                if (obj == null || obj.Count == 0)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "No Associate Allocations found with this client billing role.";
                }
                else
                {
                    response.Items = obj;
                    response.IsSuccessful = true;
                    response.Message = "";
                }
            }
            return response;
        }

        #endregion

        #region GetBillableResouceInfoByProjectId
        /// <summary>
        /// This method gets resource allocation for billable resources
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>ResourceAllocationResponse</returns>
        public async Task<ServiceListResponse<ResourceAllocation>> GetBillableResouceInfoByProjectId(int projectId)
        {
            ServiceListResponse<ResourceAllocation> response;// = new ResourceAllocationResponse();
            m_Logger.LogInformation("Calling \"GetBillableResouceInfoByProjectId\" method in ClientBillingRoleService.");

            m_Logger.LogInformation($"Getting billable resources for project Id:{projectId}");

            var associateAllocations =
                await m_ProjectContext.AssociateAllocation.Where(aa => aa.ProjectId == projectId
                                                                       && aa.ReleaseDate == null
                                                                       && aa.IsActive == true
                                                                       && aa.IsBillable == true
                                                                ).ToListAsync();

            if (associateAllocations == null || associateAllocations.Count == 0)
            {
                m_Logger.LogInformation("No billable resources found for this project");
                response = new ServiceListResponse<ResourceAllocation>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No billable resources found for this project"
                };
                return response;
            }

            List<int> employeeIds =
                associateAllocations.Where(aa => aa.EmployeeId.HasValue).Select(aa => aa.EmployeeId.Value).Distinct().ToList();

            var employees = await m_EmployeeService.GetEmployeesByIds(employeeIds);

            if (employees == null || employees.Items.Count == 0)
            {
                m_Logger.LogInformation("No Employees found with these Employee Ids:" + employeeIds);
                response = new ServiceListResponse<ResourceAllocation>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Employees found with these Employee Ids:" + employeeIds
                };
                return response;
            }


            m_Logger.LogInformation($"Adding record to ClientBillingRoleModels.");

            var clientBillingRoleModels =
                 (from aa in associateAllocations
                  join emp in employees.Items
                    on aa.EmployeeId equals emp.EmployeeId
                  join ap in m_ProjectContext.AllocationPercentage
                    on aa.AllocationPercentage equals ap.AllocationPercentageId
                  join cbr in m_ProjectContext.ClientBillingRoles
                    on aa.ClientBillingRoleId equals cbr.ClientBillingRoleId into aacbr
                  from clientBillingRole in aacbr.DefaultIfEmpty()
                  where emp.IsActive == true
                  select new ResourceAllocation
                  {
                      AssociateCode = emp.EmployeeCode,
                      AssociateName = !string.IsNullOrWhiteSpace(emp.MiddleName) ?
                                            $"{emp.FirstName} {emp.MiddleName} {emp.LastName}" :
                                            $"{emp.FirstName} {emp.LastName}",
                      AllocationPercentage = ap.Percentage,
                      ClientBillingRoleName = clientBillingRole.ClientBillingRoleName,
                      ClientBillingRoleId = clientBillingRole.ClientBillingRoleId,
                      IsPrimaryProject = aa.IsPrimary.HasValue ? aa.IsPrimary.Value.ToString() : "false",
                      IsCriticalResource = aa.IsCritical.HasValue ? aa.IsCritical.Value.ToString() : "false",
                  }).OrderBy(cbrm => cbrm.AssociateName).ToList();

            m_Logger.LogInformation($"Adding record to ClientBillingRoleModels.");

            if (clientBillingRoleModels == null && clientBillingRoleModels.Count() == 0)
            {
                m_Logger.LogInformation("No Billable resources found for this project");
                response = new ServiceListResponse<ResourceAllocation>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Billable resources found for this project"
                };
                return response;
            }
            else
            {
                response = new ServiceListResponse<ResourceAllocation>()
                {
                    Items = clientBillingRoleModels,
                    IsSuccessful = true,
                    Message = ""
                };
                return response;
            }
        }

        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Get Associate Allocations By EmployeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateAllocation>> GetByEmployeeId(int employeeId)
        {
            ServiceListResponse<AssociateAllocation> response = new ServiceListResponse<AssociateAllocation>();
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
                    var allocation = await m_ProjectContext.AssociateAllocation.Where(aa => aa.EmployeeId == employeeId && aa.IsActive == true).ToListAsync();

                    if (allocation == null || allocation.Count == 0)
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "No Associate Allocatins found for this employee.";
                    }
                    else
                    {
                        response.Items = allocation;
                        response.IsSuccessful = true;
                        response.Message = "";
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in Associate Allocation Service - GetByEmployeeId method.");
                throw ex;
            }
        }

        #endregion

        #region GetAllAllocationByEmployeeId
        /// <summary>
        /// Get All Allocation ByEmployeeId 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateAllocationHistory>> GetAllAllocationByEmployeeId(int employeeId)
        {
            ServiceListResponse<AssociateAllocationHistory> response = new ServiceListResponse<AssociateAllocationHistory>();
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

                    var clients = m_OrgService.GetClients().Result;
                    var roles = m_OrgService.GetRolesByDepartmentId(1).Result;
                    var talentpoolProject = m_OrgService.GetAllProjectTypes(true).Result.Items.Where(x => x.ProjectTypeCode.ToLower() == "Talent Pool".ToLower()).FirstOrDefault();
                    var projectdetails = await (from all in m_ProjectContext.AssociateAllocation
                                          join pro in m_ProjectContext.Projects on all.ProjectId equals pro.ProjectId                                          
                                          join pers in m_ProjectContext.AllocationPercentage on all.AllocationPercentage equals pers.AllocationPercentageId
                                          where all.EmployeeId==employeeId && !(pro.ProjectTypeId== talentpoolProject.ProjectTypeId && all.ReleaseDate!=null && all.IsActive==false)
                                          select new AssociateAllocationHistory
                                          {
                                              ProjectId = all.ProjectId??0,
                                              ProjectName = pro.ProjectName,
                                              ClientId=pro.ClientId,
                                              MasterRoleId=all.RoleMasterId??0,
                                              AllocationPercentage = pers.Percentage,
                                              Critical = all.IsCritical==true?"Yes":"No",
                                              ReportingManagerId = all.ReportingManagerId??0,
                                              LeadId = all.LeadId??0,
                                              EffectiveDate = string.Format("{0:dd-MM-yyyy}", all.EffectiveDate),
                                              ReleaseDate= string.Format("{0:dd-MM-yyyy}", all.ReleaseDate),
                                              Billable=all.IsBillable==true?"Yes":"No",
                                              IsActive=all.IsActive
                                          }).ToListAsync();

                    var RMS = m_EmployeeService.GetEmployeesByIds(projectdetails.Select(x => x.ReportingManagerId).ToList()).Result;
                    var Leads = m_EmployeeService.GetEmployeesByIds(projectdetails.Select(x => x.LeadId).ToList()).Result;
                    List<AssociateAllocationHistory> associateAllocationHistory = (from pr in projectdetails
                                join cl in clients.Items on pr.ClientId equals cl.ClientId into ClienDta
                                from client in ClienDta.DefaultIfEmpty()
                                join rl in roles.Items on pr.MasterRoleId equals rl.RoleMasterId into roleDta
                                from role in roleDta.DefaultIfEmpty()
                                join rm in RMS.Items on pr.ReportingManagerId equals rm.EmpId into RMSData
                                from rms in RMSData.DefaultIfEmpty()
                                join ld in Leads.Items on pr.LeadId equals ld.EmpId into LeadData
                                from lead in LeadData.DefaultIfEmpty()
                                select new AssociateAllocationHistory
                                {
                                    ProjectId = pr.ProjectId,
                                    ProjectName = pr.ProjectName,
                                    ClientId = pr.ClientId,
                                    ClientName = client != null ? client.ClientName : null,
                                    MasterRoleId = pr.MasterRoleId,
                                    RoleName = role != null ? role.RoleDescription : null,
                                    AllocationPercentage = pr.AllocationPercentage,
                                    Critical = pr.Critical,
                                    ReportingManager =rms!=null? rms.EmpName:null,
                                    Lead =lead!=null? lead.EmpName:null,
                                    EffectiveDate = pr.EffectiveDate,
                                    ReleaseDate = pr.ReleaseDate,
                                    Billable = pr.Billable,
                                    IsActive=pr.IsActive
                                }).OrderBy(x=>x.IsActive==true). ToList();
                              

                    if (associateAllocationHistory == null || associateAllocationHistory.Count == 0)
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "No Associate Allocatins found for this employee.";
                    }
                    else
                    {
                        response.Items = associateAllocationHistory;
                        response.IsSuccessful = true;
                        response.Message = "";
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in Associate Allocation Service - GetAllAllocationByEmployeeId method.");
                throw ex;
            }
        }

        #endregion

        #region GetByLeadId
        /// <summary>
        /// Get Associate Allocations By LeadId
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateAllocation>> GetByLeadId(int leadId)
        {
            ServiceListResponse<AssociateAllocation> response = new ServiceListResponse<AssociateAllocation>();
            try
            {
                if (leadId == 0)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Invalid Request..";
                }
                else
                {
                    var allocation = await m_ProjectContext.AssociateAllocation.Where(aa => aa.LeadId == leadId && aa.IsActive == true).ToListAsync();

                    if (allocation == null || allocation.Count == 0)
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "No Associate Allocatins found for this employee.";
                    }
                    else
                    {
                        response.Items = allocation;
                        response.IsSuccessful = true;
                        response.Message = "";
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in Associate Allocation Service - GetByEmployeeId method.");
                throw ex;
            }
        }

        #endregion

        #region GetAllocationsByEmpIds
        /// <summary>
        /// Get AssociateAllocation details by list of employee ids
        /// </summary>
        /// <param name="id">AssociateAllocation Id</param>
        /// <returns></returns>
        public async Task<List<AssociateAllocation>> GetAllocationsByEmpIds(string employeeIds)
        {
            m_Logger.LogInformation("Calling \"GetAllocationsByEmpIds\" method in ProjectManagerService");
            List<AssociateAllocation> associateAllocations = new List<AssociateAllocation>();


            try
            {
                List<int> ids = employeeIds.Split(",".ToCharArray()).Select(Int32.Parse).ToList();

                associateAllocations = await m_ProjectContext.AssociateAllocation.Where(p => ids.Contains(p.EmployeeId.Value))
                                                               .ToListAsync();

            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Exception occured in \"GetAllocationsByEmpIds\" method in ProjectManagerService" + ex.Message);
                throw ex;
            }

            return associateAllocations;
        }

        #endregion

        #region GetSkillSearchAllocations
        /// <summary>
        /// Get SkillSearchAllocations
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<SkillSearchAssociateAllocation>> GetSkillSearchAllocations(string employeeIds)
        {
            ServiceListResponse<SkillSearchAssociateAllocation> response = new ServiceListResponse<SkillSearchAssociateAllocation>();

            try
            {
                List<SkillSearchAssociateAllocation> allocations = new List<SkillSearchAssociateAllocation>();

                if (!string.IsNullOrWhiteSpace(employeeIds))
                {
                    List<int> employees = new List<int>(Array.ConvertAll(employeeIds.Split(','), int.Parse));


                    allocations = await (from aa in m_ProjectContext.AssociateAllocation
                                         join pr in m_ProjectContext.Projects
                                            on aa.ProjectId equals pr.ProjectId
                                         where
                                         aa.IsActive == true && employees.Contains(aa.EmployeeId ?? 0)
                                         select new SkillSearchAssociateAllocation
                                         {
                                             ProjectId = pr.ProjectId,
                                             ProjectCode = pr.ProjectCode,
                                             ProjectName = pr.ProjectName,
                                             EmployeeId = aa.EmployeeId ?? 0,
                                             IsPrimary = aa.IsPrimary ?? false,
                                             IsBillable = aa.IsBillable ?? false,
                                             IsCritical = aa.IsCritical ?? false,
                                             RoleMasterId = aa.RoleMasterId ?? 0

                                         }).ToListAsync();

                    return response = new ServiceListResponse<SkillSearchAssociateAllocation>()
                    {
                        Items = allocations,
                        IsSuccessful = true,
                        Message = ""
                    };
                }
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<SkillSearchAssociateAllocation>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
            return response;
        }

        #endregion

        #region AllocateAssociateToTalentPool
        /// <summary>
        /// Allocate employee to talent pool based on employee competency group 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> AllocateAssociateToTalentPool(EmployeeDetails employee)
        {
            var response = new ServiceResponse<bool>();
            int isCreated = 0;
            try
            {
                m_Logger.LogInformation("AssociateAllocationService : Calling \"AllocateAssociateToTalentPool\" method.");
                AssociateAllocation existingAllocation = m_ProjectContext.AssociateAllocation.Where(e => e.EmployeeId == employee.EmployeeId && e.IsActive == true).FirstOrDefault();
                if (employee.CompetencyGroup != null && existingAllocation == null)
                {
                    var talentPool = (await GetCompetencyAreaMananagers(employee.CompetencyGroup));

                    if (talentPool != null)
                    {
                        AllocationPercentage percentage = m_ProjectContext.AllocationPercentage.Where(p => p.Percentage == 100).FirstOrDefault();

                        //check already have allocation(inactive) if exist then Effective date should be current date else if it is the first allocation then Effective date should be joining date.
                        List<AssociateAllocation> existingAssociateAllocation = m_ProjectContext.AssociateAllocation.Where(allocation => allocation.EmployeeId == employee.EmployeeId).ToList();

                        AssociateAllocation associateAllocation = new AssociateAllocation()
                        {
                            EmployeeId = employee.EmployeeId,
                            // The commented section(Trid,RoleMasterId) needs to be modified when Talent Management module is implemented
                            // For temporary purpose hard coding the values
                            Trid = 1,
                            RoleMasterId = 1,
                            /////////////////////////////////
                            ProjectId = talentPool.ProjectId,
                            IsActive = true,
                            IsBillable = false,
                            IsCritical = false,
                            IsPrimary = true,
                            AllocationPercentage = percentage.AllocationPercentageId,
                            InternalBillingPercentage = 0,
                            ClientBillingPercentage = 0,
                            ReportingManagerId = talentPool.ReportingManagerId,
                            ProgramManagerId = talentPool.ProgramManagerId,
                            LeadId = talentPool.LeadId,
                            EffectiveDate = employee.IsDepartmentChange == true ? DateTime.Now : employee.IsDepartmentChange == false && existingAssociateAllocation.Count() == 0 ? employee.JoinDate : DateTime.Now,
                            AllocationDate = DateTime.Now
                        };

                        //add aalocation
                        m_ProjectContext.AssociateAllocation.Add(associateAllocation);
                        m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in AssociateAllocationService");
                        isCreated = await m_ProjectContext.SaveChangesAsync();

                        if (isCreated > 0)
                        {
                            response.IsSuccessful = true;
                            response.Item = true;
                            m_Logger.LogInformation("Allocation created successfully");
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "Error while creationg allocation";
                            return response;
                        }
                    }
                    else
                    {
                        m_Logger.LogInformation("AssociateAllocationService : \"AllocateAssociateToTalentPool\" method - No TalentPool found with CompetencyGroup" + employee.CompetencyGroup + " found for employee id " + employee.EmployeeId);
                    }
                }
                else
                {
                    m_Logger.LogInformation("AssociateAllocationService : \"AllocateAssociateToTalentPool\" method - Competency not found for employee id " + employee.EmployeeId);
                }
                response.IsSuccessful = true;
                response.Item = true;
            }
            catch (Exception ex)
            {
                response.Message = "Error occurred in \"AllocateAssociateToTalentPool\" of AssociateAllocationService";
                response.IsSuccessful = false;
                m_Logger.LogError("Error occurred in \"AllocateAssociateToTalentPool\" of AssociateAllocationService" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetEmployeesForAllocations
        /// <summary>
        /// Get Employees ForAllocations
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeesForAllocations()
        {
            //usp_GetEmployeesForAllocations
            ServiceListResponse<EmployeeDetails> response = new ServiceListResponse<EmployeeDetails>();
            var employees = await m_EmployeeService.GetAll(true);
            int projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower().Contains("talent pool")).Select(x => x.ProjectTypeId).FirstOrDefault();
            var allocationsList = await (from al in m_ProjectContext.AssociateAllocation
                                             //join e in employees.Items on al.EmployeeId equals e.EmployeeId
                                         join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                         where al.IsActive == true && p.ProjectTypeId == projectTypeId
                                         select al).ToListAsync();
            var empList = (from al in allocationsList
                           join e in employees.Items on al.EmployeeId equals e.EmployeeId
                           where al.IsActive == true && e.IsActive == true
                           select new EmployeeDetails
                           {
                               EmployeeId = e.EmployeeId,
                               EmployeeName = e.FirstName + " " + e.LastName
                           }).Distinct().OrderBy(x => x.EmployeeName).ToList();
            if (empList == null || empList.Count == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "No Associate Allocations found.";
            }
            else
            {
                response.Items = empList;
                response.IsSuccessful = true;
                response.Message = "";
            }
            return response;
        }

        #endregion

        #region GetAssociatesForAllocation
        /// <summary>
        /// Get Associates For Allocation
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateAllocationData>> GetAssociatesForAllocation()
        {
            ServiceListResponse<AssociateAllocationData> response = new ServiceListResponse<AssociateAllocationData>();
            //   var employees = await m_EmployeeService.GetAll(true);
            int projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower().Contains("talent pool")).Select(x => x.ProjectTypeId).FirstOrDefault();
            var allocationsList = await (from al in m_ProjectContext.AssociateAllocation
                                             //join e in employees.Items on al.EmployeeId equals e.EmployeeId
                                         join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                         where al.IsActive == true && p.ProjectTypeId == projectTypeId
                                         select new AssociateAllocationData
                                         {
                                             EmployeeId = al.EmployeeId.Value,
                                             EmployeeName = ""
                                         }).Distinct().OrderBy(x => x.EmployeeId).ToListAsync();

            List<int> employeeIdList = allocationsList.Select(c => c.EmployeeId).Distinct().ToList();
            var employees = await m_EmployeeService.GetEmployeesByIds(employeeIdList);

            var empList = (from al in allocationsList
                           join e in employees.Items on al.EmployeeId equals e.EmpId
                           where e.IsActive == true
                           select new AssociateAllocationData
                           {
                               EmployeeId = e.EmpId,
                               EmployeeName = e.EmpName
                           }).Distinct().OrderBy(x => x.EmployeeName).ToList();
            if (empList == null || empList.Count == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "No Associate Allocations found.";
            }
            else
            {
                response.Items = empList;
                response.IsSuccessful = true;
                response.Message = "";
            }
            return response;
        }

        #endregion

        #region GetEmpAllocationHistory
        /// <summary>
        /// Get Associate Allocations By EmployeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateAllocationDetails>> GetEmpAllocationHistory(int employeeId)
        {
            ServiceListResponse<AssociateAllocationDetails> response = new ServiceListResponse<AssociateAllocationDetails>();
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
                    var employee = await m_EmployeeService.GetEmployeeById(employeeId);
                    var allocationHistory = await (from al in m_ProjectContext.AssociateAllocation
                                                   join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                                   join ap in m_ProjectContext.AllocationPercentage on al.AllocationPercentage equals ap.AllocationPercentageId
                                                   where al.EmployeeId == employeeId && al.IsActive == true
                                                   select new AssociateAllocationDetails
                                                   {
                                                       EmployeeId = al.EmployeeId,
                                                       ProjectId = (int)al.ProjectId,
                                                       Project = p.ProjectName,
                                                       ProjectTypeId = p.ProjectTypeId,
                                                       AllocationPercentage = (int)ap.Percentage,
                                                       Billable = al.IsBillable == null ? false : al.IsBillable,
                                                       isCritical = al.IsCritical == null ? false : al.IsCritical,
                                                       IsPrimary = al.IsPrimary == null ? false : al.IsPrimary,
                                                       EffectiveDate = al.EffectiveDate,
                                                       AssociateName = employee == null ? string.Empty : employee.Item.FirstName + " " + employee.Item.LastName,
                                                       ClientBillingPercentage = al.ClientBillingPercentage,
                                                       ClientBillingRoleId = al.ClientBillingRoleId
                                                   }).OrderBy(i => i.EffectiveDate).ThenBy(i => i.Project).ToListAsync();

                    if (allocationHistory == null || allocationHistory.Count == 0)
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "No Associate Allocatins found for this employee.";
                    }
                    else
                    {
                        response.Items = allocationHistory;
                        response.IsSuccessful = true;
                        response.Message = "";
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in Associate Allocation Service - GetEmpAllocationHistory method.");
                throw ex;
            }
        }

        #endregion

        #region GetEmployeePrimaryAllocationProject
        /// <summary>
        /// Get Employee Primary AllocationProject
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateAllocationDetails>> GetEmployeePrimaryAllocationProject(int employeeId)
        {
            //usp_GetEmployeePrimaryAllocationProject
            var response = new ServiceResponse<AssociateAllocationDetails>();
            try
            {
                if (employeeId == 0)
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "Invalid Request..";
                }
                else
                {
                    var projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower() == "talent pool").Select(x => x.ProjectTypeId).FirstOrDefault();

                    var allocation = await (from al in m_ProjectContext.AssociateAllocation
                                            join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                            join ap in m_ProjectContext.AllocationPercentage on al.AllocationPercentage equals ap.AllocationPercentageId
                                            where (al.EmployeeId == employeeId && al.IsPrimary == true && al.IsActive == true && p.ProjectTypeId != projectTypeId)
                                            select new AssociateAllocationDetails
                                            {
                                                AssociateAllocationId = al.AssociateAllocationId,
                                                EmployeeId = al.EmployeeId,
                                                ProjectId = al.ProjectId ?? 0,
                                                Project = p.ProjectName,
                                                AllocationPercentage = (int?)ap.Percentage,
                                                IsPrimary = al.IsPrimary == null ? false : al.IsPrimary,
                                                RoleMasterId = al.RoleMasterId,
                                            }).FirstOrDefaultAsync();
                    if (allocation != null)
                    {
                        allocation.RoleName = m_OrgService.GetRoleMasterNames().Result.Items.Where(rm => rm.RoleMasterId == allocation.RoleMasterId).FirstOrDefault().RoleName;
                    }
                    if (allocation == null)
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "No primary allocation found in associate allocation table.";
                    }
                    else
                    {
                        response.Item = allocation;
                        response.IsSuccessful = true;
                        response.Message = "";
                    }

                }

            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured while fetching primary projects from Associate Allocation Service - GetEmpAllocationHistory method.", ex.StackTrace);
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching primary projects from Associate Allocation";
            }
            return response;
        }

        #endregion

        #region ResourceAllocate
        /// <summary>
        /// Code to allocate a associate to project
        /// </summary>
        /// <param name="allocationIn"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> Create(AssociateAllocationDetails allocationIn)
        {
            //usp_AllocateAssociate
            ServiceResponse<int> response;
            using (var dbContext = await m_ProjectContext.Database.BeginTransactionAsync())
            {
                try
                {
                    bool isCreated = false, isUpdated = false;
                    m_Logger.LogInformation("Calling \"Create\" method in AssociateAllocationService");
                    AssociateAllocation alreadyAllocated = await m_ProjectContext.AssociateAllocation.Where(aa => aa.EmployeeId == allocationIn.EmployeeId && aa.ProjectId == allocationIn.ProjectId && aa.IsActive == true).FirstOrDefaultAsync();
                    if (alreadyAllocated != null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "The Associate has already allocated to the selected project."
                        };
                    }

                    var totalClientBillingPercentage = await m_ProjectContext.AssociateAllocation
                           .Where(aa => aa.EmployeeId == allocationIn.EmployeeId && aa.IsBillable == true && aa.IsActive == true)
                           .SumAsync(x => x.ClientBillingPercentage);
                    if (totalClientBillingPercentage + (allocationIn.ClientBillingPercentage ?? 0) > 100)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "The Client billing is greater than 100%."
                        };
                    }
                    // if the future project is same as the new allocation project then make the future project allocation as inactive.
                    var futureAllocations = m_ProjectContext.AssociateFutureProjectAllocation.Where(x => (x.TentativeDate.Date >= DateTime.Now.Date || x.TentativeDate.Date >= Convert.ToDateTime(allocationIn.EffectiveDate).Date) && x.ProjectId == allocationIn.ProjectId && x.IsActive==true).FirstOrDefault();
                    if (futureAllocations != null)
                    {
                        futureAllocations.IsActive = false;
                        await m_ProjectContext.SaveChangesAsync();
                    }
                        var projectDetails = await m_ProjectContext.Projects.FindAsync(allocationIn.ProjectId);
                    var zeroallocationpercentage = await m_ProjectContext.AllocationPercentage.Where(percentage => percentage.Percentage == 0).Select(percentage => percentage.AllocationPercentageId).FirstOrDefaultAsync();
                    int projectStateId = (await m_OrgService.GetStatusByCategoryAndStatusCode("PPC", "Closed")).Item.StatusId;

                    //changing project state created to execution state
                    AssociateAllocation allocationStateId = await m_ProjectContext.AssociateAllocation.Where(x => x.ProjectId == allocationIn.ProjectId).FirstOrDefaultAsync();
                    if (allocationStateId == null)
                    {
                        int createdStateId = (await m_OrgService.GetStatusByCategoryAndStatusCode("PPC", "Created")).Item.StatusId;
                        if (projectDetails.ProjectStateId == createdStateId)
                        {
                            int executeStateId = (await m_OrgService.GetStatusByCategoryAndStatusCode("PPC", "Execution")).Item.StatusId;
                            projectDetails.ProjectStateId = executeStateId;
                            await m_ProjectContext.SaveChangesAsync();
                        }
                    }

                    var projectManagers = await m_ProjectContext.ProjectManagers.Where(x => x.ProjectId == allocationIn.ProjectId && x.IsActive == true).FirstOrDefaultAsync();
                    int projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower().Contains("talent pool")).Select(x => x.ProjectTypeId).FirstOrDefault();

                    if (projectManagers == null | projectTypeId == 0 | allocationIn.EmployeeId == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Project Manager or ProjectTypeId doesn't exists or Mandatory fields are required."
                        };
                    }

                    //Check user has talentpool project or not
                    var user_in_talentpool = await (from al in m_ProjectContext.AssociateAllocation
                                                    join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                                    where al.EmployeeId == allocationIn.EmployeeId && p.ProjectTypeId == projectTypeId &&
                                                    al.IsActive == true && p.ProjectStateId != projectStateId
                                                    select al).FirstOrDefaultAsync();

                    if (user_in_talentpool != null)
                    {
                        //when primary project already available 50% and if try to add another project as primary with 50% then it should show message ->"Associate already have a primary project allocated"
                        if (allocationIn.IsPrimary == true)
                        {
                            var alreadyAllocatedProject = m_ProjectContext.AssociateAllocation.Where(allocation => allocation.EmployeeId == allocationIn.EmployeeId && allocation.IsActive == true && allocation.IsPrimary == true
                            && allocation.ProjectId != user_in_talentpool.ProjectId).FirstOrDefault();

                            if (alreadyAllocatedProject != null && alreadyAllocatedProject.AllocationPercentage == allocationIn.AllocationPercentage)
                            {
                                return response = new ServiceResponse<int>()
                                {
                                    IsSuccessful = false,
                                    Message = "The Associate has already allocated a primary project."
                                };
                            }
                        }

                        if (user_in_talentpool.EffectiveDate.Value.Date == allocationIn.EffectiveDate.Value.Date && allocationIn.AllocationPercentage != zeroallocationpercentage)
                        {
                            m_ProjectContext.AssociateAllocation.Remove(user_in_talentpool);
                        }
                        else
                        {
                            if (allocationIn.AllocationPercentage != zeroallocationpercentage)
                            {
                                var allocationData = await (from al in m_ProjectContext.AssociateAllocation
                                                            join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                                            join pm in m_ProjectContext.ProjectManagers on p.ProjectId equals pm.ProjectId
                                                            where al.EmployeeId == allocationIn.EmployeeId && p.ProjectTypeId == projectTypeId &&
                                                            al.IsActive == true && pm.IsActive == true && p.ProjectStateId != projectStateId
                                                            select new
                                                            {
                                                                al.AssociateAllocationId,
                                                                PoolEffectiveDate = al.EffectiveDate,
                                                                TalentPoolReportingManagerId = pm.ReportingManagerId,
                                                                TalentPoolProjectID = p.ProjectId
                                                            }).FirstOrDefaultAsync();

                                var associateAllocation = await m_ProjectContext.AssociateAllocation.FindAsync(allocationData.AssociateAllocationId);
                                associateAllocation.ReleaseDate = (allocationData.PoolEffectiveDate == allocationIn.EffectiveDate) ? allocationIn.EffectiveDate : allocationIn.EffectiveDate.Value.AddDays(-1);
                                associateAllocation.IsActive = false;
                            }
                        }

                        await m_ProjectContext.SaveChangesAsync();
                    }
                    //Check associate already allocated to same project
                    var isProject_allocated = await (from al in m_ProjectContext.AssociateAllocation
                                                     where al.EmployeeId == allocationIn.EmployeeId && al.ProjectId == allocationIn.ProjectId &&
                                                     al.RoleMasterId == allocationIn.RoleMasterId && al.IsActive == true
                                                     select al).FirstOrDefaultAsync();

                    if (isProject_allocated != null)
                    {
                        var associateAllocation = await m_ProjectContext.AssociateAllocation.FindAsync(isProject_allocated.AssociateAllocationId);
                        associateAllocation.ReleaseDate = (isProject_allocated.EffectiveDate == allocationIn.EffectiveDate) ? allocationIn.EffectiveDate : allocationIn.EffectiveDate.Value.AddDays(-1);
                        associateAllocation.IsActive = false;
                        isUpdated = await m_ProjectContext.SaveChangesAsync() > 0;
                    }

                    if (!isUpdated)
                    {
                        AssociateAllocation associateAllocation = new AssociateAllocation();
                        associateAllocation = m_mapper.Map<AssociateAllocationDetails, AssociateAllocation>(allocationIn);
                        associateAllocation.Trid = allocationIn.TalentRequisitionId;
                        associateAllocation.AllocationDate = DateTime.Now;
                        associateAllocation.IsBillable = allocationIn.Billable;
                        associateAllocation.IsCritical = allocationIn.isCritical;
                        associateAllocation.ProgramManagerId = projectManagers.ProgramManagerId;
                        associateAllocation.LeadId = projectManagers.LeadId;
                        associateAllocation.IsActive = true;
                        await m_ProjectContext.AssociateAllocation.AddAsync(associateAllocation);
                        isCreated = await m_ProjectContext.SaveChangesAsync() > 0;

                        var percentage = await (from q in (from al in m_ProjectContext.AssociateAllocation
                                                           join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                                           join ap in m_ProjectContext.AllocationPercentage on al.AllocationPercentage equals ap.AllocationPercentageId
                                                           where al.EmployeeId == allocationIn.EmployeeId &&
                                                           p.ProjectTypeId != projectTypeId && al.IsActive == true
                                                           select new
                                                           {
                                                               al.EmployeeId,
                                                               ap.Percentage
                                                           })
                                                group q by q.EmployeeId into g
                                                select new
                                                {
                                                    TotalUtilizationpercentage = g.Sum(x => x.Percentage)
                                                }).FirstOrDefaultAsync();

                        int TotalUtilizationpercentage = percentage == null ? 0 : Convert.ToInt32(percentage.TotalUtilizationpercentage);
                        ProjectManager talent_pool_projectManagerDetails = new ProjectManager();

                        if (user_in_talentpool != null)
                        {
                            talent_pool_projectManagerDetails = await m_ProjectContext.ProjectManagers.Where(project => project.ProjectId == user_in_talentpool.ProjectId && project.IsActive == true).FirstOrDefaultAsync();
                        }

                        if (TotalUtilizationpercentage < 100 && allocationIn.AllocationPercentage != zeroallocationpercentage)
                        {
                            var allocationPercentageId = await m_ProjectContext.AllocationPercentage.Where(x => x.Percentage == (100 - TotalUtilizationpercentage)).Select(x => x.AllocationPercentageId).FirstOrDefaultAsync();
                            AssociateAllocation associateAllocation1 = new AssociateAllocation();
                            associateAllocation1 = m_mapper.Map<AssociateAllocationDetails, AssociateAllocation>(allocationIn);
                            associateAllocation1.ProjectId = user_in_talentpool?.ProjectId;
                            associateAllocation1.AllocationPercentage = allocationPercentageId;
                            associateAllocation1.Trid = allocationIn.TalentRequisitionId;
                            associateAllocation.InternalBillingPercentage = 0;
                            associateAllocation1.ClientBillingPercentage = 0;
                            associateAllocation1.IsCritical = false;
                            associateAllocation1.IsPrimary = false;
                            associateAllocation1.AllocationDate = DateTime.Now;
                            associateAllocation1.IsActive = true;
                            associateAllocation1.ReportingManagerId = user_in_talentpool?.ReportingManagerId;
                            associateAllocation1.IsBillable = false;
                            associateAllocation1.ProgramManagerId = talent_pool_projectManagerDetails.ProgramManagerId;
                            associateAllocation1.LeadId = talent_pool_projectManagerDetails.LeadId;
                            await m_ProjectContext.AssociateAllocation.AddAsync(associateAllocation1);
                            isCreated = await m_ProjectContext.SaveChangesAsync() > 0;
                        }
                    }

                    if (allocationIn.IsPrimary.HasValue && allocationIn.IsPrimary.Value)
                    {
                        var config = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<AssociateAllocation, AssociateAllocation>();
                        });
                        var mapper = config.CreateMapper();
                        var allocationList = await m_ProjectContext.AssociateAllocation.Where(x => x.EmployeeId == allocationIn.EmployeeId && x.ProjectId != allocationIn.ProjectId &&
                                                           x.IsPrimary == true && x.IsActive == true).ToListAsync();
                        List<AssociateAllocation> associateAllocations = new List<AssociateAllocation>();
                        foreach (var lst in allocationList)
                        {
                            var associateAllocation = new AssociateAllocation();
                            associateAllocation = mapper.Map<AssociateAllocation, AssociateAllocation>(lst);
                            lst.IsActive = false;
                            lst.ReleaseDate = (lst.EffectiveDate == allocationIn.EffectiveDate) ? allocationIn.EffectiveDate : allocationIn.EffectiveDate.Value.AddDays(-1);
                            associateAllocation.AssociateAllocationId = 0;
                            associateAllocation.IsPrimary = false;
                            associateAllocation.EffectiveDate = allocationIn.EffectiveDate;
                            associateAllocation.AllocationDate = DateTime.Now;
                            associateAllocation.CreatedBy = null;
                            associateAllocation.CreatedDate = null;
                            associateAllocations.Add(associateAllocation);
                        }

                        await m_ProjectContext.AssociateAllocation.AddRangeAsync(associateAllocations);
                        int result = await m_ProjectContext.SaveChangesAsync();

                        if (result > 0 && allocationList.Count != 0)
                        {
                            var associateDetails = m_EmployeeService.GetEmployeeById((int)allocationIn.EmployeeId).Result.Item;
                            string associateName = associateDetails.FirstName + " " + associateDetails.LastName;
                            var new_associateProject = m_ProjectContext.Projects.Where(project => project.ProjectId == allocationIn.ProjectId).Select(project => project.ProjectName).FirstOrDefault();
                            var projectManagerDetails = getProjectAndManagerDetails(allocationList);
                            SendNotificationToPMForAllocatonChangePrimaryProject(associateName, new_associateProject, projectManagerDetails);

                        }
                    }

                    //Update Reporting Manager in Employee
                    int reportingManagerId = await m_ProjectContext.AssociateAllocation.Where(x => x.EmployeeId == allocationIn.EmployeeId && x.IsPrimary == true && x.IsActive == true).Select(x => x.ReportingManagerId).FirstOrDefaultAsync() ?? 0;
                    await m_EmployeeService.UpdateReportingManagerId(allocationIn.EmployeeId.Value, reportingManagerId);

                    if (isCreated || isUpdated)
                    {
                        if (allocationIn.NotifyAll == true)
                        {
                            int allocatedProjects = await (from aa in m_ProjectContext.AssociateAllocation
                                                           where aa.EmployeeId == allocationIn.EmployeeId && aa.IsActive == true
                                                           select aa).CountAsync();
                            var projectManager = await m_ProjectContext.ProjectManagers.Where(pm => pm.ProjectId == allocationIn.ProjectId && pm.IsActive == true).FirstOrDefaultAsync();

                            if (allocatedProjects > 1)
                            {
                                int? primaryreportingMangerId = await m_ProjectContext.AssociateAllocation.AsNoTracking().Where(i => i.EmployeeId == allocationIn.EmployeeId && i.IsActive == true && i.IsPrimary == true).Select(r => r.ReportingManagerId).FirstOrDefaultAsync();
                                if (primaryreportingMangerId != null)
                                {
                                    int? reportingMangerId = await m_ProjectContext.AssociateAllocation.Where(i => i.EmployeeId == allocationIn.EmployeeId && i.IsActive == true && i.IsPrimary != true).Select(r => r.ReportingManagerId).FirstOrDefaultAsync();
                                    await SendAllocationNotification(allocationIn.EmployeeId, reportingMangerId ?? 0, primaryreportingMangerId ?? 0, String.Format("{0:MMM dd, yyyy}", allocationIn.EffectiveDate), projectDetails, projectManager);
                                }
                                else
                                    await SendAllocationNotification(allocationIn.EmployeeId, allocationIn.ReportingManagerId ?? 0, allocationIn.ReportingManagerId ?? 0, String.Format("{0:MMM dd, yyyy}", allocationIn.EffectiveDate), projectDetails, projectManager);
                            }
                            else
                                await SendAllocationNotification(allocationIn.EmployeeId, allocationIn.ReportingManagerId ?? 0, allocationIn.ReportingManagerId ?? 0, String.Format("{0:MMM dd, yyyy}", allocationIn.EffectiveDate), projectDetails, projectManager);
                        }

                        await dbContext.CommitAsync();

                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = true,
                            Message = string.Empty
                        };
                    }
                    else
                    {
                        m_Logger.LogError("No AssociateAllocation created.");
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "No AssociateAllocation created."
                        };
                    }
                }
                catch (Exception ex)
                {
                    await dbContext.RollbackAsync();
                    m_Logger.LogError("No AssociateAllocation created.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No AssociateAllocation created."
                    };
                }
            }
        }
        #endregion

        #region GetAssociatesToRelease
        public async Task<ServiceListResponse<Associate>> GetAssociatesToRelease(int employeeId, string roleName)
        {
            var response = new ServiceListResponse<Associate>();
            try
            {

                List<ProjectType> projectTypes = (await m_OrgService.GetAllProjectTypes(true)).Items;
                int projectTypeId = projectTypes.Where(x => x.ProjectTypeCode.ToLower() == "talent pool").Select(x => x.ProjectTypeId).FirstOrDefault();

                IQueryable<Associate> allocations = null;
                List<Associate> employees = null;
                if (roleName.ToLower() == "Program Manager".ToLower())
                {
                    allocations = (from allocation in m_ProjectContext.AssociateAllocation
                                   join project in m_ProjectContext.Projects on allocation.ProjectId equals project.ProjectId
                                   where allocation.IsActive == true && allocation.ReleaseDate == null && allocation.ProgramManagerId == employeeId
                                   && project.ProjectTypeId != projectTypeId && allocation.ProjectId.HasValue == true
                                   select new Associate
                                   {
                                       EmployeeId = (int)allocation.EmployeeId,
                                       ProjectId = project.ProjectId,
                                       EffectiveDate = allocation.EffectiveDate ?? DateTime.Now.Date
                                   }).Distinct();
                }
                else if (roleName.ToLower() == "HRM".ToLower() || roleName.ToLower() == "HRA".ToLower())
                {
                    allocations = (from allocation in m_ProjectContext.AssociateAllocation
                                   join project in m_ProjectContext.Projects on allocation.ProjectId equals project.ProjectId
                                   where allocation.IsActive == true && allocation.ReleaseDate == null && project.ProjectTypeId != projectTypeId
                                   && allocation.ProjectId.HasValue == true
                                   select new Associate
                                   {
                                       EmployeeId = (int)allocation.EmployeeId,
                                       ProjectId = project.ProjectId,
                                       EffectiveDate = allocation.EffectiveDate ?? DateTime.Now.Date
                                   }).Distinct();
                }
                else if (roleName.ToLower() == "Training Department Head".ToLower())
                {
                    List<int> proectTypes = projectTypes.Where(protype => protype.ProjectTypeCode == "Training" || protype.ProjectTypeCode == "Nipuna").Select(protype => protype.ProjectTypeId).ToList();
                    List<int> projectIds = m_ProjectContext.Projects.Where(project => proectTypes.Contains(project.ProjectTypeId ?? 0)).Select(project => project.ProjectId).ToList();
                    allocations = (from al in m_ProjectContext.AssociateAllocation
                                   where projectIds.Contains(al.ProjectId ?? 0) && al.IsActive == true
                                   select new Associate
                                   {
                                       EmployeeId = (int)al.EmployeeId,
                                       ProjectId = al.ProjectId ?? 0,
                                       EffectiveDate = al.EffectiveDate ?? DateTime.Now.Date
                                   }).Distinct();

                }
                if (allocations != null)
                {
                    var associates = m_EmployeeService.GetEmployeesByIds(allocations.Select(all => all.EmployeeId).ToList()).Result.Items.Where(employee => employee.IsActive == true)
                        .Select(employee => new Associate { EmpName = employee.EmpName, EmployeeId = employee.EmpId }).ToList();

                    var allocationList = allocations.ToList();

                    employees = (from allocation in allocationList
                                 join associate in associates on allocation.EmployeeId equals associate.EmployeeId
                                 select new Associate
                                 {
                                     EmployeeId = allocation.EmployeeId,
                                     ProjectId = allocation.ProjectId,
                                     EffectiveDate = allocation.EffectiveDate,
                                     EmpName = associate.EmpName
                                 }).OrderBy(c => c.EmpName).ToList();

                    if (employees == null || employees.Count() == 0)
                    {
                        response.IsSuccessful = false;
                        response.Message = "No data found";
                    }
                    else
                    {
                        response.IsSuccessful = true;
                        response.Items = employees;
                    }

                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No data found";
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in Associate Allocation Service - GetAssociatesToRelease method.", ex.StackTrace);
                response.Message = "Exception occured in Associate Allocation Service - GetAssociatesToRelease method.";
                response.IsSuccessful = false;
            }
            return response;
        }

        #endregion

        #region GetActiveAllocations
        public async Task<ServiceListResponse<EmployeeInfo>> GetActiveAllocations()
        {
            var response = new ServiceListResponse<EmployeeInfo>();
            try
            {
                List<EmployeeInfo> employees = new List<EmployeeInfo>();
                employees = (from allocation in m_ProjectContext.AssociateAllocation
                             join p in m_ProjectContext.Projects on allocation.ProjectId equals p.ProjectId
                             where allocation.IsActive == true && allocation.ReleaseDate == null
                             && allocation.IsPrimary == true
                             select new EmployeeInfo
                             {
                                 AssociateId = allocation.EmployeeId ?? 0,
                                 AssociateName = "",
                                 ProgramManagerId = allocation.ProgramManagerId,
                                 ProjectId = allocation.ProjectId,
                                 ProjectName = p.ProjectName,
                                 DepartmentId = 0,
                                 ReportingManagerId = allocation.ReportingManagerId ?? 0
                             }).Distinct().ToList();

                response.IsSuccessful = true;
                response.Items = employees;

                return response;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in Associate Allocation Service - GetAssociates method.", ex.StackTrace);
                response.Message = "Exception occured in Associate Allocation Service - GetAssociates method.";
                response.IsSuccessful = false;
                return response;
            }
            return response;
        }

        #endregion      

        #region TemporaryReleaseAssociate
        public async Task<BaseServiceResponse> TemporaryReleaseAssociate(AssociateAllocationDetails associateDetails)
        {
            var response = new BaseServiceResponse();
            using (var dbContext = m_ProjectContext.Database.BeginTransaction())
            {
                try
                {

                    //usp_ReleaseAssociateTemporary

                    int departmentId, practiceAreaId; int? roleMasterId = null, internalBillingRoleId = null; decimal exitingProjectAllocationPercentage = 0, exitingPoolPercentage = 0;
                    //int categoryId = (await m_OrgService.GetCategoryByName("PPC")).Item.CategoryMasterId;
                    int projectStateId = (await m_OrgService.GetStatusByCategoryAndStatusCode("PPC", "Closed")).Item.StatusId;
                    var projectManagersDetails = await m_ProjectContext.ProjectManagers.Where(x => x.ProjectId == associateDetails.ReleaseProjectId && x.IsActive == true).FirstOrDefaultAsync();
                    int projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower().Contains("training")).Select(x => x.ProjectTypeId).FirstOrDefault();
                    int zeroAllocationPercentageId = await m_ProjectContext.AllocationPercentage.Where(x => x.Percentage == 0).Select(x => x.AllocationPercentageId).FirstOrDefaultAsync();

                    var project = await m_ProjectContext.Projects.Where(x => x.ProjectId == associateDetails.ProjectId && x.ProjectTypeId == projectTypeId && x.ProjectStateId != projectStateId).FirstOrDefaultAsync();
                    if (project != null)
                    {
                        departmentId = (await m_OrgService.GetDepartmentByCode("Delivery")).Item.DepartmentId;
                        practiceAreaId = await m_ProjectContext.TalentPool.Where(x => x.ProjectId == associateDetails.ReleaseProjectId).Select(x => x.PracticeAreaId).FirstOrDefaultAsync() ?? 0;
                        var employee = new EmployeeExternal();
                        employee.CompetencyGroup = practiceAreaId;
                        employee.DepartmentId = departmentId;
                        employee.EmployeeId = associateDetails.EmployeeId ?? 0;
                        employee.External = "Associate Release";
                        var result = await m_EmployeeService.UpdateEmployee(employee);
                    }
                    var allocation = await m_ProjectContext.AssociateAllocation.Where(x => x.ProjectId == associateDetails.ProjectId && x.EmployeeId == associateDetails.EmployeeId && x.IsActive == true).FirstOrDefaultAsync();

                    //Check if the associate is in resigned state and last working date. project shoudl be release on or before the last working date.

                    var (associateExitResponse, IsLastWorkingDate) = (await GetResignedassociate((int)associateDetails.EmployeeId, (DateTime)associateDetails.ReleaseDate));

                    if (!associateExitResponse.IsSuccessful && IsLastWorkingDate == false)
                    {
                        return associateExitResponse;
                    }

                    if (allocation != null)
                    {
                        var allocatoinDetails = await (from al in m_ProjectContext.AssociateAllocation
                                                       join ap in m_ProjectContext.AllocationPercentage on al.AllocationPercentage equals ap.AllocationPercentageId
                                                       where al.ProjectId == associateDetails.ProjectId && al.EmployeeId == associateDetails.EmployeeId && al.IsActive == true
                                                       select new AssociateAllocationDetails
                                                       {
                                                           EffectiveDate = al.EffectiveDate,
                                                           RoleMasterId = al.RoleMasterId,
                                                           InternalBillingRoleId = al.InternalBillingRoleId,
                                                           Percentage = ap.Percentage, //ExitingProjectAllocationPercentage
                                                           AssociateAllocationId = al.AssociateAllocationId
                                                       }).FirstOrDefaultAsync();
                        exitingProjectAllocationPercentage = allocatoinDetails.Percentage ?? 0;
                        roleMasterId = allocatoinDetails.RoleMasterId;
                        internalBillingRoleId = allocatoinDetails.InternalBillingRoleId;
                        var associateAllocation = await m_ProjectContext.AssociateAllocation.FindAsync(allocatoinDetails.AssociateAllocationId);
                        associateAllocation.ReleaseDate = associateDetails.ReleaseDate < allocatoinDetails.EffectiveDate ? allocatoinDetails.EffectiveDate : associateDetails.ReleaseDate;
                        associateAllocation.IsActive = false;
                        response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;

                        //if primary project is releasing then making the other project(other than talent-pool) as primary.
                        var AssociateDetails = m_EmployeeService.GetEmployeeById((int)associateDetails.EmployeeId).Result.Item;
                        var AssociateName = AssociateDetails.FirstName + " " + AssociateDetails.LastName;
                        var associateReleasedProject = m_ProjectContext.Projects.Where(project => project.ProjectId == associateAllocation.ProjectId).Select(project => project.ProjectName).FirstOrDefault();
                        if (response.IsSuccessful && associateAllocation.IsPrimary == true && IsLastWorkingDate == false && (associateExitResponse.IsSuccessful == true && associateExitResponse.Item == null))
                        {

                            var otherallocatedProject = m_ProjectContext.AssociateAllocation.Where(allocation => allocation.ProjectId != associateDetails.ReleaseProjectId &&
                           allocation.IsPrimary == false && allocation.EmployeeId == associateDetails.EmployeeId
                           && allocation.AllocationPercentage != zeroAllocationPercentageId && allocation.IsActive == true).ToList();
                            if (otherallocatedProject.Count == 1)
                            {

                                otherallocatedProject.ForEach(alloc =>
                                {
                                    alloc.IsPrimary = true;
                                });
                                response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                                var projectManagerDetails = getProjectAndManagerDetails(otherallocatedProject);
                                SendNotificationRegardingPrimaryAllocationReview(projectManagerDetails, AssociateName, associateReleasedProject);

                            }
                            else if (otherallocatedProject.Count > 1)
                            {
                                var listProjects = new List<Employee>();


                                var projectManagerDetails = getProjectAndManagerDetails(otherallocatedProject);

                                SendNotificationRegardingPrimaryAllocation(projectManagerDetails, AssociateName, associateReleasedProject);
                            }
                            response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                        }

                        var allocation1 = await m_ProjectContext.AssociateAllocation.Where(x => x.ProjectId == associateDetails.ReleaseProjectId && x.EmployeeId == associateDetails.EmployeeId && x.IsActive == true).FirstOrDefaultAsync();

                        if (allocation1 != null && allocation.AllocationPercentage != zeroAllocationPercentageId)
                        {
                            var tpAllocationDetails = await (from al in m_ProjectContext.AssociateAllocation
                                                             join ap in m_ProjectContext.AllocationPercentage on al.AllocationPercentage equals ap.AllocationPercentageId
                                                             where al.ProjectId == associateDetails.ReleaseProjectId && al.EmployeeId == associateDetails.EmployeeId && al.IsActive == true
                                                             select new AssociateAllocationDetails
                                                             {
                                                                 EffectiveDate = al.EffectiveDate,
                                                                 InternalBillingRoleId = al.InternalBillingRoleId,
                                                                 Percentage = ap.Percentage, //ExitingPoolPercentage
                                                                 AssociateAllocationId = al.AssociateAllocationId
                                                             }).FirstOrDefaultAsync();
                            exitingPoolPercentage = tpAllocationDetails.Percentage ?? 0;
                            var associateAllocation1 = await m_ProjectContext.AssociateAllocation.FindAsync(tpAllocationDetails.AssociateAllocationId);
                            associateAllocation1.ReleaseDate = associateDetails.ReleaseDate < tpAllocationDetails.EffectiveDate ? tpAllocationDetails.EffectiveDate : associateDetails.ReleaseDate;
                            associateAllocation1.IsActive = false;
                            response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                        }
                        decimal allocationPercentage = exitingProjectAllocationPercentage + exitingPoolPercentage;

                        if (allocationPercentage > 0 && allocation.AllocationPercentage != zeroAllocationPercentageId && IsLastWorkingDate == false)
                        {
                            int allocationPercentageId = await m_ProjectContext.AllocationPercentage.Where(x => x.Percentage == allocationPercentage).Select(x => x.AllocationPercentageId).FirstOrDefaultAsync();
                            var associateAllocation2 = new AssociateAllocation()
                            {
                                Trid = associateDetails.TalentRequisitionId,
                                ProjectId = associateDetails.ReleaseProjectId,
                                EmployeeId = associateDetails.EmployeeId,
                                RoleMasterId = roleMasterId,
                                IsActive = true,
                                IsPrimary = allocationPercentage == 100 ? true : false,
                                AllocationPercentage = allocationPercentageId,
                                InternalBillingPercentage = 0,
                                IsCritical = false,
                                EffectiveDate = associateDetails.ReleaseDate.Value.AddDays(1),
                                AllocationDate = DateTime.Now,
                                ReportingManagerId = projectManagersDetails.ReportingManagerId,
                                LeadId = projectManagersDetails.LeadId,
                                ProgramManagerId = projectManagersDetails.ProgramManagerId,
                                IsBillable = false,
                                InternalBillingRoleId = internalBillingRoleId
                            };
                            m_ProjectContext.AssociateAllocation.Add(associateAllocation2);
                            response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                            await m_EmployeeService.UpdateReportingManagerId(associateDetails.EmployeeId.Value, (int)projectManagersDetails.ReportingManagerId);
                        }
                    }
                    //from release screen assigning primary project
                    if (associateDetails.MakePrimaryProjectId != 0)
                    {
                        var config = new MapperConfiguration(cfg =>
                            {
                                cfg.CreateMap<AssociateAllocation, AssociateAllocation>();
                            });
                        var mapper = config.CreateMapper();
                        var allocationDetails = m_ProjectContext.AssociateAllocation.Where(x => x.ProjectId == associateDetails.MakePrimaryProjectId && x.EmployeeId == associateDetails.EmployeeId && x.IsActive == true).FirstOrDefault();
                        allocationDetails.ReleaseDate = associateDetails.ReleaseDate < allocationDetails.EffectiveDate ? allocationDetails.EffectiveDate : associateDetails.ReleaseDate;
                        allocationDetails.IsActive = false;
                        response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;

                        var associateAllocation = new AssociateAllocation();
                        associateAllocation = mapper.Map<AssociateAllocation, AssociateAllocation>(allocationDetails);
                        associateAllocation.ReleaseDate = associateDetails.ReleaseDate < allocationDetails.EffectiveDate ? allocationDetails.EffectiveDate : associateDetails.ReleaseDate;
                        associateAllocation.IsActive = true;
                        associateAllocation.AssociateAllocationId = 0;
                        associateAllocation.IsPrimary = true;
                        associateAllocation.EffectiveDate = associateDetails.ReleaseDate.Value.AddDays(1);
                        associateAllocation.AllocationDate = DateTime.Now;
                        associateAllocation.CreatedBy = null;
                        associateAllocation.CreatedDate = null;
                        m_ProjectContext.AssociateAllocation.Add(associateAllocation);
                        response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                        await m_EmployeeService.UpdateReportingManagerId(associateDetails.EmployeeId.Value, associateAllocation.ReportingManagerId.Value);
                    }
                    await dbContext.CommitAsync();

                    if (associateDetails.NotifyAll == true)
                    {
                        var projectDetails = await m_ProjectContext.Projects.FindAsync(associateDetails.ProjectId);
                        var projectManager = await m_ProjectContext.ProjectManagers.Where(pm => pm.ProjectId == associateDetails.ProjectId && pm.IsActive == true).FirstOrDefaultAsync();
                        await SendReleaseNotification(associateDetails.EmployeeId, String.Format("{0:MMM dd, yyyy}", associateDetails.ReleaseDate), projectDetails, projectManager);
                    }
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    m_Logger.LogError("Exception occured in Associate Allocation Service - GetEmpAllocationHistory method.");
                }
            }
            return response;
        }

        private async Task<(ServiceResponse<AssociateExit> associateExit, bool IsLastWorkingDate)> GetResignedassociate(int EmployeeId, DateTime ReleaseDate)
        {
            ServiceResponse<AssociateExit> response = new ServiceResponse<AssociateExit>();
            AssociateExit associateExit = (await m_EmployeeService.GetResignedAssociateByID(EmployeeId)).Item;
            bool OnResignedDay = false;
            if (associateExit != null)
            {
                if (ReleaseDate > associateExit.ExitDate)
                {
                    response.IsSuccessful = false;
                    response.Message = "Invalid release date please verify the last working day";

                }
                else if (associateExit != null && Convert.ToDateTime(ReleaseDate).Date == Convert.ToDateTime(associateExit.ExitDate).Date)
                {
                    OnResignedDay = true;
                }
                else
                {
                    response.Item = associateExit;
                    response.IsSuccessful = true;
                }

            }
            else
            {
                response.IsSuccessful = true;
            }
            return (response, OnResignedDay);
        }

        public async Task<bool> SendReleaseNotification(int? employeeId, string allocationEffectiveDate, Entities.Project projectDetails, ProjectManager projectManager)
        {
            bool sentEmail = false;
            try
            {
                //EmailNotificationConfiguration notificationConfiguration = new KRA().getEmailTemplate(Convert.ToInt32(Enumeration.NotificationType.Release), Convert.ToInt32(Enumeration.CategoryMaster.TalentRequisition));
                var notificationConfiguration = await m_OrgService.GetNotificationConfiguration(Convert.ToInt32(HRMS.Common.Enums.NotificationType.Release), Convert.ToInt32(HRMS.Common.Enums.CategoryMaster.TalentRequisition));

                if (notificationConfiguration != null)
                {
                    string subject = notificationConfiguration.Item.EmailSubject;
                    var users = await m_OrgService.GetUsers();
                    var associate = users.Items.Where(x => x.EmployeeId == employeeId).FirstOrDefault();
                    var programMgr = users.Items.Where(x => x.EmployeeId == projectManager.ProgramManagerId).FirstOrDefault();
                    var reportingMgr = users.Items.Where(x => x.EmployeeId == projectManager.ReportingManagerId).FirstOrDefault();
                    var initiatedOn = DateTime.UtcNow;
                    //var initiatedBy = await m_EmployeeService.GetEmployeeByUserName(HttpContext.User.Identity.Name);
                    var initiatedBy = await m_EmployeeService.GetActiveEmployeeById(projectManager.ProgramManagerId.Value);

                    if (reportingMgr != null && associate != null && projectDetails != null && initiatedOn != null && initiatedBy != null)
                    {
                        StringBuilder emailContent = new StringBuilder(WebUtility.HtmlDecode(notificationConfiguration.Item.EmailContent));
                        subject = subject.Replace("@AssociateName", associate.UserName).Replace("@ProjectName", projectDetails.ProjectName).Replace("@EmployeeCode", associate.EmployeeCode);
                        emailContent = emailContent.Replace("@initiatedBy", initiatedBy.Item.FirstName + " " + initiatedBy.Item.LastName).Replace("@initiatedOn", String.Format("{0:MMM dd, yyyy}", initiatedOn)).Replace("@AssociateName", associate.UserName).Replace("@ProjectName", projectDetails.ProjectName)
                           .Replace("@EffectiveDate", allocationEffectiveDate);

                        NotificationDetail notificationDetail = new NotificationDetail();
                        notificationDetail.EmailBody = emailContent.ToString();
                        notificationDetail.FromEmail = notificationConfiguration.Item.EmailFrom;
                        notificationDetail.Subject = subject;
                        notificationDetail.ToEmail = programMgr.EmailAddress;

                        StringBuilder sbCcAddress = new StringBuilder();
                        sbCcAddress.Append(notificationConfiguration.Item.EmailCC).Append(";");
                        if (reportingMgr.EmailAddress != null)
                            sbCcAddress.Append(reportingMgr.EmailAddress).Append(";");
                        if (associate.EmailAddress != null)
                            sbCcAddress.Append(associate.EmailAddress).Append(";");
                        notificationDetail.CcEmail = sbCcAddress.ToString();

                        m_OrgService.SendEmail(notificationDetail);
                        sentEmail = true;
                    }
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Failed to send notification.");
            }
            return sentEmail;
        }

        #endregion

        #region GetAllocatedAssociates
        /// <summary>
        /// Get Alloacted Employees 
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetAllocatedAssociates()
        {
            ServiceListResponse<EmployeeDetails> response = new ServiceListResponse<EmployeeDetails>();
            var employees = await m_EmployeeService.GetAll(true);
            // int projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower().Contains("talent pool")).Select(x => x.ProjectTypeId).FirstOrDefault();
            //var practiceArea = m_OrgService.GetAllPracticeArea(true).Result.Items;
            var departments = m_OrgService.GetAllDepartment(true).Result.Items;

            var associates = (from e in employees.Items
                              join au in m_ProjectContext.AssociateAllocation on e.EmployeeId equals au.EmployeeId
                              join p in m_ProjectContext.Projects on au.ProjectId equals p.ProjectId
                              //join pt in practiceArea on p.PracticeAreaId equals pt.PracticeAreaId
                              join department in departments on p.DepartmentId equals department.DepartmentId
                              where au.IsActive == true /*&& p.ProjectTypeId != projectTypeId*/
                              group new { e.EmployeeId, e.FirstName, e.LastName, department.DepartmentId }
                              by new { e.EmployeeId, e.FirstName, e.LastName, department.DepartmentId } into g
                              select new EmployeeDetails
                              {
                                  EmployeeName = g.Key.FirstName + " " + g.Key.LastName,
                                  EmployeeId = g.Key.EmployeeId,
                                  DepartmentId = g.Key.DepartmentId
                              }).OrderBy(c => c.EmployeeName).ToList();

            if (associates == null || associates.Count == 0)
            {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "No Associate Allocations found.";
            }
            else
            {
                response.Items = associates;
                response.IsSuccessful = true;
                response.Message = "";
            }
            return response;
        }

        #endregion

        #region ReleaseOnExit
        /// <summary>
        /// Release From talent pool after Associate exit
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="releaseDate"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> ReleaseOnExit(int employeeId, string releaseDate)
        {
            BaseServiceResponse response = new BaseServiceResponse();
            //int projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower().Contains("talent pool")).Select(x => x.ProjectTypeId).FirstOrDefault();
            var allocations = await (from al in m_ProjectContext.AssociateAllocation
                                     join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                     where al.EmployeeId == employeeId && al.IsActive == true
                                     select al).OrderByDescending(x => x.AssociateAllocationId).ToListAsync();

            if (allocations != null && allocations.Count > 0)
            {
                foreach (AssociateAllocation allocation in allocations)
                {
                    allocation.ReleaseDate = Convert.ToDateTime(releaseDate);
                    allocation.IsActive = false;
                }

                response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0;
                response.Message = "Allocation deactivated successfully";
            }
            else
            {
                response.IsSuccessful = true;
                response.Message = "No Allocation found";
            }

            return response;
        }

        #endregion

        #region ReleaseFromTalentPool
        /// <summary>
        /// Release From talent pool 
        /// </summary>
        /// <param name="tpDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> ReleaseFromTalentPool(TalentPoolDetails tpDetails)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var allocation = await (from al in m_ProjectContext.AssociateAllocation
                                        where al.EmployeeId == tpDetails.EmployeeId && al.ProjectId == tpDetails.projectId &&
                                        al.IsActive == true
                                        select al).FirstOrDefaultAsync();
                if (allocation != null)
                {
                    allocation.ReleaseDate = tpDetails.ReleaseDate;
                    allocation.IsActive = false;
                    List<AssociateAllocation> allocationList = m_ProjectContext.AssociateAllocation.Where(allocation => allocation.EmployeeId == tpDetails.EmployeeId && allocation.ProjectId != tpDetails.projectId).ToList();
                    AssociateAllocation previousAllocation = new AssociateAllocation();
                    if (allocationList.Count > 0)
                    {
                        previousAllocation = allocationList.OrderByDescending(allocation => allocation.ReleaseDate).First();
                        //Talent-pool release date should not be less than the existing allocation release date.
                        if (previousAllocation.ReleaseDate > allocation.ReleaseDate)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Invalid date. Date should be greater than or equal to " + string.Format("{0:dd-MM-yyyy}", previousAllocation.ReleaseDate);
                            return response;
                        }
                    }
                    else if (allocationList.Count == 0)
                    {
                        if (Convert.ToDateTime(allocation.EffectiveDate).Date > Convert.ToDateTime(allocation.ReleaseDate).Date)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Invalid date. Date should be greater than or equal to " + string.Format("{0:dd-MM-yyyy}", allocation.EffectiveDate);
                            return response;
                        }
                    }
                    //// Intraday release flow 
                    if (allocation.EffectiveDate >= allocation.ReleaseDate)
                    {
                        m_ProjectContext.AssociateAllocation.Remove(allocation);
                    }

                }
                response.IsSuccessful = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
                m_Logger.LogError("Error occure while releasing Talent-Pool allocation", e.StackTrace);
            }
            return response;
        }

        #endregion

        #region AllocationChanges
        /// <summary>
        /// Code to make allocation changes(critical to non critical and vice versa)
        /// </summary>
        /// <param name="allocationIn"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> UpdateAssociateAllocation(AssociateAllocationDetails allocationIn)
        {
            ServiceResponse<int> response;
            using (var dbContext = await m_ProjectContext.Database.BeginTransactionAsync())
            {
                try
                {
                    bool isCreated = false, isUpdated = false;
                    m_Logger.LogInformation("Calling \"UpdateAssociateAllocation\" method in AssociateAllocationService");

                    //Check associate already allocated to same project
                    var isProject_allocated = await (from al in m_ProjectContext.AssociateAllocation
                                                     where al.EmployeeId == allocationIn.EmployeeId && al.ProjectId == allocationIn.ProjectId && al.IsActive == true
                                                     select al).FirstOrDefaultAsync();
                    if (isProject_allocated != null)
                    {
                        var associateAllocation = await m_ProjectContext.AssociateAllocation.FindAsync(isProject_allocated.AssociateAllocationId);
                        associateAllocation.ReleaseDate = (isProject_allocated.EffectiveDate == allocationIn.EffectiveDate) ? allocationIn.EffectiveDate : allocationIn.EffectiveDate.Value.AddDays(-1);
                        associateAllocation.IsActive = false;
                        isUpdated = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                    }
                    if (isUpdated)
                    {
                        AssociateAllocation associateAllocation = new AssociateAllocation();
                        associateAllocation = m_mapper.Map<AssociateAllocation, AssociateAllocation>(isProject_allocated);
                        associateAllocation.AssociateAllocationId = 0;
                        associateAllocation.EffectiveDate = allocationIn.EffectiveDate;
                        associateAllocation.AllocationDate = DateTime.Now;
                        //associateAllocation.IsBillable = isProject_allocated.IsBillable;
                        associateAllocation.IsCritical = allocationIn.isCritical;
                        associateAllocation.IsActive = true;
                        await m_ProjectContext.AssociateAllocation.AddAsync(associateAllocation);
                        isCreated = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                    }

                    if (isCreated)
                    {
                        await dbContext.CommitAsync();
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = true,
                            Message = string.Empty
                        };
                    }
                    else
                    {
                        m_Logger.LogError("No AssociateAllocation created.");
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "No AssociateAllocation created."
                        };
                    }
                }
                catch (Exception ex)
                {
                    await dbContext.RollbackAsync();
                    m_Logger.LogError("No AssociateAllocation created.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No AssociateAllocation created."
                    };
                }
            }
        }
        #endregion

        #region GetCurrentAllocationByEmpIdAndProjectId
        /// <summary>
        /// Get Current Allocation By EmpId And ProjectId
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<AssociateAllocationDetails>> GetCurrentAllocationByEmpIdAndProjectId(int employeeId, int projectId)
        {
            ServiceResponse<AssociateAllocationDetails> response = new ServiceResponse<AssociateAllocationDetails>();

            var employee = await m_EmployeeService.GetEmployeeById(employeeId);
            var isProject_allocated = await (from al in m_ProjectContext.AssociateAllocation
                                             join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                             join ap in m_ProjectContext.AllocationPercentage on al.AllocationPercentage equals ap.AllocationPercentageId
                                             where al.EmployeeId == employeeId && al.ProjectId == projectId && al.IsActive == true
                                             select new AssociateAllocationDetails
                                             {
                                                 EmployeeId = al.EmployeeId,
                                                 ProjectId = (int)al.ProjectId,
                                                 Project = p.ProjectName,
                                                 ProjectTypeId = p.ProjectTypeId,
                                                 AllocationPercentage = (int)ap.Percentage,
                                                 Billable = al.IsBillable == null ? false : al.IsBillable,
                                                 isCritical = al.IsCritical == null ? false : al.IsCritical,
                                                 IsPrimary = al.IsPrimary == null ? false : al.IsPrimary,
                                                 EffectiveDate = al.EffectiveDate,
                                                 AssociateName = employee == null ? string.Empty : employee.Item.FirstName + " " + employee.Item.LastName
                                             }).FirstOrDefaultAsync();

            if (isProject_allocated == null)
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = "No Associate Allocation found.";
            }
            else
            {
                response.Item = isProject_allocated;
                response.IsSuccessful = true;
                response.Message = "";
            }
            return response;
        }

        #endregion

        #region UpdatePracticeAreaOfTalentPoolProject
        /// <summary>
        /// UpdatePracticeAreaOfTalentPoolProject
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> UpdatePracticeAreaOfTalentPoolProject(int EmpID, int CompetenceyAreaId)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();

            try
            {
                int projectTypeId = (await m_OrgService.GetAllProjectTypes(true)).Items.Where(x => x.ProjectTypeCode.ToLower().Contains("talent pool")).Select(x => x.ProjectTypeId).FirstOrDefault();
                bool isSaved = false;
                var new_talentPool = (from tp in m_ProjectContext.TalentPool
                                      join p in m_ProjectContext.Projects on tp.ProjectId equals p.ProjectId
                                      join projectmgrs in m_ProjectContext.ProjectManagers on p.ProjectId equals projectmgrs.ProjectId
                                      where tp.PracticeAreaId == CompetenceyAreaId && projectmgrs.IsActive == true
                                      select new
                                      {
                                          ProjectId = p.ProjectId,
                                          ReportingManagerId = projectmgrs.ReportingManagerId,
                                          ProgramManagerId = projectmgrs.ProgramManagerId,
                                          LeadId = projectmgrs.LeadId
                                      }).FirstOrDefault();

                List<AssociateAllocation> allocations = m_ProjectContext.AssociateAllocation.Where(allocation => allocation.EmployeeId == EmpID && allocation.IsActive == true).ToList();
                AssociateAllocation talentpool_project = new AssociateAllocation();
                if (allocations.Count > 0)
                {
                    talentpool_project = (from al in allocations
                                          join p in m_ProjectContext.Projects on al.ProjectId equals p.ProjectId
                                          where al.EmployeeId == EmpID && p.ProjectTypeId == projectTypeId &&
                                          al.IsActive == true
                                          select al).FirstOrDefault();
                }
                if (talentpool_project != null)
                {
                    DateTime effeciveDate = Convert.ToDateTime(talentpool_project.EffectiveDate).Date;
                    if (talentpool_project.ProjectId != new_talentPool.ProjectId && effeciveDate < DateTime.Now.Date)
                    {
                        AssociateAllocation allocation = talentpool_project;
                        int allocationId = talentpool_project.AssociateAllocationId;
                        allocation.AssociateAllocationId = 0;
                        allocation.ProjectId = new_talentPool.ProjectId;
                        allocation.ReportingManagerId = new_talentPool.ReportingManagerId;
                        allocation.ProgramManagerId = new_talentPool.ProgramManagerId;
                        allocation.LeadId = new_talentPool.LeadId;
                        allocation.AllocationDate = DateTime.Now;
                        allocation.EffectiveDate = DateTime.Now;
                        m_ProjectContext.AssociateAllocation.Add(allocation);
                        isSaved = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                        if (isSaved)
                        {
                            var existingallocation = m_ProjectContext.AssociateAllocation.Find(allocationId);
                            if (existingallocation != null)
                            {
                                existingallocation.IsActive = false;
                                existingallocation.ReleaseDate = DateTime.Now.AddDays(-1);
                                isSaved = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                                if (!isSaved)
                                {
                                    response.IsSuccessful = false;
                                    response.Message = "Failed to update talent-pool project in associate allocation";
                                    m_Logger.LogInformation("Failed to update talent-pool project in associate allocation");
                                }
                            }
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "Failed to update talent-pool project in  associate allocation";
                            m_Logger.LogInformation("Failed to update talent-pool project in associate allocation");
                        }
                    }
                    else if (talentpool_project.ProjectId != new_talentPool.ProjectId && effeciveDate == DateTime.Now.Date)
                    {
                        talentpool_project.ProjectId = new_talentPool.ProjectId;
                        talentpool_project.ReportingManagerId = new_talentPool.ReportingManagerId;
                        talentpool_project.ProgramManagerId = new_talentPool.ProgramManagerId;
                        talentpool_project.LeadId = new_talentPool.LeadId;
                        isSaved = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                        if (!isSaved)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Failed to update talent-pool project in associate allocation";
                            m_Logger.LogInformation("Failed to update talent-pool project in associate allocation");
                        }
                    }
                }
                response.IsSuccessful = true;

            }

            catch (Exception ex)
            {
                m_Logger.LogError("No talent-pool project created/updated in AssociateAllocation.", ex.StackTrace);
                return response = new ServiceResponse<bool>()
                {
                    IsSuccessful = false,
                    Message = "No  talent-pool project created/updated in AssociateAllocation."
                };
            }
            return response;

        }
        #endregion

        #region ReleaseFromAllocations
        /// <summary>
        /// ReleaseFromAllocations
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> ReleaseFromAllocations(int EmployeeId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var associateallocation = m_ProjectContext.AssociateAllocation.Where(allocation => allocation.EmployeeId == EmployeeId && allocation.IsActive == true).ToList();
                if (associateallocation != null)
                {
                    associateallocation.ForEach(allocation =>
                    {
                        if (Convert.ToDateTime(allocation.EffectiveDate).Date == DateTime.Now.Date)
                        {
                            m_ProjectContext.AssociateAllocation.Remove(allocation);
                        }
                        else if (Convert.ToDateTime(allocation.EffectiveDate).Date < DateTime.Now.Date)
                        {
                            allocation.IsActive = false;
                            allocation.ReleaseDate = DateTime.Now;
                        }
                    });
                    var saved = await m_ProjectContext.SaveChangesAsync() > 0 ? true : false;
                    if (!saved)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occure while releasing allocation";
                    }
                }
                response.IsSuccessful = true;
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
                m_Logger.LogError("Error occure while releasing allocation", e.StackTrace);
            }
            return response;
        }

        #endregion

        #region AddAssociateFutureProject
        public async Task<ServiceResponse<bool>> AddAssociateFutureProject(AssociateFutureProjectAllocationDetails associateFutureProject)
        {
            var response = new ServiceResponse<bool>();
            m_Logger.LogInformation("Inert into associateFutureProject into AssociateFutureProjectAllocation table ");
            try
            {
                var checkIfExist = m_ProjectContext.AssociateFutureProjectAllocation.Where(project => project.EmployeeId == associateFutureProject.EmployeeId && project.IsActive == true).FirstOrDefault();
                if (checkIfExist == null)
                {

                    AssociateFutureProjectAllocation associateFutureProjectAllocation = new AssociateFutureProjectAllocation();
                    m_mapper.Map<AssociateFutureProjectAllocationDetails, AssociateFutureProjectAllocation>(associateFutureProject, associateFutureProjectAllocation);
                    m_ProjectContext.AssociateFutureProjectAllocation.Add(associateFutureProjectAllocation);
                    var isSaved = await m_ProjectContext.SaveChangesAsync();
                    if (isSaved > 0)
                    {
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                    }
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Message = "Record already exist";
                }
            }
            catch (Exception e)
            {
                m_Logger.LogError(e.Message);
                m_Logger.LogError(e.StackTrace);
                response.IsSuccessful = false;
                response.Message = e.Message;
            }
            return response;
        }
        #endregion

        #region GetAssociateFutureProjectByEmpId
        public async Task<ServiceListResponse<AssociateFutureProjectAllocationDetails>> GetAssociateFutureProjectByEmpId(int employeeId)
        {
            var response = new ServiceListResponse<AssociateFutureProjectAllocationDetails>();
            m_Logger.LogInformation("fetching data from AssociateFutureProjectAllocation table ");
            try
            {
                List<AssociateFutureProjectAllocationDetails> associateFutureProjectAllocationDetails = await (from assFP in m_ProjectContext.AssociateFutureProjectAllocation
                                                                                                               where assFP.EmployeeId == employeeId && assFP.IsActive == true
                                                                                                               select new AssociateFutureProjectAllocationDetails
                                                                                                               {
                                                                                                                   EmployeeId = assFP.EmployeeId,
                                                                                                                   ProjectName = assFP.ProjectName,
                                                                                                                   ProjectId = assFP.ProjectId,
                                                                                                                   Remarks = assFP.Remarks,
                                                                                                                   TentativeDate = assFP.TentativeDate
                                                                                                               }).ToListAsync();
                if (associateFutureProjectAllocationDetails.Count > 0)
                {
                    response.IsSuccessful = true;
                    response.Items = associateFutureProjectAllocationDetails;
                }
                else if (associateFutureProjectAllocationDetails.Count == 0)
                {
                    response.IsSuccessful = true;
                    response.Message = "No records found";
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching data from AssociateFutureProjectAllocation table";
                }
            }
            catch (Exception e)
            {
                m_Logger.LogError(e.Message);
                m_Logger.LogError(e.StackTrace);
                response.IsSuccessful = false;
                response.Message = e.Message;
            }
            return response;
        }
        #endregion

        #region DiactivateAssociateFutureProjectByEmpId
        public async Task<ServiceListResponse<bool>> DiactivateAssociateFutureProjectByEmpId(int employeeId)
        {
            var response = new ServiceListResponse<bool>();
            m_Logger.LogInformation("Updating AssociateFutureProjectAllocation table ");
            try
            {
                var futureProject = await m_ProjectContext.AssociateFutureProjectAllocation.Where(project => project.EmployeeId == employeeId && project.IsActive == true).FirstOrDefaultAsync();
                if (futureProject != null)
                {
                    futureProject.IsActive = false;
                    m_ProjectContext.AssociateFutureProjectAllocation.Update(futureProject);
                    var isSaved = await m_ProjectContext.SaveChangesAsync();
                    if (isSaved > 0)
                    {
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while updating data in AssociateFutureProjectAllocation table";

                    }
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Message = "Record not found";
                }

            }
            catch (Exception e)
            {
                m_Logger.LogError(e.Message);
                m_Logger.LogError(e.StackTrace);
                response.IsSuccessful = false;
                response.Message = e.Message;
            }
            return response;
        }
        #endregion


        #region GetCompetencyAreaManagersDetails
        public async Task<ServiceResponse<CompetencyAreaMananagers>> GetCompetencyAreaManagersDetails(int competencyAreaId)
        {
            var response = new ServiceResponse<CompetencyAreaMananagers>();
            m_Logger.LogInformation("Updating GetCompetencyAreaManagersDetails table ");
            try
            {
                var managersDetails = (await GetCompetencyAreaMananagers(competencyAreaId));

                if (managersDetails==null)
                {
                    response.IsSuccessful = false;
                    response.Message = "No Data found";
                    return response;
                }
                response.IsSuccessful = true;
                response.Item = managersDetails;

            }
            catch (Exception e)
            {
                m_Logger.LogError(e.Message);
                m_Logger.LogError(e.StackTrace);
                response.IsSuccessful = false;
                response.Message = e.Message;
            }
            return response;
        }
        #endregion

        private async Task<CompetencyAreaMananagers> GetCompetencyAreaMananagers(int? competencyAreaId)
        {
            var managersDetails =await (from tp in m_ProjectContext.TalentPool
                                        join p in m_ProjectContext.Projects on tp.ProjectId equals p.ProjectId
                                        join projectmgrs in m_ProjectContext.ProjectManagers on p.ProjectId equals projectmgrs.ProjectId
                                        where tp.PracticeAreaId == competencyAreaId && projectmgrs.IsActive == true
                                        select new CompetencyAreaMananagers
                                        {
                                            ReportingManagerId = projectmgrs.ReportingManagerId,
                                            ProgramManagerId = projectmgrs.ProgramManagerId,
                                            LeadId = projectmgrs.LeadId,
                                            ProjectId= p.ProjectId
                                        }).FirstOrDefaultAsync();
            return managersDetails;

        }

        #region GetAllAllocationDetails
        public async Task<ServiceListResponse<ActiveAllocationDetails>> GetAllAllocationDetails()
        {
            var response = new ServiceListResponse<ActiveAllocationDetails>();
            try
            {
                var allocation =await (from all in m_ProjectContext.AssociateAllocation
                                  join pr in m_ProjectContext.Projects on all.ProjectId equals pr.ProjectId
                                  join pm in m_ProjectContext.ProjectManagers on all.ProjectId equals pm.ProjectId
                                  where all.IsActive == true && all.IsPrimary == true && pm.IsActive == true
                                  select new ActiveAllocationDetails
                                  {
                                      ProjectId = all.ProjectId ?? 0,
                                      ProjectName = pr.ProjectName,
                                      ClientId = pr.ClientId,
                                      ProgramManagerId = pm.ProgramManagerId,
                                      EffectiveFromDate = all.EffectiveDate,
                                      EffectiveToDate = all.ReleaseDate,
                                      RoleMasterId = all.RoleMasterId,
                                      EmployeeId = all.EmployeeId ?? 0,
                                      IsPrimary = all.IsPrimary
                                  }
                                 ).ToListAsync();

                if (allocation == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "No Data found";
                    return response;
                }
                response.IsSuccessful = true;
                response.Items = allocation;

            }
            catch (Exception e)
            {
                m_Logger.LogError(e.Message);
                m_Logger.LogError(e.StackTrace);
                response.IsSuccessful = false;
                response.Message = e.Message;
            }
            return response;
        }
        #endregion

        #region Notification

        public async Task<bool> SendAllocationNotification(int? employeeId, int reportingManagerId, int? primaryReportingManagerId, string allocationEffectiveDate, Entities.Project projectDetails, ProjectManager projectManager)
        {
            bool sentMail = false;
            try
            {
                var notificationConfiguration = await m_OrgService.GetNotificationConfiguration(Convert.ToInt32(HRMS.Common.Enums.NotificationType.Allocation), Convert.ToInt32(HRMS.Common.Enums.CategoryMaster.TalentRequisition));

                if (notificationConfiguration != null)
                {
                    string subject = notificationConfiguration.Item.EmailSubject;
                    var users = await m_OrgService.GetUsers();
                    var associate = users.Items.Where(x => x.EmployeeId == employeeId.Value).FirstOrDefault();
                    var programMgr = users.Items.Where(x => x.EmployeeId == projectManager.ProgramManagerId.Value).FirstOrDefault();
                    var reportingMgr = users.Items.Where(x => x.EmployeeId == reportingManagerId).FirstOrDefault();
                    var primaryReportingMgr = users.Items.Where(x => x.EmployeeId == primaryReportingManagerId).FirstOrDefault();
                    var initiatedOn = DateTime.UtcNow;

                    var initiatedBy = await m_EmployeeService.GetActiveEmployeeById(projectManager.ProgramManagerId.Value);

                    if (reportingMgr != null && associate != null && projectDetails != null && primaryReportingMgr != null && initiatedOn != null && initiatedBy != null)
                    {
                        subject = subject.Replace("@AssociateName", associate.UserName).Replace("@ProjectName", projectDetails.ProjectName).Replace("@EmployeeCode", associate.EmployeeCode);
                        StringBuilder emailContent = new StringBuilder(WebUtility.HtmlDecode(notificationConfiguration.Item.EmailContent));

                        NotificationDetail notificationDetail = new NotificationDetail();
                        notificationDetail.FromEmail = notificationConfiguration.Item.EmailFrom;
                        notificationDetail.Subject = subject;
                        notificationDetail.ToEmail = programMgr.EmailAddress;

                        StringBuilder sbCcAddress = new StringBuilder();
                        sbCcAddress.Append(notificationConfiguration.Item.EmailCC).Append(";");
                        if (reportingMgr.EmailAddress != null)
                            sbCcAddress.Append(reportingMgr.EmailAddress).Append(";");
                        if (associate.EmailAddress != null)
                            sbCcAddress.Append(associate.EmailAddress).Append(";");

                        sbCcAddress.Append(primaryReportingMgr.EmailAddress).Append(";");

                        emailContent = emailContent.Replace("@AssociateName", associate.UserName)
                                        .Replace("@ProjectName", projectDetails.ProjectName)
                                        .Replace("@FirstName", associate.FirstName)
                                        .Replace("@ReportingManager", reportingMgr.UserName)
                                        .Replace("@EffectiveDate", allocationEffectiveDate)
                                        .Replace("@initiatedBy", initiatedBy.Item.FirstName + " " + initiatedBy.Item.LastName)
                                        .Replace("@initiatedOn", String.Format("{0:MMM dd, yyyy}", initiatedOn))
                                        .Replace("@EffectiveDate", allocationEffectiveDate);

                        notificationDetail.CcEmail = sbCcAddress.ToString();
                        notificationDetail.EmailBody = emailContent.ToString();

                        m_OrgService.SendEmail(notificationDetail);
                        sentMail = true;
                    }
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in NotificationConfiguration.");
            }
            return sentMail;
        }

        public void SendNotificationRegardingPrimaryAllocation(List<ProgramManagerDetails> programManager, string AssociateName, string associateReleasedProject)
        {

            NotificationDetail notificationConfig = new NotificationDetail();

            notificationConfig.FromEmail = m_EmailConfigurations.FromEmail;

            if (m_MiscellaneousSettings.Environment == "PROD")
            {
                notificationConfig.ToEmail = string.Join(";", programManager.Select(pro => pro.ProgramManagerEmail).Distinct().ToArray());
                notificationConfig.CcEmail = m_EmailConfigurations.ToEmail;
            }
            if (m_MiscellaneousSettings.Environment == "QA")
            {
                notificationConfig.ToEmail = m_EmailConfigurations.ToEmail;
                notificationConfig.CcEmail = m_EmailConfigurations.CCEmail;
            }
            notificationConfig.Subject = "Request for associate allocation update";
            notificationConfig.EmailBody = RequestForPrimaryProjectAllocationSetupConfig(programManager, AssociateName, associateReleasedProject);
            bool IsNotification = m_EmailConfigurations.SendEmail;
            if (IsNotification)
            {
                m_OrgService.SendEmail(notificationConfig);
            }
        }

        public string RequestForPrimaryProjectAllocationSetupConfig(List<ProgramManagerDetails> programManager, string AssociateName, string associateReleasedProject)
        {
            string FilePath = Environment.CurrentDirectory + "/NotificationTemplates/RequestForPrimaryProjectAllocationSetup.html";
            StreamReader stream = new StreamReader(FilePath);
            string MailText = stream.ReadToEnd();
            stream.Close();
            var programManagers = string.Join(",", programManager.Select(programManager => programManager.ProgramManagerName).Distinct().ToArray());
            var Projects = string.Join(",", programManager.Select(project => project.ProjectName).Distinct().ToArray());

            string generatedHTMLtable = GenerateHTMLTable(programManager);
            MailText = MailText.Replace("{ProgramManager}", programManagers);
            MailText = MailText.Replace("{Associate}", AssociateName);
            MailText = MailText.Replace("{associateReleasedProject}", associateReleasedProject);
            MailText = MailText.Replace("{table}", generatedHTMLtable);
            return MailText;
        }

        public void SendNotificationRegardingPrimaryAllocationReview(List<ProgramManagerDetails> projectManagerDetails, string AssociateName, string associateReleasedProject)
        {

            NotificationDetail notificationConfig = new NotificationDetail();

            notificationConfig.FromEmail = m_EmailConfigurations.FromEmail;

            if (m_MiscellaneousSettings.Environment == "PROD")
            {
                notificationConfig.ToEmail = projectManagerDetails.Select(pro => pro.ProgramManagerEmail).FirstOrDefault();
                notificationConfig.CcEmail = m_EmailConfigurations.ToEmail;

            }
            if (m_MiscellaneousSettings.Environment == "QA")
            {
                notificationConfig.CcEmail = m_EmailConfigurations.CCEmail;
                notificationConfig.ToEmail = m_EmailConfigurations.ToEmail;
            }
            notificationConfig.Subject = "Associate allocation update review";
            notificationConfig.EmailBody = PrimaryProjectAllocationUpdateConfig(projectManagerDetails, AssociateName, associateReleasedProject);

            bool IsNotification = m_EmailConfigurations.SendEmail;
            if (IsNotification)
            {
                m_OrgService.SendEmail(notificationConfig);
            }

        }

        public string PrimaryProjectAllocationUpdateConfig(List<ProgramManagerDetails> programManager, string AssociateName, string associateReleasedProject)
        {
            string FilePath = Environment.CurrentDirectory + "/NotificationTemplates/PrimaryProjectChangedNotification.html";
            StreamReader stream = new StreamReader(FilePath);
            string MailText = stream.ReadToEnd();
            stream.Close();
            var programManagers = programManager.Select(programManager => programManager.ProgramManagerName).FirstOrDefault();
            var Project = programManager.Select(project => project.ProjectName).FirstOrDefault();

            MailText = MailText.Replace("{ProgramManager}", programManagers);
            MailText = MailText.Replace("{ProjectName}", Project);
            MailText = MailText.Replace("{Associate}", AssociateName);
            MailText = MailText.Replace("{associateReleasedProject}", associateReleasedProject);

            return MailText;
        }

        public void SendNotificationToPMForAllocatonChangePrimaryProject(string associateName, string newProject, List<ProgramManagerDetails> programManagerDetails)
        {
            NotificationDetail notificationConfig = new NotificationDetail();

            notificationConfig.FromEmail = m_EmailConfigurations.FromEmail;
            if (m_MiscellaneousSettings.Environment == "PROD")
            {
                notificationConfig.ToEmail = programManagerDetails.Select(pm => pm.ProgramManagerEmail).FirstOrDefault();
                notificationConfig.CcEmail = m_EmailConfigurations.ToEmail;
            }
            if (m_MiscellaneousSettings.Environment == "QA")
            {
                notificationConfig.ToEmail = m_EmailConfigurations.ToEmail;
                notificationConfig.CcEmail = m_EmailConfigurations.CCEmail;
            }
            notificationConfig.Subject = "Associate allocation primary project update review";
            notificationConfig.EmailBody = OnAllocationPrimaryProjectChangeConfig(associateName, newProject, programManagerDetails);
            bool IsNotification = m_EmailConfigurations.SendEmail;
            if (IsNotification)
            {
                m_OrgService.SendEmail(notificationConfig);
            }
        }

        public string OnAllocationPrimaryProjectChangeConfig(string asscoiateName, string newProject, List<ProgramManagerDetails> programManagerDetails)
        {

            string FilePath = Environment.CurrentDirectory + "/NotificationTemplates/AllocationChangesPrimaryProject.html";
            StreamReader stream = new StreamReader(FilePath);
            string MailText = stream.ReadToEnd();
            stream.Close();
            var programManagers = programManagerDetails.Select(pm => pm.ProgramManagerName).FirstOrDefault();
            MailText = MailText.Replace("{ProgramManager}", programManagers);
            MailText = MailText.Replace("{Associate}", asscoiateName);
            MailText = MailText.Replace("{AssociateNewProject}", newProject);
            MailText = MailText.Replace("{AssociateExistingProject}", programManagerDetails.Select(pm => pm.ProjectName).FirstOrDefault());
            return MailText;
        }

        public Employee GetProgramManagerDetais(int programPranagerID)
        {
            var ProgramManagerDetails = m_EmployeeService.GetActiveEmployeeById(programPranagerID).Result.Item;
            return ProgramManagerDetails;

        }

        public List<ProgramManagerDetails> getProjectAndManagerDetails(List<AssociateAllocation> associateAllocations)
        {
            var projectManagerDetails = (from proj in associateAllocations
                                         join pr in m_ProjectContext.ProjectManagers on proj.ProjectId equals pr.ProjectId
                                         join prj in m_ProjectContext.Projects on proj.ProjectId equals prj.ProjectId
                                         where pr.IsActive == true
                                         select new ProgramManagerDetails
                                         {
                                             ProjectID = (int)proj.ProjectId,
                                             ProjectName = prj.ProjectName,
                                             ProgramManagerName = GetProgramManagerDetais((int)pr.ProgramManagerId).FirstName + " " + GetProgramManagerDetais((int)pr.ProgramManagerId).LastName,
                                             ProgramManagerEmail = GetProgramManagerDetais((int)pr.ProgramManagerId).WorkEmailAddress
                                         }).Distinct().ToList();
            return projectManagerDetails;
        }

        public static string GenerateHTMLTable(List<ProgramManagerDetails> programManager)
        {
            string tableHtml = "";
            DataTable dt = new DataTable("ProjectProgramManager");
            dt.Columns.Add(new DataColumn("S.No", typeof(int)));
            dt.Columns.Add(new DataColumn("Project", typeof(string)));
            dt.Columns.Add(new DataColumn("ProgramManager", typeof(string)));
            int i = 1;
            foreach (var pm in programManager)
            {

                DataRow dr = dt.NewRow();
                dr["S.No"] = i;
                dr["Project"] = pm.ProjectName;
                dr["ProgramManager"] = pm.ProgramManagerName;
                dt.Rows.Add(dr);
                i++;
            }

            tableHtml += "<table>";
            tableHtml += "<tr><th>S.No</th><th>Project</th><th>ProgramManager</th></tr>";

            foreach (DataRow drPM in dt.Rows)
            {
                tableHtml += "<tr><td>" + drPM["S.No"] + "</td><td>" + drPM["Project"] + "</td><td>" + drPM["ProgramManager"] + "</td></tr>";
            }
            tableHtml += "</table>";
            return tableHtml;

        }

        public class ProgramManagerDetails
        {
            public int ProjectID { get; set; }
            public int ProgramManagerId { get; set; }
            public string ProjectName { get; set; }
            public string ProgramManagerName { get; set; }
            public string ProgramManagerEmail { get; set; }

        }

        #endregion
    }
}
