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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class SOWService : ISOWService
    {
        #region Global Varibles

        private readonly ILogger<SOWService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_Mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;

        #endregion

        #region Constructor
        public SOWService(ILogger<SOWService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SOW, SOW>();
            });
            m_Mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints != null ? apiEndPoints.Value : null;
        }
        #endregion

        #region Create
        /// <summary>
        /// This method create sow. 
        /// </summary>
        /// <param name="sowRequest"></param>
        /// <returns>dynamic</returns>
        public async Task<ServiceResponse<int>> Create(SOWRequest sowRequest)
        {
            int isCreated;
            m_Logger.LogInformation("Calling \"Create\" method in SOWService");
            ServiceResponse<int> response;
            var sowExists = await m_ProjectContext.SOW.Where(sw => sw.SOWId == sowRequest.SOWId && sw.ProjectId == sowRequest.ProjectId).FirstOrDefaultAsync();
            if (sowExists != null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = -1,
                    IsSuccessful = false,
                    Message = "SOW already exists."
                };
            }
            if (Roles.ProgramManager.GetEnumDescription().TrimLowerCase().Equals(sowRequest.RoleName.TrimLowerCase()) ||
                Roles.DepartmentHead.GetEnumDescription().TrimLowerCase().Equals(sowRequest.RoleName.TrimLowerCase()))
            {
                response = new ServiceResponse<int>();
                SOW sow = new SOW();

                if (!sowRequest.IsActive.HasValue)
                    sowRequest.IsActive = true;

                m_Logger.LogInformation("Assigning to automapper.");

                sow = m_Mapper.Map<SOW>(sowRequest);

                m_ProjectContext.SOW.Add(sow);
                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in SOWService");
                isCreated = await m_ProjectContext.SaveChangesAsync();

                if (isCreated > 0)
                {
                    m_Logger.LogInformation("Creating Response object in SOWService");
                    response.Item = 1;
                    response.IsSuccessful = true;
                    response.Message = string.Empty;

                    m_Logger.LogInformation("Response object created in SOWService");
                }
                else
                {
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "No SOW created.";

                    m_Logger.LogInformation("Response object created in SOWService");
                }
            }
            else
            {
                response = new ServiceResponse<int>();
                m_Logger.LogInformation("Creating Response object in SOWService");

                response.Item = 0;
                response.IsSuccessful = false;
                response.Message = "Your current role does not have permission to create SOW.";

                m_Logger.LogInformation("Response object created in SOWService");
            }
            return response;
        }

        #endregion

        #region Delete
        /// <summary>
        /// Deletes SOW by projectId
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <returns>dynamic</returns>
        public async Task<ServiceResponse<int>> Delete(int id)
        {
            try
            {
                int isUpdated;
                ServiceResponse<int> response;
                m_Logger.LogInformation("Calling \"Delete\" method in SOWService");
                m_Logger.LogInformation("Fetching SOW for delete in SOWService");

                //Fetch notification type for delete
                var sow = await m_ProjectContext.SOW.Where(sw => sw.Id == id).FirstOrDefaultAsync();

                //Notification type exists?
                if (sow == null)
                {
                    response = new ServiceResponse<int>();
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response.IsSuccessful = false;
                    response.Message = "SOW not found for delete.";

                    m_Logger.LogInformation("Response object created in SOWService");

                    return response;
                }

                //Fetch addendum details bind with SOWID
                var addendum = await m_ProjectContext.Addendum.Where(w => w.SOWId.Equals(sow.SOWId)).FirstOrDefaultAsync();

                if (addendum != null)
                {
                    response = new ServiceResponse<int>();
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response.IsSuccessful = false;
                    response.Message = "Addendum is exist for the SOW.";

                    m_Logger.LogInformation("Response object created in SOWService");

                    return response;
                }

                m_Logger.LogInformation("Deleting SOW in SOWService.");

                m_ProjectContext.SOW.Remove(sow);

                m_Logger.LogInformation("Calling SaveChanges method on DB Context in SOWService.");

                isUpdated = await m_ProjectContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    m_Logger.LogInformation("Updating SOW record in SOWService.");
                    response = new ServiceResponse<int>();
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response.IsSuccessful = true;
                    response.Message = "SOW deleted successfully.";

                    m_Logger.LogInformation("Response object created in SOWService");

                    return response;
                }
                else
                {
                    response = new ServiceResponse<int>();
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response.IsSuccessful = false;
                    response.Message = "No record updated.";

                    m_Logger.LogInformation("Response object created in SOWService");

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetAllByProjectId
        /// <summary>
        /// Get the all the sow by projectId
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <returns>List<Entities.SOW></returns>
        public async Task<ServiceListResponse<SOW>> GetAllByProjectId(int projectId)
        {
            ServiceListResponse<SOW> response;
            if (projectId == 0)
            {
                response = new ServiceListResponse<SOW>();
                m_Logger.LogInformation("Creating Response object in SOWService");

                response.Items = new List<SOW>();
                response.IsSuccessful = false;
                response.Message = "Invalid Request.";

                m_Logger.LogInformation("Response object created in SOWService");

                return response;
            }
            else
            {
                response = new ServiceListResponse<SOW>();
                response.Items = await m_ProjectContext.SOW.Where(sw => sw.ProjectId == projectId).ToListAsync();

                m_Logger.LogInformation("Creating Response object in SOWService");
                if (response.Items == null || response.Items.Count == 0)
                {
                    response.Items = new List<SOW>();
                    response.IsSuccessful = false;
                    response.Message = "No SOWs found with this project";
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Message = "";
                }
                m_Logger.LogInformation("Response object created in SOWService");

                return response;
            }
        }

        #endregion

        #region GetByIdAndProjectId
        /// <summary>
        /// Get the all the sow by Id and projectId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectId"></param>
        /// <param name="roleName"></param>
        /// <returns>List<Entities.SOW></returns>
        public async Task<ServiceResponse<SOW>> GetByIdAndProjectId(int id, int projectId, string roleName)
        {
            ServiceResponse<SOW> response;
            if (id == 0 || projectId == 0 || string.IsNullOrEmpty(roleName))
            {
                m_Logger.LogInformation("Creating Response object in SOWService");
                response = new ServiceResponse<SOW>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request."
                };
                m_Logger.LogInformation("Response object created in SOWService");

                return response;
            }
            else
            {
                if (Roles.DepartmentHead.GetEnumDescription().TrimLowerCase().Equals(roleName.TrimLowerCase()) ||
                            Roles.ProgramManager.GetEnumDescription().TrimLowerCase().Equals(roleName.TrimLowerCase()))
                {
                    m_Logger.LogInformation("Creating Response object in SOWService");
                    response = new ServiceResponse<SOW>();
                    response.Item = await m_ProjectContext.SOW.Where(sw => sw.Id == id && sw.ProjectId == projectId).FirstOrDefaultAsync();
                    if (response.Item == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "SOW not found..";
                    }
                    else
                    {
                        response.IsSuccessful = true;
                        response.Message = "";
                    }
                    m_Logger.LogInformation("Response object created in SOWService");

                    return response;
                }
                else
                    return response = new ServiceResponse<SOW>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = "Unauthorized access."
                    }; ;
            }
        }

        #endregion

        #region Update
        /// <summary>
        /// This method updates sow.
        /// </summary>
        /// <param name="sowRequest"></param>
        /// <returns>dynamic</returns>
        public async Task<ServiceResponse<int>> Update(SOWRequest sowRequest)
        {
            int isUpdated;
            ServiceResponse<int> response;
            m_Logger.LogInformation("SOWService: Calling \"Update\" method.");

            if (Roles.ProgramManager.GetEnumDescription().TrimLowerCase().Equals(sowRequest.RoleName.TrimLowerCase()) ||
                Roles.DepartmentHead.GetEnumDescription().TrimLowerCase().Equals(sowRequest.RoleName.TrimLowerCase()))
            {
                m_Logger.LogInformation("Fetching SOW for update.");

                var sow = m_ProjectContext.SOW.Find(sowRequest.Id);

                m_Logger.LogInformation("SOWService: SOW exists?");

                if (sow == null)
                {
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response = new ServiceResponse<int>();
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "SOW not found to update.";

                    m_Logger.LogInformation("Response object created in SOWService");

                    return response;
                }
                else
                    m_Logger.LogInformation("SOWService: SOW found.");

                m_Logger.LogInformation("SOWService: Update feilds.");

                if (!sowRequest.IsActive.HasValue)
                    sowRequest.IsActive = sow.IsActive;

                m_Logger.LogInformation("SOWService: assigning to automapper.");

                sow.SOWId = sowRequest.SOWId;
                sow.SOWFileName = sowRequest.SOWFileName;
                sow.SOWSignedDate = sowRequest.SOWSignedDate;

                m_Logger.LogInformation("SOWService: Calling SaveChanges method on DB Context.");

                isUpdated = await m_ProjectContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    m_Logger.LogInformation("SOWService: Updating sow record.");
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response = new ServiceResponse<int>();
                    response.Item = 1;
                    response.IsSuccessful = true;
                    response.Message = string.Empty;

                    m_Logger.LogInformation("Response object created in SOWService");

                    return response;
                }
                else
                {
                    m_Logger.LogInformation("Creating Response object in SOWService");

                    response = new ServiceResponse<int>();
                    response.Item = -1;
                    response.IsSuccessful = false;
                    response.Message = "No record updated.";

                    m_Logger.LogInformation("Response object created in SOWService");

                    return response;
                }
            }
            else
            {
                m_Logger.LogInformation("Creating Response object in SOWService");

                response = new ServiceResponse<int>();
                response.Item = -2;
                response.IsSuccessful = false;
                response.Message = "Your current role does not have permission to update SOW.";

                m_Logger.LogInformation("Response object created in SOWService");

                return response;
            }
        }

        #endregion
    }
}
