using AutoMapper;
using HRMS.Common.Enums;
//using HRMS.Common.Redis;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to get the data required for reports
    /// </summary>
    public class ReportService : IReportService
    {
        #region Global Varibles

        private readonly ILogger<ReportService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        //private readonly ICacheService m_CacheService;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;

        #endregion

        #region Constructor
        public ReportService(EmployeeDBContext employeeDBContext,
                               ILogger<ReportService> logger,
                               IHttpClientFactory clientFactory,
                               IOptions<APIEndPoints> apiEndPoints,
                               //ICacheService cacheService,
                               IProjectService projectService,
                               IOrganizationService orgService)
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

        #region Get Finance Report Associates
        /// <summary>
        /// Get Finance Report Associates
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<FinanceReportEmployee>> GetFinanceReportAssociates(FinanceReportEmployeeFilter filter)
        {
            ServiceListResponse<FinanceReportEmployee> response = new ServiceListResponse<FinanceReportEmployee>();

            try
            {
                List<FinanceReportEmployee> employeeDetails = new List<FinanceReportEmployee>();
                List<FinanceReportEmployee> leadDetails = new List<FinanceReportEmployee>();

                if (filter.EmployeeIdList != null && filter.EmployeeIdList.Count > 0)
                {
                    employeeDetails = await (from emp in m_EmployeeContext.Employees
                                             join ae in m_EmployeeContext.AssociateExit.Where(x => x.StatusId != Convert.ToInt32(AssociateExitStatusCodesNew.ResignationRevoked)) on emp.EmployeeId equals ae.EmployeeId into resig
                                             from res in resig.DefaultIfEmpty()
                                             join al in m_EmployeeContext.AssociateLongLeaves.Where(al => al.IsActive == true) on emp.EmployeeId equals al.EmployeeId into leave
                                             from lv in leave.DefaultIfEmpty()
                                             where filter.EmployeeIdList.Contains(emp.EmployeeId)
                                             select new FinanceReportEmployee
                                             {
                                                 EmployeeId = emp.EmployeeId,
                                                 EmployeeCode = emp.EmployeeCode,
                                                 EmployeeName = emp.FirstName + " " + emp.LastName,
                                                 IsActive = (emp.IsActive ?? false),
                                                 GradeId = emp.GradeId,
                                                 DesignationId = emp.DesignationId,
                                                 DepartmentId = emp.DepartmentId,
                                                 RecordType = (int)RecordTypeEnum.Associate,
                                                 IsResigned = res == null ? false : (res.IsActive.Value == true && res.ResignationDate.HasValue && res.ResignationDate.Value.Date <= filter.ToDate.Date ? true : false),
                                                 IsLongLeave = ((lv == null ? false : (lv.IsActive != null ? lv.IsActive.Value : false)) == true ? (lv.LongLeaveStartDate.Date <= filter.ToDate ? true : false) : false),
                                                 LastWorkingDate = res == null ? null : res.ExitDate,
                                             })
                                            .ToListAsync();

                    List<SkillSearch> SkillSearchData = m_EmployeeContext.SkillSearch.Where(Skill => filter.EmployeeIdList.Contains((int)Skill.EmployeeId) && Skill.IsSkillPrimary == true).ToList();
                    var skills = (from empID in filter.EmployeeIdList
                                  select new
                                  {
                                      EmployeeId = empID,
                                      Skills = String.Join(", ", SkillSearchData.Where(skill => skill.EmployeeId == empID).Select(c => c.SkillName).Distinct().ToList())
                                  }).ToList();

                    skills.ForEach(skill =>
                    {
                        employeeDetails.ForEach(employee =>
                        {
                            if (skill.EmployeeId == employee.EmployeeId)
                            {
                                employee.Skills = skill.Skills;
                            }
                        });
                    });

                    leadDetails = await m_EmployeeContext.Employees
                                   .Where(emp => filter.LeadIdList.Contains(emp.EmployeeId))
                                   .Select(emp => new FinanceReportEmployee
                                   {
                                       EmployeeId = emp.EmployeeId,
                                       EmployeeCode = emp.EmployeeCode,
                                       EmployeeName = emp.FirstName + " " + emp.LastName,
                                       IsActive = (emp.IsActive ?? false),
                                       GradeId = emp.GradeId,
                                       DesignationId = emp.DesignationId,
                                       DepartmentId = emp.DepartmentId,
                                       RecordType = (int)RecordTypeEnum.Manager,
                                       Skills = ""
                                   })
                                   .ToListAsync();

                    employeeDetails.AddRange(leadDetails);
                }



                return response = new ServiceListResponse<FinanceReportEmployee>()
                {
                    Items = employeeDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<FinanceReportEmployee>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region Get Utlilization Report Associates
        /// <summary>
        /// Get Utlilization Report Associates
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<UtilizationReportEmployee>> GetUtilizationReportAssociates(UtilizationReportEmployeeFilter filter)
        {
            ServiceListResponse<UtilizationReportEmployee> response = new ServiceListResponse<UtilizationReportEmployee>();

            try
            {
                List<UtilizationReportEmployee> employeeDetails = new List<UtilizationReportEmployee>();
                List<UtilizationReportEmployee> leadDetails = new List<UtilizationReportEmployee>();
                if (filter.EmployeeIdList != null && filter.EmployeeIdList.Count > 0)
                {
                    IQueryable<UtilizationReportEmployee> query = (from emp in m_EmployeeContext.Employees
                                                                   join ae in m_EmployeeContext.AssociateExit.Where(x => x.StatusId != Convert.ToInt32(AssociateExitStatusCodesNew.ResignationRevoked)) on emp.EmployeeId equals ae.EmployeeId into resig
                                                                   from res in resig.DefaultIfEmpty()
                                                                   join al in m_EmployeeContext.AssociateLongLeaves.Where(al => al.IsActive == true) on emp.EmployeeId equals al.EmployeeId into leave
                                                                   from lv in leave.DefaultIfEmpty()
                                                                   where filter.EmployeeIdList.Contains(emp.EmployeeId)
                                                                   select new UtilizationReportEmployee
                                                                   {
                                                                       EmployeeId = emp.EmployeeId,
                                                                       EmployeeCode = emp.EmployeeCode,
                                                                       EmployeeName = emp.FirstName + " " + emp.LastName,
                                                                       IsActive = (emp.IsActive ?? false),
                                                                       GradeId = emp.GradeId,
                                                                       DesignationId = emp.DesignationId,
                                                                       DepartmentId = emp.DepartmentId,
                                                                       EmployeeTypeId = emp.EmployeeTypeId,
                                                                       EmployeeType = "", //empType.EmpType,
                                                                       Experience = emp.Experience,
                                                                       PracticeAreaId = emp.CompetencyGroup ?? 0,
                                                                       CompetencyGroup = emp.CompetencyGroup ?? 0,
                                                                       AadharNumber = emp.AadharNumber,
                                                                       CareerBreak = emp.CareerBreak,
                                                                       TotalExperience = emp.TotalExperience ?? 0,
                                                                       JoinDate = emp.JoinDate,
                                                                       ExperienceExcludingCareerBreak = emp.ExperienceExcludingCareerBreak ?? 0,
                                                                       RecordType = (int)RecordTypeEnum.Associate,
                                                                       ResignationDate = res == null ? null : string.Format("{0:yyyy-MM-dd}", res.ResignationDate),
                                                                       LastWorkingDate = res == null ? null : string.Format("{0:yyyy-MM-dd}", res.ExitDate),
                                                                       IsResigned = res == null ? false : res.IsActive.Value,
                                                                       IsLongLeave = ((lv == null ? false : (lv.IsActive != null ? lv.IsActive.Value : false)) == true ? (lv.LongLeaveStartDate.Date <= DateTime.Now.Date ? true : false) : false),
                                                                       LongLeaveStartDate = lv == null ? null : string.Format("{0:yyyy-MM-dd}", lv.LongLeaveStartDate),
                                                                       TentativeJoinDate = lv == null ? null : string.Format("{0:yyyy-MM-dd}", lv.TentativeJoinDate)
                                                                   });

                    if (filter.EmployeeId > 0)
                        query = (from re in query
                                 where re.EmployeeId == filter.EmployeeId
                                 select re);

                    if (filter.GradeId > 0)
                        query = (from re in query
                                 where re.GradeId == filter.GradeId
                                 select re);

                    if (filter.DesignationId > 0)
                        query = (from re in query
                                 where re.DesignationId == filter.DesignationId
                                 select re);

                    if (filter.PracticeAreaId > 0)
                        query = (from re in query
                                 where re.PracticeAreaId == filter.PracticeAreaId
                                 select re);

                    if (filter.MinExperience >= 0 && filter.MaxExperience > 0)
                        query = (from re in query
                                 where re.TotalExperience >= filter.MinExperience && re.TotalExperience <= filter.MaxExperience
                                 select re);

                    employeeDetails = await query.ToListAsync();

                    List<SkillSearch> SkillSearchData = m_EmployeeContext.SkillSearch.Where(Skill => filter.EmployeeIdList.Contains((int)Skill.EmployeeId) && Skill.IsSkillPrimary == true).ToList();
                    var skills = (from empID in filter.EmployeeIdList
                                  select new
                                  {
                                      EmployeeId = empID,
                                      Skills = String.Join(", ", SkillSearchData.Where(skill => skill.EmployeeId == empID).Select(c => c.SkillName).Distinct().ToList())
                                  }).ToList();

                    skills.ForEach(skill =>
                    {
                        employeeDetails.ForEach(employee =>
                        {
                            if (skill.EmployeeId == employee.EmployeeId)
                            {
                                employee.SkillCode = skill.Skills;
                            }
                        });
                    });

                    leadDetails = await m_EmployeeContext.Employees
                                   .Where(emp => filter.LeadIdList.Contains(emp.EmployeeId))
                                   .Select(emp => new UtilizationReportEmployee
                                   {
                                       EmployeeId = emp.EmployeeId,
                                       EmployeeCode = emp.EmployeeCode,
                                       EmployeeName = emp.FirstName + " " + emp.LastName,
                                       IsActive = (emp.IsActive ?? false),
                                       GradeId = null,
                                       DesignationId = null,
                                       DepartmentId = null,
                                       EmployeeTypeId = null,
                                       EmployeeType = "",
                                       Experience = 0,
                                       CompetencyGroup = 0,
                                       AadharNumber = "",
                                       CareerBreak = 0,
                                       TotalExperience = 0,
                                       ExperienceExcludingCareerBreak = 0,
                                       RecordType = (int)RecordTypeEnum.Manager,
                                       SkillCode = ""
                                   })
                                   .ToListAsync();

                    employeeDetails.AddRange(leadDetails);
                }

                return response = new ServiceListResponse<UtilizationReportEmployee>()
                {
                    Items = employeeDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<UtilizationReportEmployee>()
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
                list = await (from ep in m_EmployeeContext.EmployeeProjects
                              join emp in m_EmployeeContext.Employees
                              on ep.EmployeeId equals emp.EmployeeId
                              where ep.IsActive == true && emp.IsActive == true && ep.EmployeeId.HasValue && ep.DomainId.HasValue
                              group ep by new { ep.DomainId, ep.EmployeeId } into g
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

        #region Get Domain Report Associates
        /// <summary>
        /// GetDomainReportAssociates
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<DomainReportEmployee>> GetDomainReportAssociates(string employeeIds)
        {
            ServiceListResponse<DomainReportEmployee> response = new ServiceListResponse<DomainReportEmployee>();

            try
            {
                List<int> employeeList = employeeIds.Split(",".ToCharArray()).Select(Int32.Parse).ToList();
                List<DomainReportEmployee> list = new List<DomainReportEmployee>();
                list = await (from emp in m_EmployeeContext.Employees
                              where emp.IsActive == true && employeeList.Contains(emp.EmployeeId)
                              select new DomainReportEmployee
                              {
                                  EmployeeId = emp.EmployeeId,
                                  EmployeeCode = emp.EmployeeCode,
                                  EmployeeName = emp.FirstName + " " + emp.LastName,
                                  DesignationId = emp.DesignationId ?? 0,
                                  GradeId = emp.GradeId ?? 0,
                                  Experience = emp.TotalExperience ?? 0

                              }).Distinct().ToListAsync();

                return response = new ServiceListResponse<DomainReportEmployee>()
                {
                    Items = list,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                return response = new ServiceListResponse<DomainReportEmployee>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region Get Talent Pool Report Associates
        /// <summary>
        /// GetTalentPoolReportAssociates
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<TalentPoolReportEmployee>> GetTalentPoolReportAssociates(int projectId)
        {

            ServiceListResponse<TalentPoolReportEmployee> response = new ServiceListResponse<TalentPoolReportEmployee>();

            try
            {
                #region Project Service

                ServiceListResponse<ProjectResourceData> employee = await m_ProjectService.GetResourceByProject(projectId);

                if (employee == null || employee.Items.Count == 0)
                {
                    m_Logger.LogInformation("No Employee found");
                    response = new ServiceListResponse<TalentPoolReportEmployee>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Employee found"
                    };
                    return response;
                }

                #endregion

                List<int> employeeIdList = employee.Items.Select(c => c.EmployeeId).Distinct().ToList();
                List<TalentPoolReportEmployee> list = new List<TalentPoolReportEmployee>();
                list = await (from emp in m_EmployeeContext.Employees
                                  // AssociateExit for new data
                              join ae in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true && w.StatusId != Convert.ToInt32(AssociateExitStatusCodesNew.ResignationRevoked)) on emp.EmployeeId equals ae.EmployeeId into assexit
                              from asse in assexit.DefaultIfEmpty()
                              join al in m_EmployeeContext.AssociateLongLeaves.Where(al => al.IsActive == true) on emp.EmployeeId equals al.EmployeeId into leave
                              from lv in leave.DefaultIfEmpty()
                              where employeeIdList.Contains(emp.EmployeeId)
                              select new TalentPoolReportEmployee
                              {
                                  EmployeeId = emp.EmployeeId,
                                  EmployeeCode = emp.EmployeeCode,
                                  EmployeeName = emp.FirstName + " " + emp.LastName,
                                  DesignationId = emp.DesignationId ?? 0,
                                  GradeId = emp.GradeId ?? 0,
                                  Experience = emp.TotalExperience ?? 0,
                                  IsResigned = asse == null ? false : asse.IsActive.Value,
                                  IsLongLeave = lv == null ? false : lv.IsActive != null ? lv.IsActive.Value : false,
                                  DurationInDays = 0,
                                  LastWorkingDate = asse == null ? null : asse.ExitDate,
                              }).Distinct().ToListAsync();

                list.ForEach(i =>
                {
                    employee.Items.ForEach(emp =>
                    {
                        if (i.EmployeeId == emp.EmployeeId)
                        {
                            i.DurationInDays = emp.DurationInDays;
                            i.FutureProjectName = emp.FutureProjectName;
                            i.FutureProjectTentativeDate = emp.FutureProjectTentativeDate;
                        }
                    });
                });

                return response = new ServiceListResponse<TalentPoolReportEmployee>()
                {
                    Items = list,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                return response = new ServiceListResponse<TalentPoolReportEmployee>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region Get SkillSearch Associates
        /// <summary>
        /// GetSkillSearchAssociates
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<SkillSearchEmployee>> GetSkillSearchAssociates(SkillSearchFilter filter)
        {
            ServiceListResponse<SkillSearchEmployee> response = new ServiceListResponse<SkillSearchEmployee>();

            try
            {
                #region Project Service

                ServiceListResponse<SkillSearchAllocation> allocations = await m_ProjectService.GetSkillSearchAllocations();

                if (allocations == null || allocations.Items.Count == 0)
                {
                    m_Logger.LogInformation("No allocations found");
                    response = new ServiceListResponse<SkillSearchEmployee>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No allocations found"
                    };
                    return response;
                }

                #endregion

                List<SkillSearchEmployee> employeeDetails = new List<SkillSearchEmployee>();

                bool? isBillable = null;
                if (filter.IsBillable == true && filter.IsnonBillable == false)
                    isBillable = true;
                else if (filter.IsBillable == false && filter.IsnonBillable == true)
                    isBillable = false;

                bool? isCritical = null;
                if (filter.IsCritical == true && filter.IsnonCritical == false)
                    isCritical = true;
                else if (filter.IsCritical == false && filter.IsnonCritical == true)
                    isCritical = false;

                bool? isSkillPrimary = null;
                if (filter.IsPrimary == true && filter.IsSecondary == false)
                    isSkillPrimary = true;
                else if (filter.IsSecondary == true && filter.IsPrimary == false)
                    isSkillPrimary = false;

                List<int> skillIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(filter.SkillIds))
                    skillIds = filter.SkillIds.Split(',').Select(Int32.Parse).ToList();

                List<int> projectIds =
                       allocations.Items.Select(aa => aa.ProjectId).Distinct().ToList();

                List<int> empIds =
                        allocations.Items.Select(aa => aa.EmployeeId).Distinct().ToList();

                List<int> leadIds =
                        allocations.Items.Select(aa => aa.LeadId).Distinct().ToList();

                leadIds =
                        allocations.Items.Select(aa => aa.ReportingManagerId).Distinct().ToList();

                List<SkillSearchEmployee> leadDetails = await m_EmployeeContext.Employees
                                   .Where(emp => leadIds.Contains(emp.EmployeeId))
                                   .Select(emp => new SkillSearchEmployee
                                   {
                                       EmployeeId = emp.EmployeeId,
                                       EmployeeName = emp.FirstName + " " + emp.LastName
                                   })
                                   .ToListAsync();

                IQueryable<SkillSearchEmployee> query = (from ss in m_EmployeeContext.SkillSearch
                                                             //join al in allocations.Items
                                                             //on new { a = ss.ProjectId.Value, b = ss.EmployeeId.Value } equals new { a = al.ProjectId, b = al.EmployeeId }
                                                         join em in m_EmployeeContext.Employees
                                                              on ss.EmployeeId equals em.EmployeeId
                                                         where empIds.Contains(ss.EmployeeId.Value) && projectIds.Contains(ss.ProjectId.Value)
                                                         select new SkillSearchEmployee
                                                         {
                                                             SkillId = ss.SkillId.Value,
                                                             EmployeeId = em.EmployeeId,
                                                             ProjectId = ss.ProjectId.Value,
                                                             EmployeeName = em.FirstName + " " + em.LastName,
                                                             Experience = em.TotalExperience ?? 0,
                                                             DesignationId = em.DesignationId ?? 0,
                                                             GradeId = em.GradeId ?? 0,
                                                             IsSkillPrimary = ss.IsSkillPrimary ?? false,
                                                             IsCritical = ss.IsCritical ?? false,
                                                             IsBillable = ss.IsBillable ?? false,
                                                             PrimarySkill = ((ss.IsSkillPrimary ?? false) == true ? ss.SkillName : ""),
                                                             SecondarySkill = ((ss.IsSkillPrimary ?? false) == false ? ss.SkillName : ""),
                                                             //ProjectName = allocations.Items.Where(e => e.ProjectId == ss.ProjectId).Select(c => c.ProjectName).First(), 
                                                             //Allocationpercentage = allocations.Items.Where(e => e.EmployeeId == ss.EmployeeId.Value && e.ProjectId == ss.ProjectId.Value).Select(c => c.Allocationpercentage).First(),
                                                             //LeadName =  leadDetails.Where(c => c.EmployeeId == (allocations.Items.Where(e => e.EmployeeId == ss.EmployeeId && e.ProjectId == ss.ProjectId).Select(c => c.LeadId).FirstOrDefault())).Select(c => c.EmployeeName).FirstOrDefault(),
                                                             //ManagerName = leadDetails.Where(c => c.EmployeeId == (allocations.Items.Where(e => e.EmployeeId == ss.EmployeeId && e.ProjectId == ss.ProjectId).Select(c => c.ReportingManagerId).FirstOrDefault())).Select(c => c.EmployeeName).FirstOrDefault(),
                                                         });

                if (isBillable.HasValue)
                    query = (from re in query
                             where re.IsBillable == isBillable.Value
                             select re);

                if (isCritical.HasValue)
                    query = (from re in query
                             where re.IsCritical == isCritical.Value
                             select re);

                if (isSkillPrimary.HasValue)
                    query = (from re in query
                             where re.IsSkillPrimary == isSkillPrimary.Value
                             select re);

                if (skillIds != null && skillIds.Count > 0)
                    query = (from re in query
                             where skillIds.Contains(re.SkillId)
                             select re);
                List<SkillSearchEmployee> empDetails = await query.ToListAsync();

                employeeDetails = (from re in empDetails
                                   group re by new
                                   {
                                       re.EmployeeId,
                                       re.EmployeeName,
                                       re.ProjectId,
                                       //re.ProjectName,
                                       re.Experience,
                                       re.DesignationId,
                                       re.GradeId,
                                       re.Allocationpercentage,
                                       re.LeadName,
                                       re.ManagerName
                                   } into g
                                   select new SkillSearchEmployee
                                   {
                                       EmployeeId = g.Key.EmployeeId,
                                       EmployeeName = g.Key.EmployeeName,
                                       //ProjectName = g.Key.ProjectName,
                                       Experience = g.Key.Experience,
                                       DesignationId = g.Key.DesignationId,
                                       GradeId = g.Key.GradeId,
                                       //Allocationpercentage = g.Key.Allocationpercentage,
                                       //LeadName = g.Key.LeadName,
                                       //ManagerName = g.Key.ManagerName,                                  
                                       SkillId = g.Select(c => c.SkillId).Max(),
                                       ProjectName = allocations.Items.Where(alp => alp.ProjectId == g.Key.ProjectId).Select(alp_inner => alp_inner.ProjectName).FirstOrDefault(),
                                       Allocationpercentage = allocations.Items.Where(aac => aac.EmployeeId == g.Key.EmployeeId && aac.ProjectId == g.Key.ProjectId).Select(c => c.Allocationpercentage).FirstOrDefault(),
                                       LeadName = leadDetails.Where(ln => ln.EmployeeId == (allocations.Items.Where(al => al.EmployeeId == g.Key.EmployeeId && al.ProjectId == g.Key.ProjectId).Select(al_inner => al_inner.LeadId).FirstOrDefault())).Select(al_inner => al_inner.EmployeeName).FirstOrDefault(),
                                       ManagerName = leadDetails.Where(mn => mn.EmployeeId == (allocations.Items.Where(am => am.EmployeeId == g.Key.EmployeeId && am.ProjectId == g.Key.ProjectId).Select(am_inner => am_inner.ReportingManagerId).FirstOrDefault())).Select(am_inner => am_inner.EmployeeName).FirstOrDefault(),
                                       IsSkillPrimary = g.Select(c => c.IsSkillPrimary).Max(),
                                       IsCritical = g.Select(c => c.IsCritical).Max(),
                                       IsBillable = g.Select(c => c.IsBillable).Max(),
                                       PrimarySkill = String.Join(", ", g.Where(ap => string.IsNullOrWhiteSpace(ap.PrimarySkill) == false).Select(ap_inner => ap_inner.PrimarySkill).ToList().ToArray()),
                                       SecondarySkill = String.Join(", ", g.Where(an => string.IsNullOrWhiteSpace(an.SecondarySkill) == false).Select(an_inner => an_inner.SecondarySkill).ToList().ToArray())
                                   }).ToList();

                return response = new ServiceListResponse<SkillSearchEmployee>()
                {
                    Items = employeeDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<SkillSearchEmployee>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region GetActiveAssociates
        /// <summary>
        ///  GetActiveAssociates
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetActiveAssociates(List<int> employeeIds)
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> employees = await (from emp in m_EmployeeContext.Employees
                                                     where emp.IsActive == true && employeeIds.Contains(emp.EmployeeId)
                                                     select new GenericType { Id = emp.EmployeeId, Name = emp.FirstName + " " + emp.LastName })
                            .OrderBy(c => c.Name).ToListAsync();
                response.Items = employees;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError("Error occured in GetEmployeesForDropdown() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region Get Service typeWise Resource Count
        /// <summary>
        /// GetServiceTypeResourceCount
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ServiceTypeCount>> GetServiceTypeResourceCount()
        {
            ServiceListResponse<ServiceTypeCount> response = new ServiceListResponse<ServiceTypeCount>();

            try
            {
                List<ServiceTypeCount> list = new List<ServiceTypeCount>();

                ServiceListResponse<AssociateAllocation> allocation = await m_ProjectService.GetAllAssociateAllocations();
                if (allocation.Items == null)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Allocation Details not found.";
                    return response;
                }
                var associateAllocation = allocation.Items.Where(st => st.IsActive == true && st.IsPrimary == true).ToList();
                ServiceListResponse<Project> projectDetails = await m_ProjectService.GetAllProjects();
                if (projectDetails.Items == null)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Project Details not found.";
                    return response;
                }

                list = await (from ep in m_EmployeeContext.ServiceTypeToEmployee
                              join emp in m_EmployeeContext.Employees
                              on ep.EmployeeId equals emp.EmployeeId
                              where ep.IsActive == true && emp.IsActive == true

                              select new ServiceTypeCount
                              {
                                  EmployeeId = ep.EmployeeId,
                                  ServiceTypeId = ep.ServiceTypeId,
                              }).ToListAsync();
                foreach (ServiceTypeCount emp in list)
                {
                    emp.ProjectId = associateAllocation.Where(st => st.EmployeeId == emp.EmployeeId).Select(st => st.ProjectId).FirstOrDefault() ?? 0;
                    if (emp.ProjectId > 0)
                    {
                        emp.ProjectName = projectDetails.Items.Where(st => st.ProjectId == emp.ProjectId).Select(st => st.ProjectName).FirstOrDefault();
                    }
                }


                return response = new ServiceListResponse<ServiceTypeCount>()
                {
                    Items = list,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                return response = new ServiceListResponse<ServiceTypeCount>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region GetUtilizationReport
        /// <summary>
        /// GetUtilizationReport
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<UtilizationReport>> GetUtilizationReport()
        {
            ServiceListResponse<UtilizationReport> response = new ServiceListResponse<UtilizationReport>();
            try
            {
                var utilizationInfo = await m_EmployeeContext.UtilizationReport.OrderBy(c => c.DateOfJoining).ThenBy(d => d.AssociateCode).ToListAsync();
                if (utilizationInfo != null)
                {
                    response.IsSuccessful = true;
                    response.Items = utilizationInfo;
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

        #region GetResourceUtilization
        /// <summary>
        /// GetResourceUtilization
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<UtilizationReport>> GetResourceUtilization(int? year = null, bool nightlyJob = false)
        {
            ServiceListResponse<UtilizationReport> response = new ServiceListResponse<UtilizationReport>();

            try
            {
                List<UtilizationReport> list = new List<UtilizationReport>();

                ServiceListResponse<AssociateAllocation> associateAllocations = await m_ProjectService.GetAllAssociateAllocations(nightlyJob);
                if (associateAllocations.Items == null)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Allocation Details not found.";
                    return response;
                }
                var associateAllocation = associateAllocations.Items.ToList();
                ServiceListResponse<Project> projectDetails = await m_ProjectService.GetAllProjects(nightlyJob);
                if (projectDetails.Items == null)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Project Details not found.";
                    return response;
                }

                List<int> deliveryDepartment = new List<int>();

                // Delivery Department Employees
                var departments = await m_OrgService.GetAllDepartments(nightlyJob);
                if (departments != null)
                {
                    deliveryDepartment = departments.Items.Where(c => c.DepartmentTypeId == 1).Select(s => s.DepartmentId).ToList();
                }

                // Practice Areas
                var technologies = await m_OrgService.GetAllPracticeAreas(nightlyJob);

                // Skills
                var skills = (from emp in m_EmployeeContext.Employees
                              join ss in m_EmployeeContext.SkillSearch.Where(w => w.IsSkillPrimary == true) on emp.EmployeeId equals ss.EmployeeId
                              where emp.JoinDate.HasValue &&
                                      deliveryDepartment.Contains(emp.DepartmentId ?? 0)
                              select new
                              {
                                  EmployeeId = emp.EmployeeId,
                                  Skill = ss.SkillName
                              }).Distinct().ToList();

                // Get Employees
                var empList = await (from emp in m_EmployeeContext.Employees
                                     where emp.JoinDate.HasValue &&
                                     deliveryDepartment.Contains(emp.DepartmentId ?? 0)
                                     select new
                                     {
                                         AssociateId = emp.EmployeeId,
                                         AssociateCode = emp.EmployeeCode,
                                         AssociateName = emp.FirstName + " " + emp.LastName,
                                         DateOfJoining = emp.JoinDate.Value.Date,
                                         LastWorkingDate = emp.RelievingDate,
                                         TotalWorkedDays = (int)((emp.IsActive == true ? DateTime.Now : (emp.RelievingDate ?? DateTime.Now)).Date - emp.JoinDate.Value.Date).TotalDays,
                                         EmploymentStartDate = emp.EmploymentStartDate,
                                         ExperienceExcludingCareerBreak = emp.ExperienceExcludingCareerBreak ?? 0,
                                         CompetencyGroup = Convert.ToString(emp.CompetencyGroup ?? 0),
                                         Active = emp.IsActive ?? false
                                     }).OrderBy(c => c.DateOfJoining).ToListAsync();

                // Employees joined later or in the year
                if (year.HasValue)
                    empList = empList.Where(c => (c.DateOfJoining.Year >= year) ||
                    (c.DateOfJoining.Year < year && c.Active == true)).ToList();

                foreach (var emp in empList)
                {
                    UtilizationReport associate = new UtilizationReport();
                    associate.AssociateCode = emp.AssociateCode;
                    associate.AssociateName = emp.AssociateName;
                    associate.DateOfJoining = emp.DateOfJoining;
                    associate.LastWorkingDate = emp.Active == false ? emp.LastWorkingDate : null;
                    associate.LastBillingDate = null;
                    associate.TotalWorkedDays = (int)((emp.Active == true ? DateTime.Now : (emp.LastWorkingDate ?? DateTime.Now)).Date - (emp.DateOfJoining.Year < year.Value ? new DateTime(year.Value, 1, 1) : emp.DateOfJoining.Date)).TotalDays;
                    associate.EmploymentStartDate = emp.EmploymentStartDate;
                    associate.TimeTakenForBillable = 0;
                    associate.TotalBillingDays = 0;
                    associate.TotalNonBillingDays = 0;
                    associate.BillingDaysPercentage = 0;
                    associate.ExperienceExcludingCareerBreak = emp.ExperienceExcludingCareerBreak;
                    associate.CompetencyGroup = technologies.Items.Where(c => c.PracticeAreaId.ToString() == emp.CompetencyGroup).Select(d => d.PracticeAreaDescription).FirstOrDefault();
                    associate.Active = emp.Active;
                    associate.Fresher = false;
                    associate.Skills = string.Join(", ",
                                                    (
                                                        from sk in skills
                                                        where sk.EmployeeId == emp.AssociateId
                                                        orderby sk.Skill
                                                        select sk.Skill)
                                                    );
                    if (emp.EmploymentStartDate.HasValue && emp.EmploymentStartDate.Value.Date == emp.DateOfJoining.Date)
                        associate.Fresher = true;

                    List<AssociateAllocation> allocations = associateAllocation.Where(st => st.EmployeeId == emp.AssociateId && st.EffectiveDate.HasValue).OrderBy(c => c.EffectiveDate.Value).ToList();
                    if (allocations != null)
                    {
                        DateTime? billableDate = allocations.Where(w => (w.IsBillable ?? false) == true).Select(c => c.EffectiveDate).FirstOrDefault();
                        associate.TimeTakenForBillable = (int)((billableDate ?? DateTime.Now).Date - (emp.DateOfJoining.Date.Year < year.Value ? new DateTime(year.Value, 1, 1) : emp.DateOfJoining.Date)).TotalDays;

                        List<int> projects = allocations.Where(c => c.IsBillable.HasValue && c.IsBillable.Value == true).Select(s => s.ProjectId ?? 0).Distinct().ToList();

                        foreach (int projectId in projects)
                        {
                            if (string.IsNullOrWhiteSpace(associate.ProjectsWorked))
                                associate.ProjectsWorked = projectDetails.Items.Where(st => st.ProjectId == projectId).Select(st => st.ProjectName).FirstOrDefault();
                            else
                                associate.ProjectsWorked = associate.ProjectsWorked + ", " + projectDetails.Items.Where(st => st.ProjectId == projectId).Select(st => st.ProjectName).FirstOrDefault();
                        }

                        foreach (AssociateAllocation allocation in allocations)
                        {
                            if (allocation.IsBillable.HasValue && allocation.IsBillable == true)
                            {
                                associate.TotalBillingDays = associate.TotalBillingDays + (int)((allocation.ReleaseDate ?? DateTime.Now).Date - allocation.EffectiveDate.Value.Date).TotalDays;
                            }
                        }

                        AssociateAllocation lastBillableAllocation = allocations.Where(c => c.IsBillable.HasValue && c.IsBillable == true).OrderByDescending(c => c.EffectiveDate).FirstOrDefault();
                        if(lastBillableAllocation != null)
                        {
                            associate.LastBillingDate = lastBillableAllocation.ReleaseDate ?? DateTime.Now;                            
                        }                       

                        associate.TotalNonBillingDays = associate.TotalWorkedDays - associate.TotalBillingDays;
                        if (associate.TotalWorkedDays != 0)
                            associate.BillingDaysPercentage = ((associate.TotalBillingDays * 100) / associate.TotalWorkedDays);
                    }
                    list.Add(associate);
                }

                return response = new ServiceListResponse<UtilizationReport>()
                {
                    Items = list,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                return response = new ServiceListResponse<UtilizationReport>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region GetAssociateExitReport
        /// <summary>
        /// GetAssociateExitReport
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateExitReport>> GetAssociateExitReport()
        {
            ServiceListResponse<AssociateExitReport> response = new ServiceListResponse<AssociateExitReport>();
            try
            {
                var assocciateExitInfo = await m_EmployeeContext.AssociateExitReport.OrderBy(c => c.ExitDate).ThenBy(d => d.AssociateCode).ToListAsync();
                if (assocciateExitInfo != null)
                {
                    response.IsSuccessful = true;
                    response.Items = assocciateExitInfo;
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Items = new List<AssociateExitReport>();
                }
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
            }

            return response;
        }

        public async Task<ServiceListResponse<AssociateExitReport>> GetAssociateExitReport(AssociateExitReportFilter filter)
        {
            ServiceListResponse<AssociateExitReport> response = new ServiceListResponse<AssociateExitReport>();
            try
            {
                var assocciateExitInfo = await m_EmployeeContext.AssociateExitReport.Where(c => c.ExitDate.Date >= filter.FromDate.Date &&
           c.ExitDate.Date < filter.ToDate.AddDays(1).Date).OrderBy(c => c.ExitDate).ThenBy(d => d.AssociateCode).ToListAsync();

                if (assocciateExitInfo != null)
                {
                    response.IsSuccessful = true;
                    response.Items = assocciateExitInfo;
                }
                else
                {
                    response.Items = new List<AssociateExitReport>();
                    response.IsSuccessful = true;
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

        #region GetAssociateExitData
        /// <summary>
        /// GetAssociateExitData
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateExitReport>> GetAssociateExitData(bool nightlyJob = false)
        {
            ServiceListResponse<AssociateExitReport> response = new ServiceListResponse<AssociateExitReport>();

            try
            {
                List<AssociateExitReport> list = new List<AssociateExitReport>();

                //ServiceListResponse<AssociateAllocation> associateAllocations = await m_ProjectService.GetAllAssociateAllocations(nightlyJob);
                //if (associateAllocations.Items == null)
                //{
                //    response.Items = null;
                //    response.IsSuccessful = false;
                //    response.Message = "Allocation Details not found.";
                //    return response;
                //}
                //var associateAllocation = associateAllocations.Items.ToList();
                ServiceListResponse<Project> projectDetails = await m_ProjectService.GetAllProjects(nightlyJob);
                if (projectDetails.Items == null)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Project Details not found.";
                    return response;
                }

                List<int> deliveryDepartment = new List<int>();

                // Delivery Department Employees
                var departments = await m_OrgService.GetAllDepartments(nightlyJob);
                if (departments != null)
                {
                    deliveryDepartment = departments.Items.Where(c => c.DepartmentTypeId == 1).Select(s => s.DepartmentId).ToList();
                }

                // Practice Areas
                var technologies = await m_OrgService.GetAllPracticeAreas(nightlyJob);

                // Grades
                var grades = await m_OrgService.GetAllGrades();

                // ExitTypes
                var exitTypes = await m_OrgService.GetAllExitTypes();

                // ExitReasons
                var exitReasons = await m_OrgService.GetAllExitReasons();

                // Get Employees
                var empList = await (from ext in m_EmployeeContext.AssociateExit
                                     join emp in m_EmployeeContext.Employees on ext.EmployeeId equals emp.EmployeeId
                                     where emp.JoinDate.HasValue && ext.ExitDate.HasValue && ext.StatusId == (int)AssociateExitStatusCodesNew.Resigned
                                     && deliveryDepartment.Contains(emp.DepartmentId ?? 0)
                                     select new AssociateExitReport
                                     {
                                         AssociateId = emp.EmployeeId,
                                         AssociateExitId = ext.AssociateExitId,
                                         AssociateCode = emp.EmployeeCode,
                                         AssociateName = emp.FirstName + " " + emp.LastName,
                                         JoinDate = emp.JoinDate.Value.Date,
                                         ExitDate = ext.ExitDate.Value.Date,
                                         ResignedDate = ext.ResignationDate,
                                         ServiceTenure = emp.ExperienceExcludingCareerBreak ?? 0,
                                         ServiceTenureWithSG = 0,
                                         ServiceTenurePriorToSG = 0,
                                         ServiceTenureRange = "",
                                         ServiceTenureWithSGRange = "",
                                         TechnologyGroup = Convert.ToString(emp.CompetencyGroup ?? 0),
                                         Grade = Convert.ToString(emp.GradeId ?? 0),
                                         Department = Convert.ToString(emp.DepartmentId ?? 0),
                                         RehireEligibility = ext.RehireEligibility ?? false,
                                         LegalExit = ext.LegalExit,
                                         ImpactOnClientDelivery = ext.ImpactOnClientDelivery ?? false,
                                         Gender = emp.Gender,
                                         ExitType = Convert.ToString(ext.ExitTypeId),
                                         ExitCause = Convert.ToString(ext.ExitReasonId ?? 0),
                                         Remarks = ext.AssociateRemarks
                                     }).OrderBy(c => c.ExitDate).ToListAsync();

                List<int> employeeIds = new List<int>();
                employeeIds = empList.Select(c => c.AssociateId).Distinct().ToList();
                // Get Managers
                List<int> managerIds = new List<int>();
                var allocations = m_ProjectService.GetAssociateAllocations(employeeIds).Result.Items;

                if (allocations != null && allocations.Count() > 0)
                {
                    managerIds = allocations.Where(d => d.ProgramManagerId.HasValue).Select(c => c.ProgramManagerId.Value).Distinct().ToList();
                    managerIds.AddRange(allocations.Where(d => d.ReportingManagerId.HasValue).Select(c => c.ReportingManagerId.Value).Distinct().ToList());
                }

                var managers = (from emp in m_EmployeeContext.Employees
                                where managerIds.Contains(emp.EmployeeId)
                                && deliveryDepartment.Contains(emp.DepartmentId ?? 0)
                                select new AssociateExitReport
                                {
                                    AssociateId = emp.EmployeeId,
                                    AssociateCode = emp.EmployeeCode,
                                    AssociateName = emp.FirstName + " " + emp.LastName
                                }).ToList();

                foreach (var emp in empList)
                {
                    emp.TechnologyGroup = technologies.Items.Where(c => c.PracticeAreaId.ToString() == emp.TechnologyGroup).Select(d => d.PracticeAreaDescription).FirstOrDefault();
                    emp.Grade = grades.Items.Where(c => c.GradeId.ToString() == emp.Grade).Select(d => d.GradeName).FirstOrDefault();
                    emp.Department = departments != null ? departments.Items.Where(c => c.DepartmentId.ToString() == emp.Department).Select(d => d.Description).FirstOrDefault() : "";
                    int projectId = allocations.Where(c => c.EmployeeId == emp.AssociateId).OrderByDescending(d => d.EffectiveDate).Select(d => d.ProjectId ?? 0).FirstOrDefault();
                    emp.Project = projectDetails.Items.Where(c => c.ProjectId == projectId).Select(d => d.ProjectName).FirstOrDefault();
                    emp.ExitType = exitTypes.Items.Where(c => c.Id.ToString() == emp.ExitType).Select(d => d.Name).FirstOrDefault();
                    emp.ExitCause = exitReasons.Items.Where(c => c.Id.ToString() == emp.ExitCause).Select(c => c.Name).FirstOrDefault();
                    emp.ServiceTenureWithSG = GetExperience(emp.JoinDate, emp.ExitDate);
                    emp.ServiceTenurePriorToSG = emp.ServiceTenure - emp.ServiceTenureWithSG;
                    emp.ServiceTenureRange = GetServiceTenureRange(emp.ServiceTenure);
                    emp.ServiceTenureWithSGRange = GetServiceTenureRange(emp.ServiceTenureWithSG);
                    int programManagerId = allocations.Where(c => c.EmployeeId == emp.AssociateId).OrderByDescending(d => d.EffectiveDate).Select(d => d.ProgramManagerId.Value).FirstOrDefault();
                    int reportManagerId = allocations.Where(c => c.EmployeeId == emp.AssociateId).OrderByDescending(d => d.EffectiveDate).Select(d => d.ReportingManagerId.Value).FirstOrDefault();
                    emp.ProgramManager = managers.Where(c => c.AssociateId == programManagerId).Select(d => d.AssociateName).FirstOrDefault();
                    emp.ReportingManager = managers.Where(c => c.AssociateId == reportManagerId).Select(d => d.AssociateName).FirstOrDefault();
                    emp.FinancialYear = GetFinancialYear(emp.ExitDate);
                    emp.Quarter = GetQuarter(emp.ExitDate);
                    list.Add(emp);
                }

                return response = new ServiceListResponse<AssociateExitReport>()
                {
                    Items = list,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                return response = new ServiceListResponse<AssociateExitReport>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }

        #endregion

        #region GetAssociateExitReportTypes
        /// <summary>
        /// GetAssociateExitReportTypes
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetAssociateExitReportTypes()
        {
            ServiceListResponse<GenericType> response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> reportTypes = new List<GenericType>();
                reportTypes.Add(new GenericType { Id = 1, Name = "Cause Of Exit" });
                reportTypes.Add(new GenericType { Id = 2, Name = "Eligible For Rehire" });
                reportTypes.Add(new GenericType { Id = 3, Name = "Gender Ratio" });
                reportTypes.Add(new GenericType { Id = 4, Name = "Grade Wise" });
                reportTypes.Add(new GenericType { Id = 5, Name = "Impacting Client Delivery" });
                reportTypes.Add(new GenericType { Id = 6, Name = "Legal Exit" });
                reportTypes.Add(new GenericType { Id = 7, Name = "Program Manager" });
                reportTypes.Add(new GenericType { Id = 8, Name = "Technology Group" });
                reportTypes.Add(new GenericType { Id = 9, Name = "Service Tenure Range" });
                reportTypes.Add(new GenericType { Id = 10, Name = "Service Tenure Range SG" });
                reportTypes.Add(new GenericType { Id = 11, Name = "Type Of Exit" });
                response.IsSuccessful = true;
                response.Items = reportTypes;
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
            }

            return response;
        }

        #endregion

        #region GetAssociateExitChartReport
        /// <summary>
        /// GetAssociateExitChartReport
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ChartData>> GetAssociateExitChartReport(AssociateExitReportFilter filter)
        {
            ServiceListResponse<ChartData> response = new ServiceListResponse<ChartData>();
            try
            {
                List<AssociateExitReport> list = m_EmployeeContext.AssociateExitReport.Where(c => c.ExitDate.Date >= filter.FromDate.Date &&
           c.ExitDate.Date < filter.ToDate.AddDays(1).Date).ToList();
                List<ChartData> data = new List<ChartData>();
                switch (filter.ReportType)
                {
                    case 1:
                        data = list.GroupBy(c => c.ExitCause)
                                         .Select(d => new ChartData { Label = d.Key, Value = d.Count() })
                                         .ToList();

                        break;
                    case 2:
                        data = list.GroupBy(c => c.RehireEligibility)
                                         .Select(d => new ChartData { Label = d.Key.ToString(), Value = d.Count() })
                                         .ToList();
                        break;
                    case 3:
                        data = list.GroupBy(c => c.Gender)
                                         .Select(d => new ChartData { Label = d.Key, Value = d.Count() })
                                         .ToList();
                        break;
                    case 4:
                        data = list.GroupBy(c => c.Grade)
                                         .Select(d => new ChartData { Label = d.Key, Value = d.Count() })
                                         .ToList();
                        break;
                    case 5:
                        data = list.GroupBy(c => c.ImpactOnClientDelivery)
                                         .Select(d => new ChartData { Label = d.Key.ToString(), Value = d.Count() })
                                         .ToList();
                        break;
                    case 6:
                        data = list.GroupBy(c => c.LegalExit)
                                         .Select(d => new ChartData { Label = d.Key.ToString(), Value = d.Count() })
                                         .ToList();
                        break;
                    case 7:
                        data = list.GroupBy(c => c.ProgramManager)
                                         .Select(d => new ChartData { Label = d.Key, Value = d.Count() })
                                         .ToList();
                        break;
                    case 8:
                        data = list.GroupBy(c => c.TechnologyGroup)
                                         .Select(d => new ChartData { Label = d.Key, Value = d.Count() })
                                         .ToList();
                        break;
                    case 9:
                        data = list.GroupBy(c => c.ServiceTenureRange)
                                         .Select(d => new ChartData { Label = d.Key, Value = d.Count() })
                                         .ToList();
                        break;
                    case 10:
                        data = list.GroupBy(c => c.ServiceTenureWithSGRange)
                                         .Select(d => new ChartData { Label = d.Key, Value = d.Count() })
                                         .ToList();
                        break;
                    case 11:
                        data = list.GroupBy(c => c.ExitType)
                                         .Select(d => new ChartData { Label = d.Key, Value = d.Count() })
                                         .ToList();
                        break;
                }

                if (data != null)
                {
                    response.IsSuccessful = true;
                    response.Items = data;
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Items = new List<ChartData>();
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

        #region GetParkingSloteport
        /// <summary>
        /// GetParkingSloteport
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ParkingSlotReport>> GetParkingSlotReport(ParkingSearchFilter filter)
        {
            ServiceListResponse<ParkingSlotReport> response = new ServiceListResponse<ParkingSlotReport>();
            try
            {
                DateTime starteDate = Convert.ToDateTime(filter.StartDate.ToString(), CultureInfo.InvariantCulture);
                DateTime endDate = Convert.ToDateTime(filter.Enddate.ToString(), CultureInfo.InvariantCulture);
                List<ParkingSlotReport> parkingSlotReport = null;
                List<BookedParkingSlots> bookedParkingSlot = await m_EmployeeContext.BookedParkingSlots.Where(slot=>slot.IsActive==true).ToListAsync();
                if (bookedParkingSlot.Count>0)
                {
                    if (filter.Location != null && filter.Location != "")
                    {
                        parkingSlotReport = bookedParkingSlot.Where(parkingSlot => (Convert.ToDateTime(parkingSlot.BookedDate.ToString(), CultureInfo.InvariantCulture) >= starteDate && Convert.ToDateTime(parkingSlot.BookedDate.ToString(), CultureInfo.InvariantCulture) <= endDate && parkingSlot.PlaceName.ToLower() == filter.Location.ToLower())).ToList().
                  Select(parkingSlot => new ParkingSlotReport { Email = parkingSlot.EmailID, BookedDate = parkingSlot.BookedDate, BookedTime = parkingSlot.BookedTime, VehicleNumber = parkingSlot.VehicleNumber, Location = parkingSlot.PlaceName }).ToList();
                    }
                    else
                    {
                        parkingSlotReport = bookedParkingSlot.Where(parkingSlot => (Convert.ToDateTime(parkingSlot.BookedDate.ToString(), CultureInfo.InvariantCulture) >= starteDate && Convert.ToDateTime(parkingSlot.BookedDate.ToString(), CultureInfo.InvariantCulture) <= endDate)).ToList().
                       Select(parkingSlot => new ParkingSlotReport { Email = parkingSlot.EmailID, BookedDate = parkingSlot.BookedDate, BookedTime = parkingSlot.BookedTime, VehicleNumber = parkingSlot.VehicleNumber, Location = parkingSlot.PlaceName }).ToList();
                    }
                   if (parkingSlotReport.Count > 0)
                    {                       
                        response.IsSuccessful = true;
                        response.Items = parkingSlotReport;

                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "No data found";
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No data found";
                }
                response.Items = parkingSlotReport;
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching data from Parking slot report";
            }

            return response;
        }

        #endregion

        #region private methods
        private decimal GetExperience(DateTime fromDate, DateTime toDate)
        {
            DateTime age = new DateTime(toDate.Subtract(fromDate).Ticks);

            return (age.Year * 1.0M) + (age.Month * 0.10M);
        }

        private string GetServiceTenureRange(decimal serviceTenure)
        {
            string range = "<3";
            if (serviceTenure < 3.0M)
                range = "<3";
            else if (serviceTenure >= 3.0M && serviceTenure < 6.0M)
                range = "3 - 6";
            else if (serviceTenure >= 6.0M && serviceTenure < 9.0M)
                range = "6 - 9";
            else if (serviceTenure >= 9.0M && serviceTenure < 12.0M)
                range = "9 - 12";
            else if (serviceTenure >= 12.0M && serviceTenure <= 15.0M)
                range = "12 - 15";
            else if (serviceTenure > 15.0M)
                range = ">15";
            return range;
        }

        private string GetFinancialYear(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            string finanicalYear = "";
            if (month >= 4)
                finanicalYear = $"{year.ToString()}-{(year + 1).ToString()}";
            else
                finanicalYear = $"{(year - 1).ToString()}-{year.ToString()}";

            return finanicalYear;
        }
        private string GetQuarter(DateTime date)
        {
            int month = date.Month;
            string quarter = "Q1";
            if (month >= 4 && month <= 6)
                quarter = "Q1";
            else if (month >= 7 && month <= 9)
                quarter = "Q2";
            else if (month >= 10 && month <= 12)
                quarter = "Q3";
            else if (month >= 1 && month <= 3)
                quarter = "Q4";

            return quarter;
        }

        #endregion
    }
}
