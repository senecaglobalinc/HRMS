using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure;
using HRMS.KRA.Infrastructure.Constants;
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
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace HRMS.KRA.Service.External
{
    /// <summary>
    /// Service class to get the details from Org Db
    /// </summary>
    public class OrganizationService : IOrganizationService
    {
        #region Global Variables
        private readonly ILogger<OrganizationService> m_Logger;
        private IHttpClientFactory m_ClientFactory;
        private APIEndPoints m_ApiEndPoints;
        #endregion

        #region Constructor
        /// <summary>
        /// OrganizationService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="clientFactory"></param>
        /// <param name="apiEndPoints"></param>
        public OrganizationService(ILogger<OrganizationService> logger,
        IHttpClientFactory clientFactory,
        IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ClientFactory = clientFactory;
            m_ApiEndPoints = apiEndPoints?.Value;
        }
        #endregion 

        /// <summary>
        /// GetAllDepartments
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Department>> GetAllDepartmentsAsync()
        {
            var response = new ServiceListResponse<Department>();

            try
            {
                //Get the Department information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETALLDEPARTMENTS}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var departments = JsonConvert.DeserializeObject<List<Department>>(json);
                    response.Items = departments;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the departments data";
                    m_Logger.LogError($"{nameof(GetAllDepartmentsAsync)} - Error occured while fetching the departments data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the departments data";
                m_Logger.LogError($"{nameof(GetAllDepartmentsAsync)} - {ex?.Message}");
            }
            return response;
        }

        #region GetUserDepartmentDetails
        public async Task<ServiceListResponse<Department>> GetUserDepartmentDetailsAsync()
        {
            var response = new ServiceListResponse<Department>();
            try
            {
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");

                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETUSERDEPARTMENT}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var departmentList = JsonConvert.DeserializeObject<List<Department>>(json);

                    if (departmentList.Count > 0)
                    {
                        response.Items = departmentList;
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "No department found";
                        m_Logger.LogError($"{nameof(GetUserDepartmentDetailsAsync)} - No department found");
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fething the department data";
                    m_Logger.LogError($"{nameof(GetUserDepartmentDetailsAsync)} - Error occured while fething the department data");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fething the department data";
                m_Logger.LogError($"{nameof(GetUserDepartmentDetailsAsync)} - {ex?.Message}");
            }

            return response;

        }
        #endregion

        /// <summary>
        /// GetAllGrades
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<Grade>> GetAllGradesAsync()
        {
            var response = new ServiceListResponse<Grade>();

            try
            {
                //Get the Department information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETALLGRADES}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var grades = JsonConvert.DeserializeObject<List<Grade>>(json);
                    response.Items = grades;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the grades data";
                    m_Logger.LogError($"{nameof(GetAllGradesAsync)} - Error occured while fetching the grades data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the grades data";
                m_Logger.LogError($"{nameof(GetAllGradesAsync)} - {ex?.Message}");
            }
            return response;
        }

        #region GetAllFinancialYears
        /// <summary>
        /// GetFinancialYears
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<FinancialYear>> GetAllFinancialYearsAsync()
        {
            var response = new ServiceListResponse<FinancialYear>();

            try
            {
                //Get the Financial year information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                 $"{ServicePaths.OrgEndPoint.GetAllFinancialYears}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var financialYears = JsonConvert.DeserializeObject<List<FinancialYear>>(json);
                    response.Items = financialYears;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the Financial Year data";
                    m_Logger.LogError($"{nameof(GetAllFinancialYearsAsync)} - Error occured while fetching the Financial Year data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Financial Year data";

                m_Logger.LogError($"{nameof(GetAllFinancialYearsAsync)} - {ex?.Message}");
            }
            return response;
        }
        #endregion

        #region GetFinancialYearById
        /// <summary>
        /// GetFinancialYearById
        /// </summary>
        /// <returns></returns>
        public async Task<FinancialYear> GetFinancialYearByIdAsync(int financialYearId)
        {
            var response = new FinancialYear();

            try
            {
                //Get the Financial year information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                 $"{ServicePaths.OrgEndPoint.GETFINANCIALYEARBYID}{financialYearId}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<FinancialYear>(json);                    
                }
                else
                {                   
                    m_Logger.LogError($"{nameof(GetFinancialYearByIdAsync)} - Error occured while fetching the Financial Year data.");
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"{nameof(GetByIdAsync)} - {ex?.Message}");
            }
            return response;
        }
        #endregion

        #region GetAllRoleTypes
        /// <summary>
        /// Gets All RoleTypes
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<RoleType>> GetAllRoleTypesAsync()
        {
            var response = new ServiceListResponse<RoleType>();

            try
            {
                //Get the RoleTypes information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                 $"{ServicePaths.OrgEndPoint.GetAllRoleTypes}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var roleTypes = JsonConvert.DeserializeObject<List<RoleType>>(json);
                    response.Items = roleTypes;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the Role Types data";
                    m_Logger.LogError($"{nameof(GetAllRoleTypesAsync)} Error occured while fetching the Role Types data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Role Types data";
                m_Logger.LogError($"{nameof(GetAllRoleTypesAsync)} - {ex?.Message}");
            }
            return response;
        }
        #endregion

        #region GetAllGradeRoleTypes
        /// <summary>
        /// Gets All GradeRoleTypes
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<GradeRoleType>> GetAllGradeRoleTypesAsync()
        {
            var response = new ServiceListResponse<GradeRoleType>();

            try
            {
                //Get the RoleTypes information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                 $"{ServicePaths.OrgEndPoint.GetAllRoleTypes}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var graderoleTypes = JsonConvert.DeserializeObject<List<GradeRoleType>>(json);
                    response.Items = graderoleTypes;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the Grade Role Types data";
                    m_Logger.LogError($"{nameof(GetAllGradeRoleTypesAsync)} - Error occured while fetching the Grade Role Types data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Grade Role Types data";

                m_Logger.LogError($"{nameof(GetAllGradeRoleTypesAsync)} - {ex?.Message}");
            }

            return response;
        }
        #endregion

        #region GetNotificationByNotificationTypeAndCategory
        public async Task<ServiceResponse<NotificationConfiguration>> GetByNotificationTypeAndCategoryAsync(int notificationTypeId, int categoryMasterId)
        {
            var response = new ServiceResponse<NotificationConfiguration>();
            try
            {
                //Get the NoificationConfiguration information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GETNOTIFICATIONBYNOTIFICATIONTYPEANDCATEGORY}{notificationTypeId}{"&categoryMasterId="}{categoryMasterId}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var notificationConfiguration = JsonConvert.DeserializeObject<NotificationConfiguration>(json);
                    response.Item = notificationConfiguration;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the NoificationConfiguration data";
                    m_Logger.LogError($"{nameof(GetByNotificationTypeAndCategoryAsync)} - Error occured while fetching the NoificationConfiguration data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Status data";

                m_Logger.LogError($"{nameof(GetByNotificationTypeAndCategoryAsync)} - {ex?.Message}");
            }
            return response;
        }
        #endregion

        #region GetNotificationConfiguration
        public async Task<ServiceResponse<NotificationConfiguration>> GetNotificationConfigurationAsync(string notificationCode, int categoryMasterId)
        {
            var response = new ServiceResponse<NotificationConfiguration>();
            try
            {
                //Get the NoificationConfiguration information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.GetNotificationConfiguration}{notificationCode}{"&categoryMasterId="}{categoryMasterId}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var notificationConfiguration = JsonConvert.DeserializeObject<NotificationConfiguration>(json);
                    response.Item = notificationConfiguration;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the NoificationConfiguration data";
                    m_Logger.LogError($"{nameof(GetNotificationConfigurationAsync)} - Error occured while fetching the NoificationConfiguration data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Status data";

                m_Logger.LogError($"{nameof(GetNotificationConfigurationAsync)} - {ex?.Message}");
            }
            return response;
        }
        #endregion

        #region SendEmail
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> SendEmailAsync(NotificationDetail notificationDetail)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                //send email from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");

                var uri = new Uri(m_ApiEndPoints.OrgEndPoint +
                    $"{ServicePaths.OrgEndPoint.SENDEMAIL}");
                var httpResponse = await httpClientFactory.PostAsync(uri, notificationDetail, new JsonMediaTypeFormatter());

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response.Item = true;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while sending email";
                    m_Logger.LogError($"{nameof(SendEmailAsync)} - Error occured while sending email");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while sending email";
                m_Logger.LogError($"{nameof(SendEmailAsync)} - {ex?.Message}");
            }
            return response;
        }
        #endregion

        /// <summary>
        /// GetUserRoles
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<RoleDetails>> GetUserRolesAsync()
        {
            var response = new ServiceListResponse<RoleDetails>();

            try
            {
                //Get the Department information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                $"{ServicePaths.OrgEndPoint.GetUserRoles}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var roles = JsonConvert.DeserializeObject<List<RoleDetails>>(json);
                    response.Items = roles;
                    response.IsSuccessful = true;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the user roles data";
                    m_Logger.LogError($"{nameof(GetUserRolesAsync)} - Error occured while fetching the user roles data.");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the user roles data";

                m_Logger.LogError($"{nameof(GetUserRolesAsync)} - {ex?.Message}");
            }
            return response;
        }

        /// <summary>
        /// GetById
        /// </summary>
        /// <returns></returns>
        public async Task<Department> GetByIdAsync(int departmentId)
        {
            Department department = null;

            try
            {
                //Get the Department information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");

                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                  $"{ServicePaths.OrgEndPoint.GetById}{departmentId}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    department = JsonConvert.DeserializeObject<Department>(json);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"{nameof(GetByIdAsync)} - {ex?.Message}");
            }

            return department;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<List<RoleTypeDepartment>> GetRoleTypesAndDepartmentsAsync(int departmentId = 0)
        {
            var roleTypesAndDepartments = new List<RoleTypeDepartment>();

            try
            {
                //Get the department and graderoletypeIds information from Org Microservice
                using var httpClientFactory = m_ClientFactory.CreateClient("OrgClient");

                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(m_ApiEndPoints.OrgEndPoint +
                  $"{ServicePaths.OrgEndPoint.GetRoleTypesAndDepartments}/{departmentId}")
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage httpResponseMessage = await httpClientFactory.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    roleTypesAndDepartments = JsonConvert.DeserializeObject<List<RoleTypeDepartment>>(json);
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"{nameof(GetRoleTypesAndDepartmentsAsync)} - {ex?.Message}");
            }

            return roleTypesAndDepartments;
        }
    }
}
