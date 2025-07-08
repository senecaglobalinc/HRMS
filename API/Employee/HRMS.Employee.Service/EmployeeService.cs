using AutoMapper;
using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to get the Employee details
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        #region Global Varibles

        private readonly ILogger<EmployeeService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;
        private readonly IConfiguration m_configuration;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;

        #endregion

        #region Constructor

        public EmployeeService(EmployeeDBContext employeeDBContext,
                                ILogger<EmployeeService> logger,
                                IProjectService projectService,
                                IOrganizationService orgService,
                                IConfiguration configuration,
                                IOptions<MiscellaneousSettings> miscellaneousSettings)
        {
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee.Entities.Employee, Employee.Entities.Employee>();
            });
            m_mapper = config.CreateMapper();
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_ProjectService = projectService;
            m_OrgService = orgService;
            m_configuration = configuration;
            m_MiscellaneousSettings = miscellaneousSettings?.Value;
        }

        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the employees
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<Employee.Entities.Employee>> GetAll(bool? isActive)
        {
            var response = new ServiceListResponse<Employee.Entities.Employee>();
            try
            {
                var employeeList = await m_EmployeeContext.Employees.ToListAsync();
                if (isActive.HasValue)
                    employeeList = employeeList.Where(x => x.IsActive == isActive).ToList();
                response.IsSuccessful = true;
                response.Items = employeeList;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employees";
                m_Logger.LogError("Error occured while fetching employees" + ex.StackTrace);
            }
            return response;
        }


        #endregion

        #region GetEmpTypes
        /// <summary>
        /// Get all the active employee Types
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeType>> GetEmpTypes()
        {
            var response = new ServiceListResponse<EmployeeType>();
            try
            {
                var employeeTypesList = await m_EmployeeContext.EmployeeTypes.Where(et => et.IsActive == true).OrderBy(x => x.EmpType).ToListAsync();
                response.IsSuccessful = true;
                response.Items = employeeTypesList;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employee types";
                m_Logger.LogError("Error occured while fetching employee types" + ex.StackTrace);
            }
            return response;
        }


        #endregion

        #region GetEmployeeOnUserName
        /// <summary>
        ///  Gets the employee information by username
        /// </summary>
        /// <param name="userName">userName</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeeByUserName(string userName)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                var users = await m_OrgService.GetUsers();
                if (users.IsSuccessful == true)
                {
                    response.Message = users.Message;
                    response.IsSuccessful = false;
                    return response;
                }
                List<Employee.Entities.Employee> employees = m_EmployeeContext.Employees.ToList();
                var emps = (from emp in employees
                            join user in users.Items on emp.UserId equals user.UserId
                            where emp.IsActive == true
                            && user.EmailAddress == userName
                            select new EmployeeDetails { EmpId = emp.EmployeeId, EmpName = $"{emp.FirstName}  {emp.LastName}" }).Distinct().ToList<EmployeeDetails>();
                response.Items = emps;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data with username";
                m_Logger.LogError("Error occured in GetEmployeeByUserName() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetEmployeeNames
        /// <summary>
        /// Get the active Employee's names
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeeNames()
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                var employeeNamesList = await (from employee in m_EmployeeContext.Employees.Where(emp => emp.IsActive == true)
                                               select new EmployeeDetails
                                               {
                                                   EmpId = employee.EmployeeId,
                                                   EmpName = employee.FirstName + " " + employee.LastName,
                                                   StatusId = employee.StatusId,
                                                   DepartmentId = employee.DepartmentId
                                               }).OrderBy(emp => emp.EmpName).ToListAsync();
                response.IsSuccessful = true;
                response.Items = employeeNamesList;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employee names";
                m_Logger.LogError("Error occured while fetching GetEmployeeNames() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetBusinessValues
        /// <summary>
        /// Get the BusinessValues by valueKey
        /// </summary>
        /// <param name="valueKey">valueKey</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<lkValue>> GetBusinessValues(string valueKey)
        {
            var response = new ServiceListResponse<lkValue>();
            try
            {
                var getBusinessValues = await (from lkv in m_EmployeeContext.lkValue
                                               join vt in m_EmployeeContext.ValueType on lkv.ValueTypeKey equals vt.ValueTypeKey
                                               where vt.ValueTypeId == valueKey && lkv.IsActive == true
                                               select new lkValue
                                               {
                                                   ValueId = lkv.ValueId,
                                                   ValueName = lkv.ValueName,
                                                   ValueKey = lkv.ValueKey
                                               }).OrderBy(x => x.ValueName).ToListAsync();

                response.Items = getBusinessValues;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Business Values";
                m_Logger.LogError("Error occured in GetBusinessValues() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetById
        /// <summary>
        /// Get employee by id
        /// </summary>
        /// <param name="id">employee Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<Employee.Entities.Employee>> GetById(int id)
        {
            var response = new ServiceResponse<Employee.Entities.Employee>();
            try
            {
                var employee = await m_EmployeeContext.Employees.FindAsync(id);
                response.IsSuccessful = true;
                response.Item = employee;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employee by Id";
                m_Logger.LogError("Error occured in GetById() method" + ex.StackTrace);
            }
            return response;
        }


        #endregion

        #region GetByIds
        /// <summary>
        /// Get employees by id's
        /// </summary>
        /// <param name="idList">employee Id</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetByIds(string idList)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            var ids = idList.Split(",".ToCharArray()).Select(Int32.Parse).ToList();
            try
            {
                var employee = await m_EmployeeContext.Employees
                                    .Where(emp => ids.Contains(emp.EmployeeId))
                                    .Select(employeelocal => new EmployeeDetails
                                    {
                                        EmpId = employeelocal.EmployeeId,
                                        EmpCode = employeelocal.EmployeeCode,
                                        EmpName = employeelocal.FirstName + " " + employeelocal.LastName,
                                        IsActive = employeelocal.IsActive
                                    })
                                    .ToListAsync();
                response.IsSuccessful = true;
                response.Items = employee;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employee by id's";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }


        #endregion

        #region GetJoinedEmployees
        /// <summary>
        /// Get the active Employees based on PracticeArea, Departments, designations and statusId 
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<object>> GetJoinedEmployees()
        {
            var response = new ServiceResponse<object>();
            try
            {
                var departments = await m_OrgService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = departments.Message;
                    return response;
                }
                var designations = await m_OrgService.GetAllDesignations();
                if (!designations.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = designations.Message;
                    return response;
                }
                var practiceAreas = await m_OrgService.GetAllPracticeAreas();
                if (!practiceAreas.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = practiceAreas.Message;
                    return response;
                }
                var Employees = m_EmployeeContext.Employees.Where(e => e.IsActive == true).ToList();

                var employees = (from emp in Employees
                                 join pr in practiceAreas.Items on emp.CompetencyGroup equals pr.PracticeAreaId into result
                                 from res in result.DefaultIfEmpty()
                                 join dept in departments.Items on emp.DepartmentId equals dept.DepartmentId
                                 join desig in designations.Items on emp.DesignationId equals desig.DesignationId
                                 where (emp.IsActive == true && (emp.StatusId == null))//|| emp.StatusId == (int)EPCStatusCode.Pending
                                                                                       //  &&( emp.StatusId != (int)EPCStatusCode.Rejected && emp.StatusId != (int)EPCStatusCode.Approved && emp.StatusId!=(int)EPCStatusCode.Pending)))
                                 select new
                                 {
                                     emp.EmployeeId,
                                     Name = emp.FirstName + ' ' + emp.LastName,
                                     emp.DesignationId,
                                     emp.JoinDate,
                                     emp.Hradvisor,
                                     TechnologyName = res != null ? res.PracticeAreaCode : "",
                                     desig.DesignationName,
                                     dept.DepartmentCode
                                 }).OrderBy(e => e.JoinDate).ToList();
                response.IsSuccessful = true;
                response.Item = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetEmployeeInfo
        /// <summary>
        /// Gets the Employee info based on departments and approved status.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeeInfo(string searchString = null, int pageIndex = 0, int pageSize = 0)
        {

            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                var statusApproved = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.EPC.ToString(), EPCStatusCode.Approved.ToString());
                if (!statusApproved.IsSuccessful)
                {
                    response.Message = statusApproved.Message;
                    response.IsSuccessful = false;
                    return response;
                }

                var departments = await m_OrgService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.Message = departments.Message;
                    response.IsSuccessful = false;
                    return response;
                }
                List<Employee.Entities.Employee> Employees = null;
                if (pageIndex > 0 && pageSize > 0)
                {
                    if (searchString != null)
                    {
                        Employees = m_EmployeeContext.Employees
                            .Where(e => e.IsActive == true && e.StatusId == statusApproved.Item.StatusId && (e.FirstName.ToLower().Contains(searchString.ToLower()) || e.LastName.ToLower().Contains(searchString.ToLower())
                                || e.EmployeeCode.ToLower().Contains(searchString.ToLower()) || e.MobileNo.ToLower().Contains(searchString.ToLower()) || e.PersonalEmailAddress.ToLower().Contains(searchString.ToLower())))
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
                    }
                    else
                    {
                        Employees = m_EmployeeContext.Employees
                            .Where(e => e.IsActive == true && e.StatusId == statusApproved.Item.StatusId)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
                    }
                }
                else
                {
                    if (searchString != null)
                    {
                        Employees = m_EmployeeContext.Employees.Where(e => e.IsActive == true && (e.FirstName.ToLower().Contains(searchString.ToLower()) || e.LastName.ToLower().Contains(searchString.ToLower())
                            || e.EmployeeCode.ToLower().Contains(searchString.ToLower()) || e.MobileNo.ToLower().Contains(searchString.ToLower()) || e.PersonalEmailAddress.ToLower().Contains(searchString.ToLower()))).ToList();
                    }
                    else
                    {
                        Employees = m_EmployeeContext.Employees.Where(e => e.IsActive == true).ToList();
                    }
                }
                if (statusApproved.Item.StatusId <= 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "Approved Status is not found.";
                    return response;
                }

                var employees = (from e in Employees
                                 join dep in departments.Items on e.DepartmentId equals dep.DepartmentId into employee
                                 from emp in employee.DefaultIfEmpty()
                                 where e.IsActive == true && e.StatusId == statusApproved.Item.StatusId
                                 select new EmployeeDetails()
                                 {
                                     EmpId = e.EmployeeId,
                                     EmpCode = e.EmployeeCode,
                                     EmpName = e.FirstName + " " + e.LastName,
                                     BgvStatus = e.Bgvstatus,
                                     MobileNo = Utility.DecryptStringAES(e.MobileNo),
                                     EncryptedMobileNumber = e.MobileNo,
                                     WorkEmail = e.WorkEmailAddress,
                                     PersonalEmailAddress = e.PersonalEmailAddress,
                                     Dob = e.DateofBirth,
                                     DepartmentId = emp != null ? emp.DepartmentId : 0,
                                     Department = emp != null ? emp.Description : ""
                                 }).OrderByDescending(a => a.EmpId).ToList();
                response.IsSuccessful = true;
                response.Items = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Employee information";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;

        }
        #endregion

        #region GetEmployeeCount
        /// <summary>
        /// Gets the Employee info based on departments and approved status.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> GetEmployeeCount(string searchString = null)
        {
            var response = new ServiceResponse<int>();
            int employeeCount = 0;
            try
            {
                var statusApproved = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.EPC.ToString(), EPCStatusCode.Approved.ToString());
                if (!statusApproved.IsSuccessful)
                {
                    response.Message = statusApproved.Message;
                    response.IsSuccessful = false;
                    return response;
                }
                if (searchString != null)
                {

                    employeeCount = m_EmployeeContext.Employees
                        .Where(e => (e.IsActive == true && e.StatusId == statusApproved.Item.StatusId) && (e.FirstName.ToLower().Contains(searchString.ToLower()) || e.LastName.ToLower().Contains(searchString.ToLower())
                        || e.EmployeeCode.ToLower().Contains(searchString.ToLower()) || e.MobileNo.ToLower().Contains(searchString.ToLower()) || e.PersonalEmailAddress.ToLower().Contains(searchString.ToLower())))
                        .Count();
                }
                else
                {
                    employeeCount = await m_EmployeeContext.Employees.Where(e => e.IsActive == true && e.StatusId == statusApproved.Item.StatusId).CountAsync();
                }
                response.IsSuccessful = true;
                response.Item = employeeCount;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Employee information";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;

        }
        #endregion

        #region GetEmployeeOnPagination
        /// <summary>
        /// Gets the GetEmployeeOnPagination based on departments and approved status.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeeOnPagination(string searchString = null, int pageIndex = 1, int pageSize = 10)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            List<Employee.Entities.Employee> employees = new List<Employee.Entities.Employee>();
            try
            {
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    response.Message = "Page Index and Page Size should be greater than zero";
                    response.IsSuccessful = false;
                    return response;
                }
                var statusApproved = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.EPC.ToString(), EPCStatusCode.Approved.ToString());
                if (!statusApproved.IsSuccessful)
                {
                    response.Message = statusApproved.Message;
                    response.IsSuccessful = false;
                    return response;
                }

                var departments = await m_OrgService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.Message = departments.Message;
                    response.IsSuccessful = false;
                    return response;
                }
                if (statusApproved.Item.StatusId <= 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "Approved Status is not found.";
                    return response;
                }

                if (searchString != null)
                {
                    employees = m_EmployeeContext.Employees
                        .OrderBy(x => x.EmployeeId)
                        .Where(e => (e.IsActive == true && e.StatusId == statusApproved.Item.StatusId) && (e.FirstName.ToLower().Contains(searchString.ToLower()) || e.LastName.ToLower().Contains(searchString.ToLower())
                        || e.EmployeeCode.ToLower().Contains(searchString.ToLower()) || e.MobileNo.ToLower().Contains(searchString.ToLower()) || e.PersonalEmailAddress.ToLower().Contains(searchString.ToLower())))
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                }
                else
                {
                    employees = m_EmployeeContext.Employees
                       .OrderBy(x => x.EmployeeId)
                       .Where(e => e.IsActive == true && e.StatusId == statusApproved.Item.StatusId)
                       .Skip((pageIndex - 1) * pageSize)
                       .Take(pageSize)
                       .ToList();
                }
                var employes = (from e in employees
                                join dep in departments.Items on e.DepartmentId equals dep.DepartmentId into employee
                                from emp in employee.DefaultIfEmpty()
                                where e.IsActive == true && e.StatusId == statusApproved.Item.StatusId
                                select new EmployeeDetails()
                                {
                                    EmpId = e.EmployeeId,
                                    EmpCode = e.EmployeeCode,
                                    EmpName = e.FirstName + " " + e.LastName,
                                    BgvStatus = e.Bgvstatus,
                                    MobileNo = Utility.DecryptStringAES(e.MobileNo),
                                    EncryptedMobileNumber = e.MobileNo,
                                    WorkEmail = e.WorkEmailAddress,
                                    PersonalEmailAddress = e.PersonalEmailAddress,
                                    Dob = e.DateofBirth,
                                    DepartmentId = emp != null ? emp.DepartmentId : 0,
                                    Department = emp != null ? emp.Description : ""
                                }).OrderByDescending(a => a.EmpId).ToList();
                response.IsSuccessful = true;
                response.Items = employes;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Employee information based on Pagination";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;

        }
        #endregion

        #region GetEmployeesBySkillUsingCache
        /// <summary>
        /// GetEmployeesBySkillUsingCache
        /// </summary>
        public List<EmployeeSkills> GetEmployeesBySkillUsingCache()
        {
            var skills = new List<EmployeeSkills>();
            bool isAdded = false;

            var employee = m_EmployeeContext.Employees.Where(x => x.IsActive == true).ToList();
            var employeeIds = employee.Select(x => x.EmployeeId)
                                      .ToList();
            var skillSearch = m_EmployeeContext.SkillSearch.Where(x => employeeIds.Contains(x.EmployeeId.Value)).ToList();
            var associateAllocation = m_ProjectService.GetAssociateAllocationsUsingCache(employeeIds).Items;
            var allocationPercentage = m_ProjectService.GetAllocationPercentage().Result?.Items;
            var grades = m_OrgService.GetAllGrades().Result?.Items;
            var projectManagers = m_ProjectService.GetProjectManagersByEmployeeIds(employeeIds).Result?.Items;


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

            return skills.ToList();
        }
        #endregion

        #region GetEmployeesBySkill
        /// <summary>
        /// GetEmployeesBySkill
        /// </summary>
        public List<EmployeeSkills> GetEmployeesBySkill()
        {
            var skills = new List<EmployeeSkills>();
            bool isAdded = false;

            var employee = m_EmployeeContext.Employees.Where(x => x.IsActive == true).ToList();
            var employeeIds = employee.Select(x => x.EmployeeId)
                                      .ToList();
            var skillSearch = m_EmployeeContext.SkillSearch.Where(x => employeeIds.Contains(x.EmployeeId.Value)).ToList();
            var associateAllocation = m_ProjectService.GetAssociateAllocations(employeeIds).Result.Items;
            var allocationPercentage = m_ProjectService.GetAllocationPercentage().Result?.Items;
            var grades = m_OrgService.GetAllGrades().Result?.Items;
            var projectManagers = m_ProjectService.GetProjectManagersByEmployeeIds(employeeIds).Result?.Items;


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

            return skills.ToList();
        }
        #endregion

        #region GetByUserId
        /// <summary>
        /// Get employee by user id
        /// </summary>
        /// <param name="id">employee Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<Employee.Entities.Employee>> GetByUserId(int userId)
        {
            var response = new ServiceResponse<Employee.Entities.Employee>();
            try
            {
                if (userId == 0)
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "Invalid Request..";
                    return response;
                }
                var employee = await m_EmployeeContext.Employees.Where(q => q.UserId == userId && q.IsActive==true).FirstOrDefaultAsync();
                if (employee == null)
                {
                    response.IsSuccessful = false;
                    response.Item = null;
                    response.Message = "Employee not found with this User Id";
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Item = employee;
                    response.Message = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employee";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }


        #endregion

        #region GetActiveEmployeeById
        /// <summary>
        /// Gets active employee by id
        /// </summary>
        /// <param name="id">employee Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<Employee.Entities.Employee>> GetActiveEmployeeById(int id)
        {
            var response = new ServiceResponse<Employee.Entities.Employee>();
            try
            {
                var employee = await m_EmployeeContext.Employees
                                            .Where(q => q.IsActive == true && q.EmployeeId == id).FirstOrDefaultAsync();
                response.IsSuccessful = true;
                response.Item = employee;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching active employee";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }


        #endregion

        #region GetProgramManagersList
        public async Task<ServiceListResponse<Manager>> GetProgramManagersList()
        {
            ServiceListResponse<Manager> response;
            try
            {
                var designations = await m_OrgService.GetAllDesignations();
                if (designations == null || designations.Items.Count == 0)
                {
                    return response = new ServiceListResponse<Manager>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = designations.Message
                    };
                }

                var grades = await m_OrgService.GetAllGrades();
                if (grades == null || grades.Items.Count == 0)
                {
                    return response = new ServiceListResponse<Manager>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = grades.Message
                    };
                }

                var mgrList = await (from employee in m_EmployeeContext.Employees
                                     join designation in designations.Items on employee.DesignationId equals designation.DesignationId
                                     join grade in grades.Items on designation.GradeId equals grade.GradeId
                                     where employee.IsActive == true && grade.GradeCode == "G5-B" && designation.DesignationCode == "PM"
                                     select new Manager()
                                     {
                                         ManagerId = employee.EmployeeId,
                                         ManagerName = employee.FirstName + " " + employee.LastName
                                     }).ToListAsync();
                if (mgrList.Count == 0)
                {
                    return response = new ServiceListResponse<Manager>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Managers found.."
                    };
                }
                else
                {
                    return response = new ServiceListResponse<Manager>()
                    {
                        Items = mgrList,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                return response = new ServiceListResponse<Manager>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "Error occured while fetching GetProgramManagersList information"
                };
            }
        }
        #endregion

        #region GetAllActiveEmployees
        public async Task<ServiceListResponse<ActiveEmployee>> GetAllActiveEmployees()
        {
            var response = new ServiceListResponse<ActiveEmployee>();
            try
            {
                var employeeList = await (m_EmployeeContext.Employees.Where(q => q.IsActive == true)
                                               .Select(e => new ActiveEmployee
                                               {
                                                   EmpCode = e.EmployeeId,
                                                   EmpName = e.FirstName + " " + e.LastName,
                                                   Code = e.EmployeeCode,
                                                   Gender = e.Gender
                                               })).ToListAsync();

                employeeList = employeeList.OrderBy(x => x.EmpName).ThenBy(x => x.EmpCode).ToList();
                response.IsSuccessful = true;
                response.Items = employeeList;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching active employees";
                m_Logger.LogError(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetManagersAndLeads
        /// <summary>
        /// Get the active Employees based on PracticeArea, Departments, designations and statusId 
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetManagersAndLeads(int? departmentId)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                List<EmployeeDetails> finalManagersList = new List<EmployeeDetails>();
                Department Departmentdetails = null;
                if (departmentId != null)
                {
                    Departmentdetails = m_OrgService.GetDepartmentById((int)departmentId).Result.Item;

                }
                if (Departmentdetails != null && Departmentdetails.DepartmentTypeId != 1)
                {
                    finalManagersList = m_EmployeeContext.Employees.Where(employee => employee.DepartmentId!=1 && employee.IsActive == true)
                        .Select(employee => new EmployeeDetails { EmpId = employee.EmployeeId, EmpName = employee.FirstName + " " + employee.LastName }).ToList();
                }
                else if (departmentId == null || Departmentdetails.DepartmentTypeId == 1)
                {
                    var statusClosed = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.PPC.ToString(),
                                                                               StatusCategory.Closed.ToString());
                    if (!statusClosed.IsSuccessful)
                    {
                        response.Message = statusClosed.Message;
                        response.IsSuccessful = false;
                        return response;
                    }

                    var projectManagers = await m_ProjectService.GetActiveProjectManagers();

                    if (!projectManagers.IsSuccessful)
                    {
                        response.Message = projectManagers.Message;
                        response.IsSuccessful = false;
                        return response;
                    }

                    var projectIds = projectManagers.Items.Select(pm => pm.ProjectId.Value).ToList();
                    var projects = await m_ProjectService.GetAllProjects();

                    if (!projects.IsSuccessful)
                    {
                        response.Message = projects.Message;
                        response.IsSuccessful = false;
                        return response;
                    }

                    var departments = await m_OrgService.GetAllDepartments();
                    if (!departments.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = departments.Message;
                        return response;
                    }

                    var userRoles = await m_OrgService.GetAllUserRoles();
                    if (!userRoles.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = userRoles.Message;
                        return response;
                    }

                    var roles = await m_OrgService.GetRoleByRoleName(Roles.CompetencyLeader.GetEnumDescription());

                    if (!roles.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = roles.Message;
                        return response;
                    }


                    //Gets the Leads, program managers and Reporting managers
                    var managersList = (from proj in projects.Items
                                        where proj != null
                                        join pm in projectManagers.Items
                                        on proj.ProjectId equals pm.ProjectId
                                        where pm.IsActive == true && proj.ProjectStateId != (int)statusClosed.Item.StatusId &&
                                        (pm.ProgramManagerId != null || pm.ReportingManagerId != null || pm.LeadId != null)
                                        select new
                                        {
                                            ID = (
                                             (pm.ProgramManagerId != null && pm.ReportingManagerId == null && pm.LeadId == null) ? Convert.ToInt32(pm.ProgramManagerId) :
                                             (pm.ReportingManagerId != null && pm.LeadId == null) ? Convert.ToInt32(pm.ReportingManagerId) :
                                             (pm.LeadId != null) ? Convert.ToInt32(pm.LeadId) : 0
                                             ),
                                            Name = (
                                             (pm.ProgramManagerId != null && pm.ReportingManagerId == null && pm.LeadId == null) ? GetEmployeeName(pm.ProgramManagerId.Value) :
                                             (pm.ReportingManagerId != null && pm.LeadId == null) ? GetEmployeeName(pm.ReportingManagerId.Value) :
                                             (pm.LeadId != null) ? GetEmployeeName(pm.LeadId.Value) : ""
                                             )
                                        }).Distinct().ToList();

                    var employees = m_EmployeeContext.Employees.Where(emp => emp.IsActive == true).ToList();

                    //Gets the department Heads list
                    var departmentHeadsList = from dep in departments.Items
                                              join emp in employees
                                              on dep.DepartmentHeadId equals emp.EmployeeId
                                              where dep.DepartmentHeadId != null
                                              select new { ID = emp.EmployeeId, Name = GetEmployeeName(emp.EmployeeId) };

                    //Gets the Competency Leads list
                    var competencyLeadsList = from emp in employees
                                              join usr in userRoles.Items on emp.UserId equals usr.UserId
                                              where usr.RoleId == (roles.Item.RoleId)
                                              select new { ID = emp.EmployeeId, Name = GetEmployeeName(emp.EmployeeId) };

                    //perform union of managers list, departmentHeadsList and competencyLeadsList
                    var depHeadsAndComLeads = departmentHeadsList.Union(competencyLeadsList).ToList();
                    finalManagersList = depHeadsAndComLeads.Union(managersList).Select(x => new EmployeeDetails
                    {
                        EmpId = x.ID,
                        EmpName = x.Name
                    }).Distinct().OrderBy(x => x.EmpName).ToList<EmployeeDetails>();
                }
                response.Items = finalManagersList;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the managers and leads data.";
                m_Logger.LogError("Error occured while fetching the managers and leads data." + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetEmployeeName
        /// <summary>
        /// Get the employee name by employeeId
        /// </summary>
        /// <param name="employeeId">employeeId</param>
        /// <returns></returns>
        public string GetEmployeeName(int employeeId)
        {

            var Name = (from emp in m_EmployeeContext.Employees
                        where emp.EmployeeId == employeeId
                        select new
                        {
                            EmployeeName =
                            (emp.MiddleName.Length > 0) ? emp.FirstName + " " + emp.MiddleName + " " + emp.LastName : emp.FirstName + " " + emp.LastName

                        }).FirstOrDefault();

            return Name.EmployeeName;
        }
        #endregion

        #region GetStatusbyId
        /// <summary>
        /// Get employee Status based on Id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> GetStatusbyId(int empId)
        {
            var response = new ServiceResponse<int>();
            try
            {
                m_Logger.LogInformation("EmployeeProfessionalService: Calling \"GetStatusbyId\" method.");
                int statusId = await m_EmployeeContext.Employees
                                .Where(emp => emp.EmployeeId == empId)
                                .Select(emp => emp.StatusId.Value)
                                .FirstOrDefaultAsync();

                response.Item = statusId;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching ProspectiveAssociate";
                m_Logger.LogError("Error occured in GetbyId() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetPendingProfiles
        /// <summary>
        /// Gets the list of Employees whose profiles are pending.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeProfileStatus>> GetPendingProfiles()
        {
            var response = new ServiceListResponse<EmployeeProfileStatus>();
            try
            {
                var employees = await (m_EmployeeContext.Employees
                                                        .Where(emp => emp.StatusId == (int)EPCStatusCode.Pending)
                                                        .Select(employee =>
                                                           new EmployeeProfileStatus
                                                           {
                                                               EmpId = employee.EmployeeId,
                                                               EmpCode = employee.EmployeeCode,
                                                               EmpName = employee.FirstName + " " + employee.LastName,
                                                               HRAdvisor = employee.Hradvisor,
                                                               NotificationType = StringConstants.EPC
                                                           })
                                                        .Distinct().OrderByDescending(o => o.EmpId))
                                                        .ToListAsync();
                response.IsSuccessful = true;
                response.Items = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching pending employee profiles";
                m_Logger.LogError("Error occured while fetching GetPendingProfiles() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetRejectedProfiles
        /// <summary>
        /// Gets the list of Employees whose profiles are rejected.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeProfileStatus>> GetRejectedProfiles()
        {
            var response = new ServiceListResponse<EmployeeProfileStatus>();
            try
            {
                var employees = await (m_EmployeeContext.Employees
                                                        .Where(emp => emp.StatusId == (int)EPCStatusCode.Rejected)
                                                        .Select(employee =>
                                                           new EmployeeProfileStatus
                                                           {
                                                               EmpId = employee.EmployeeId,
                                                               EmpCode = employee.EmployeeCode,
                                                               EmpName = employee.FirstName + " " + employee.LastName,
                                                               HRAdvisor = employee.Hradvisor,
                                                               NotificationType = StringConstants.EPC,
                                                               Remarks = employee.Remarks

                                                           })
                                                        .Distinct().OrderByDescending(o => o.EmpId))
                                                        .ToListAsync();
                response.IsSuccessful = true;
                response.Items = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching rejected employee profiles";
                m_Logger.LogError("Error occured while fetching GetRejectedProfiles() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetEmployeeBySearchString
        /// <summary>
        ///  Gets the employee information by searchString
        /// </summary>
        /// <param name="searchString">userName</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeeBySearchString(string searchString)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                var emps = await (from emp in m_EmployeeContext.Employees
                                  where emp.IsActive == true
                                  && emp.FirstName.ToLower().Contains(searchString.ToLower()) || emp.LastName.ToLower().Contains(searchString.ToLower())
                                  select new EmployeeDetails
                                  {
                                      EmpId = emp.EmployeeId,
                                      EmpName = $"{emp.FirstName}  {emp.LastName}",
                                      IsActive = emp.IsActive,
                                      UserId = (int)emp.UserId,
                                      DepartmentId = emp.DepartmentId,
                                      EmployeeCode = emp.EmployeeCode,
                                      FirstName = emp.FirstName
                                  }).Distinct().ToListAsync();
                response.Items = emps;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data with search string";
                m_Logger.LogError("Error occured in GetEmployeeBySearchString() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetEmployeesForDropdown
        /// <summary>
        ///  GetEmployeesForDropdown
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetEmployeesForDropdown()
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> employees = await (from emp in m_EmployeeContext.Employees
                                                     where emp.IsActive == true
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

        #region GetAssociateNamesList
        /// <summary>
        ///  Gets the employee information
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetAssociatesForDropdown()
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> emps = await (from emp in m_EmployeeContext.Employees
                                                where emp.IsActive == true
                                                && emp.Nationality != "US"
                                                select new GenericType
                                                {
                                                    Id = emp.EmployeeId,
                                                    Name = emp.FirstName + " " + emp.LastName, //$"{emp.FirstName}  {emp.LastName}" 
                                                    DepartmentId = emp.DepartmentId
                                                }).OrderBy(e => e.Name).ToListAsync();
                response.Items = emps;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data ";
                m_Logger.LogError("Error occured in GetAssociateNamesList() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetAssociatesByDepartmentId
        /// <summary>
        ///  GetAssociatesByDepartmentId
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetAssociatesByDepartmentId(int departmentId)
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> emps = await (from emp in m_EmployeeContext.Employees
                                                where emp.IsActive == true
                                                && emp.DepartmentId == departmentId
                                                select new GenericType
                                                {
                                                    Id = emp.EmployeeId,
                                                    Name = emp.FirstName + " " + emp.LastName
                                                }).OrderBy(e => e.Name).ToListAsync();

                if (emps == null || emps.Count == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "no employees found by departmentId";
                    return response;
                }
                else
                {
                    response.Items = emps;
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data ";
                m_Logger.LogError("Error occured in GetAssociatesByDepartmentId() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetDepartmentHeadByDepartmentId
        /// <summary>
        ///  GetDepartmentHeadByDepartmentId
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceResponse<GenericType>> GetDepartmentHeadByDepartmentId(int departmentId)
        {
            var response = new ServiceResponse<GenericType>();
            try
            {
                Department Departmentdetails = null;
                GenericType employee = null;
                if (departmentId != 0)
                {
                    Departmentdetails = m_OrgService.GetDepartmentById((int)departmentId).Result.Item;

                }
                if (Departmentdetails != null)
                {
                    employee = await (from emp in m_EmployeeContext.Employees
                                      where emp.IsActive == true
                                      && emp.EmployeeId == Departmentdetails.DepartmentHeadId
                                      select new GenericType
                                      {
                                          Id = emp.EmployeeId,
                                          Name = emp.FirstName + " " + emp.LastName
                                      }).OrderBy(e => e.Name).FirstOrDefaultAsync();
                    if (employee == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "department head not found by departmentId";
                        return response;
                    }
                    else
                    {
                        response.Item = employee;
                        response.IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data ";
                m_Logger.LogError("Error occured in GetDepartmentHeadByDepartmentId() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetEmployeeDetailsByNameString
        /// <summary>
        ///  Gets the employee details by nameString
        /// </summary>
        /// <param name="nameString">nameString</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeSearchDetails>> GetEmployeeDetailsByNameString(string nameString)
        {
            var response = new ServiceListResponse<EmployeeSearchDetails>();
            try
            {
                List<EmployeeSearchDetails> employees = await m_EmployeeContext.Employees
                    .Where(w => w.FirstName.ToLower().Contains(nameString.ToLower()) || w.LastName.ToLower().Contains(nameString.ToLower()) || w.MiddleName.ToLower().Contains(nameString.ToLower()))
                    .Select(x => new EmployeeSearchDetails
                    {
                        EmployeeId = x.EmployeeId,
                        EmployeeCode = x.EmployeeCode,
                        EmployeeName = !string.IsNullOrWhiteSpace(x.MiddleName) ? x.FirstName + " " + x.MiddleName + "" + x.LastName : x.FirstName + " " + x.LastName,
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName ?? "",
                        LastName = x.LastName
                    }).Distinct().ToListAsync();

                response.Items = employees;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data with name string";
                m_Logger.LogError("Error occured in GetEmployeeDetailsByNameString() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetEmployeeWorkEmailAddress
        /// <summary>
        /// GetEmployeeWorkEmailAddress
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> GetEmployeeWorkEmailAddress(int empId)
        {
            var response = new ServiceResponse<string>();
            try
            {
                m_Logger.LogInformation("EmployeeService: Calling \"GetEmployeeWorkEmailAddress\" method.");
                string workEmailAddress = await m_EmployeeContext.Employees
                                .Where(emp => emp.EmployeeId == empId)
                                .Select(emp => emp.WorkEmailAddress)
                                .FirstOrDefaultAsync();

                response.Item = workEmailAddress;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Employee Work Email Address";
                m_Logger.LogError("Error occured in GetEmployeeWorkEmailAddress() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetEmployeesByRole
        /// <summary>
        /// GetEmployeesByRole
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateRoleType>> GetEmployeesByRole(string employeeCode, int? departmentId = null, int? roleId = null)
        {
            var response = new ServiceListResponse<AssociateRoleType>();
            try
            {
                Department department = null;
                RoleType roleType = null;
                var roles = await m_OrgService.GetRoleTypesForDropdown();
                var grades = await m_OrgService.GetAllGrades();
                if (!string.IsNullOrWhiteSpace(employeeCode))
                {
                    List<AssociateRoleType> employees = (from emp in m_EmployeeContext.Employees
                                                         where (emp.IsActive == true
                                                         && emp.EmployeeCode == employeeCode)
                                                         select new AssociateRoleType
                                                         {
                                                             EmployeeId = emp.EmployeeId,
                                                             EmployeeName = emp.FirstName + ' ' + emp.LastName,
                                                             EmployeeEmail = emp.WorkEmailAddress,
                                                             EmployeeCode = emp.EmployeeCode,
                                                             DepartmentId = emp.DepartmentId ?? 0,
                                                             GradeId = emp.GradeId ?? 0,
                                                             DepartmentName = "",
                                                             EmployeeRoleId = emp.RoleTypeId ?? 0,
                                                             EmployeeRole = "",
                                                             DepartmentHeadName = ""
                                                         }).OrderBy(e => e.EmployeeCode).ToList();

                    if (employees != null && employees.Count == 1)
                    {

                        var dept = await m_OrgService.GetDepartmentById(employees[0].DepartmentId);
                        if (!dept.IsSuccessful)
                        {
                            response.IsSuccessful = false;
                            response.Message = dept.Message;
                            return response;
                        }

                        department = dept.Item;

                        employees[0].DepartmentName = department.Description;

                        var departmentHead = (from emp in m_EmployeeContext.Employees
                                              where (emp.IsActive == true
                                              && emp.EmployeeId == department.DepartmentHeadId)
                                              select new
                                              {
                                                  EmployeeId = emp.EmployeeId,
                                                  EmployeeName = emp.FirstName + ' ' + emp.LastName,
                                                  EmployeeEmail = emp.WorkEmailAddress,
                                                  EmployeeCode = emp.EmployeeCode
                                              }).FirstOrDefault();
                        if (departmentHead == null)
                        {
                            response.IsSuccessful = false;
                            response.Message = "Invalid Department Head";
                            return response;
                        }
                        employees[0].DepartmentHeadName = departmentHead.EmployeeName;
                        employees[0].EmployeeRole = roles.Items.Where(c => c.Id == employees[0].EmployeeRoleId).Select(t => t.Name).FirstOrDefault();
                        employees.ForEach(e => e.GradeName = grades.Items.Where(g => g.GradeId == e.GradeId).Select(s => s.GradeName).FirstOrDefault());
                    }

                    response.IsSuccessful = true;
                    response.Items = employees;
                }
                else if (departmentId.HasValue && roleId.HasValue)
                {
                    var dept = await m_OrgService.GetDepartmentById(departmentId.Value);
                    if (!dept.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = dept.Message;
                        return response;
                    }

                    department = dept.Item;

                    var departmentHead = (from emp in m_EmployeeContext.Employees
                                          where (emp.IsActive == true
                                          && emp.EmployeeId == department.DepartmentHeadId)
                                          select new
                                          {
                                              EmployeeId = emp.EmployeeId,
                                              EmployeeName = emp.FirstName + ' ' + emp.LastName,
                                              EmployeeEmail = emp.WorkEmailAddress,
                                              EmployeeCode = emp.EmployeeCode
                                          }).FirstOrDefault();
                    if (departmentHead == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Invalid Department Head";
                        return response;
                    }

                    var role = await m_OrgService.GetRoleTypeById(roleId.Value);                   
                    if (!role.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = role.Message;
                        return response;
                    }

                    //roleType = role.Item;

                    List<AssociateRoleType> employees = (from emp in m_EmployeeContext.Employees
                                                         where (emp.IsActive == true
                                                         && emp.RoleTypeId == role.Item.RoleTypeId && emp.DepartmentId == departmentId
                                                         && emp.GradeId == role.Item.GradeId)
                                                         select new AssociateRoleType
                                                         {
                                                             EmployeeId = emp.EmployeeId,
                                                             EmployeeName = emp.FirstName + ' ' + emp.LastName,
                                                             EmployeeEmail = emp.WorkEmailAddress,
                                                             EmployeeCode = emp.EmployeeCode,
                                                             DepartmentId = department.DepartmentId,
                                                             DepartmentName = department.Description,
                                                             GradeId = emp.GradeId ?? 0,
                                                             EmployeeRoleId = emp.RoleTypeId ?? 0,
                                                             EmployeeRole = "",
                                                             DepartmentHeadName = departmentHead.EmployeeName
                                                         }).OrderBy(e => e.EmployeeCode).ToList();

                    employees.ForEach(e => e.EmployeeRole = roles.Items.Where(c => c.Id == e.EmployeeRoleId).Select(s => s.Name).FirstOrDefault());
                    employees.ForEach(e => e.GradeName = grades.Items.Where(g => g.GradeId == e.GradeId).Select(s => s.GradeName).FirstOrDefault());
                    response.IsSuccessful = true;
                    response.Items = employees;
                }
                else if (departmentId.HasValue && !roleId.HasValue)
                {
                    var dept = await m_OrgService.GetDepartmentById(departmentId.Value);
                    if (!dept.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = dept.Message;
                        return response;
                    }

                    department = dept.Item;

                    var departmentHead = (from emp in m_EmployeeContext.Employees
                                          where (emp.IsActive == true
                                          && emp.EmployeeId == department.DepartmentHeadId)
                                          select new
                                          {
                                              EmployeeId = emp.EmployeeId,
                                              EmployeeName = emp.FirstName + ' ' + emp.LastName,
                                              EmployeeEmail = emp.WorkEmailAddress,
                                              EmployeeCode = emp.EmployeeCode
                                          }).FirstOrDefault();
                    if (departmentHead == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Invalid Department Head";
                        return response;
                    }

                    if (!roles.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = roles.Message;
                        return response;
                    }

                    List<AssociateRoleType> employees = (from emp in m_EmployeeContext.Employees
                                                         where (emp.IsActive == true
                                                         && emp.DepartmentId == departmentId && emp.RoleTypeId.HasValue)
                                                         select new AssociateRoleType
                                                         {
                                                             EmployeeId = emp.EmployeeId,
                                                             EmployeeName = emp.FirstName + ' ' + emp.LastName,
                                                             EmployeeEmail = emp.WorkEmailAddress,
                                                             EmployeeCode = emp.EmployeeCode,
                                                             DepartmentId = emp.DepartmentId.Value,
                                                             DepartmentName = department.Description,
                                                             GradeId = emp.GradeId ?? 0,
                                                             EmployeeRoleId = emp.RoleTypeId.Value,
                                                             EmployeeRole = "",
                                                             DepartmentHeadName = departmentHead.EmployeeName
                                                         }).OrderBy(e => e.EmployeeCode).ToList();


                    employees.ForEach(e => e.EmployeeRole = roles.Items.Where(c => c.Id == e.EmployeeRoleId).Select(s => s.Name).FirstOrDefault());
                    employees.ForEach(e => e.GradeName = grades.Items.Where(g => g.GradeId == e.GradeId).Select(s => s.GradeName).FirstOrDefault());

                    response.IsSuccessful = true;
                    response.Items = employees;
                }
                else if (!departmentId.HasValue && !roleId.HasValue)
                {
                    var depts = await m_OrgService.GetAllDepartments();
                    if (!depts.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = depts.Message;
                        return response;
                    }

                    List<int> departmentHeadIds = depts.Items.Select(d => d.DepartmentHeadId ?? 0).Distinct().ToList();

                    var departmentHeads = (from emp in m_EmployeeContext.Employees
                                           where (emp.IsActive == true
                                           && departmentHeadIds.Contains(emp.EmployeeId))
                                           select new
                                           {
                                               DepartmentHeadId = emp.EmployeeId,
                                               DepartmentHeadName = emp.FirstName + ' ' + emp.LastName
                                           }).ToList();
                    if (departmentHeads == null || departmentHeads.Count == 0)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Invalid Department Head";
                        return response;
                    }

                    List<AssociateRoleType> employees = (from emp in m_EmployeeContext.Employees
                                                         where (emp.IsActive == true
                                                         && emp.DepartmentId.HasValue && emp.RoleTypeId.HasValue)
                                                         select new AssociateRoleType
                                                         {
                                                             EmployeeId = emp.EmployeeId,
                                                             EmployeeName = emp.FirstName + ' ' + emp.LastName,
                                                             EmployeeEmail = emp.WorkEmailAddress,
                                                             EmployeeCode = emp.EmployeeCode,
                                                             DepartmentId = emp.DepartmentId.Value,
                                                             GradeId = emp.GradeId ?? 0,
                                                             DepartmentName = "",
                                                             EmployeeRoleId = emp.RoleTypeId.Value,
                                                             EmployeeRole = "",
                                                             DepartmentHeadId = 0,
                                                             DepartmentHeadName = ""
                                                         }).OrderBy(e => e.EmployeeCode).ToList();

                    employees.ForEach(e => e.DepartmentName = depts.Items.Where(c => c.DepartmentId == e.DepartmentId).Select(s => s.Description).FirstOrDefault());
                    employees.ForEach(e => e.DepartmentHeadId = depts.Items.Where(c => c.DepartmentId == e.DepartmentId).Select(s => s.DepartmentHeadId ?? 0).FirstOrDefault());
                    employees.ForEach(e => e.EmployeeRole = roles.Items.Where(c => c.Id == e.EmployeeRoleId).Select(s => s.Name).FirstOrDefault());
                    employees.ForEach(e => e.DepartmentHeadName = departmentHeads.Where(c => c.DepartmentHeadId == e.DepartmentHeadId).Select(s => s.DepartmentHeadName).FirstOrDefault());

                    response.IsSuccessful = true;
                    response.Items = employees;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetListAssociatesByRoles
        /// <summary>
        /// Get List Associates By Roles
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeRoleDetails>> GetListAssociatesByRoles(string roles)
        {
            var response = new ServiceListResponse<EmployeeRoleDetails>();
            try
            {
                var usersByRoles = await m_OrgService.GetUsersByRoles(roles);
                var employee = await m_EmployeeContext.Employees.Where(employee => employee.IsActive == true).ToListAsync();
                if (usersByRoles.IsSuccessful)
                {
                    var employeebyroles = (from user in usersByRoles.Items
                                           join emp in employee on user.UserId equals emp.UserId
                                           select new EmployeeRoleDetails
                                           {
                                               EmployeeId = emp.EmployeeId,
                                               EmpName = emp.FirstName + " " + emp.LastName,
                                               RoleId = user.RoleId,
                                               RoleName = user.RoleName
                                           }).ToList();
                    response.Items = employeebyroles;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching employee details by roles";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching employee details by roles";
                m_Logger.LogError("Error occured in GetListAssociatesByRoles() method" + ex.StackTrace);
            }
            return response;
        }


        #endregion

        #region GetEmployeesByCode
        /// <summary>
        /// GetEmployeesByRole
        /// </summary>
        /// <param name="employeeCodes"></param>        
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateModel>> GetEmployeesByCode(List<string> employeeCodes)
        {
            var response = new ServiceListResponse<AssociateModel>();
            try
            {
                if (employeeCodes != null && employeeCodes.Count > 0)
                {
                    List<AssociateModel> employees = (from emp in m_EmployeeContext.Employees
                                                      where (emp.IsActive == true
                                                      && employeeCodes.Contains(emp.EmployeeCode))
                                                      select new AssociateModel
                                                      {
                                                          AssociateId = emp.EmployeeId,
                                                          AssociateName = emp.FirstName + ' ' + emp.LastName,
                                                          AssociateEmail = emp.WorkEmailAddress,
                                                          AssociateCode = emp.EmployeeCode,
                                                          AssociateRole = Convert.ToString(emp.RoleTypeId ?? 0),
                                                          ReportingManagerEmail = "",
                                                          ProgramManagerEmail = ""
                                                      }).OrderBy(e => e.AssociateCode).ToList();

                    if (employees != null && employees.Count > 0)
                    {
                        var roles = await m_OrgService.GetAllRoleTypes();
                        if (!roles.IsSuccessful)
                        {
                            response.IsSuccessful = false;
                            response.Message = roles.Message;
                            return response;
                        }

                        employees.ForEach(e => e.AssociateRole = roles.Items.Where(c => c.RoleTypeId == Convert.ToInt32(e.AssociateRole)).Select(s => s.RoleTypeName).FirstOrDefault());

                    }

                    response.IsSuccessful = true;
                    response.Items = employees;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetEmployeesOnLongLeave
        /// <summary>
        /// GetEmployeesOnLongLeave
        /// </summary>            
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateLongLeaveModel>> GetEmployeesOnLongLeave(int daysLeftToJoin)
        {
            var response = new ServiceListResponse<AssociateLongLeaveModel>();
            try
            {
                List<AssociateLongLeaveModel> employees =
              await (from ell in m_EmployeeContext.AssociateLongLeaves
                     join emp in m_EmployeeContext.Employees on ell.EmployeeId equals emp.EmployeeId
                     where (emp.IsActive == true && ell.IsActive == true
                         && ell.TentativeJoinDate.Date <= DateTime.Now.Date.AddDays(daysLeftToJoin))
                     select new AssociateLongLeaveModel
                     {
                         AssociateId = emp.EmployeeId,
                         AssociateName = emp.FirstName + " " + emp.LastName,
                         AssociateEmail = emp.WorkEmailAddress,
                         AssociateCode = emp.EmployeeCode,
                         LongLeaveStartDate = ell.LongLeaveStartDate.Date,
                         TentativeJoinDate = ell.TentativeJoinDate.Date,
                         Reason = ell.Reason,
                         NoOfDaysLeft = (ell.TentativeJoinDate <= DateTime.Now.Date ? 0 : Convert.ToInt32((ell.TentativeJoinDate.Date - DateTime.Now.Date).TotalDays))
                     }).OrderBy(e => e.AssociateCode).ToListAsync();

                response.IsSuccessful = true;
                response.Items = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee Long Leave data";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetEmployeeRoleTypes
        /// <summary>
        /// GetEmployeeRoleTypes
        /// </summary>
        /// <param name="employeeId"></param>

        /// <returns></returns>
        public async Task<ServiceListResponse<FinancialYearRoleTypeModel>> GetEmployeeRoleTypes(int employeeId)
        {
            var response = new ServiceListResponse<FinancialYearRoleTypeModel>();
            List<FinancialYearRoleTypeModel> items = new List<FinancialYearRoleTypeModel>();
            try
            {
                var roles = await m_OrgService.GetRoleTypesForDropdown();


                var employee = (from emp in m_EmployeeContext.Employees
                                where (emp.IsActive == true
                                && emp.EmployeeId == employeeId)
                                select new { EmployeeId = emp.EmployeeId, EmployeeCode = emp.EmployeeCode, RoleTypeId = emp.RoleTypeId }).FirstOrDefault();

                if (employee.RoleTypeId.HasValue)
                {
                    string roleTypeName = roles.Items.Where(c => c.Id == employee.RoleTypeId.Value).Select(d => d.Name).FirstOrDefault();
                    var financialYears = await m_OrgService.GetAllFinancialYearsAsync();
                    foreach (FinancialYear financialYear in financialYears.Items)
                    {
                        FinancialYearRoleTypeModel employeeRoleType = new FinancialYearRoleTypeModel();
                        employeeRoleType.EmployeeId = employee.EmployeeId;
                        employeeRoleType.EmployeeCode = employee.EmployeeCode;
                        employeeRoleType.FinancialYearId = financialYear.Id;
                        employeeRoleType.FinancialYearName = financialYear.FinancialYearName;
                        employeeRoleType.RoleTypeId = employee.RoleTypeId.Value;
                        employeeRoleType.RoleTypeName = roleTypeName;
                        employeeRoleType.DownloadKRA = false;

                        string filePath = "";
                        string fileName = "";
                        string repoPath = m_MiscellaneousSettings.RepositoryPath;
                        filePath = $@"{repoPath}/{employee.EmployeeCode}/KRA";
                        fileName = $@"{employee.EmployeeCode}_{financialYear.FinancialYearName}_{roleTypeName}_KRA.pdf";

                        if (File.Exists(filePath + @"/" + fileName))
                        {
                            employeeRoleType.DownloadKRA = true;
                        }

                        items.Add(employeeRoleType);
                    }
                }
                response.IsSuccessful = true;
                response.Items = items;

            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region DownloadKRA
        public async Task<ServiceResponse<FileDetail>> DownloadKRA(string employeeCode, string financialYear, string roleType)
        {
            var response = new ServiceResponse<FileDetail>();
            try
            {
                FileDetail file = new FileDetail();
                m_Logger.LogInformation("EmployeeService: Calling \"DownloadKRA\" method.");
                string repoPath = m_MiscellaneousSettings.RepositoryPath;
                string filePath = $@"{repoPath}\{employeeCode}\KRA";
                string fileName = $@"{employeeCode}_{financialYear}_{roleType}_KRA.pdf";

                if (File.Exists(Path.Combine(filePath, fileName)))
                {
                    using (Stream stream = File.OpenRead(Path.Combine(filePath, fileName)))
                    {
                        using (var binaryReader = new BinaryReader(stream))
                        {
                            var fileContent = binaryReader.ReadBytes((int)stream.Length);
                            file.FileName = fileName;
                            file.FileType = @"application/pdf";
                            file.Content = Convert.ToBase64String(fileContent);
                            file.FileContent = fileContent;
                        }
                    }
                }

                response.Item = file;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Employee Work Email Address";
                m_Logger.LogError("Error occured in GetEmployeeWorkEmailAddress() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetAssociatesByProjectId
        /// <summary>
        ///  GetAssociatesByProjectId
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetAssociatesByProjectId(int projectId)
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> employees = null;

                var result = await m_ProjectService.GetUtilizationReportAllocations(projectId);

                if (result.IsSuccessful && result.Items != null)
                {
                    var listEmployeeIdsByProject = result.Items.Where(w => w?.IsFutureProjectMarked != true).Select(x => x.EmployeeId).ToList();

                    employees = await (from emp in m_EmployeeContext.Employees
                                       join ext in m_EmployeeContext.AssociateExit.Where(w => w.IsActive == true)
                                       on emp.EmployeeId equals ext.EmployeeId into exit
                                       from ae in exit.DefaultIfEmpty()
                                       join abs in m_EmployeeContext.AssociateAbscond.Where(w => w.IsActive == true && w.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.AbscondConfirmed))
                                       on emp.EmployeeId equals abs.AssociateId into absc
                                       from aa in absc.DefaultIfEmpty()
                                       where emp.IsActive == true && listEmployeeIdsByProject.Contains(emp.EmployeeId) && ae == null && aa == null
                                       select new GenericType { Id = emp.EmployeeId, Name = emp.FirstName + " " + emp.LastName })
                                                          .OrderBy(c => c.Name).ToListAsync();
                }

                response.Items = employees;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data";
                m_Logger.LogError("Error occured in GetAssociatesByProjectId() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetAssociateRMDetailsByDepartmentId
        /// <summary>
        ///  GetAssociatesByDepartmentId
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateRM>> GetAssociateRMDetailsByDepartmentId(int departmentId)
        {
            var response = new ServiceListResponse<AssociateRM>();
            try
            {
                List<AssociateRM> emps = await (from emp in m_EmployeeContext.Employees
                                                where emp.IsActive == true
                                                && emp.DepartmentId == departmentId
                                                select new AssociateRM
                                                {
                                                    EmployeeId = emp.EmployeeId,
                                                    EmpName = emp.FirstName + " " + emp.LastName,
                                                    ReportingManagerId=emp.ReportingManager
                                                }).OrderBy(e => e.EmpName).ToListAsync();
                emps.ForEach(emp =>
                {
                    var RMDetails = m_EmployeeContext.Employees.Where(x => x.EmployeeId == emp.ReportingManagerId).FirstOrDefault();
                    emp.ReportingManager = RMDetails.FirstName + " " + RMDetails.LastName;
                });

                if (emps == null || emps.Count == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "no employees found by departmentId";
                    return response;
                }
                else
                {
                    response.Items = emps;
                    response.IsSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data ";
                m_Logger.LogError("Error occured in GetAssociatesByDepartmentId() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetServiceDepartmentAssociates
        /// <summary>
        ///  GetServiceDepartmentAssociates
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetServiceDepartmentAssociates()
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> emps = await (from emp in m_EmployeeContext.Employees
                                                    where emp.IsActive == true
                                                    && emp.DepartmentId !=1 
                                                    select new GenericType
                                                    {
                                                        Id = emp.EmployeeId,
                                                        Name = emp.FirstName + " " + emp.LastName
                                                    }).OrderBy(e => e.Name).ToListAsync();
                response.IsSuccessful = true;
                if (emps == null || emps.Count == 0)
                {                  
                    response.Items = new List<GenericType>();                   
                }
                else
                {
                    response.Items = emps;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data ";
                m_Logger.LogError("Error occured in GetServiceDepartmentAssociates() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region UpdateServiceDepartmentAssociateRM
        /// <summary>
        ///  UpdateServiceDepartmentAssociateRM
        /// </summary>        
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> UpdateServiceDepartmentAssociateRM(AssociatesRMDetails  associatesRMDetails)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                associatesRMDetails.Associates.ForEach(associate =>
                {
                    var employee = m_EmployeeContext.Employees.Where(emp => emp.EmployeeId == associate.EmployeeId).FirstOrDefault();
                    employee.ReportingManager = associatesRMDetails.ReportingManagerId;
                });
               await m_EmployeeContext.SaveChangesAsync();
             
                response.IsSuccessful = true;
               
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Employee data ";
                m_Logger.LogError("Error occured in UpdateServiceDepartmentAssociateRM() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion
    }
}
