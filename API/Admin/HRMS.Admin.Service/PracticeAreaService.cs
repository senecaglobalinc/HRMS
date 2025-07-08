using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Infrastructure.Models.Domain;
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
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    /// <summary>
    /// Service class to get the Client details
    /// </summary>
    public class PracticeAreaService : IPracticeAreaService
    {
        #region Global Varible

        private readonly ILogger<PracticeAreaService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IClientService m_ClientService;
        private readonly IDepartmentService m_DepartmentService;
        private readonly IProjectTypeService m_ProjectTypeService;
        private IHttpClientFactory m_ClientFactory;
        private readonly IMapper m_Mapper;
        private APIEndPoints m_apiEndPoints;
        private IEmployeeService m_employeeService;


        #endregion

        #region Constructor
        public PracticeAreaService(ILogger<PracticeAreaService> logger,
            AdminContext adminContext,
            IHttpClientFactory clientFactory,
            IClientService clientService,
            IDepartmentService departmentService,
            IProjectTypeService projectTypeService,
            IOptions<APIEndPoints> apiEndPoints, IEmployeeService employeeService)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
            m_ClientFactory = clientFactory;
            m_ClientService = clientService;
            m_DepartmentService = departmentService;
            m_ProjectTypeService = projectTypeService;
            m_employeeService = employeeService;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PracticeArea, PracticeArea>();
            });

            m_Mapper = config.CreateMapper();
            m_apiEndPoints = apiEndPoints?.Value;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method create practive area. 
        /// This method also one entry for talent project and project manager for perticular practive area.
        /// </summary>
        /// <param name="practiceAreaIn">Practice area detail information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Create(PracticeArea practiceAreaIn)
        {
            try
            {
                int isCreated;
                m_Logger.LogInformation("Calling \"Create\" method in PracticeAreaService");

                PracticeArea practiceAreaAlreadyExits =
                    await GetByPracticeAreaCode(practiceAreaIn.PracticeAreaCode);

                if (practiceAreaAlreadyExits != null)
                    return CreateResponse(null, false, "Practice area code already exists.");

                var client = await m_ClientService.GetByName("SenecaGlobal");

                if (client != null)
                    m_Logger.LogInformation("Client with code \"SenecaGlobal\" found.");
                else
                {
                    m_Logger.LogInformation("Client with code \"SenecaGlobal\" not found.");
                    return CreateResponse(null, false, "Client with code \"SenecaGlobal\" not found.");
                }

                var department = await m_DepartmentService.GetByDepartmentCode("Delivery");

                if (department != null)
                    m_Logger.LogInformation("Department with code \"Delivery\" found.");
                else
                {
                    m_Logger.LogInformation("Department with code \"Delivery\" not found.");
                    return CreateResponse(null, false, "Department with code \"Delivery\" not found.");
                }

                var projectType = await m_ProjectTypeService.GetByProjectTypeCode("Talent Pool");

                if (projectType != null)
                    m_Logger.LogInformation("Project type with code \"Talent Pool\" found.");
                else
                {
                    m_Logger.LogInformation("Project type with code \"Talent Pool\" not found.");
                    return CreateResponse(null, false, "Project type with code \"Talent Pool\" not found.");
                }

               

                //var httpClientFactory = m_ClientFactory.CreateClient();
                //var apiResponse = httpClientFactory.GetStringAsync(m_apiEndPoints.ProjectEndPoint + "Project/GetAll").Result;
                //if (apiResponse == null)
                //    return CreateResponse(null, false, "Error occured while fetching existing projects.");

                //List<Project> existingProjects = JsonConvert.DeserializeObject<List<Project>>(apiResponse.ToString());

                PracticeArea practiceArea = new PracticeArea();

                if (!practiceAreaIn.IsActive.HasValue)
                    practiceAreaIn.IsActive = true;

                m_Logger.LogInformation("Assigning to automapper.");

                m_Mapper.Map<PracticeArea, PracticeArea>(practiceAreaIn, practiceArea);

                m_AdminContext.PracticeAreas.Add(practiceArea);
                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in PracticeAreaService");
                isCreated = await m_AdminContext.SaveChangesAsync();

                if (isCreated > 0)
                {
                    if (!"talent pool".Equals(practiceArea.PracticeAreaCode.ToLower().Trim()))
                    {
                        m_Logger.LogInformation("Practice area created successfully.");

                        ProjectDetails project = new ProjectDetails()
                        {
                            ProjectName = "Talent Pool - " + practiceAreaIn.PracticeAreaDescription,
                            ProjectTypeId = projectType.ProjectTypeId,
                            ClientId = client.ClientId,
                            DepartmentId = department.DepartmentId,
                            PracticeAreaId = practiceArea.PracticeAreaId,
                            DepartmentHeadId = practiceAreaIn.PracticeAreaHeadId ?? 0,
                            ProjectStateId = -1, /* Currently value is hardcoded, Please fetch it from status table 
                                        with below condition.
                                        StatusCode=“Drafted” and Category=“PPC”
                                     */
                        };

                        //Create Talent pool project
                        dynamic response = await CreateProjectAndProjectManager(project, practiceArea.PracticeAreaCode, practiceArea.PracticeAreaId, practiceArea.PracticeAreaHeadId ?? 0);

                        if (!response.IsSuccessful)
                        {
                            m_Logger.LogInformation("Error occurred while creating practice area: " + (string)response.Message);

                            return CreateResponse(null, false, response.Message);
                        }



                    }

                    return CreateResponse(practiceArea, true, string.Empty);
                }
                else
                    return CreateResponse(null, false, "No practice area created.");
            }
            catch (Exception ex)
            {
                return CreateResponse(null, false, "Error Occured in Practice Area Create Service");
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// Updates practice area
        /// </summary>
        /// <param name="practiceAreaIn">Practice area detail information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Delete(int practiceAreaID)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling \"Delete\" method in PracticeAreaService");

            var practiceArea = m_AdminContext.PracticeAreas.Find(practiceAreaID);
            if (practiceArea == null)
                return CreateResponse(null, false, "Practice area not found for delete.");

            practiceArea.IsActive = false;

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in PracticeAreaService.");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Deleting practice area record in PracticeAreaService.");
                return CreateResponse(null, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record deleted.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the practice area
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<PracticeAreaDetails>>GetAll(bool? isActive)
        {
            var response = new List<PracticeAreaDetails>();
            try
            {
                var getPracticeAreas =  (from ed in m_AdminContext.PracticeAreas
                                              where ed.IsActive == isActive
                                              select new PracticeAreaDetails
                                              {
                                                  PracticeAreaId = ed.PracticeAreaId,
                                                  PracticeAreaCode = ed.PracticeAreaCode,
                                                  PracticeAreaDescription = ed.PracticeAreaDescription,
                                                  PracticeAreaHeadId = ed.PracticeAreaHeadId ?? 0,
                                              }).OrderBy(x => x.PracticeAreaDescription).ToList();

                var practiceAreaIds = getPracticeAreas.Select(x => x.PracticeAreaHeadId).ToList();
                var employees = m_employeeService.GetEmployeesByIds(practiceAreaIds).Result;

                var practiceAreaList = (from pract in getPracticeAreas
                                        join emp in employees.Items on pract.PracticeAreaHeadId equals emp.EmpId into g1
                                        from preEmp in g1.DefaultIfEmpty()
                                        select new PracticeAreaDetails
                                        {
                                            PracticeAreaId = pract.PracticeAreaId,
                                            PracticeAreaCode = pract.PracticeAreaCode,
                                            PracticeAreaDescription = pract.PracticeAreaDescription,
                                            PracticeAreaHeadId = pract.PracticeAreaHeadId ?? 0,
                                            PracticeAreaHeadName = preEmp?.EmpName ?? "NA"
                                        }).OrderBy(x => x.PracticeAreaDescription).ToList();

                response = practiceAreaList;
            }
            catch(Exception ex)
            {
                m_Logger.LogError("Error occured in GetAllPracticeDetails() method" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetTechnologyForDropdown
        /// <summary>
        /// GetTechnologyForDropdown
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetTechnologyForDropdown() =>
                        await m_AdminContext.PracticeAreas.Where(cl => cl.IsActive == true).Select(ci => new GenericType { Id = ci.PracticeAreaId, Name = ci.PracticeAreaCode }).OrderBy(x => x.Name).ToListAsync();

        #endregion

        #region GetByPracticeAreaCode
        /// <summary>
        /// Get practice area code
        /// </summary>
        /// <param name="practiceAreaCode">practice area code</param>
        /// <returns>PracticeAreaDetails</returns>
        public async Task<PracticeArea> GetByPracticeAreaCode(string practiceAreaCode) =>
                        await m_AdminContext.PracticeAreas.Where(pa => pa.PracticeAreaCode.ToLower().Trim() == practiceAreaCode.ToLower().Trim() && pa.IsActive == true)
                        .FirstOrDefaultAsync();

        #endregion

        #region GetPracticeAreaByIds
        /// <summary>
        /// Gets list of practice areas by ids
        /// </summary>
        /// <param name="practiceAreaCode">practice area code</param>
        /// <returns>PracticeAreaDetails</returns>
        public async Task<List<PracticeArea>> GetPracticeAreaByIds(int[] practiceAreaIds) =>
                        await m_AdminContext.PracticeAreas.Where(pa => practiceAreaIds.Contains(pa.PracticeAreaId))
                        .ToListAsync();

        #endregion

        #region GetByPracticeAreaId
        /// <summary>
        /// Get practice area id
        /// </summary>
        /// <param name="practiceAreaCode">practice area code</param>
        /// <returns>PracticeAreaDetails</returns>
        public async Task<PracticeArea> GetPracticeAreaById(int practiceAreaId) =>
                        await m_AdminContext.PracticeAreas.Where(pa => pa.PracticeAreaId == practiceAreaId)
                        .FirstOrDefaultAsync();

        #endregion

        #region Update
        /// <summary>
        /// Updates practice area
        /// </summary>
        /// <param name="practiceAreaIn">Practice area detail information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Update(PracticeArea practiceAreaIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling \"Update\" method in PracticeAreaService");

            var practiceArea = m_AdminContext.PracticeAreas.Find(practiceAreaIn.PracticeAreaId);
            if (practiceArea == null)
                return CreateResponse(null, false, "Practice area not found for update.");

            PracticeArea practiceAreaAlreadyExits =
               await GetByPracticeAreaCode(practiceAreaIn.PracticeAreaCode);

            if (practiceAreaAlreadyExits != null &&
                practiceAreaAlreadyExits.PracticeAreaId != practiceArea.PracticeAreaId)
                return CreateResponse(null, false, "Practice area code already exists.");

            if (!practiceAreaIn.IsActive.HasValue)
                practiceAreaIn.IsActive = practiceArea.IsActive;

            practiceAreaIn.CreatedBy = practiceArea.CreatedBy;
            practiceAreaIn.CreatedDate = practiceArea.CreatedDate;
            m_Logger.LogInformation("Assigning to automapper.");

            m_Mapper.Map<PracticeArea, PracticeArea>(practiceAreaIn, practiceArea);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in PracticeAreaService.");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating practice area record in PracticeAreaService.");
                return CreateResponse(practiceArea, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated.");
        }
        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="practiceArea"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(PracticeArea practiceArea, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling \"CreateResponse\" method in PracticeAreaService");

            dynamic response = new ExpandoObject();
            response.PracticeArea = practiceArea;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in PracticeAreaService");

            return response;
        }

        #endregion

        #region CreateProjectAndProjectManager
        /// <summary>
        /// This method create entry for Talent project.
        /// This method also add entry for Project manager table
        /// Calls the project API for creating Talent project.
        /// Calls the Project Manager API for creating Project Manager.
        /// Project Code is determined by using below formula
        /// "TP" + {{ First two character of practice area description/First character of practice area description if length is less than 2}}
        /// + {{ ss = seconds }} + {{ dd = date }} + {{ MM = month }} + {{ yyyy = year }}
        /// </summary>
        /// <param name="practiceAreaIn">practice area code</param>
        /// <returns>PracticeAreaDetails</returns>
        private async Task<dynamic> CreateProjectAndProjectManager(ProjectDetails project, string practiceAreaCode, int practiceAreaId,int PracticeAreaHeadId)
        {
            string projectCode = string.Empty;

            m_Logger.LogInformation("Calling \"CreateProjectAndProjectManager\" method in PracticeAreaService");

            //Create project code
            projectCode = "TP" + (practiceAreaCode.Length > 1 ? practiceAreaCode.Substring(0, 2).ToUpper() :
                                  practiceAreaCode.Substring(0, 1).ToUpper()) +
                           DateTime.Now.ToString("ssMMddyyyy");

            m_Logger.LogInformation("Project Code:" + projectCode);

            project.ProjectCode = projectCode;

            var httpClientFactory = m_ClientFactory.CreateClient("ProjectClient");

            m_Logger.LogInformation("Calling project api in project microservice.");

            var apiResponse = await httpClientFactory.PostAsJsonAsync(m_apiEndPoints.ProjectEndPoint + "Project/Create", project);

            if (!apiResponse.IsSuccessStatusCode)
                return CreateResponse(null, false, "Error occured while creating talent project.");

            m_Logger.LogInformation("Request Message Information:-" + apiResponse.RequestMessage);
            m_Logger.LogInformation("Response Message Header" + apiResponse.Content.Headers);

            // Get the response
            var customerJsonString = await apiResponse.Content.ReadAsStringAsync();
            m_Logger.LogInformation("Your response data is: " + customerJsonString);

            // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
            project.ProjectId = JsonConvert.DeserializeObject<int>(custome‌​rJsonString);

            m_Logger.LogInformation("After calling project api in project microservice.");

            if (project.ProjectId != 0 )
            {
                m_Logger.LogInformation("Talent Pool Project created.");
                //m_Logger.LogInformation("Creating project manager in PracticeAreaService.");

                //ProjectManager projectManager = new ProjectManager()
                //{
                //    ProjectId = project.ProjectId,
                //    ProgramManagerId = PracticeAreaHeadId,
                //    ReportingManagerId = PracticeAreaHeadId,
                //    LeadId = PracticeAreaHeadId
                //};

                //m_Logger.LogInformation("Calling project manager api in project microservice.");

                //apiResponse = await httpClientFactory.PostAsJsonAsync(m_apiEndPoints.ProjectEndPoint +
                //                    "ProjectManager/Create", projectManager);

                //if (!apiResponse.IsSuccessStatusCode)
                //    return CreateResponse(null, false, "Error occured while creating talent project manager.");
                //m_Logger.LogInformation("Request Message Information:-" + apiResponse.RequestMessage);
                //m_Logger.LogInformation("Response Message Header" + apiResponse.Content.Headers);

                //// Get the response
                //customerJsonString = await apiResponse.Content.ReadAsStringAsync();
                //m_Logger.LogInformation("Your response data is: " + customerJsonString);

                //// Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)

                //projectManager = JsonConvert.DeserializeObject<ProjectManager>(customerJsonString);

                //m_Logger.LogInformation("after project manager api in project microservice.");

                //if (projectManager != null)
                //    m_Logger.LogInformation("Project manager created for Talent Pool Project.");
                //else
                //    return CreateResponse(null, false, "Project manager created  not for Talent Pool Project.");
                TalentPoolRequest talentPoolRequest = new TalentPoolRequest()
                {
                    ProjectId = project.ProjectId,
                    PracticeAreaId = practiceAreaId
                };
                var httpClient = m_ClientFactory.CreateClient("ProjectClient");

                m_Logger.LogInformation("Calling talentpool api in project microservice.");

                var serviceResponse = await httpClient.PostAsJsonAsync(m_apiEndPoints.ProjectEndPoint + "TalentPool/Create", talentPoolRequest);

                if (!serviceResponse.IsSuccessStatusCode)
                    return CreateResponse(null, false, "Error occured while creating talent pool.");

                m_Logger.LogInformation("Request Message Information:-" + apiResponse.RequestMessage);
                m_Logger.LogInformation("Response Message Header" + apiResponse.Content.Headers);

                // Get the response
                var responseJsonString = await apiResponse.Content.ReadAsStringAsync();
                m_Logger.LogInformation("Your response data is: " + responseJsonString);
            }
            else
                return CreateResponse(null, false, "Talent Pool Project not created.");

            return CreateResponse(null, true, string.Empty);
        }
        #endregion
    }
}
