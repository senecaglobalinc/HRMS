using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.API.Auth;
namespace HRMS.Employee.API.Controllers
{
    [Route("employee/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : Controller
    {
        #region Global Variables

        private readonly IEmployeeService m_EmployeeService;
        private readonly ILogger<EmployeeController> m_Logger;

        #endregion

        #region Constructor
        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            m_EmployeeService = employeeService;
            m_Logger = logger;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// GetAll Employees
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Employee.Entities.Employee>>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("Retrieving records from employee table.");
            try
            {
                var employees = await m_EmployeeService.GetAll(isActive);
                if (!employees.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in employee table.");
                    return NotFound(employees.Message);                
                }
                else
                {
                    m_Logger.LogInformation($"Returning { employees.Items.Count } Skill Search.");
                    return Ok(employees.Items);                
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetAll method in EmployeeController"+ ex.StackTrace);
                return BadRequest("Error Occured while getting all the employees in GetAll method");
            }


        }
        #endregion

        #region GetEmployeeByUserName
        /// <summary>
        /// Gets the employee information by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeeByUserName/{userName}")]
        public async Task<ActionResult<List<EmployeeDetails>>> GetEmployeeByUserName(string userName)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table by {userName}.");

            try
            {
                var employee = await m_EmployeeService.GetEmployeeByUserName(userName);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table on {userName}.");
                    
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeByUserName() in EmployeeController:" + stopwatch.Elapsed);
                    
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for Username {userName}.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeByUserName() in EmployeeController:" + stopwatch.Elapsed);

                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeByUserName() method in EmployeeController:" + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeOnUserName() in EmployeeController:" + stopwatch.Elapsed);

                return BadRequest("Error Occured in GetEmployeeByUserName() method in EmployeeController:");
            }

        }
        #endregion

        #region GetEmployeeNames
        /// <summary>
        /// Get the active Employee's names
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeeNames")]
        public async Task<ActionResult<List<EmployeeDetails>>> GetEmployeeNames()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from Employee table.");

            try
            {
                var empNames = await m_EmployeeService.GetEmployeeNames();
                if (!empNames.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in Employee table.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeNames() in EmployeeController:" + stopwatch.Elapsed);

                    return NotFound(empNames.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employee names.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeNames() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(empNames.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeNames() method in EmployeeController:" + ex.Message);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeNames() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmployeeNames() method in EmployeeController");
            }


        }
        #endregion

        #region GetById
        /// <summary>
        /// Get the employee by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Employee.Entities.Employee>> GetById(int id)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table by {id}.");

            try
            {
                var employee = await m_EmployeeService.GetById(id);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for employeeId {id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetById() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for employeeId {id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetById() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetById() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetById() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetById() method in EmployeeController");
            }

        }
        #endregion

        #region GetByIds
        /// <summary>
        /// Get the employees by ids
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetByIds")]
        public async Task<ActionResult<List<EmployeeDetails>>> GetByIds([FromQuery(Name = "employeeIds")] string id)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table by {id}.");

            try
            {
                var employee = await m_EmployeeService.GetByIds(id);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for employeeId {id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByIds() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for employeeId {id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByIds() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetById() method in EmployeeController:"+ ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetByIds() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetById() method in EmployeeController");
            }

        }
        #endregion

        #region GetEmpTypes
        /// <summary>
        /// Get All Active Employee Types
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmpTypes")]
        public async Task<ActionResult<List<EmployeeType>>> GetEmpTypes()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from employee type table.");

            try
            {
                var employeeTypes = await m_EmployeeService.GetEmpTypes();
                if (!employeeTypes.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in employee type table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmpTypes() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employeeTypes.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { employeeTypes.Items.Count } Employee Types.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmpTypes() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employeeTypes.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmpTypes() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmpTypes() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmpTypes() method in EmployeeController");
            }
        }
        #endregion

        #region GetJoinedEmployees
        /// <summary>
        ///  Get the active Employees based on PracticeArea, Departments, designations and statusId.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetJoinedEmployees")]
        public async Task<ActionResult<IEnumerable>> GetJoinedEmployees()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Joined Employees records");

            try
            {
                var employee =  await m_EmployeeService.GetJoinedEmployees();
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No Joined employee records found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetJoinedEmployees() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Joined Employees");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetJoinedEmployees() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetJoinedEmployees() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetJoinedEmployees() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetJoinedEmployees() method in EmployeeController:");
            }

        }
        #endregion

        #region GetBusinessValues
        /// <summary>
        /// Get the BusinessValues by valueKey
        /// </summary>
        /// <param name="valueKey"></param>
        /// <returns></returns>
        [HttpGet("GetBusinessValues/{valueKey}")]
        public async Task<ActionResult<List<lkValue>>> GetBusinessValues(string valueKey)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from lkValue table by {valueKey}.");

            try
            {
                var lkValues = await m_EmployeeService.GetBusinessValues(valueKey);
                if (!lkValues.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in lkValue table on {valueKey}.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetBusinessValues() in EmployeeController:" + stopwatch.Elapsed);

                    return NotFound(lkValues.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for valueKey {valueKey}.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetBusinessValues() in EmployeeController:" + stopwatch.Elapsed);

                    return Ok(lkValues.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetBusinessValues() method in EmployeeController:" + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetBusinessValues() in EmployeeController:" + stopwatch.Elapsed);

                return BadRequest("Error Occured in retreiving business values");
            }
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
        [HttpGet("GetEmployeeInfo")]
        public async Task<ActionResult<List<EmployeeDetails>>> GetEmployeeInfo(string searchString = null, int pageIndex = 0, int pageSize = 0)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employees Information");

            try
            {
                var employee = await m_EmployeeService.GetEmployeeInfo(searchString, pageIndex, pageSize);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"employees information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeInfo() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeInfo() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeInfo() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeInfo() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmployeeInfo() method in EmployeeController");
            }

        }
        #endregion

        #region GetEmployeeOnPagination
        /// <summary>
        /// Gets the GetEmployeeOnPagination based on departments and approved status.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeeOnPagination")]
        public async Task<ActionResult<List<EmployeeDetails>>> GetEmployeeOnPagination(string searchString,int pageIndex, int pageSize)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employees Information");

            try
            {
                var employee = await m_EmployeeService.GetEmployeeOnPagination(searchString,pageIndex, pageSize);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"employees information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeOnPagination() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeOnPagination() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeOnPagination() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeOnPagination() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmployeeOnPagination() method in EmployeeController");
            }

        }
        #endregion

        #region GetEmployeeCount
        /// <summary>
        /// Gets the Active GetEmployeeCount .
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeeCount")]
        public async Task<ActionResult<int>> GetEmployeeCount(string searchString)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employee Count");

            try
            {
                var employeeCount = await m_EmployeeService.GetEmployeeCount(searchString);
                if (!employeeCount.IsSuccessful)
                {
                    m_Logger.LogInformation($"employees information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeCount() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employeeCount.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute employeeCount() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employeeCount.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in employeeCount() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute employeeCount() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in employeeCount() method in EmployeeController");
            }

        }
        #endregion

        #region GetEmployeesBySkillUsingCache
        /// <summary>
        /// GetEmployeesBySkillUsingCache
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeesBySkillUsingCache")]
        public ActionResult<EmployeeSkills> GetEmployeesBySkillUsingCache()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            m_Logger.LogInformation($"Retrieving records for EmployeeSkills.");

            try
            {
                var employeeSkills = m_EmployeeService.GetEmployeesBySkillUsingCache();
                if (employeeSkills == null)
                {
                    m_Logger.LogInformation($"No records found for employeeSkills.");
                    return NotFound();
                }
                else
                {
                    m_Logger.LogInformation($"{employeeSkills.Count} records found for employeeSkills.");
                    stopwatch.Stop();
                    m_Logger.LogInformation($"Time to complete execution id {stopwatch.Elapsed}.");
                    return Ok(employeeSkills);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in  GetEmployeesBySkillUsingCache() method" + ex.StackTrace);
                return BadRequest();
            }

        }
        #endregion

		#region GetByUserId
        /// <summary>
        /// GetByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetByUserId/{userId}")]
        public async Task<ActionResult<Employee.Entities.Employee>> GetByUserId(int userId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table by userId {userId}.");

            try
            {
                var employee = await m_EmployeeService.GetByUserId(userId);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for userId {userId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByUserId() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for userId {userId}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetByUserId() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetByUserId() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest();
            }

        }
        #endregion

		#region GetActiveEmployeeById
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetActiveEmployeeById/{id}")]
        public async Task<ActionResult<Employee.Entities.Employee>> GetActiveEmployeeById(int id)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table by {id}.");

            try
            {
                var employee = await m_EmployeeService.GetActiveEmployeeById(id);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for employeeId {id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetActiveEmployeeById() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for employeeId {id}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetActiveEmployeeById() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetActiveEmployeeById() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest();
            }

        }
        #endregion

        #region GetManagersAndLeads
        /// <summary>
        ///  Get the active Managers, Leads, ProgramMangers, ReportingManagers, department heads and competency leads.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetManagersAndLeads")]
        public async Task<ActionResult<IEnumerable>> GetManagersAndLeads(int? departmentId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving active Managers, Leads, ProgramMangers, ReportingManagers, " +
                                    $" department heads and competency leads.");

            try
            {
                var employee = await m_EmployeeService.GetManagersAndLeads(departmentId);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetManagersAndLeads() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning active Managers, Leads, ProgramMangers, ReportingManagers, " +
                                    $" department heads and competency leads.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetManagersAndLeads() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetManagersAndLeads() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetManagersAndLeads() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetManagersAndLeads() method in EmployeeController:");
            }

        }
        #endregion

		#region GetProgramManagersList
        /// <summary>
        /// Gets Managers list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProgramManagersList")]
        public async Task<ActionResult<List<Manager>>> GetProgramManagersList()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from employee table.");

            try
            {
                var managers = await m_EmployeeService.GetProgramManagersList();
                if (!managers.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetProgramManagersList() in EmployeeController:" + stopwatch.Elapsed);

                    return NotFound(managers.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { managers.Items.Count } Skill Search.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAll() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(managers.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetProgramManagersList() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest();
            }


        }
        #endregion

        #region GetAllActiveEmployees
        /// <summary>
        /// Gets All Active Employees
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmpList")]
        public async Task<ActionResult<List<Employee.Entities.Employee>>> GetEmpList()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving records from employee table.");

            try
            {
                var employees = await m_EmployeeService.GetAllActiveEmployees();
                if (!employees.IsSuccessful)
                {
                    m_Logger.LogInformation("No records found in employee table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmpList() in EmployeeController:" + stopwatch.Elapsed);

                    return NotFound(employees.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning { employees.Items.Count } Skill Search.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmpList() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employees.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError(ex.Message);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmpList() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest();
            }


        }
        #endregion

        #region GetStatusbyId
        /// <summary>
        /// Get employee Status based on Id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet("GetStatusbyId/{empId}")]
        public async Task<ActionResult<int>> GetStatusbyId(int empId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving status from employee table.");

            try
            {
                var status = await m_EmployeeService.GetStatusbyId(empId);
                if (!status.IsSuccessful)
                {
                    m_Logger.LogInformation("No record found in employee table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetStatusbyId() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(status.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Successfully fetched status of the employee with id {empId}");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetStatusbyId() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(status.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in GetStatusbyId() method of EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetStatusbyId() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while fecthing status");
            }
        }
        #endregion

        #region GetPendingProfiles
        /// <summary>
        /// Gets the list of Employees whose profiles are pending.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPendingProfiles")]
        public async Task<ActionResult<List<EmployeeProfileStatus>>> GetPendingProfiles()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employees Information");

            try
            {
                var employee = await m_EmployeeService.GetPendingProfiles();
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"employees information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetPendingProfiles() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetPendingProfiles() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetPendingProfiles() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetPendingProfiles() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetPendingProfiles() method in EmployeeController");
            }

        }
        #endregion

        #region GetRejectedProfiles
        /// <summary>
        /// Gets the list of Employees whose profiles are rejected.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRejectedProfiles")]
        public async Task<ActionResult<List<EmployeeProfileStatus>>> GetRejectedProfiles()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employees Information");

            try
            {
                var employee = await m_EmployeeService.GetRejectedProfiles();
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"employees information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetRejectedProfiles() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetRejectedProfiles() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetRejectedProfiles() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetRejectedProfiles() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetRejectedProfiles() method in EmployeeController");
            }

        }
        #endregion

        #region GetEmployeeBySearchString
        /// <summary>
        /// Gets the employee information by username
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeeBySearchString/{searchString}")]
        public async Task<ActionResult<List<EmployeeDetails>>> GetEmployeeBySearchString(string searchString)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table by {searchString}.");

            try
            {
                var employee = await m_EmployeeService.GetEmployeeBySearchString(searchString);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table on {searchString}.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeBySearchString() in EmployeeController:" + stopwatch.Elapsed);

                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for {searchString}.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeBySearchString() in EmployeeController:" + stopwatch.Elapsed);

                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeBySearchString() method in EmployeeController:" + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeBySearchString() in EmployeeController:" + stopwatch.Elapsed);

                return BadRequest("Error Occured in GetEmployeeBySearchString() method in EmployeeController:");
            }

        }
        #endregion

        #region GetEmployeesForDropdown
        /// <summary>
        /// Gets the employee information 
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetEmployeesForDropdown")]
        public async Task<ActionResult<List<GenericType>>> GetEmployeesForDropdown()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table.");

            try
            {
                var employee = await m_EmployeeService.GetEmployeesForDropdown();
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeesForDropdown() in EmployeeController:" + stopwatch.Elapsed);

                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeesForDropdown() in EmployeeController:" + stopwatch.Elapsed);

                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeBySearchString() method in EmployeeController:" + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeBySearchString() in EmployeeController:" + stopwatch.Elapsed);

                return BadRequest("Error Occured in GetEmployeeBySearchString() method in EmployeeController:");
            }

        }
        #endregion        

        #region GetAssociatesForDropdown
        /// <summary>
        /// Gets the employee information 
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetAssociatesForDropdown")]
        public async Task<ActionResult<List<GenericType>>> GetAssociatesForDropdown()
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table.");

            try
            {
                var employee = await m_EmployeeService.GetAssociatesForDropdown();
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesForDropdown() in EmployeeController:" + stopwatch.Elapsed);

                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesForDropdown() in EmployeeController:" + stopwatch.Elapsed);

                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeBySearchString() method in EmployeeController:" + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeBySearchString() in EmployeeController:" + stopwatch.Elapsed);

                return BadRequest("Error Occured in GetEmployeeBySearchString() method in EmployeeController:");
            }

        }
        #endregion

        #region GetAssociatesByDepartmentId
        /// <summary>
        /// Get Associates By DepartmentId
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetAssociatesByDepartmentId")]
        public async Task<ActionResult<List<GenericType>>> GetAssociatesByDepartmentId(int departmentId)
        {
            m_Logger.LogInformation($"Retrieving records from Employee table by department Id.");

            try
            {
                var employees = await m_EmployeeService.GetAssociatesByDepartmentId(departmentId);
                if (!employees.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table.");
                    m_Logger.LogInformation("Time to execute GetAssociatesByDepartmentId() in EmployeeController:");
                    return NotFound(employees.Message);
                }
                else
                {
                    m_Logger.LogInformation($"returning employees records");
                    return Ok(employees.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetAssociatesByDepartmentId() method in EmployeeController:" + ex.StackTrace);
                return BadRequest("Error Occured in GetAssociatesByDepartmentId() method in EmployeeController:");
            }

        }
        #endregion

        #region GetDepartmentHeadByDepartmentId
        /// <summary>
        /// GetDepartmentHeadByDepartmentId
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetDepartmentHeadByDepartmentId/{departmentId}")]
        public async Task<ActionResult<GenericType>> GetDepartmentHeadByDepartmentId(int departmentId)
        {
            m_Logger.LogInformation($"Retrieving records from  Employee table by department head Id.");

            try
            {
                var employees = await m_EmployeeService.GetDepartmentHeadByDepartmentId(departmentId);
                if (!employees.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table.");
                    m_Logger.LogInformation("Time to execute GetDepartmentHeadByDepartmentId() in EmployeeController:");
                    return NotFound(employees.Message);
                }
                else
                {
                    m_Logger.LogInformation($"returning employees records");
                    return Ok(employees.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetDepartmentHeadByDepartmentId() method in EmployeeController:" + ex.StackTrace);
                return BadRequest("Error Occured in GetDepartmentHeadByDepartmentId() method in EmployeeController:");
            }

        }
        #endregion     

        #region GetEmployeeDetailsByNameString
        /// <summary>
        ///  Gets the employee id and name by nameString
        /// </summary>
        /// <param name="nameString">nameString</param>
        /// <returns></returns>
        [HttpGet("GetEmployeeDetailsByNameString/{nameString}")]
        public async Task<ActionResult<List<EmployeeSearchDetails>>> GetEmployeeDetailsByNameString(string nameString)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table by {nameString}.");

            try
            {
                var employee = await m_EmployeeService.GetEmployeeDetailsByNameString(nameString);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table on {nameString}.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeDetailsByNameString() in EmployeeController:" + stopwatch.Elapsed);

                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for {nameString}.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeDetailsByNameString() in EmployeeController:" + stopwatch.Elapsed);

                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeDetailsByNameString() method in EmployeeController:" + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeDetailsByNameString() in EmployeeController:" + stopwatch.Elapsed);

                return BadRequest("Error Occured in GetEmployeeDetailsByNameString() method in EmployeeController:");
            }

        }
        #endregion

        #region GetEmployeeWorkEmailAddress
        /// <summary>
        /// Get Employee Work Email Address
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeeWorkEmailAddress/{empId}")]
        public async Task<ActionResult<string>> GetEmployeeWorkEmailAddress(int empId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation("Retrieving Employee Work Email Address from employee table.");

            try
            {
                var workEmailAddress = await m_EmployeeService.GetEmployeeWorkEmailAddress(empId);
                if (!workEmailAddress.IsSuccessful)
                {
                    m_Logger.LogInformation("No record found in employee table.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeWorkEmailAddress() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(workEmailAddress.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Successfully fetched work email address of the employee with id {empId}");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeWorkEmailAddress() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(workEmailAddress.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in GetEmployeeWorkEmailAddress() method of EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeWorkEmailAddress() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error occured while fecthing status");
            }
        }
        #endregion

        #region GetEmployeesByRole
        /// <summary>
        /// Gets the list of Employees whose profiles are pending.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeesByRole")]
        public async Task<ActionResult<List<AssociateRoleType>>> GetEmployeesByRole(string employeeCode, int? departmentId = null, int? roleId = null)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employees Information");

            try
            {
                var employee = await m_EmployeeService.GetEmployeesByRole(employeeCode, departmentId, roleId);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"employees information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeesByRole() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeesByRole() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeesByRole() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeesByRole() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmployeesByRole() method in EmployeeController");
            }

        }
        #endregion

        #region GetEmployeesByCode
        /// <summary>
        /// Gets the list of Employees whose profiles are pending.
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetEmployeesByCode")]
        public async Task<ActionResult<List<AssociateModel>>> GetEmployeesByCode([FromBody]List<string> employeeCodes)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employees Information");

            try
            {
                var employee = await m_EmployeeService.GetEmployeesByCode(employeeCodes);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"employees information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeesByCode() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeesByCode() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeesByCode() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeesByCode() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmployeesByCode() method in EmployeeController");
            }

        }
        #endregion  
        
        #region GetListAssociatesByRoles
        /// <summary>
        /// Get List Associates By Roles
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpGet("GetListAssociatesByRoles/{roles}")]
        public async Task<ActionResult<List<EmployeeRoleDetails>>> GetListAssociatesByRoles(string roles)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table by {roles}.");

            try
            {
                var employee = await m_EmployeeService.GetListAssociatesByRoles(roles);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found for employees by roles{roles}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetListAssociatesByRoles() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found for employee by roles{roles}.");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetListAssociatesByRoles() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetListAssociatesByRoles() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetListAssociatesByRoles() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetListAssociatesByRoles() method in EmployeeController");
            }

        }
        #endregion

        #region GetEmployeesOnLongLeave
        /// <summary>
        /// Gets the list of Employees on Long Leave.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeesOnLongLeave")]
        public async Task<ActionResult<List<AssociateLongLeaveModel>>> GetEmployeesOnLongLeave(int daysLeftToJoin)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employees On LongLeave Information");

            try
            {
                var employee = await m_EmployeeService.GetEmployeesOnLongLeave(daysLeftToJoin);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"Employees On LongLeave information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeesOnLongLeave() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning GetEmployees On LongLeave Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeesOnLongLeave() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeesOnLongLeave() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeesOnLongLeave() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmployeesOnLongLeave() method in EmployeeController");
            }

        }
        #endregion

        #region GetEmployeeRoleTypes
        /// <summary>
        /// Gets the Employee RoleTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeeRoleTypes")]
        public async Task<ActionResult<List<FinancialYearRoleTypeModel>>> GetEmployeeRoleTypes(int employeeId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving Employees Information");

            try
            {
                var employee = await m_EmployeeService.GetEmployeeRoleTypes(employeeId);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"employees information not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeRoleTypes() in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Returning Employees Information");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetEmployeeRoleTypes() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetEmployeeRoleTypes() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetEmployeeRoleTypes() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in GetEmployeeRoleTypes() method in EmployeeController");
            }

        }
        #endregion

        #region DownloadKRA
        /// <summary>
        /// Download KRA
        /// </summary>
        /// <returns></returns>
        [HttpGet("DownloadKRA")]
        public async Task<ActionResult<FileDetail>> DownloadKRA(string employeeCode, string financialYear, string roleType)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Download KRA");

            try
            {
                var employee = await m_EmployeeService.DownloadKRA(employeeCode, financialYear, roleType);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"KRA File not found");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute DownloadKRA in EmployeeController:" + stopwatch.Elapsed);
                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"Download KRA");
                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute DownloadKRA() in EmployeeController:" + stopwatch.Elapsed);
                    return Ok(employee.Item);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in DownloadKRA() method in EmployeeController:" + ex.StackTrace);
                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute DownloadKRA() in EmployeeController:" + stopwatch.Elapsed);
                return BadRequest("Error Occured in DownloadKRA() method in EmployeeController");
            }

        }
        #endregion

        #region GetAssociatesByProjectId
        /// <summary>
        /// GetAssociatesByProjectId 
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetAssociatesByProjectId/{projectId}")]
        public async Task<ActionResult<List<GenericType>>> GetAssociatesByProjectId(int projectId)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            m_Logger.LogInformation($"Retrieving records from Employee table.");

            try
            {
                var employee = await m_EmployeeService.GetAssociatesByProjectId(projectId);
                if (!employee.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesByProjectId() in EmployeeController:" + stopwatch.Elapsed);

                    return NotFound(employee.Message);
                }
                else
                {
                    m_Logger.LogInformation($"records found.");

                    //// Stop timing.
                    stopwatch.Stop();
                    m_Logger.LogInformation("Time to execute GetAssociatesByProjectId() in EmployeeController:" + stopwatch.Elapsed);

                    return Ok(employee.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetAssociatesByProjectId() method in EmployeeController:" + ex.StackTrace);

                //// Stop timing.
                stopwatch.Stop();
                m_Logger.LogInformation("Time to execute GetAssociatesByProjectId() in EmployeeController:" + stopwatch.Elapsed);

                return BadRequest("Error Occured in GetAssociatesByProjectId() method in EmployeeController:");
            }

        }
        #endregion

        #region GetAssociateRMDetailsByDepartmentId
        /// <summary>
        /// Get Associates RM Details By DepartmentId
        /// <paramref name="departmentId"/>
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetAssociateRMDetailsByDepartmentId/{departmentId}")]
        public async Task<IActionResult> GetAssociateRMDetailsByDepartmentId(int departmentId)
        {

            try
            {
                var employees = await m_EmployeeService.GetAssociateRMDetailsByDepartmentId(departmentId);
                if (!employees.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table.");
                    m_Logger.LogInformation("Time to execute GetAssociateRMDetailsByDepartmentId() in EmployeeController:");
                    return NotFound(employees.Message);
                }
                else
                {
                    m_Logger.LogInformation($"returning employees records");
                    return Ok(employees.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetAssociateRMDetailsByDepartmentId() method in EmployeeController:" + ex.StackTrace);
                return BadRequest("Error Occured in GetAssociateRMDetailsByDepartmentId() method in EmployeeController:");
            }

        }
        #endregion

        #region GetServiceDepartmentAssociates 
        /// <summary>
        /// GetServiceDepartmentAssociates
        /// </summary>       
        /// <returns></returns>
        [HttpGet("GetServiceDepartmentAssociates")]
        public async Task<ActionResult> GetServiceDepartmentAssociates()
        {
           
            try
            {
                var employees = await m_EmployeeService.GetServiceDepartmentAssociates();
                if (!employees.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table.");
                    m_Logger.LogInformation("Time to execute GetServiceDepartmentAssociates() in EmployeeController:");
                    return NotFound(employees.Message);
                }
                else
                {
                    m_Logger.LogInformation($"returning employees records");
                    return Ok(employees.Items);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in GetServiceDepartmentAssociates() method in EmployeeController:" + ex.StackTrace);
                return BadRequest("Error Occured in GetServiceDepartmentAssociates() method in EmployeeController:");
            }

        }
        #endregion

        #region UpdateServiceDepartmentAssociateRM 
        /// <summary>
        ///UpdateServiceDepartmentAssociateRM
        /// </summary>       
        /// <returns></returns>
        [HttpPost("UpdateServiceDepartmentAssociateRM")]
        public async Task<IActionResult> UpdateNonDeliveryAssociateRM(AssociatesRMDetails associatesRMDetails)
        {

            try
            {
                var employees = await m_EmployeeService.UpdateServiceDepartmentAssociateRM(associatesRMDetails);
                if (!employees.IsSuccessful)
                {
                    m_Logger.LogInformation($"No records found in employee table.");
                    m_Logger.LogInformation("Time to execute UpdateServiceDepartmentAssociateRM() in EmployeeController:");
                    return NotFound(employees.Message);
                }
                else
                {
                    m_Logger.LogInformation($"returning employees records");
                    return Ok(employees);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error Occured in UpdateServiceDepartmentAssociateRM() method in EmployeeController:" + ex.StackTrace);
                return BadRequest("Error Occured in UpdateServiceDepartmentAssociateRM() method in EmployeeController:");
            }

        }
        #endregion
    }
}
