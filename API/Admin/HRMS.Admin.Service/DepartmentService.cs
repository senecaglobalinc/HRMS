using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Types;
using HRMS.Admin.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    /// <summary>
    /// Service class to Department details
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        #region Global Variables 
        private readonly ILogger<DepartmentService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private IEmployeeService m_EmployeeService;
        #endregion

        #region constructor
        public DepartmentService(ILogger<DepartmentService> logger,
            AdminContext adminContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IEmployeeService employeeService)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Department, Department>();
            });
            m_mapper = config.CreateMapper();
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
            m_EmployeeService = employeeService;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a Department
        /// </summary>
        /// <param name="departmentIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(Department departmentIn)
        {
            m_Logger.LogInformation("Calling Create method in ProficiencyLevelService");
            int isCreated;

            //Checking if Department Code already exists
            var isExists = m_AdminContext.Departments.Where(d => d.DepartmentCode.ToLower().Trim() == departmentIn.DepartmentCode.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Department code already exists");

            //Checking if Department Name already exists
            var isExistsName = m_AdminContext.Departments.Where(d => d.Description.ToLower().Trim() == departmentIn.Description.ToLower().Trim()).Count();

            if (isExistsName > 0)
                return CreateResponse(null, false, "Department Name already exists");

            Department department = new Department();

            if (!departmentIn.IsActive.HasValue)
                departmentIn.IsActive = true;

            //create fields
            m_mapper.Map<Department, Department>(departmentIn, department);

            m_Logger.LogInformation("Add department to list");
            m_AdminContext.Departments.Add(department);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in DepartmentService");
            isCreated = await m_AdminContext.SaveChangesAsync();
            if (isCreated > 0)
            {
                m_Logger.LogInformation("Department created successfully.");
                return CreateResponse(department, true, string.Empty);
            }
            else
            {
                return CreateResponse(null, false, "No Department created.");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets all Departments
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Object>> GetAll(bool isActive = true)
        {
            var departmentHeadIds = m_AdminContext.Departments.Where(dept => dept.DepartmentHeadId != null).Select(dep => dep.DepartmentHeadId).Distinct().ToList();
            var departments = (from department in m_AdminContext.Departments
                               join departmentType in m_AdminContext.DepartmentTypes
                               on department.DepartmentTypeId equals departmentType.DepartmentTypeId into departmentstypes
                               from dept in departmentstypes.DefaultIfEmpty()
                               where department.IsActive == true
                               select new Department
                               {
                                   DepartmentTypeId = department.DepartmentTypeId,
                                   DepartmentHeadId = department.DepartmentHeadId,
                                   DepartmentCode = department.DepartmentCode,
                                   Description = department.Description,
                                   DepartmentId = department.DepartmentId,
                                   DepartmentType = new DepartmentType
                                   {
                                       DepartmentTypeId = dept.DepartmentTypeId,
                                       DepartmentTypeDescription = dept.DepartmentTypeDescription
                                   }
                               }).OrderBy(x => x.Description).ToList();

            if (departmentHeadIds.Count > 0)
            {
                var httpClientFactory = m_clientFactory.CreateClient("EmployeeClient");
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Get;
                httpRequestMessage.RequestUri = new Uri(m_apiEndPoints.AssociateEndPoint +
                 $"{"Employee/GetByIds?employeeIds="}{string.Join(',', departmentHeadIds)} ");
                httpRequestMessage.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await httpClientFactory.SendAsync(httpRequestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    m_Logger.LogError("Error occured while fetching the employee data.");
                }

                string json = await response.Content.ReadAsStringAsync();
                List<EmployeeDetails> employees = JsonConvert.DeserializeObject<List<EmployeeDetails>>(json);

                return from department in departments
                       join employee in employees
                       on department.DepartmentHeadId equals employee.EmpId into DE
                       from subEmployee in DE.DefaultIfEmpty()
                       orderby department.Description
                       select new
                       {
                           DepartmentTypeId = department.DepartmentTypeId,
                           DepartmentHeadId = department.DepartmentHeadId,
                           DepartmentCode = department.DepartmentCode,
                           Description = department.Description,
                           DepartmentId = department.DepartmentId,
                           DepartmentType = new DepartmentType
                           {
                               DepartmentTypeId = department.DepartmentType.DepartmentTypeId,
                               DepartmentTypeDescription = department.DepartmentType.DepartmentTypeDescription
                           },
                           DepartmentHeadName = subEmployee?.EmpName ?? String.Empty
                       };
            }
            return departments.OrderBy(dept => dept.Description);
        }
        #endregion

        #region GetAllDepartments
        /// <summary>
        /// Gets all Departments
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<List<Department>> GetAllDepartments(bool isActive = true)
        {
            List<Department> departments = await m_AdminContext.Departments.Where(department => department.IsActive == isActive)
                .Select(x => new Department { DepartmentId = x.DepartmentId, DepartmentCode = x.DepartmentCode, Description = x.Description }).ToListAsync();
            return departments;
        }
        #endregion

        #region GetUserDepartmentDetails

        public async Task<ServiceListResponse<DepartmentDetails>> GetUserDepartmentDetails()
        {
            var response = new ServiceListResponse<DepartmentDetails>();
            try
            {
                var employees = await m_EmployeeService.GetAll(true);
                var departments = await m_AdminContext.Departments.ToListAsync();
                var departmentList = (from dept in departments
                                      join emp in employees.Items on dept.DepartmentHeadId equals emp.EmployeeId
                                      where dept.IsActive == true && emp.IsActive == true
                                      select new DepartmentDetails
                                      {
                                          DepartmentId = dept.DepartmentId,
                                          Description = dept.Description,
                                          DepartmentCode = dept.DepartmentCode,
                                          DepartmentHeadId = dept.DepartmentHeadId
                                      }).ToList();
                if (departmentList == null || departmentList.Count == 0)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "No Associate Allocatins found for this employee.";
                }
                else
                {
                    response.Items = departmentList;
                    response.IsSuccessful = true;
                    response.Message = "";
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

        #region GetUserDepartmentDetailsByEmployeeID

        public async Task<ServiceListResponse<DepartmentDetails>> GetUserDepartmentDetailsByEmployeeID(int employeeId)
        {
            var response = new ServiceListResponse<DepartmentDetails>();
            try
            {
                var employees = await m_EmployeeService.GetAll(true);
                if (!employees.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Items = null;
                    response.Message = employees.Message;
                    return response;
                }
                var departments = await m_AdminContext.Departments.ToListAsync();

                var departmentList = (from dept in departments
                                      join emp in employees.Items on dept.DepartmentHeadId equals emp.EmployeeId
                                      where dept.IsActive == true && emp.IsActive == true && dept.DepartmentHeadId == employeeId
                                      select new DepartmentDetails
                                      {
                                          DepartmentId = dept.DepartmentId,
                                          Description = dept.Description,
                                          DepartmentCode = dept.DepartmentCode,
                                          DepartmentHeadId = dept.DepartmentHeadId
                                      }).ToList();
                if (departmentList.Count == 0)
                {
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "No departments found for this employee" + employeeId;
                }
                else
                {
                    response.Items = departmentList;
                    response.IsSuccessful = true;
                    response.Message = "";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Items = null;
                response.Message = "Error occured in Department Service-GetUserDepartmentDetailsByEmployeeID method";
                m_Logger.LogError("Error occured in Department Service-GetUserDepartmentDetailsByEmployeeID method" + ex.StackTrace);
            }
            return response;
        }

        #endregion  

        #region GetByDepartmentCode
        /// <summary>
        /// Get department by code
        /// </summary>
        /// <param name="departmentCode">Client code</param>
        /// <returns></returns>

        public async Task<Department> GetByDepartmentCode(string departmentCode) =>
            await m_AdminContext.Departments.Where(cl => cl.DepartmentCode.ToLower().Trim() == departmentCode.ToLower().Trim())
            .FirstOrDefaultAsync();
        #endregion

        #region GetByDepartmentCodes
        /// <summary>
        /// Get department by codes
        /// </summary>
        /// <param name="departmentCodes">Client code</param>
        /// <returns></returns>

        public async Task<List<Department>> GetByDepartmentCodes(string departmentCodes)
        {
            m_Logger.LogInformation("Calling \"GetByDepartmentCodes\" method in AdminService");
            List<Department> departments = new List<Department>();
            try
            {
                List<string> deptCodes = departmentCodes.Split(",".ToCharArray()).ToList();
                departments = await m_AdminContext.Departments.Where(w => deptCodes.Contains(w.DepartmentCode)).ToListAsync();
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Exception occured in \"GetByDepartmentCodes\" method in AdminService" + ex.Message);
            }

            return departments;
        }
        #endregion

        #region GetById
        /// <summary>
        /// Get department details by id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<Department> GetById(int departmentId) =>
            await m_AdminContext.Departments.Where(cl => cl.DepartmentId == departmentId)
            .FirstOrDefaultAsync();
        #endregion

        #region Update
        /// <summary>
        /// Updates the Department details
        /// </summary>
        /// <param name="n_proficiencyLevel"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(Department departmentIn)
        {
            int isUpdated;
            m_Logger.LogInformation("Calling Update method in DepartmentService");

            //Checking if Department code already exists
            var isExists = m_AdminContext.Departments.Where(p => p.DepartmentCode.ToLower().Trim() == departmentIn.DepartmentCode.ToLower().Trim() && p.DepartmentId != departmentIn.DepartmentId).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Department code already exists");

            //Checking if Department Name already exists
            var isExistsName = m_AdminContext.Departments.Where(d => d.Description.ToLower().Trim() == departmentIn.Description.ToLower().Trim() && d.DepartmentId != departmentIn.DepartmentId).Count();

            if (isExistsName > 0)
                return CreateResponse(null, false, "Department Name already exists");

            //Fetch Department for update
            var department = m_AdminContext.Departments.Find(departmentIn.DepartmentId);

            if (department == null)
                return CreateResponse(null, false, "Department not found for update.");

            if (!departmentIn.IsActive.HasValue)
                departmentIn.IsActive = department.IsActive;

            departmentIn.CreatedBy = department.CreatedBy;
            departmentIn.CreatedDate = department.CreatedDate;

            //Fetch proficiencyLevel for update
            m_mapper.Map<Department, Department>(departmentIn, department);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in DepartmentService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating Department record in DepartmentService.");
                return CreateResponse(department, true, string.Empty);
            }
            else
            {
                return CreateResponse(null, false, "No record updated.");
            }
        }
        #endregion

        #region GetDepartmentDLByDeptId 
        /// <summary>
        /// Get DepartmentDL By DeptId
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public async Task<DepartmentDL> GetDepartmentDLByDeptId(int deptId) =>
             await m_AdminContext.DepartmentDL.Where(w => w.DepartmentId == deptId).FirstOrDefaultAsync();
        #endregion

        #region GetMasterTablesData 
        /// <summary>
        /// Get MasterTablesData
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<MasterDetails>> GetMasterTablesData()
        {
            ServiceListResponse<MasterDetails> response = new ServiceListResponse<MasterDetails>();

            try
            {
                List<MasterDetails> masterDetails = new List<MasterDetails>();

                masterDetails.AddRange(await m_AdminContext.Departments
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new MasterDetails
                                        {
                                            DepartmentId = c.DepartmentId,
                                            Description = c.Description,
                                            RecordType = (int)RecordTypeEnum.Department
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Grades
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new MasterDetails
                                        {
                                            GradeId = c.GradeId,
                                            GradeName = c.GradeName,
                                            GradeCode = c.GradeCode,
                                            RecordType = (int)RecordTypeEnum.Grade
                                        })
                                        .ToListAsync());


                masterDetails.AddRange(await m_AdminContext.FinancialYears
                                        .Where(c => c.Id > 0)
                                        .Select(c => new MasterDetails
                                        {
                                            Id = c.Id,
                                            FinancialYearName = Convert.ToString(c.FromYear) + " - " + Convert.ToString(c.ToYear),
                                            RecordType = (int)RecordTypeEnum.FinancialYears
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await (from gr in m_AdminContext.GradeRoleTypes
                                              join g in m_AdminContext.Grades on gr.GradeId equals g.GradeId
                                              join r in m_AdminContext.RoleTypes on gr.RoleTypeId equals r.RoleTypeId
                                              where gr.IsActive == true && r.IsActive == true
                                              select new MasterDetails()
                                              {
                                                  RoleTypeId = gr.RoleTypeId,
                                                  RoleTypeName = r.RoleTypeName,
                                                  RoleTypeDescription = r.RoleTypeDescription,
                                                  GradeId = gr.GradeId,
                                                  GradeRoleTypeId = gr.GradeRoleTypeId,
                                                  RecordType = (int)RecordTypeEnum.GradeRoleTypes
                                              }).ToListAsync());


                return response = new ServiceListResponse<MasterDetails>()
                {
                    Items = masterDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<MasterDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion

        #region GetAllDepartmentDLs 
        /// <summary>
        /// Get AllDepartmentDLs 
        /// </summary>
        /// <returns></returns>
        public async Task<List<DepartmentWithDLAddress>> GetAllDepartmentDLs()
        {
            m_Logger.LogInformation("Calling \"GetAllDepartmentDLs\" method in AdminService");
            List<DepartmentWithDLAddress> departmentDLAddress = new List<DepartmentWithDLAddress>();
            try
            {
                departmentDLAddress = await (from depts in m_AdminContext.Departments
                                             join deptdls in m_AdminContext.DepartmentDL
                                             on depts.DepartmentId equals deptdls.DepartmentId
                                             select new DepartmentWithDLAddress
                                             {
                                                 DepartmentId = depts.DepartmentId,
                                                 DepartmentCode = depts.DepartmentCode,
                                                 DepartmentDescription = depts.Description,
                                                 DepartmentHeadId = depts.DepartmentHeadId,
                                                 DepartmentDLAddress = deptdls.DLEmailAdress
                                             }).ToListAsync();
            }
            catch (Exception ex)
            {
                m_Logger.LogInformation("Exception occured in \"GetAllDepartmentDLs\" method in AdminService" + ex.Message);
            }

            return departmentDLAddress;
        }
        #endregion

        //Private Method
        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="department"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(Department department, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in DepartmentService");

            dynamic response = new ExpandoObject();
            response.Department = department;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in DepartmentService");

            return response;
        }

        #endregion
    }
}
