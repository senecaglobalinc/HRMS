using AutoMapper;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class SerciveTypeToEmployeeService : IServiceTypeToEmployeeService
    {
        #region Global Variables
        private readonly ILogger<SerciveTypeToEmployeeService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        #endregion

        #region EmployeeProjectService
        public SerciveTypeToEmployeeService(ILogger<SerciveTypeToEmployeeService> logger,
                    EmployeeDBContext employeeDBContext)
        {
            m_Logger = logger;
            m_EmployeeContext = employeeDBContext;
            //Create mapper for certification
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SerciveTypeToEmployeeService, SerciveTypeToEmployeeService>();
            });
            m_mapper = config.CreateMapper();

        }
        #endregion

        #region Create
        /// <summary>
        /// Removes existing projects and inserts new projects based on employeeId
        /// </summary>
        /// <param name="employeeDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> Create(ServiceType employeeDetails)
        {
            int isCreated;
            var response = new ServiceResponse<int>();

            try
            {
                m_Logger.LogInformation("ServiceTypeToEmployeeService: Calling \"Create\" method.");
                var employee = m_EmployeeContext.Employees.Where(s => s.EmployeeId == employeeDetails.EmployeeId & s.IsActive == true).FirstOrDefault();
                if (employee == null)
                {
                    response.Message = "Employee not found";
                    response.IsSuccessful = false;
                    response.Item = 0;
                }
                var employeeService = m_EmployeeContext.ServiceTypeToEmployee.Where(s => s.EmployeeId == employeeDetails.EmployeeId).FirstOrDefault();
                if (employeeService != null)
                {
                    response.Message = "Employee already exsists";
                    response.IsSuccessful = false;
                    response.Item = 0;
                }
                ServiceTypeToEmployee empService = new ServiceTypeToEmployee();
                empService.EmployeeId = employeeDetails.EmployeeId;
                empService.ServiceTypeId = employeeDetails.ServiceTypeId;
                empService.IsActive = true;
                m_EmployeeContext.ServiceTypeToEmployee.Add(empService);
                isCreated = await m_EmployeeContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = 1;
                    m_Logger.LogInformation("EmployeeService created successfully");
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No record created";
                    m_Logger.LogInformation("No record created");
                }

            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while inserting Employee service details";
                m_Logger.LogError("Error occurred in Create() method of ServiceTypeToEmployeeService " + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region Update
        /// <summary>
        /// Removes existing projects and inserts new projects based on employeeId
        /// </summary>
        /// <param name="employeeDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> Update(ServiceType employeeDetails)
        {
            int isCreated;
            var response = new ServiceResponse<int>();

            try
            {
                m_Logger.LogInformation("ServiceTypeToEmployeeService: Calling \"Create\" method.");
                var employee = m_EmployeeContext.Employees.Where(s => s.EmployeeId == employeeDetails.EmployeeId & s.IsActive == true).FirstOrDefault();
                if (employee == null)
                {
                    response.Message = "Employee not found";
                    response.IsSuccessful = false;
                    response.Item = 0;
                }
                var employeeService = m_EmployeeContext.ServiceTypeToEmployee.Where(s => s.EmployeeId == employeeDetails.EmployeeId).FirstOrDefault();
                if (employeeService != null)
                {
                    employeeService.ServiceTypeId = employeeDetails.ServiceTypeId;
                    isCreated = await m_EmployeeContext.SaveChangesAsync();

                    if (isCreated > 0)
                    {
                        response.IsSuccessful = true;
                        response.Item = 1;
                        m_Logger.LogInformation("EmployeeService Updated successfully");
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "No record Updated";
                        m_Logger.LogInformation("No record Updated");
                    }
                }
                else
                {
                    response.Message = "Employee details not found in ServiceTypeToEmployee table";
                    response.IsSuccessful = false;
                    response.Item = 0;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while updating Employee service details";
                m_Logger.LogError("Error occurred in Update() method of ServiceTypeToEmployeeService " + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Fetches project details of an employee in previous organization based on employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<ServiceType>> GetServiceTypeByEmployeeId(int employeeId)
        {
            var response = new ServiceResponse<ServiceType>();
            try
            {
                m_Logger.LogInformation("ServiceTypeToEmployeeService: Calling \"GetServiceTypeByEmployeeId\" method.");

                var employee = await m_EmployeeContext.Employees.Where(s => s.EmployeeId == employeeId & s.IsActive == true).FirstOrDefaultAsync();
                if (employee == null)
                {
                    response.Message = "Employee not found";
                    response.IsSuccessful = false;
                    response.Item = null;
                }
                var employeeService = await m_EmployeeContext.ServiceTypeToEmployee.Where(s => s.EmployeeId == employeeId).FirstOrDefaultAsync();
                if (employeeService != null)
                {
                    ServiceType empService = new ServiceType();
                    empService.EmployeeId = employeeService.EmployeeId;
                    empService.ServiceTypeId = employeeService.ServiceTypeId;

                    response.IsSuccessful = true;
                    response.Item = empService;
                }
                else
                {
                    response.Message = "Employee details not found in ServiceTypeToEmployee table";
                    response.IsSuccessful = false;
                    response.Item = null;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while fetching ServiceType";
                m_Logger.LogError("Error occurred while fetching ServiceTypes  in GetServiceTypeByEmployeeId() method of ServiceTypeToEmployee" + ex.StackTrace);
            }
            return response;
        }
        #endregion 
    }
}
