using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Infrastructure.Constants;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Admin.Service.External
{
    public class EmployeeService : IEmployeeService
    {
        #region Global Varaibles
        private readonly ILogger<EmployeeService> m_Logger;
        private IHttpClientFactory m_ClientFactory;
        private APIEndPoints m_ApiEndPoints;

        #endregion

        #region Constructor

        public EmployeeService(ILogger<EmployeeService> logger,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ClientFactory = clientFactory;
            m_ApiEndPoints = apiEndPoints?.Value;
        }

        #endregion

        #region GetAllEmployees
        public async Task<ServiceListResponse<EmployeeDetails>> GetAll(bool? isActive)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.AssociateEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETALL}" + isActive);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var employeeList = JsonConvert.DeserializeObject<List<EmployeeDetails>>(json);
                        response.Items = employeeList;
                        if (employeeList.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employees found";

                            m_Logger.LogError("No employees found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employees data";

                        m_Logger.LogError("Error occured while fething the employees data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetEmployeeBySearchString
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeeBySearchString(string text)
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.AssociateEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEESBYSEARCHSTRING}" + text);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var employeeList = JsonConvert.DeserializeObject<List<EmployeeDetails>>(json);
                        response.Items = employeeList;
                        if (employeeList.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employees found";

                            m_Logger.LogError("No employees found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employees data";

                        m_Logger.LogError("Error occured while fething the employees data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetEmployeeByUserId
        public async Task<ServiceResponse<Employee>> GetEmployeeByUserId(int userId)
        {
            var response = new ServiceResponse<Employee>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.AssociateEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEEBYUSERID}" + userId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var employeeList = JsonConvert.DeserializeObject<Employee>(json);
                        response.Item = employeeList;
                        if (employeeList!=null )
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employee found";

                            m_Logger.LogError("No employee found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employee data";

                        m_Logger.LogError("Error occured while fething the employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetEmployeeByIds
        public async Task<ServiceListResponse<Employee>> GetEmployeesByIds(List<int?> employeeIds)
        {
            List<Employee> employeeList = null;
            var response = new ServiceListResponse<Employee>();
            try
            {
                //Get the Employee information from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.AssociateEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEEBYIDS}" + string.Join(",", employeeIds));
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        employeeList = JsonConvert.DeserializeObject<List<Employee>>(json);
                        response.Items = employeeList;
                        if (employeeList.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No employees found";

                            m_Logger.LogError("No employees found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employee data";

                        m_Logger.LogError("Error occured while fething the employee data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employees data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion
    }
}
