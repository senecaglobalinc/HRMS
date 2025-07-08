using AutoMapper;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class ReportsService : IReportsService
    {

        #region Global Varibles

        private readonly ILogger<ReportsService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_Mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private IOrganizationService m_OrgService;
        private IAssociateAllocationService m_associateAllocationService;
        private IEmployeeService m_EmployeeService;

        #endregion

        #region ReportsService
        public ReportsService(ILogger<ReportsService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IAssociateAllocationService associateAllocationService,
            IOrganizationService orgService,
            IEmployeeService employeeService)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientBillingRoles, ClientBillingRoles>();
            });
            m_Mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            //m_apiEndPoints = apiEndPoints?.Value;
            m_apiEndPoints = apiEndPoints != null ? apiEndPoints.Value : null;
            m_OrgService = orgService;
            m_associateAllocationService = associateAllocationService;
            m_EmployeeService = employeeService;
        }
        #endregion

        #region GetResourceByProject
        /// <summary>
        /// Get Resource By Project
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<Entities.AllocationDetails>> GetResourceByProject(int projectId)
        {
            
            ServiceResponse<Entities.AllocationDetails> allocationDetailsResponse = new ServiceResponse<AllocationDetails>();

            try
            {
                var allocations = new AllocationDetails
                {
                    AllocationCount = (await GetAllocationCountByProject(projectId)).Item,
                    lstBillableResources = (await GetBillableResourcesByProject(projectId)).Items,
                    lstNonBillableCriticalResources = (await GetNonBillableCriticalResourcesByProject(projectId)).Items,
                    lstNonBillableNonCriticalResources = (await GetNonBillableNonCriticalResourcesByProject(projectId)).Items
                };
               
                return allocationDetailsResponse = new ServiceResponse<AllocationDetails>()
                {
                    Item = allocations,
                    IsSuccessful = true,
                    Message = ""
                };

            }
            catch (Exception ex)
            {
                //log the exception
                return allocationDetailsResponse = new ServiceResponse<AllocationDetails>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = ""
                };
//                throw ex;
            }
            
        }

        #endregion

        #region GetAllocationCountByProject
        /// <summary>
        /// GetAllocationCountByProject
        /// </summary>
        /// /// <param name="allocationDetails">allocation Details</param>
        /// <param name="id">project Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<AllocationCount>> GetAllocationCountByProject(int projectId)
        {
            ServiceResponse<Entities.AllocationCount> response = new ServiceResponse<AllocationCount>();
            try
            {
                if (projectId == 0)
                {
                    return response = new ServiceResponse<AllocationCount>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = "Invalid Request.."
                    };
                }

                var project = await m_ProjectContext.Projects.FindAsync(projectId);

                if (project == null)
                {
                    return response = new ServiceResponse<AllocationCount>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = "No Project found with this Id.."
                    };
                }
                else
                {
                    var allResources = await m_associateAllocationService.GetByProjectId(projectId);

                    var resources = allResources.Items.Where(res => res.IsActive == true);

                    var allocationCount = new AllocationCount
                    {
                        ProjectName = project.ProjectName,
                        ResourceCount = resources.Count(),
                        BillableCount = resources.Where(res => res.IsBillable == true && res.IsCritical == true).Count(),
                        NonBillableCriticalCount = resources.Where(res => res.IsBillable == false && res.IsCritical == true).Count(),
                        NonBillableNonCriticalCount = resources.Where(res => res.IsBillable == false && res.IsCritical == false).Count()
                    };


                    return response = new ServiceResponse<AllocationCount>()
                    {
                        Item = allocationCount,
                        IsSuccessful = true,
                        Message = ""
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        #endregion

        #region GetBillableResourcesByProject
        /// <summary>
        /// GetBillableResourcesByProject
        /// </summary>
        /// /// <param name="allocationDetails">allocation Details</param>
        /// <param name="id">project Id</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ResourceAllocationDetails>> GetBillableResourcesByProject(int projectId)
        {
            ServiceListResponse<ResourceAllocationDetails> response = new ServiceListResponse<ResourceAllocationDetails>();
            m_Logger.LogInformation("Calling \"GetBillableResourcesByProject\" method in Reports Service.");

            m_Logger.LogInformation($"Getting billable resources for project Id:{projectId}");
            try
            {
                if (projectId == 0)
                {
                    return response = new ServiceListResponse<ResourceAllocationDetails>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "Invalid Request.."
                    };
                }

                var project = await m_ProjectContext.Projects.FindAsync(projectId);

                if (project == null)
                {
                    return response = new ServiceListResponse<ResourceAllocationDetails>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Project found with this Id.."
                    };
                }
                else
                {
                    var associateAllocations =
                        await m_ProjectContext.AssociateAllocation.Where(aa => aa.ProjectId == projectId
                                                                               && aa.ReleaseDate == null
                                                                               && aa.IsActive == true
                                                                               && aa.IsBillable == true
                                                                               && aa.IsCritical == true
                                                                        ).ToListAsync();

                    if (associateAllocations == null || associateAllocations.Count == 0)
                    {
                        m_Logger.LogInformation("No billable resources found for this project");
                        response = new ServiceListResponse<ResourceAllocationDetails>()
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
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No Employees found with these Employee Ids:" + employeeIds
                        };
                        return response;
                    }


                    m_Logger.LogInformation($"Adding record to ResourceAllocationDetails.");

                    var resourceAllocationDetails =
                         (from aa in associateAllocations
                          join emp in employees.Items
                            on aa.EmployeeId equals emp.EmpId
                          join ap in m_ProjectContext.AllocationPercentage
                            on aa.AllocationPercentage equals ap.AllocationPercentageId
                          join cbr in m_ProjectContext.ClientBillingRoles
                            on aa.ClientBillingRoleId equals cbr.ClientBillingRoleId into aacbr
                          from clientBillingRole in aacbr.DefaultIfEmpty()
                          where emp.IsActive == true
                          select new ResourceAllocationDetails
                          {
                              AssociateCode = emp.EmpCode,
                              AssociateName = emp.EmpName,
                              AllocationPercentage = ap.Percentage,
                              ClientBillingRoleName = clientBillingRole.ClientBillingRoleName,
                              ClientBillingRoleId = clientBillingRole.ClientBillingRoleId,
                              IsPrimaryProject = aa.IsPrimary.HasValue ? aa.IsPrimary.Value.ToString() : "false",
                              IsCriticalResource = aa.IsCritical.HasValue ? aa.IsCritical.Value.ToString() : "false",
                              EffectiveDate = aa.EffectiveDate.Value
                          }).OrderBy(cbrm => cbrm.AssociateName).ToList();


                    if (resourceAllocationDetails == null && resourceAllocationDetails.Count() == 0)
                    {
                        m_Logger.LogInformation("No Billable resources found for this project");
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No Billable resources found for this project"
                        };
                        return response;
                    }
                    else
                    {
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = resourceAllocationDetails,
                            IsSuccessful = true,
                            Message = ""
                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetNonBillableResourcesByProject
        /// <summary>
        /// GetNonBillableResourcesByProject
        /// </summary>
        /// /// <param name="allocationDetails">allocation Details</param>
        /// <param name="id">project Id</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ResourceAllocationDetails>> GetNonBillableCriticalResourcesByProject(int projectId)
        {
            ServiceListResponse<ResourceAllocationDetails> response = new ServiceListResponse<ResourceAllocationDetails>();
            m_Logger.LogInformation("Calling \"GetNonBillableResourcesByProject\" method in Reports Service.");

            m_Logger.LogInformation($"Getting non billable resources for project Id:{projectId}");
            try
            {
                if (projectId == 0)
                {
                    return response = new ServiceListResponse<ResourceAllocationDetails>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "Invalid Request.."
                    };
                }

                var project = await m_ProjectContext.Projects.FindAsync(projectId);

                if (project == null)
                {
                    return response = new ServiceListResponse<ResourceAllocationDetails>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Project found with this Id.."
                    };
                }
                else
                {
                    var associateAllocations =
                        await m_ProjectContext.AssociateAllocation.Where(aa => aa.ProjectId == projectId
                                                                               && aa.ReleaseDate == null
                                                                               && aa.IsActive == true
                                                                               && aa.IsBillable == false
                                                                               && aa.IsCritical == true
                                                                        ).ToListAsync();

                    if (associateAllocations == null || associateAllocations.Count == 0)
                    {
                        m_Logger.LogInformation("No non-billable resources found for this project");
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No non-billable resources found for this project"
                        };
                        return response;
                    }

                    List<int> employeeIds =
                        associateAllocations.Where(aa => aa.EmployeeId.HasValue).Select(aa => aa.EmployeeId.Value).Distinct().ToList();

                    var employees = await m_EmployeeService.GetEmployeesByIds(employeeIds);

                    if (employees == null || employees.Items.Count == 0)
                    {
                        m_Logger.LogInformation("No Employees found with these Employee Ids:" + employeeIds);
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No Employees found with these Employee Ids:" + employeeIds
                        };
                        return response;
                    }


                    m_Logger.LogInformation($"Adding record to ResourceAllocationDetails.");

                    var resourceAllocationDetails =
                         (from aa in associateAllocations
                          join emp in employees.Items
                            on aa.EmployeeId equals emp.EmpId
                          join ap in m_ProjectContext.AllocationPercentage
                            on aa.AllocationPercentage equals ap.AllocationPercentageId
                          where emp.IsActive == true
                          select new ResourceAllocationDetails
                          {
                              AssociateCode = emp.EmpCode,
                              AssociateName = emp.EmpName,
                              AllocationPercentage = ap.Percentage,
                              IsPrimaryProject = aa.IsPrimary.HasValue ? aa.IsPrimary.Value.ToString() : "false",
                              IsCriticalResource = aa.IsCritical.HasValue ? aa.IsCritical.Value.ToString() : "false",
                              EffectiveDate = aa.EffectiveDate.Value
                          }).OrderBy(cbrm => cbrm.AssociateName).ToList();

                    if (resourceAllocationDetails == null && resourceAllocationDetails.Count() == 0)
                    {
                        m_Logger.LogInformation("No non-Billable resources found for this project");
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No non-Billable resources found for this project"
                        };
                        return response;
                    }
                    else
                    {
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = resourceAllocationDetails,
                            IsSuccessful = true,
                            Message = ""
                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetNonBillableResourcesByProject
        /// <summary>
        /// GetNonBillableResourcesByProject
        /// </summary>
        /// /// <param name="allocationDetails">allocation Details</param>
        /// <param name="id">project Id</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<ResourceAllocationDetails>> GetNonBillableNonCriticalResourcesByProject(int projectId)
        {
            ServiceListResponse<ResourceAllocationDetails> response = new ServiceListResponse<ResourceAllocationDetails>();
            m_Logger.LogInformation("Calling \"GetNonBillableNonCriticalResourcesByProject\" method in Reports Service.");

            m_Logger.LogInformation($"Getting non billable resources for project Id:{projectId}");
            try
            {
                if (projectId == 0)
                {
                    return response = new ServiceListResponse<ResourceAllocationDetails>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "Invalid Request.."
                    };
                }

                var project = await m_ProjectContext.Projects.FindAsync(projectId);

                if (project == null)
                {
                    return response = new ServiceListResponse<ResourceAllocationDetails>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Project found with this Id.."
                    };
                }
                else
                {
                    var associateAllocations =
                        await m_ProjectContext.AssociateAllocation.Where(aa => aa.ProjectId == projectId
                                                                               && aa.ReleaseDate == null
                                                                               && aa.IsActive == true
                                                                               && aa.IsBillable == false
                                                                               && aa.IsCritical == false
                                                                        ).ToListAsync();

                    if (associateAllocations == null || associateAllocations.Count == 0)
                    {
                        m_Logger.LogInformation("No non-billable non-critical resources found for this project");
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No non-billable resources found for this project"
                        };
                        return response;
                    }

                    List<int> employeeIds =
                        associateAllocations.Where(aa => aa.EmployeeId.HasValue).Select(aa => aa.EmployeeId.Value).Distinct().ToList();

                    var employees = await m_EmployeeService.GetEmployeesByIds(employeeIds);

                    if (employees == null || employees.Items.Count == 0)
                    {
                        m_Logger.LogInformation("No Employees found with these Employee Ids:" + employeeIds);
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No Employees found with these Employee Ids:" + employeeIds
                        };
                        return response;
                    }


                    m_Logger.LogInformation($"Adding record to ResourceAllocationDetails.");

                    var resourceAllocationDetails =
                         (from aa in associateAllocations
                          join emp in employees.Items
                            on aa.EmployeeId equals emp.EmpId
                          join ap in m_ProjectContext.AllocationPercentage
                            on aa.AllocationPercentage equals ap.AllocationPercentageId
                          where emp.IsActive == true
                          select new ResourceAllocationDetails
                          {
                              AssociateCode = emp.EmpCode,
                              AssociateName = emp.EmpName,
                              AllocationPercentage = ap.Percentage,
                              IsPrimaryProject = aa.IsPrimary.HasValue ? aa.IsPrimary.Value.ToString() : "false",
                              IsCriticalResource = aa.IsCritical.HasValue ? aa.IsCritical.Value.ToString() : "false",
                              EffectiveDate = aa.EffectiveDate.Value
                          }).OrderBy(cbrm => cbrm.AssociateName).ToList();

                    if (resourceAllocationDetails == null && resourceAllocationDetails.Count() == 0)
                    {
                        m_Logger.LogInformation("No non-Billable non-critical resources found for this project");
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No non-Billable resources found for this project"
                        };
                        return response;
                    }
                    else
                    {
                        response = new ServiceListResponse<ResourceAllocationDetails>()
                        {
                            Items = resourceAllocationDetails,
                            IsSuccessful = true,
                            Message = ""
                        };
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetProjectDetailsReport
        /// <summary>
        /// GetProjectDetailsReport
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectReportData>> GetProjectDetailsReport()
        {
            var response = new ServiceListResponse<ProjectReportData>();
            var clients = await m_OrgService.GetClients();
            var practiceAreas = await m_OrgService.GetAllPracticeArea(true);

            try
            {
                var resources = GetProjectWiseResources();

                List<int> programManagerList = resources.Select(c => c.ProgramManagerId).Distinct().ToList();
                var programManagers = await m_EmployeeService.GetEmployeesByIds(programManagerList);

                var projRes = (from alloc in resources
                               join cl in clients.Items on alloc.ClientId equals cl.ClientId
                               join pa in practiceAreas.Items on alloc.PracticeAreaId equals pa.PracticeAreaId
                               join e in programManagers.Items on alloc.ProgramManagerId equals e.EmpId into empgr
                               from emp in empgr.DefaultIfEmpty()
                               group alloc by new { alloc.ProjectId, alloc.ProjectCode, alloc.ProjectName, pa.PracticeAreaCode, ProgramManager = (emp != null ? emp.EmpName : ""), cl.ClientName } into grp
                               select new ProjectReportData
                               {
                                   ProjectCode = grp.Key.ProjectCode,
                                   ProjectName = grp.Key.ProjectName,
                                   Technology = grp.Key.PracticeAreaCode,
                                   ClientName = grp.Key.ClientName,
                                   ProgramManager = grp.Key.ProgramManager,
                                   Billable = grp.Sum(c => c.IsBillable == true ? c.Total : 0),
                                   NonBillable = grp.Sum(c => c.IsBillable == false ? c.Total : 0),
                                   Total = grp.Sum(c => c.Total)
                               }).OrderBy(r => r.ProjectName).ToList();

                if (projRes != null)
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
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Private Methods

        private List<ProjectResourceReportData> GetProjectWiseResources()
        {
            var totalResources = (from alloc in m_ProjectContext.AssociateAllocation
                                  join pr in m_ProjectContext.Projects on alloc.ProjectId equals pr.ProjectId
                                  join pt in m_ProjectContext.ProjectManagers.Where(pt => pt.IsActive == true) on pr.ProjectId equals pt.ProjectId
                                  where alloc.ReleaseDate == null && pr.IsActive == true && pr.ProjectStateId == 19 && pr.ProjectTypeId != 6
                                 && alloc.IsActive == true && alloc.ProjectId.HasValue                                 
                                  group alloc by new
                                  {
                                      pr.ProjectId,
                                      alloc.IsBillable.Value,
                                      pr.ClientId,
                                      pr.ProjectCode,
                                      pr.ProjectName,
                                      pr.PracticeAreaId,
                                      ProgramManagerId = pt.ProgramManagerId ?? 0
                                  } into grp
                                  select new ProjectResourceReportData
                                  {
                                      ProjectId = grp.Key.ProjectId,
                                      IsBillable = grp.Key.Value,
                                      ClientId = grp.Key.ClientId,
                                      ProjectCode = grp.Key.ProjectCode,
                                      ProjectName = grp.Key.ProjectName,
                                      PracticeAreaId = grp.Key.PracticeAreaId,
                                      ProgramManagerId = grp.Key.ProgramManagerId,
                                      Total = grp.Count()
                                  }).ToList<ProjectResourceReportData>();
            return totalResources;
        }

        private int GetBillableResourcesByProject(int projectId, bool IsBillable) {
            var totalResources =  (from alloc in m_ProjectContext.AssociateAllocation
                           where alloc.ProjectId == projectId && alloc.ReleaseDate == null
                           && alloc.IsActive == true && alloc.IsBillable == IsBillable select alloc).Count();
            return totalResources;
        }

        #endregion
    }
}