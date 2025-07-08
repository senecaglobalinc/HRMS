using AutoMapper;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class AssociateExitTypesService: IAssociateExitTypesService
    {
        #region Global Varibles

        private readonly ILogger<AssociateExitTypesService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;
        private readonly IAssociateExitService m_AssociateExitService;
        #endregion

        #region Constructor
        public AssociateExitTypesService(EmployeeDBContext employeeDBContext,
            ILogger<AssociateExitTypesService> logger,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IOrganizationService orgService,
            IProjectService projectService,
            IAssociateExitService associateExitService)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssociateExit, AssociateExit>();
                cfg.CreateMap<HRMS.Employee.Entities.Employee, HRMS.Employee.Entities.Employee>();
            });
            m_mapper = config.CreateMapper();
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
            m_OrgService = orgService;
            m_ProjectService = projectService;
            m_AssociateExitService = associateExitService;
        }

        #endregion

        #region GetDepartmentActivitiesForPM
        /// <summary>
        /// Gets Department Activities by employeeId
        /// </summary>
        /// <param name="employeeId">employeeId</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeDetails>> GetEmployeesByEmpIdAndRole(int employeeId, string roleName)
        {
            ServiceListResponse<EmployeeDetails> response;
            ServiceListResponse<EmployeeDetails> empDetails = null;

            if (employeeId == 0)
            {
                response = new ServiceListResponse<EmployeeDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            else
            {
                try
                {
                    empDetails = await m_ProjectService.GetEmployeesByEmployeeIdAndRole(employeeId, roleName);
                    
                    if (empDetails.IsSuccessful == false)
                    {
                        return response = new ServiceListResponse<EmployeeDetails>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "Error Occured while fetching Employee Details"
                        };
                    }
                   
                    response = new ServiceListResponse<EmployeeDetails>();
                    response.Items = empDetails.Items;
                    response.IsSuccessful = true;
                    response.Message = string.Empty;

                }
                catch (Exception ex)
                {
                    response = new ServiceListResponse<EmployeeDetails>();
                    response.Items = null;
                    response.IsSuccessful = false;
                    response.Message = "Error occured while fetching the data";
                    m_Logger.LogError($"Error occured in GetEmployeesByEmployeeIdAndRole() method {ex.StackTrace}");
                }

            }

            return response;
        }

        #endregion

        #region GetByExitType
        /// <summary>
        /// Get AssociateExit by exit type
        /// </summary>
        /// <param name="exitTypeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<AssociateExit>> GetByExitType(int exitTypeId)
        {
            var response = new ServiceListResponse<AssociateExit>();
            try
            {
                m_Logger.LogInformation("Calling GetAll method in AssociateExitService");
                var associateExits = m_EmployeeContext.AssociateExit.Where(pa => (pa.IsActive == true) && (pa.ExitTypeId == exitTypeId)).ToList();
                response.Items = associateExits;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching AssociateExits";

                m_Logger.LogError("Error occured in GetAssociateExits() method." + ex.StackTrace);
            }
            return response;
        }
        #endregion
    }
}
