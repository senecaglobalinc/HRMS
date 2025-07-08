using AutoMapper;
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
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class ReportService : IReportService
    {
        #region Global Varibles

        private readonly ILogger<ReportService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_Mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private IOrganizationService m_OrgService;
        private IAssociateAllocationService m_associateAllocationService;
        private IEmployeeService m_EmployeeService;

        #endregion

        #region ReportService
        public ReportService(ILogger<ReportService> logger,
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

        #region Get FinanceReportAllocations
        /// <summary>
        /// Get FinanceReportAllocations
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<FinanceReportAllocation>> GetFinanceReportAllocations(FinanceReportFilter filter)
        {
            ServiceListResponse<FinanceReportAllocation> response = new ServiceListResponse<FinanceReportAllocation>();

            try
            {
                List<FinanceReportAllocation> allocations = new List<FinanceReportAllocation>();

                if (filter.ProjectId.HasValue)
                {
                    var project = await m_ProjectContext.Projects.FindAsync(filter.ProjectId.Value);

                    if (project == null)
                    {
                        return new ServiceListResponse<FinanceReportAllocation>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No Project found with this Id.."
                        };
                    }
                }

               allocations = (from aa in m_ProjectContext.AssociateAllocation.ToList()
                                                             join pr in m_ProjectContext.Projects
                                                                on aa.ProjectId equals pr.ProjectId
                                                             //join pm in m_ProjectContext.ProjectManagers
                                                             //   on new { a = pr.ProjectId, b = true } equals new { a = pm.ProjectId.Value, b = pm.IsActive.Value }
                                                             join ap in m_ProjectContext.AllocationPercentage
                                                                on aa.AllocationPercentage equals ap.AllocationPercentageId
                                                             join cb in m_ProjectContext.ClientBillingRoles
                                                                on aa.ClientBillingRoleId equals cb.ClientBillingRoleId into cbrjoin
                                                             from cbr in cbrjoin.DefaultIfEmpty()
                                                             where 
                                                             ap.Percentage > 0M
                                                             && ((filter.ToDate >= aa.EffectiveDate && filter.ToDate <= aa.ReleaseDate )
                                                             || (filter.FromDate >= aa.EffectiveDate && filter.FromDate <= (aa.ReleaseDate ?? filter.FromDate))
                                                             || (aa.EffectiveDate >= filter.FromDate && aa.EffectiveDate <= filter.ToDate)
                                                             || (aa.ReleaseDate >= filter.FromDate && aa.ReleaseDate <= filter.ToDate)
                                                             )
                                                             select new FinanceReportAllocation
                                                             {
                                                                 ProjectId = pr.ProjectId,
                                                                 ProjectCode = pr.ProjectCode,
                                                                 ProjectName = pr.ProjectName,
                                                                 ClientId = pr.ClientId,
                                                                 EmployeeId = aa.EmployeeId.Value,
                                                                 EffectiveDate = aa.EffectiveDate,
                                                                 ReleaseDate = aa.ReleaseDate,
                                                                 AssociateAllocationId = aa.AssociateAllocationId,
                                                                 ProgramManagerId = aa.ProgramManagerId ?? 0,
                                                                 ReportingManagerId = aa.ReportingManagerId ?? 0,
                                                                 LeadId = aa.LeadId ?? 0,
                                                                 IsBillable = (aa.IsBillable ?? false),
                                                                 IsCritical = (aa.IsCritical ?? false),
                                                                 ClientBillingPercentage = (aa.ClientBillingPercentage ?? 0.0M),
                                                                 Allocationpercentage = ap.Percentage,
                                                                 InternalBillingPercentage = (aa.InternalBillingPercentage ?? 0.0M),
                                                                 InternalBillingRoleId = aa.InternalBillingRoleId ?? 0,
                                                                 ClientBillingRoleCode =cbr==null?null:cbr.ClientBillingRoleName,
                                                                 FromDate = (filter.FromDate > aa.EffectiveDate ? filter.FromDate : aa.EffectiveDate),
                                                                 ToDate = (filter.ToDate > aa.ReleaseDate? aa.ReleaseDate : filter.ToDate)
                                                             }).ToList();

                if (filter.ProjectId.HasValue)
                    allocations = (from re in allocations
                             where re.ProjectId == filter.ProjectId.Value
                             select re).ToList();

                return response = new ServiceListResponse<FinanceReportAllocation>()
                {
                    Items = allocations,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<FinanceReportAllocation>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region Get GetUtilizationReportAllocations
        /// <summary>
        /// Get UtilizationReportAllocations
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<UtilizationReportAllocation>> GetUtilizationReportAllocations(UtilizationReportFilter filter)
        {

            ServiceListResponse<UtilizationReportAllocation> response = new ServiceListResponse<UtilizationReportAllocation>();

            try
            {
                List<UtilizationReportAllocation> allocations = new List<UtilizationReportAllocation>();

                IQueryable<UtilizationReportAllocation> query = (from aa in m_ProjectContext.AssociateAllocation where aa.IsActive == true
                                                             join pr in m_ProjectContext.Projects
                                                                on aa.ProjectId equals pr.ProjectId
                                                             //join pm in m_ProjectContext.ProjectManagers
                                                             //   on new { a = pr.ProjectId, b = true } equals new { a = pm.ProjectId.Value, b = pm.IsActive.Value }
                                                             join ap in m_ProjectContext.AllocationPercentage
                                                                on aa.AllocationPercentage equals ap.AllocationPercentageId   
                                                             join fp in m_ProjectContext.AssociateFutureProjectAllocation.Where(project=>project.IsActive==true)
                                                                 on aa.EmployeeId equals fp.EmployeeId into futureProject
                                                                 from fpro in futureProject.DefaultIfEmpty()
                                                             select new UtilizationReportAllocation
                                                             {
                                                                 AssociateAllocationId = aa.AssociateAllocationId,
                                                                 ProjectId = pr.ProjectId,
                                                                 ProjectName = pr.ProjectName,
                                                                 ClientId = pr.ClientId,
                                                                 EmployeeId = aa.EmployeeId.Value,
                                                                 ProgramManagerId = aa.ProgramManagerId ?? 0,
                                                                 ReportingManagerId = aa.ReportingManagerId ?? 0,
                                                                 LeadId = aa.LeadId ?? 0,
                                                                 IsBillable = (aa.IsBillable ?? false),
                                                                 IsCritical = (aa.IsCritical ?? false),
                                                                 AllocationPercentageId = aa.AllocationPercentage,
                                                                 Allocationpercentage = ap.Percentage,
                                                                 IsFutureProjectMarked = fpro == null ? null : fpro.IsActive,
                                                                 FutureProjectName = fpro == null ? null : fpro.ProjectName,
                                                                 FutureProjectTentativeDate = fpro == null ?null:(DateTime?)fpro.TentativeDate,
                                                                 ProjectTypeId=pr.ProjectTypeId ??0
                                                             });

                if (filter.ProjectId > 0)
                    query = (from re in query
                             where re.ProjectId == filter.ProjectId 
                             select re);                

                if (filter.ClientId > 0)                   
                query = (from re in query
                         where re.ClientId == filter.ClientId
                         select re);

                if (filter.IsBillable >= 0)                 
                query = (from re in query
                         where re.IsBillable == (filter.IsBillable == 1 ? true : false)
                         select re);

                if (filter.IsCritical >= 0)
                    query = (from re in query
                             where re.IsCritical == (filter.IsCritical == 1 ? true : false)
                             select re);
                

                if (filter.AllocationPercentageId > 0)
                    query = (from re in query
                             where re.AllocationPercentageId == filter.AllocationPercentageId
                             select re);               

                if (filter.EmployeeId > 0)
                    query = (from re in query
                             where re.EmployeeId == filter.EmployeeId
                             select re);               

                if (filter.ProgramManagerId > 0)
                    query = (from re in query
                             where re.ProgramManagerId == filter.ProgramManagerId
                             select re);

                allocations = await query.ToListAsync();

                return response = new ServiceListResponse<UtilizationReportAllocation>()
                {
                    Items = allocations,
                    IsSuccessful = true,
                    Message = ""
                };

            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<UtilizationReportAllocation>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion      

        #region Get Domain Wise Resource Count
        /// <summary>
        /// GetDomainWiseResourceCount
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<DomainDataCount>> GetDomainWiseResourceCount()
        {
            ServiceListResponse<DomainDataCount> response = new ServiceListResponse<DomainDataCount>();

            try
            {
                List<DomainDataCount> list = new List<DomainDataCount>();
                list = await (from aa in m_ProjectContext.AssociateAllocation
                              join pr in m_ProjectContext.Projects
                              on aa.ProjectId equals pr.ProjectId
                              where aa.EmployeeId.HasValue && pr.DomainId.HasValue                              
                              group new { pr.DomainId, aa.EmployeeId }
                by new { pr.DomainId, aa.EmployeeId } into g
                              select new DomainDataCount
                              {
                                  EmployeeID = g.Key.EmployeeId.Value,
                                  DomainID = g.Key.DomainId.Value,
                              }).ToListAsync();

                return response = new ServiceListResponse<DomainDataCount>()
                {
                    Items = list,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                return response = new ServiceListResponse<DomainDataCount>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region GetTalentPoolWiseResourceCount
        /// <summary>
        /// GetTalentPoolWiseResourceCount
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<TalentpoolDataCount>> GetTalentPoolWiseResourceCount(string projectTypeIds)
        {
            ServiceListResponse<TalentpoolDataCount> response = new ServiceListResponse<TalentpoolDataCount>();

            try
            {
                //var projectType = await m_OrgService.GetAllProjectTypes(true);
                //int projectTypeId = projectType.Items.Where(x => x.ProjectTypeCode.ToLower().Contains("talent pool")).Select(x => x.ProjectTypeId).FirstOrDefault();
                
                List<TalentpoolDataCount> list = new List<TalentpoolDataCount>();

                var projectTypIds = projectTypeIds.Split(",".ToCharArray()).Select(Int32.Parse).ToList();
                var associates = (await m_EmployeeService.GetAllExitAssociates()).Items
                    .Where(x =>x.IsActive==true && Convert.ToDateTime(x.ExitDate).Date < DateTime.Now.Date).ToList()
                    .Select(x => x.EmployeeId).ToList();
                
                projectTypIds.ForEach(protypeId =>
                {
                    IQueryable<TalentpoolDataCount> dataCounts = (from aa in m_ProjectContext.AssociateAllocation
                                                             where aa.IsActive == true && !(associates.Contains((int)aa.EmployeeId))
                                                                  join pr in m_ProjectContext.Projects
                                                             on new { a = aa.ProjectId.Value, b = protypeId } equals new { a = pr.ProjectId, b = pr.ProjectTypeId.Value }
                                                             //group aa by new { pr.ProjectId, pr.ProjectName, aa.EmployeeId } into g
                                                             group aa by new { pr.ProjectTypeId.Value,pr.ProjectId ,pr.ProjectName} into g
                                                             select new TalentpoolDataCount
                                                             {
                                                                 ResourceCount = g.Count(),
                                                                 ProjectID = g.Key.ProjectId,
                                                                 ProjectName = g.Key.ProjectName
                                                             });
                    list.AddRange(dataCounts);

                });
                   
                return response = new ServiceListResponse<TalentpoolDataCount>()
                {
                    Items = list,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                return response = new ServiceListResponse<TalentpoolDataCount>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceListResponse<int>> GetEmployeeByProjectId(int projectId)
        {
            ServiceListResponse<int> response = new ServiceListResponse<int>();

            try
            {               
                List<int> list = new List<int>();

                list = await (from aa in m_ProjectContext.AssociateAllocation
                                                         where aa.IsActive == true && aa.EmployeeId.HasValue && aa.ProjectId == projectId                             
                                                         select aa.EmployeeId.Value
                                                         ).Distinct().ToListAsync();
                
                return response = new ServiceListResponse<int>()
                {
                    Items = list,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                return response = new ServiceListResponse<int>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        public async Task<ServiceListResponse<ProjectResourceData>> GetResourceByProject(int projectId)
        {
            ServiceListResponse<ProjectResourceData> response = new ServiceListResponse<ProjectResourceData>();

            try
            {
                List<ProjectResourceData> list =await (from aa in m_ProjectContext.AssociateAllocation
                                                  where aa.IsActive == true && aa.EmployeeId.HasValue && aa.ProjectId == projectId
                                                  join fpro in m_ProjectContext.AssociateFutureProjectAllocation.Where(fproject => fproject.IsActive == true)
                                                  on aa.EmployeeId equals fpro.EmployeeId into futureProject
                                                  from fupro in futureProject.DefaultIfEmpty()
                                                      select new ProjectResourceData
                                                      {
                                                          EmployeeId = aa.EmployeeId.Value,
                                                          DurationInDays = (DateTime.Now - aa.EffectiveDate.Value).Days,
                                                          FutureProjectName = fupro == null ? null : fupro.ProjectName,
                                                          FutureProjectTentativeDate = fupro == null ? null : (DateTime?)fupro.TentativeDate
                                                      }).Distinct().ToListAsync();

                return response = new ServiceListResponse<ProjectResourceData>()
                {
                    Items = list,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                return response = new ServiceListResponse<ProjectResourceData>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion

        #region Get GetSkillSearchAllocations
        /// <summary>
        /// Get GetSkillSearchAllocations
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<SkillSearchAllocation>> GetSkillSearchAllocations()
        {
            ServiceListResponse<SkillSearchAllocation> response = new ServiceListResponse<SkillSearchAllocation>();

            try
            {
                List<SkillSearchAllocation> allocations = new List<SkillSearchAllocation>();

                allocations = await (from aa in m_ProjectContext.AssociateAllocation where aa.ReleaseDate.HasValue == false
                                                             join pr in m_ProjectContext.Projects
                                                                on aa.ProjectId equals pr.ProjectId
                                                             //join pm in m_ProjectContext.ProjectManagers
                                                             //   on new { a = pr.ProjectId, b = true } equals new { a = pm.ProjectId.Value, b = pm.IsActive.Value }
                                                             join ap in m_ProjectContext.AllocationPercentage
                                                                on aa.AllocationPercentage equals ap.AllocationPercentageId                                                 
                                                             select new SkillSearchAllocation
                                                             {
                                                                 ProjectId = pr.ProjectId,                                                                 
                                                                 ProjectName = pr.ProjectName,                                                                 
                                                                 EmployeeId = aa.EmployeeId.Value,                                                     
                                                                 ProgramManagerId = aa.ProgramManagerId ?? 0,
                                                                 ReportingManagerId = aa.ReportingManagerId ?? 0,
                                                                 LeadId = aa.LeadId ?? 0,                                                            
                                                                 Allocationpercentage = ap.Percentage          
                                                             }).ToListAsync();              


                return response = new ServiceListResponse<SkillSearchAllocation>()
                {
                    Items = allocations,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<SkillSearchAllocation>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region GetAllProjects
        /// <summary>
        /// Get the all the projects
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectResponse>> GetAllProjects(bool nightlyJob = false)
        {
            ServiceListResponse<ProjectResponse> response;
            ServiceListResponse<Status> statusesforPPC = null;
            statusesforPPC = await m_OrgService.GetStatusesByCategoryName(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), nightlyJob);

            if (statusesforPPC.Items == null || (statusesforPPC.Items != null && statusesforPPC.Items.Count <= 0))
            {
                return response = new ServiceListResponse<ProjectResponse>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "Status for project category not found."
                };
            }
            var projects = await m_ProjectContext.Projects.OrderBy(x => x.ProjectName).ToListAsync();
            if (projects == null || projects.Count == 0)
            {
                return response = new ServiceListResponse<ProjectResponse>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Projects found.."
                };
            }
            else
            {
                List<ProjectResponse> projRes = new List<ProjectResponse>();
                ServiceListResponse<Client> clients = null;
                ServiceListResponse<PracticeArea> practiceAreas = null;
                ServiceListResponse<ProjectType> projectTypes = null;
                ServiceListResponse<Employee> emp = null;

                List<int> clientIds = projects.Where(proj => proj.ClientId != 0).Select(proj => proj.ClientId).Distinct().ToList();
                if (clientIds != null && clientIds.Count > 0)
                    clients = await m_OrgService.GetClientsByIds(clientIds);

                List<int> practiceAreaIds = projects
                    .Where(proj => proj.PracticeAreaId != 0).Select(proj => proj.PracticeAreaId).Distinct().ToList();
                if (practiceAreaIds != null && practiceAreaIds.Count > 0)
                    practiceAreas = await m_OrgService.GetPracticeAreasByIds(practiceAreaIds);

                List<int> projectTypeIds = projects
                    .Where(proj => proj.ProjectTypeId != 0).Select(proj => proj.ProjectTypeId??0).Distinct().ToList();
                if (projectTypeIds != null && projectTypeIds.Count > 0)
                    projectTypes = await m_OrgService.GetProjectTypesByIds(projectTypeIds);
                emp = await m_EmployeeService.GetAll(true);

                foreach (var project in projects)
                {
                    Status projStatus = null;
                    if (project.ProjectStateId != null)
                    {
                        projStatus = statusesforPPC.Items.Where(st => st.StatusId == project.ProjectStateId).FirstOrDefault();
                    }
                    ProjectResponse pr = new ProjectResponse();
                    pr.ProjectId = project.ProjectId;
                    pr.ProjectCode = project.ProjectCode;
                    pr.ProjectName = project.ProjectName;
                    pr.ActualStartDate = project.ActualStartDate;
                    pr.ActualEndDate = project.ActualEndDate;
                    pr.IsActive = project.IsActive;
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
                    var progManager = m_ProjectContext.ProjectManagers.Where(pm => pm.ProjectId == project.ProjectId && pm.IsActive == true).FirstOrDefault();
                    if (progManager != null && progManager.ProgramManagerId != 0)
                    {

                        if (emp.IsSuccessful)
                        {
                            var employee = emp.Items.Where(st => st.EmployeeId == progManager.ProgramManagerId.Value).FirstOrDefault();
                            pr.ManagerName = employee.FirstName + " " + employee.LastName;
                        }
                    }
                    projRes.Add(pr);





                }
                return response = new ServiceListResponse<ProjectResponse>()
                {
                    Items = projRes,
                    IsSuccessful = true,
                    Message = ""
                };
            }
        }
        #endregion

        #region GetServiceTypeProjectCount
        /// <summary>
        /// Get the all the projects
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ServiceTypeCount>> GetServiceTypeProjectCount()
        {
            ServiceListResponse<ServiceTypeCount> response;
            ServiceListResponse<Status> statusesforPPC = null;
            statusesforPPC = await m_OrgService.GetStatusesByCategoryName(HRMS.Common.Enums.CategoryMaster.PPC.ToString());



            if (statusesforPPC.Items == null || (statusesforPPC.Items != null && statusesforPPC.Items.Count <= 0))
            {
                return response = new ServiceListResponse<ServiceTypeCount>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "Status for project category not found."
                };



            }
           
                List<ServiceTypeCount> projRes = new List<ServiceTypeCount>();
            projRes = await (from ep in m_ProjectContext.Projects
                          join emp in m_ProjectContext.Projects
                          on ep.ProjectId equals emp.ProjectId
                          where ep.IsActive == true && emp.IsActive == true && ep.ProjectStateId!=16
                          select new ServiceTypeCount
                          {
                              ProjectId = ep.ProjectId,
                              ServiceTypeId = ep.ProjectTypeId??0,
                              ProjectName=ep.ProjectName
                          }).OrderBy(st=>st.ProjectName).ToListAsync();




            return response = new ServiceListResponse<ServiceTypeCount>()
                {
                    Items = projRes,
                    IsSuccessful = true,
                    Message = ""
                };
        }
        
        #endregion

        #region GetCriticalResourceReport
        /// <summary>
        /// GetCriticalResourceReport
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateInformationReport>> GetCriticalResourceReport()
        {
            ServiceListResponse<AssociateInformationReport> response=new ServiceListResponse<AssociateInformationReport>();
            try
            {
                var CriticalResourceInfo =await m_ProjectContext.AssociateInformationReport.Where(associate=>associate.IsCritical==true ).ToListAsync();
                if(CriticalResourceInfo!=null)
                {
                    response.IsSuccessful = true;
                    response.Items = CriticalResourceInfo;
                }
                else
                {
                    response.IsSuccessful = false;
                }
            }
            catch(Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
            }

            return response;
        }

        #endregion

        #region GetNonCriticalResourceReport
        /// <summary>
        /// GetNonCriticalResourceReport
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateInformationReport>> GetNonCriticalResourceReport()
        {
            ServiceListResponse<AssociateInformationReport> response = new ServiceListResponse<AssociateInformationReport>();
            try
            {
                var NonCriticalResourceInfo = await m_ProjectContext.AssociateInformationReport.Where(x=>x.IsCritical==false).ToListAsync();
                if (NonCriticalResourceInfo != null)
                {
                   
                        foreach (var nonCriticalResourceInfo in NonCriticalResourceInfo)
                        {
                            foreach (PropertyInfo pi in nonCriticalResourceInfo.GetType().GetProperties())
                            {
                                if (pi.PropertyType == typeof(string))
                                {
                                    string value = (string)pi.GetValue(nonCriticalResourceInfo);
                                    if (value == "")
                                    {
                                        pi.SetValue(nonCriticalResourceInfo, null);
                                    }
                                }
                            }
                        }

                        response.IsSuccessful = true;
                        response.Items = NonCriticalResourceInfo;
                }
                else
                {
                    response.IsSuccessful = false;
                }
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
            }

            return response;
        }

        #endregion


        #region GetTalentPoolReportProject
        /// <summary>
        /// GetTalentPoolReportProject
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateInformationReport>> GetTalentPoolReportProject(string employeeIds)
        {
            ServiceListResponse<AssociateInformationReport> response = new ServiceListResponse<AssociateInformationReport>();
            try
            {
                List<int> empIds = employeeIds.Split(",".ToCharArray()).Select(int.Parse).ToList(); 

            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
            }

            return response;
        }

        #endregion

        #region GetNonCriticalResourceBillingReport
        /// <summary>
        /// GetNonCriticalResourceBillingReport
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateInformationReports>> GetNonCriticalResourceBillingReport()
        {
            ServiceListResponse<AssociateInformationReports> response = new ServiceListResponse<AssociateInformationReports>();
            try
            {
                var NonCriticalResourceInfo =await (from re in m_ProjectContext.AssociateInformationReport.Where(x => x.IsCritical == false)
                                               select new AssociateInformationReports
                                               {
                                                   EmployeeId=re.EmployeeId,
                                                   EmployeeCode = re.EmployeeCode,
                                                   AssociateName = re.AssociateName,                                                   
                                                   Designation = re.Designation,
                                                   ProjectName = re.ProjectName,
                                                   Technology=re.Technology,
                                                   Grade = re.Grade,
                                               }).ToListAsync();
                NonCriticalResourceInfo.ForEach(resource =>
                {
                    var allocation = m_ProjectContext.AssociateAllocation.Where(x => x.EmployeeId == resource.EmployeeId && x.IsBillable == false).OrderByDescending(x => x.AssociateAllocationId).FirstOrDefault();
                    resource.LastBillingDate = allocation!=null? allocation.ReleaseDate:null;
                });

                response.IsSuccessful = true;
                response.Items = NonCriticalResourceInfo;
            }                
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
            }

            return response;
        }

        #endregion
    }
}