using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class AddendumService : IAddendumService
    {
        #region Global Varibles

        private readonly ILogger<AddendumService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_Mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        IMapper mapper;
        #endregion

        #region Constructor
        public AddendumService(ILogger<AddendumService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Addendum, Addendum>();
            });
            m_Mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints != null ? apiEndPoints.Value : null;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method create addendum. 
        /// </summary>
        /// <param name="addendumRequest"></param>
        /// <returns>AddendumResponse</returns>
        public async Task<ServiceResponse<bool>> Create(AddendumRequest addendumRequest)
        {
            int isCreated;
            ServiceResponse<bool> response;
            m_Logger.LogInformation("Calling \"Create\" method in AddendumService");

            //Checking if Department Code already exists
            var isExists = m_ProjectContext.Addendum.Where(d => d.AddendumNo.ToLower().Trim() == addendumRequest.AddendumNo.ToLower().Trim()
            && d.ProjectId== addendumRequest.ProjectId && d.SOWId== addendumRequest.SOWId
            ).Count();

            if (isExists > 0)
            {

                m_Logger.LogInformation("Addendum No. Already Exists.");

                response = new ServiceResponse<bool>()
                {
                    Item = false,
                    IsSuccessful = false,
                    Message = "Addendum No. Already Exists"
                };
                return response;
            }
            if (Roles.ProgramManager.GetEnumDescription().TrimLowerCase().Equals(addendumRequest.RoleName.TrimLowerCase()) ||
                Roles.DepartmentHead.GetEnumDescription().TrimLowerCase().Equals(addendumRequest.RoleName.TrimLowerCase()))
            {
                Addendum addendum = new Addendum();

                if (!addendumRequest.IsActive.HasValue)
                    addendumRequest.IsActive = true;

                m_Logger.LogInformation("Assigning to automapper.");

                addendum = m_Mapper.Map<Addendum, Addendum>(addendumRequest);
                m_ProjectContext.Addendum.Add(addendum);
                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in AddendumService");
                isCreated = await m_ProjectContext.SaveChangesAsync();

                if (isCreated > 0)
                {
                    m_Logger.LogInformation("Assigning Addendum object to response in Create Addendum service.");

                    response = new ServiceResponse<bool>()
                    {
                        Item = true,
                        IsSuccessful = true,
                        Message = ""
                    };

                    m_Logger.LogInformation("Assigned Addendum object to response in Create Addendum service.");

                    return response;
                }
                else
                {
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response = new ServiceResponse<bool>()
                    {
                        Item = false,
                        IsSuccessful = false,
                        Message = "No Addendum created."
                    };

                    m_Logger.LogInformation("Response object created in SOWService");

                    return response;
                }
            }
            else
            {
                m_Logger.LogInformation("Creating Response object in SOWService");

                response = new ServiceResponse<bool>()
                {
                    Item = false,
                    IsSuccessful = false,
                    Message = "Your current role does not have permission to create Addendum."
                };

                m_Logger.LogInformation("Response object created in SOWService");

                return response;
            }
        }

        #endregion

        #region GetAllBySOWIdAndProjectId
        /// <summary>
        /// Get the all the addendum by sowId and projectId
        /// </summary>
        /// <param name="sowId"></param>
        /// <param name="projectId"></param>
        /// <returns>List<Entities.SOW></returns>
        public async Task<ServiceListResponse<Addendum>> GetAllBySOWIdAndProjectId(int sowId, int projectId)
        {
            ServiceListResponse<Addendum> response;
            //check for invalid request sowid ==0 or projectId==0
            if (sowId == 0 || projectId == 0)
            {
                response = new ServiceListResponse<Addendum>();
                response.IsSuccessful = false;
                response.Items = null;
                response.Message = "Invalid Request.. please verify.";
            }
            else
            {
                response = new ServiceListResponse<Addendum>();
                response.Items = await m_ProjectContext.Addendum
                                                       .Where(ad => ad.Id == sowId && ad.ProjectId == projectId)
                                                       .ToListAsync();
                response.Message = "";
                response.IsSuccessful = true;
            }
            return response;
        }

        #endregion

        #region GetByIdAndProjectId
        /// <summary>
        /// Get the all the addendum by Id and projectId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectId"></param>
        /// <param name="roleName"></param>
        /// <returns>List<Addendum></returns>
        public async Task<ServiceResponse<Addendum>> GetByIdAndProjectId(int id, int projectId, string roleName)
        {
            ServiceResponse<Addendum> response;
            if (id == 0 || projectId == 0 || string.IsNullOrEmpty(roleName))
            {
                response = new ServiceResponse<Addendum>();
                response.IsSuccessful = false;
                response.Message = "Invalid Request.";
                return response;
            }
            else
            {
                if (Roles.DepartmentHead.GetEnumDescription().TrimLowerCase().Equals(roleName.TrimLowerCase()) ||
                            Roles.ProgramManager.GetEnumDescription().TrimLowerCase().Equals(roleName.TrimLowerCase()))
                {
                    response = new ServiceResponse<Addendum>();
                    response.Item = await m_ProjectContext.Addendum.Where(ad => ad.AddendumId == id && ad.ProjectId == projectId).FirstOrDefaultAsync();
                    if (response.Item == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "No Addendum found for this request";
                    }
                    else
                    {
                        response.IsSuccessful = true;
                        response.Message = "";
                    }
                    return response;
                }
                else
                    return null;
            }
        }

        #endregion

        #region Update
        /// <summary>
        /// This method updates addendum.
        /// </summary>
        /// <param name="addendumRequest"></param>
        /// <returns>AddendumResponse</returns>
        public async Task<ServiceResponse<bool>> Update(AddendumRequest addendumRequest)
        {
            int isUpdated;
            ServiceResponse<bool> response;
            m_Logger.LogInformation("AddendumService: Calling \"Update\" method.");

            if (Roles.ProgramManager.GetEnumDescription().TrimLowerCase().Equals(addendumRequest.RoleName.TrimLowerCase()) ||
                Roles.DepartmentHead.GetEnumDescription().TrimLowerCase().Equals(addendumRequest.RoleName.TrimLowerCase()))
            {
                m_Logger.LogInformation("Fetching addendum for update.");

                var addendum = await m_ProjectContext.Addendum
                    .Where(ad => ad.AddendumId == addendumRequest.AddendumId).FirstOrDefaultAsync();

                m_Logger.LogInformation("AddendumService: Addendum exists?");

                if (addendum == null)
                {
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response = new ServiceResponse<bool>()
                    {
                        Item = false,
                        IsSuccessful = false,
                        Message = "Addendum not found for update."
                    };
                    m_Logger.LogInformation("Response object created in SOWService");
                    return response;
                }
                else
                    m_Logger.LogInformation("AddendumService: Addendum found.");

                m_Logger.LogInformation("AddendumService: Update feilds.");

                if (!addendumRequest.IsActive.HasValue)
                    addendumRequest.IsActive = addendum.IsActive;

                m_Logger.LogInformation("AddendumService: assigning to automapper.");

                addendum.AddendumNo = addendumRequest.AddendumNo;
                addendum.AddendumDate = addendumRequest.AddendumDate;
                addendum.RecipientName = addendumRequest.RecipientName;
                addendum.Note = addendumRequest.Note;
                addendum.SOWId = addendumRequest.SOWId;
                m_Logger.LogInformation("AddendumService: Calling SaveChanges method on DB Context.");

                isUpdated = await m_ProjectContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    m_Logger.LogInformation("AddendumService: Updating addendum record.");

                    response = new ServiceResponse<bool>()
                    {
                        Item = true,
                        IsSuccessful = true,
                        Message = ""
                    };
                    m_Logger.LogInformation("Response object created in SOWService");
                    return response;
                }
                else
                {
                    m_Logger.LogInformation("AddendumService: Updating addendum record.");

                    response = new ServiceResponse<bool>()
                    {
                        Item = false,
                        IsSuccessful = false,
                        Message = "No record updated."
                    };
                    m_Logger.LogInformation("Response object created in SOWService");
                    return response;
                }
            }
            else
            {
                m_Logger.LogInformation("AddendumService: Updating addendum record.");

                response = new ServiceResponse<bool>()
                {
                    Item = false,
                    IsSuccessful = false,
                    Message = "Your current role does not have permission to update Addendum."
                };
                m_Logger.LogInformation("Response object created in SOWService");
                return response;
            }
        }

        #endregion


    }
}
