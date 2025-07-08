using HRMS.Report.Infrastructure.Models.Domain;
using HRMS.Report.Infrastructure.Models.Request;
using HRMS.Report.Infrastructure.Models.Response;
using HRMS.Report.Types;
using HRMS.Report.Types.External;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Report.Service
{
    public class ReportService : IReportService
    {
        #region Global Varibles
        private readonly ILogger<ReportService> m_Logger;
        private IEmployeeService m_EmployeeService;
        private IProjectService m_ProjectService;
        private IOrganizationService m_OrgService;
        #endregion

        #region Constructor
        public ReportService(ILogger<ReportService> logger,
            IEmployeeService employeeService,
            IProjectService projectService,
            IOrganizationService organizationService)
        {
            m_Logger = logger;
            m_EmployeeService = employeeService;
            m_ProjectService = projectService;
            m_OrgService = organizationService;
        }
        #endregion        

        #region GetFinanceReport
        /// <summary>
        /// GetFinanceReport
        /// </summary>
        /// <param name="filter">filter</param>       
        /// <returns></returns>
        public async Task<ServiceListResponse<FinanceReport>> GetFinanceReport(FinanceReportFilter filter)
        {
            ServiceListResponse<FinanceReport> response = new ServiceListResponse<FinanceReport>();

            try
            {
                #region Prject Service
                ServiceListResponse<FinanceReportAllocation> allocations =
                    await m_ProjectService.GetFinanceReportAllocations(filter);

                if (allocations.Items == null || allocations.Items.Count == 0)
                {
                    m_Logger.LogInformation("No allocations found");
                    response = new ServiceListResponse<FinanceReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No billable resources found for this project"
                    };
                    return response;
                }
                #endregion

                #region Employee Service

                List<int> employeeIds =
                        allocations.Items.Select(aa => aa.EmployeeId).Distinct().ToList();

                List<int> leadIds =
                    allocations.Items.Select(aa => aa.LeadId).Distinct().ToList();

                leadIds.AddRange(allocations.Items.Select(aa => aa.ProgramManagerId).Distinct().ToList());
                leadIds.AddRange(allocations.Items.Select(aa => aa.ReportingManagerId).Distinct().ToList());

                FinanceReportEmployeeFilter empFilter = new FinanceReportEmployeeFilter
                {
                    EmployeeIdList = employeeIds,
                    LeadIdList = leadIds,
                    FromDate = filter.FromDate,
                    ToDate = filter.ToDate
                };

                ServiceListResponse<FinanceReportEmployee> associates = await m_EmployeeService.GetFinanceReportAssociates(empFilter);

                if (associates.Items == null || associates.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Associates found");
                    response = new ServiceListResponse<FinanceReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Associates found"
                    };
                    return response;
                }

                List<FinanceReportEmployee> leads = associates.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Manager).ToList();
                List<FinanceReportEmployee> employees = associates.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Associate).ToList();

                #endregion

                #region Admin Service
                ServiceListResponse<ReportDetails> masters = await m_OrgService.GetFinanceReportMasters();

                if (masters.Items == null || masters.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Masters found");
                    response = new ServiceListResponse<FinanceReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Masters found"
                    };
                    return response;
                }

                List<ReportDetails> clients = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Client).ToList();
                List<ReportDetails> departments = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Department).ToList();
                List<ReportDetails> grades = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Grade).ToList();
                List<ReportDetails> designations = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Designation).ToList();

                #endregion

                List<FinanceReport> financeReportDetails =
                         (from aa in allocations.Items
                          join emp in employees
                            on aa.EmployeeId equals emp.EmployeeId
                          select new FinanceReport
                          {
                              ProjectId = aa.ProjectId,
                              ProjectCode = aa.ProjectCode,
                              ProjectName = aa.ProjectName,
                              ClientId = aa.ClientId,
                              ClientName = clients.Where(c => c.Id == aa.ClientId).Select(c => c.Name).FirstOrDefault(),
                              EmployeeId = aa.EmployeeId,
                              EffectiveDate = aa.EffectiveDate,
                              ReleaseDate = aa.ReleaseDate,
                              AssociateAllocationId = aa.AssociateAllocationId,
                              ProgramManagerId = aa.ProgramManagerId,
                              ProgramManagerName = leads.Where(c => c.EmployeeId == aa.ProgramManagerId).Select(c => c.EmployeeName).FirstOrDefault(),
                              ReportingManagerId = aa.ReportingManagerId,
                              ReportingManagerName = leads.Where(c => c.EmployeeId == aa.ReportingManagerId).Select(c => c.EmployeeName).FirstOrDefault(),
                              LeadId = aa.LeadId,
                              LeadName = leads.Where(c => c.EmployeeId == aa.LeadId).Select(c => c.EmployeeName).FirstOrDefault(),
                              IsBillable = aa.IsBillable,
                              IsCritical = aa.IsCritical,
                              ClientBillingPercentage = aa.ClientBillingPercentage,
                              Allocationpercentage = aa.Allocationpercentage,
                              InternalBillingPercentage = aa.InternalBillingPercentage,
                              InternalBillingRoleId = aa.InternalBillingRoleId,
                              ClientBillingRoleCode = aa.ClientBillingRoleCode,
                              FromDate = aa.FromDate,
                              ToDate = aa.ToDate,
                              EmployeeCode = emp.EmployeeCode,
                              EmployeeName = emp.EmployeeName,
                              GradeId = emp.GradeId,
                              GradeName = grades.Where(c => c.Id == emp.GradeId).Select(c => c.Name).FirstOrDefault(),
                              DepartmentId = emp.DepartmentId,
                              DepartmentName = departments.Where(c => c.Id == emp.DepartmentId).Select(c => c.Name).FirstOrDefault(),
                              DesignationId = emp.DesignationId,
                              DesignationName = designations.Where(c => c.Id == emp.DesignationId).Select(c => c.Name).FirstOrDefault(),
                              SkillCode = emp.Skills,
                              IsResigned = emp.IsResigned,
                              IsLongLeave = emp.IsLongLeave
                          }).OrderBy(cbrm => cbrm.EmployeeName).ToList();


                if (financeReportDetails == null && financeReportDetails.Count() == 0)
                {
                    m_Logger.LogInformation("No allocations found");
                    response = new ServiceListResponse<FinanceReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No allocations found"
                    };
                    return response;
                }
                else
                {
                    response = new ServiceListResponse<FinanceReport>()
                    {
                        Items = financeReportDetails,
                        IsSuccessful = true,
                        Message = ""
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetUtilizationReport
        /// <summary>
        /// GetUtilizationReport
        /// </summary>
        /// <param name="filter">filter</param>       
        /// <returns></returns>
        public async Task<ServiceListResponse<UtilizationReport>> GetUtilizationReport(UtilizationReportFilter filter,bool isNightJob)
        {
            ServiceListResponse<UtilizationReport> response = new ServiceListResponse<UtilizationReport>();

            try
            {
                int minExperience = -1;
                int maxExperience = -1;

                if (!string.IsNullOrWhiteSpace(filter.ExperienceRange))
                {
                    string[] experienceRange = filter.ExperienceRange.Split("-");
                    if (experienceRange != null && experienceRange.Length == 2)
                    {
                        minExperience = Convert.ToInt32(experienceRange[0]);
                        maxExperience = Convert.ToInt32(experienceRange[1]);
                    }
                }

                #region Project Service
                UtilizationReportAllocationFilter allocationFilter = new UtilizationReportAllocationFilter
                {
                    ClientId = filter.ClientId,
                    ProjectId = filter.ProjectId,
                    EmployeeId = filter.EmployeeId,
                    ProgramManagerId = filter.ProgramManagerId,
                    AllocationPercentageId = filter.AllocationPercentageId,
                    IsBillable = filter.IsBillable,
                    IsCritical = filter.IsCritical
                };

                ServiceListResponse<UtilizationReportAllocation> allocations =
                    await m_ProjectService.GetUtilizationReportAllocations(allocationFilter, isNightJob);

                if (allocations.Items == null || allocations.Items.Count == 0)
                {
                    m_Logger.LogInformation("No allocations found");
                    response = new ServiceListResponse<UtilizationReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No allocations found"
                    };
                    return response;
                }
                #endregion

                #region Employee Service

                List<int> employeeIds =
                        allocations.Items.Select(aa => aa.EmployeeId).Distinct().ToList();

                List<int> leadIds =
                    allocations.Items.Select(aa => aa.LeadId).Distinct().ToList();

                leadIds.AddRange(allocations.Items.Select(aa => aa.ProgramManagerId).Distinct().ToList());
                leadIds.AddRange(allocations.Items.Select(aa => aa.ReportingManagerId).Distinct().ToList());

                UtilizationReportEmployeeFilter empFilter = new UtilizationReportEmployeeFilter
                {
                    EmployeeIdList = employeeIds,
                    LeadIdList = leadIds,
                    GradeId = filter.GradeId,
                    PracticeAreaId = filter.PracticeAreaId,
                    EmployeeId = filter.EmployeeId,
                    DesignationId = filter.DesignationId,
                    ExperienceId = filter.ExperienceId,
                    MinExperience = minExperience,
                    MaxExperience = maxExperience
                };

                ServiceListResponse<UtilizationReportEmployee> associates = await m_EmployeeService.GetUtilizationReportAssociates(empFilter,isNightJob);

                if (associates.Items == null || associates.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Associates found");
                    response = new ServiceListResponse<UtilizationReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Associates found"
                    };
                    return response;
                }

                List<UtilizationReportEmployee> leads = associates.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Manager).ToList();
                List<UtilizationReportEmployee> employees = associates.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Associate).ToList();

                #endregion

                #region Admin Service
                ServiceListResponse<ReportDetails> masters = await m_OrgService.GetUtilizationReportMasters(isNightJob);

                if (masters.Items == null || masters.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Masters found");
                    response = new ServiceListResponse<UtilizationReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Masters found"
                    };
                    return response;
                }

                List<ReportDetails> clients = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Client).ToList();
                List<ReportDetails> departments = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Department).ToList();
                List<ReportDetails> grades = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Grade).ToList();
                List<ReportDetails> designations = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Designation).ToList();
                List<ReportDetails> practiceAreas = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.PracticeArea).ToList();

                #endregion

                List<UtilizationReport> utilizationReportDetails =
                         (from aa in allocations.Items
                          join emp in employees
                            on aa.EmployeeId equals emp.EmployeeId 
                          select new UtilizationReport
                          {
                              ProjectName = aa.ProjectName,
                              ClientName = clients.Where(c => c.Id == aa.ClientId).Select(c => c.Name).FirstOrDefault(),
                              ProgramManagerName = leads.Where(c => c.EmployeeId == aa.ProgramManagerId).Select(c => c.EmployeeName).FirstOrDefault(),
                              ReportingManagerName = leads.Where(c => c.EmployeeId == aa.ReportingManagerId).Select(c => c.EmployeeName).FirstOrDefault(),
                              LeadName = leads.Where(c => c.EmployeeId == aa.LeadId).Select(c => c.EmployeeName).FirstOrDefault(),
                              IsBillable = aa.IsBillable,
                              IsCritical = aa.IsCritical,
                              Allocationpercentage = aa.Allocationpercentage,
                              JoinDate = emp.JoinDate,
                              Experience = emp.TotalExperience,
                              ExperienceExcludingCareerBreak = emp.ExperienceExcludingCareerBreak,
                              EmployeeCode = emp.EmployeeCode,
                              EmployeeName = emp.EmployeeName,
                              GradeName = grades.Where(c => c.Id == emp.GradeId).Select(c => c.Name).FirstOrDefault(),
                              DesignationName = designations.Where(c => c.Id == emp.DesignationId).Select(c => c.Name).FirstOrDefault(),
                              Technology = practiceAreas.Where(c => c.Id == emp.PracticeAreaId).Select(c => c.Name).FirstOrDefault(),
                              SkillCode = emp.SkillCode,
                              ResignationDate = emp.ResignationDate,
                              LastWorkingDate = emp.LastWorkingDate,
                              IsResigned = emp.IsResigned,
                              IsLongLeave = emp.IsLongLeave,
                              LongLeaveStartDate = emp.LongLeaveStartDate,
                              TentativeJoinDate = emp.TentativeJoinDate,
                              IsFutureProjectMarked=aa.IsFutureProjectMarked,
                              FutureProjectName=aa.FutureProjectName,
                              FutureProjectTentativeDate=aa.FutureProjectTentativeDate,
                              EmployeeId=emp.EmployeeId,
                              ProjectTypeId=aa.ProjectTypeId

                          }).OrderBy(cbrm => cbrm.EmployeeCode).ToList();

                if (utilizationReportDetails == null && utilizationReportDetails.Count() == 0)
                {
                    m_Logger.LogInformation("No allocations found");
                    response = new ServiceListResponse<UtilizationReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No allocations found"
                    };
                    return response;
                }
                else
                {
                    response = new ServiceListResponse<UtilizationReport>()
                    {
                        Items = utilizationReportDetails,
                        IsSuccessful = true,
                        Message = ""
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Domain Report
        /// <summary>
        /// GetDomainReportCount
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<DomainReportCount>> GetDomainReportCount()
        {
            ServiceListResponse<DomainReportCount> response = new ServiceListResponse<DomainReportCount>();

            try
            {
                List<DomainDataCount> domainData = new List<DomainDataCount>();
                #region Project Service

                ServiceListResponse<DomainDataCount> domainDataCurrent = await m_ProjectService.GetDomainWiseResourceCount();

                if (domainDataCurrent.Items != null && domainDataCurrent.Items.Count > 0)
                {
                    domainData.AddRange(domainDataCurrent.Items);
                }

                #endregion

                #region Employee Service

                ServiceListResponse<DomainDataCount> domainDataPrevious = await m_EmployeeService.GetDomainWiseResourceCount();

                if (domainDataPrevious.Items != null && domainDataPrevious.Items.Count > 0)
                {
                    domainData.AddRange(domainDataPrevious.Items);
                }

                #endregion

                if (domainData == null || domainData.Count == 0)
                {
                    m_Logger.LogInformation("No Domains found");
                    response = new ServiceListResponse<DomainReportCount>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Domains found"
                    };
                    return response;
                }

                #region Admin Service
                ServiceListResponse<ReportDetails> masters = await m_OrgService.GetDomainReportMasters();

                if (masters.Items == null || masters.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Masters found");
                    response = new ServiceListResponse<DomainReportCount>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Masters found"
                    };
                    return response;
                }

                List<ReportDetails> domains = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Domain).ToList();
                List<ReportDetails> grades = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Grade).ToList();
                List<ReportDetails> designations = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Designation).ToList();

                #endregion


                List<DomainDataCount> list = new List<DomainDataCount>();
                list = (from ep in domainData
                        group ep by new { ep.DomainID, ep.EmployeeID } into g
                        select new DomainDataCount
                        {
                            EmployeeID = g.Key.EmployeeID,
                            DomainID = g.Key.DomainID,
                        }).ToList();

                List<int> employeeIds =
                       list.Select(aa => aa.EmployeeID).Distinct().ToList();

                ServiceListResponse<GenericType> employeeList = await m_EmployeeService.GetActiveAssociates(employeeIds);

                List<DomainReportCount> domainDetails =
                         (from dd in list
                          join dm in domains on dd.DomainID equals dm.Id
                          join ep in employeeList.Items on dd.EmployeeID equals ep.Id
                          group new { dd.EmployeeID, dd.DomainID, dm.Name } by new { dd.DomainID, dm.Name } into g
                          select new DomainReportCount
                          {
                              ResourceCount = g.Count(),
                              DomainID = g.Key.DomainID,
                              DomainName = g.Key.Name
                          }).OrderBy(cbrm => cbrm.DomainName).ToList();

                if (domainDetails == null || domainDetails.Count() == 0)
                {
                    m_Logger.LogInformation("No Domains found");
                    response = new ServiceListResponse<DomainReportCount>()
                    {
                        Items = new List<DomainReportCount>(),
                        IsSuccessful = true,
                        Message = "No Domains found"
                    };
                    return response;
                }
                else
                {
                    response = new ServiceListResponse<DomainReportCount>()
                    {
                        Items = domainDetails,
                        IsSuccessful = true,
                        Message = ""
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetDomainReport
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<DomainReport>> GetDomainReport(int domainId)
        {
            ServiceListResponse<DomainReport> response = new ServiceListResponse<DomainReport>();

            try
            {

                List<DomainDataCount> domainData = new List<DomainDataCount>();
                #region Project Service

                ServiceListResponse<DomainDataCount> domainDataCurrent = await m_ProjectService.GetDomainWiseResourceCount();

                if (domainDataCurrent.Items != null && domainDataCurrent.Items.Count > 0)
                {
                    domainData.AddRange(domainDataCurrent.Items);
                }

                #endregion

                #region Employee Service

                ServiceListResponse<DomainDataCount> domainDataPrevious = await m_EmployeeService.GetDomainWiseResourceCount();

                if (domainDataPrevious.Items != null && domainDataPrevious.Items.Count > 0)
                {
                    domainData.AddRange(domainDataPrevious.Items);
                }

                #endregion

                if (domainData == null)
                {
                    m_Logger.LogInformation("No Domains found");
                    response = new ServiceListResponse<DomainReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Domains found"
                    };
                    return response;
                }
                else
                {

                    #region Employee Service
                    string employeeIds = string.Join(",", domainData.Where(d => d.DomainID == domainId).Select(c => c.EmployeeID).Distinct().ToList());
                    ServiceListResponse<DomainReportEmployee> domainEmployee = await m_EmployeeService.GetDomainReportAssociates(employeeIds);

                    #endregion

                    if (domainEmployee.Items == null || domainEmployee.Items.Count == 0)
                    {
                        m_Logger.LogInformation("No Domains found");
                        response = new ServiceListResponse<DomainReport>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No Domains found"
                        };
                        return response;
                    }

                    #region Admin Service
                    ServiceListResponse<ReportDetails> masters = await m_OrgService.GetDomainReportMasters();

                    if (masters.Items == null || masters.Items.Count == 0)
                    {
                        m_Logger.LogInformation("No Masters found");
                        response = new ServiceListResponse<DomainReport>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No Masters found"
                        };
                        return response;
                    }

                    List<ReportDetails> domains = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Domain).ToList();
                    List<ReportDetails> grades = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Grade).ToList();
                    List<ReportDetails> designations = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Designation).ToList();

                    #endregion

                    List<DomainReport> domainDetails =
                             (from dd in domainEmployee.Items
                              join gr in grades on dd.GradeId equals gr.Id into gs
                              from gr in gs.DefaultIfEmpty()
                              join dg in designations on dd.DesignationId equals dg.Id into ds
                              from dg in ds.DefaultIfEmpty()
                              select new DomainReport
                              {
                                  EmployeeCode = dd.EmployeeCode,
                                  EmployeeName = dd.EmployeeName,
                                  Experience = dd.Experience,
                                  Grade = (gr != null ? gr.Name : ""),
                                  Designation = (dg != null ? dg.Name : "")
                              }).OrderBy(cbrm => cbrm.EmployeeName).ToList();


                    if (domainDetails == null && domainDetails.Count() == 0)
                    {
                        m_Logger.LogInformation("No Domains found");
                        response = new ServiceListResponse<DomainReport>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "No Domains found"
                        };
                        return response;
                    }
                    else
                    {
                        response = new ServiceListResponse<DomainReport>()
                        {
                            Items = domainDetails,
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

        #region Get TalentPool Report
        /// <summary>
        /// GetTalentPoolReport
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<TalentPoolReportCount>> GetTalentPoolReportCount()
        {
            ServiceListResponse<TalentPoolReportCount> response = new ServiceListResponse<TalentPoolReportCount>();

            try
            {

                #region Admin Service
                List<int> projectTypeIds = new List<int>();
                ServiceListResponse<ReportDetails> masters = await m_OrgService.GetTalentPoolReportMasters();

                if (masters.Items == null || masters.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Masters found");
                    response = new ServiceListResponse<TalentPoolReportCount>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Masters found"
                    };
                    return response;
                }

                List<ReportDetails> projectTypes = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.ProjectType).ToList();
                List<ReportDetails> grades = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Grade).ToList();
                List<ReportDetails> designations = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Designation).ToList();

                projectTypeIds = projectTypes.Select(c => c.Id).ToList();

                #endregion

                #region Employee Service

                ServiceListResponse<TalentPoolReportCount> talentPool = await m_ProjectService.GetTalentPoolWiseResourceCount(projectTypeIds);

                if (talentPool.Items == null || talentPool.Items.Count == 0)
                {
                    m_Logger.LogInformation("No TalentPool found");
                    response = new ServiceListResponse<TalentPoolReportCount>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No TalentPool found"
                    };
                    return response;
                }
                else
                {
                    response = new ServiceListResponse<TalentPoolReportCount>()
                    {
                        Items = talentPool.Items,
                        IsSuccessful = true,
                        Message = ""
                    };
                    return response;
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// TalentPoolReport
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<TalentPoolReport>> GetTalentPoolReport(int projectId)
        {
            ServiceListResponse<TalentPoolReport> response = new ServiceListResponse<TalentPoolReport>();

            try
            {
                #region Employee Service

                ServiceListResponse<TalentPoolReportEmployee> talentPoolData = await m_EmployeeService.GetTalentPoolReportAssociates(projectId);

                if (talentPoolData.Items == null || talentPoolData.Items.Count == 0)
                {
                    m_Logger.LogInformation("No TalentPools found");
                    response = new ServiceListResponse<TalentPoolReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No TalentPools found"
                    };
                    return response;
                }

                #endregion

                #region Admin Service
                ServiceListResponse<ReportDetails> masters = await m_OrgService.GetTalentPoolReportMasters();

                if (masters.Items == null || masters.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Masters found");
                    response = new ServiceListResponse<TalentPoolReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Masters found"
                    };
                    return response;
                }


                List<ReportDetails> grades = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Grade).ToList();
                List<ReportDetails> designations = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Designation).ToList();

                #endregion

                List<TalentPoolReport> domainDetails =
                         (from dd in talentPoolData.Items
                          join gr in grades on dd.GradeId equals gr.Id into gs
                          from gr in gs.DefaultIfEmpty()
                          join dg in designations on dd.DesignationId equals dg.Id into ds
                          from dg in ds.DefaultIfEmpty()
                          select new TalentPoolReport
                          {
                              EmployeeCode = dd.EmployeeCode,
                              EmployeeName = dd.EmployeeName,
                              Experience = dd.Experience,
                              Grade = (gr != null ? gr.Name : ""),
                              Designation = (dg != null ? dg.Name : ""),
                              IsResigned = dd.IsResigned,
                              IsLongLeave = dd.IsLongLeave,
                              DurationInDays = dd.DurationInDays,
                              FutureProjectName=dd.FutureProjectName,
                              FutureProjectTentativeDate=dd.FutureProjectTentativeDate
                          }).OrderByDescending(cbrm => cbrm.DurationInDays).ToList();


                if (domainDetails == null && domainDetails.Count() == 0)
                {
                    m_Logger.LogInformation("No TalentPools found");
                    response = new ServiceListResponse<TalentPoolReport>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No TalentPools found"
                    };
                    return response;
                }
                else
                {
                    response = new ServiceListResponse<TalentPoolReport>()
                    {
                        Items = domainDetails,
                        IsSuccessful = true,
                        Message = ""
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetEmployeeDetailsBySkill
        /// <summary>
        /// GetEmployeeDetailsBySkill
        /// </summary>
        /// <param name="filter">filter</param>       
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateSkillSearch>> GetEmployeeDetailsBySkill(AssociateSkillSearchFilter filter)
        {
            ServiceListResponse<AssociateSkillSearch> response = new ServiceListResponse<AssociateSkillSearch>();
            List<AssociateSkillSearch> list = new List<AssociateSkillSearch>();
            try
            {
                #region Employee Service             

                ServiceListResponse<SkillSearchEmployee> associates = await m_EmployeeService.GetSkillSearchAssociates(filter);

                if (associates.Items == null)
                {
                    m_Logger.LogInformation("No Associates found");
                    response = new ServiceListResponse<AssociateSkillSearch>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Associates found"
                    };
                    return response;
                }
                else if (associates.Items.Count == 0)
                {
                    response = new ServiceListResponse<AssociateSkillSearch>()
                    {
                        Items = list,
                        IsSuccessful = true,
                        Message = ""
                    };
                    return response;
                }



                #endregion

                #region Admin Service
                ServiceListResponse<ReportDetails> masters = await m_OrgService.GetSkillSearchMasters();

                if (masters.Items == null || masters.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Masters found");
                    response = new ServiceListResponse<AssociateSkillSearch>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Masters found"
                    };
                    return response;
                }

                List<ReportDetails> grades = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Grade).ToList();
                List<ReportDetails> designations = masters.Items.Where(c => c.RecordType == (int)RecordTypeEnum.Designation).ToList();

                #endregion

                list =
                          (from aa in associates.Items
                           select new AssociateSkillSearch
                           {
                               EmployeeId = aa.EmployeeId,
                               EmployeeName = aa.EmployeeName,
                               ProjectName = aa.ProjectName,
                               Experience = aa.Experience,
                               Designation = designations.Where(c => c.Id == aa.DesignationId).Select(c => c.Name).FirstOrDefault(),
                               Grade = grades.Where(c => c.Id == aa.GradeId).Select(c => c.Name).FirstOrDefault(),
                               Allocationpercentage = aa.Allocationpercentage,
                               LeadName = aa.LeadName,
                               ManagerName = aa.ManagerName,
                               IsCritical = aa.IsCritical,
                               IsBillable = aa.IsBillable,
                               PrimarySkill = aa.PrimarySkill,
                               SecondarySkill = aa.SecondarySkill
                           }).OrderBy(cbrm => cbrm.EmployeeName).ToList();


                if (list == null)
                {
                    m_Logger.LogInformation("No allocations found");
                    response = new ServiceListResponse<AssociateSkillSearch>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No allocations found"
                    };
                    return response;
                }
                else
                {
                    response = new ServiceListResponse<AssociateSkillSearch>()
                    {
                        Items = list,
                        IsSuccessful = true,
                        Message = ""
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get ServiceTypeReport
        /// <summary>
        /// Get ServiceTypeReport
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<ServiceTypeReportCount>> GetServiceTypeReport(string filter)
        {
            ServiceListResponse<ServiceTypeReportCount> response = new ServiceListResponse<ServiceTypeReportCount>();
            try
            {
                string[] filterSplit = null;
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    filterSplit = filter.Split(",");
                }

                List<ServiceTypeCount> domainData = new List<ServiceTypeCount>();


                ServiceListResponse<ServiceTypeCount> domainDataCurrent = await m_ProjectService.GetServiceTypeResourceCount();

                if (domainDataCurrent.Items != null && domainDataCurrent.Items.Count > 0)
                {
                    domainData.AddRange(domainDataCurrent.Items);
                }
                if (domainData == null || domainData.Count == 0)
                {
                    m_Logger.LogInformation("No Domains found");
                    response = new ServiceListResponse<ServiceTypeReportCount>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No ServiceType found"
                    };
                    return response;
                }

                ServiceListResponse<ServiceType> masters = await m_OrgService.GetServiceTypetMasters();

                if (masters.Items == null || masters.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Masters found");
                    response = new ServiceListResponse<ServiceTypeReportCount>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Masters found"
                    };
                    return response;
                }

                List<ServiceTypeReportCount> serviceTypeDetails =
                  (from dd in masters.Items
                   join dm in domainData on dd.ProjectTypeId equals dm.ServiceTypeId
                   // from dm in Details.DefaultIfEmpty()

                   group new { dd.ProjectTypeId } by new { dd.ProjectTypeId, dd.ProjectTypeCode, dd.Description } into g
                   select new ServiceTypeReportCount()
                   {

                       ResourceCount = g.Count(),
                       ServiceTypeId = g.Key.ProjectTypeId,
                       ServiceTypeCode = g.Key.ProjectTypeCode,
                       ServiceTypeDescription = g.Key.Description

                   }).OrderBy(cbrm => cbrm.ServiceTypeDescription).ToList();
                List<int> serviceTypeId = serviceTypeDetails.Select(st => st.ServiceTypeId).Distinct().ToList();
                foreach (ServiceType st in masters.Items)
                {
                    if (!serviceTypeId.Contains(st.ProjectTypeId))
                    {
                        ServiceTypeReportCount sType = new ServiceTypeReportCount()
                        {
                            ResourceCount = 0,
                            ServiceTypeId = st.ProjectTypeId,
                            ServiceTypeCode = st.ProjectTypeCode,
                            ServiceTypeDescription = st.Description
                        };
                        serviceTypeDetails.Add(sType);
                    }

                }

                //List<ServiceTypeReportCount> serviceTypeDetails =
                //  (from dd in masters.Items
                //   join dm in domainData on dd.ServiceTypeId equals dm.ServiceTypeId into Details
                //   from dm in Details.DefaultIfEmpty()


                //   select new ServiceTypeReportCount()
                //   {


                //       ServiceTypeId = dd.ServiceTypeId,
                //       ServiceTypeName = dd.ServiceTypeName,
                //       ServiceTypeDescription = dd.ServiceDescription

                //   }).OrderBy(cbrm => cbrm.ServiceTypeName).ToList();
                ;
                //  List<ServiceTypeReportCount> sDetail = new List<ServiceTypeReportCount> ();
                List<ServiceTypeReportCount> sDetails = new List<ServiceTypeReportCount>();


                if (!string.IsNullOrWhiteSpace(filter) && filter != "-1")
                {
                    foreach (string fil in filterSplit)
                    {
                        ServiceTypeReportCount sDetail = new ServiceTypeReportCount();
                        int filNum = Convert.ToInt32(fil);
                        sDetail = serviceTypeDetails.Where(st => st.ServiceTypeId == filNum).FirstOrDefault();
                        sDetails.Add(sDetail);
                    }
                    serviceTypeDetails = sDetails;
                }

                if (serviceTypeDetails == null || serviceTypeDetails.Count() == 0)
                {
                    m_Logger.LogInformation("No ServiceTypes found");
                    response = new ServiceListResponse<ServiceTypeReportCount>()
                    {
                        Items = new List<ServiceTypeReportCount>(),
                        IsSuccessful = true,
                        Message = "No ServiceTypes found"
                    };
                    return response;
                }
                else
                {
                    response = new ServiceListResponse<ServiceTypeReportCount>()
                    {
                        Items = serviceTypeDetails,
                        IsSuccessful = true,
                        Message = ""
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region ServiceTypeProjectDetails
        /// <summary>
        /// GetServiceTypeReport
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectRespose>> GetServiceTypeReportProject(int serviceTypeId)
        {
            ServiceListResponse<ProjectRespose> response = new ServiceListResponse<ProjectRespose>();

            try
            {
                ServiceListResponse<ProjectRespose> projects = await m_ProjectService.GetAllProjects();



                if (projects.Items == null || projects.Items.Count == 0)
                {
                    m_Logger.LogInformation("No projects found");
                    response = new ServiceListResponse<ProjectRespose>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Projects found"
                    };
                    return response;
                }
                var projectsDetails = projects.Items.Where(st => st.ProjectTypeId == serviceTypeId).ToList();






                if (projectsDetails == null && projectsDetails.Count() == 0)
                {
                    m_Logger.LogInformation("No Projects found");
                    response = new ServiceListResponse<ProjectRespose>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Projects found"
                    };
                    return response;
                }
                else
                {
                    response = new ServiceListResponse<ProjectRespose>()
                    {
                        Items = projectsDetails,
                        IsSuccessful = true,
                        Message = ""
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        # region GetResourceReportByFilter
        /// <summary>
        /// GetResourceReportByFilter
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<UtilizationReport>> GetResourceReportByFilter(bool isNightJob)
        {
            var filter = new UtilizationReportFilter();
            ServiceListResponse<UtilizationReport> response = new ServiceListResponse<UtilizationReport>();
            try
            {
                var utilizationReport = await GetUtilizationReport(filter, isNightJob);
                if (utilizationReport.Items != null)
                {
                    var CriticalNonBillableEmployeeData = utilizationReport.Items.Where(employee => 
                      employee.IsResigned == false && employee.IsLongLeave == false && employee.Allocationpercentage != 0 && employee.ProjectName.ToUpper() != "Training".ToUpper()).ToList();
                    if (CriticalNonBillableEmployeeData != null)
                    {
                        response.IsSuccessful = true;
                        response.Items = CriticalNonBillableEmployeeData;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;

        }
        #endregion

        #region GetCriticalResourceReport
        /// <summary>
        /// GetCriticalResourceReport
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateInformationReport>> GetCriticalResourceReport()
        {
            var filter = new UtilizationReportFilter();
            ServiceListResponse<AssociateInformationReport> response = new ServiceListResponse<AssociateInformationReport>();
            try
            {

                var result = await m_ProjectService.GetCriticalResourceReport();
                if (result.Items != null)
                {
                    response.IsSuccessful = true;
                    response.Items = result.Items;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            var filter = new UtilizationReportFilter();
            ServiceListResponse<AssociateInformationReport> response = new ServiceListResponse<AssociateInformationReport>();
            try
            {

                var result = await m_ProjectService.GetNonCriticalResourceReport();
               
                if (result.Items != null)
                {
                    response.IsSuccessful = true;
                    response.Items = result.Items;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;

        }

        #endregion

        #region GetTalentPoolResourceReport
        /// <summary>
        /// GetTalentPoolResourceReport
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<UtilizationReport>> GetTalentPoolResourceReport(bool isNightJob)
        {
            var filter = new UtilizationReportFilter();
            ServiceListResponse<UtilizationReport> response = new ServiceListResponse<UtilizationReport>();
            try
            {
                ServiceListResponse<UtilizationReport> resourceReport =await GetUtilizationReport(filter,isNightJob);

                //Talent pool project typenid
                int TP_projectTypeId =  m_OrgService.GetProjectTypeByCode("Talent Pool").Result.Item.ProjectTypeId;
                List<UtilizationReport> talentpoolResourceReport = resourceReport.Items.Where(resourcereport => resourcereport.ProjectTypeId == TP_projectTypeId).ToList();
               
                if (talentpoolResourceReport != null)
                {
                    response.IsSuccessful = true;
                    response.Items = talentpoolResourceReport;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "no record found";
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured  while fetching talent-pool resource data",ex.StackTrace);
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching talent-pool resource data";               
            }
            return response;

        }

        #endregion

        #region GetAssociatesForFutureAllocation
        /// <summary>
        /// GetAssociatesForFutureAllocation
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateInformationReport>> GetAssociatesForFutureAllocation()
        {
            var filter = new UtilizationReportFilter();
            ServiceListResponse<AssociateInformationReport> response = new ServiceListResponse<AssociateInformationReport>();
            try
            {
                ServiceListResponse<UtilizationReport> resourceReport = await GetUtilizationReport(filter ,false);

                List<AssociateInformationReport> AssociateInformation = resourceReport.Items.Where(employee =>employee.IsResigned == false && employee.IsLongLeave == false && employee.Allocationpercentage != 0 
                && employee.ProjectName.ToUpper() != "Training".ToUpper() && employee.IsFutureProjectMarked!=true).Select(x=>new AssociateInformationReport {EmployeeId=x.EmployeeId,AssociateName=x.EmployeeName }).ToList();

                if (AssociateInformation != null)
                {
                    response.IsSuccessful = true;
                    response.Items = AssociateInformation;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "no record found";
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured  while fetching resource data", ex.StackTrace);
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching resource data";
            }
            return response;

        }

        #endregion

        #region GetNonCriticalResourceBillingReport
        /// <summary>
        /// GetNonCriticalResourceBillingReport
        /// </summary>       
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateInformationReport>> GetNonCriticalResourceBillingReport()
        {
            var filter = new UtilizationReportFilter();
            ServiceListResponse<AssociateInformationReport> response = new ServiceListResponse<AssociateInformationReport>();
            try
            {
                var AssociateInformation =await(m_ProjectService.GetNonCriticalResourceBillingReport());
                if (AssociateInformation.IsSuccessful)
                {
                    response.IsSuccessful = true;
                    response.Items = AssociateInformation.Items;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "no record found";
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured  while fetching resource data", ex.StackTrace);
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching resource data";
            }
            return response;

        }

        #endregion

        #region GetParkingSloteport
        public async Task<ServiceListResponse<ParkingSlotReport>> GetParkingSloteport(ParkingSearchFilter parkingSearchFilter)
        {
            ServiceListResponse<ParkingSlotReport> response = new ServiceListResponse<ParkingSlotReport>();
            try
            {
                response = (await m_EmployeeService.GetParkingSlotReport(parkingSearchFilter));
              
            }
            catch(Exception ex)
            {
                m_Logger.LogError("Error occured  while fetching Parking Slot report details", ex.StackTrace);
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Parking Slot report details";
            }

            return response;
        }
        #endregion
    }
}