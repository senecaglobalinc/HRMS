using HRMS.KRA.Infrastructure;
using HRMS.KRA.Infrastructure.Constants;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Models.Response;
using HRMS.KRA.Infrastructure.Response;
using HRMS.KRA.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.KRA.Service.External
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

        #region GetEmployeeNames
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeeNames()
        {
            var response = new ServiceListResponse<EmployeeDetails>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEENAMES}");
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
                            response.Message = "No employee names found";

                            m_Logger.LogError("No employees found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the employee names data";

                        m_Logger.LogError("Error occured while fething the employee names data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the employee names data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion

        #region GetEmployeeWorkEmailAddress
        public async Task<ServiceResponse<string>> GetEmployeeWorkEmailAddress(int employeeId)
        {
            //Employee employee = null;
            var response = new ServiceResponse<string>();
            try
            {
                //Get the Employee information from Employee Microservice
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.RequestUri = new Uri(m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEEWORKEMAILADDRESS}" + employeeId);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        //string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        //employee = JsonConvert.DeserializeObject<Employee>(json);
                        string workEmailAddress = await httpResponseMessage.Content.ReadAsStringAsync();
                        //response.Item = employee;
                        response.Item = workEmailAddress;
                        if (workEmailAddress != null)
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

        #region GetEmployeesByRole
        public async Task<ServiceListResponse<AssociateRoleType>> GetEmployeesByRole(string employeeCode, int? departmentId, int? roleId)
        {
            var response = new ServiceListResponse<AssociateRoleType>();
            try
            {
                using (var httpClientFactory = m_ClientFactory.CreateClient("EmployeeClient"))
                {
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Method = HttpMethod.Get;
                    string uri = m_ApiEndPoints.EmployeeEndPoint +
                        $"{ServicePaths.EmployeeEndPoint.GETEMPLOYEESBYROLE}?employeeCode=";
                    if (!string.IsNullOrWhiteSpace(employeeCode))
                        uri = uri + employeeCode;
                    uri = uri + "&departmentId=";
                    if (departmentId.HasValue)
                        uri = uri + departmentId.Value.ToString();
                    uri = uri + "&roleId=";
                    if (roleId.HasValue)
                        uri = uri + roleId.Value.ToString();

                    httpRequestMessage.RequestUri = new Uri(uri);
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var employeeList = JsonConvert.DeserializeObject<List<AssociateRoleType>>(json);
                        response.Items = employeeList;
                        if (employeeList.Count > 0)
                        {
                            response.IsSuccessful = true;
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No Associates found";

                            m_Logger.LogError("No Associates found");
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fething the Associates data";

                        m_Logger.LogError("Error occured while fething the Associates data");
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the Associates data";

                m_Logger.LogError(ex.Message);
            }

            return response;

        }
        #endregion
    }
}
