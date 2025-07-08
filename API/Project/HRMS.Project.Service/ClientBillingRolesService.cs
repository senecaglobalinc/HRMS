using AutoMapper;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class ClientBillingRolesService : IClientBillingRoleService
    {
        #region Global Varibles

        private readonly ILogger<ClientBillingRolesService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_Mapper;
        private readonly IAssociateAllocationService m_AssociateAllocationService;
        private readonly IClientBillingRoleHistoryService m_ClientBillingRoleHistoryService;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private IOrganizationService m_OrgService;
        IMapper mapper;
        #endregion

        #region ClientBillingRolesService
        public ClientBillingRolesService(ILogger<ClientBillingRolesService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IAssociateAllocationService associateAllocationService,
            IClientBillingRoleHistoryService clientBillingRoleHistoryService,
            IOrganizationService orgService)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientBillingRoles, Entities.ClientBillingRoles>();
            });
            m_Mapper = config.CreateMapper();
            m_clientFactory = clientFactory;
            //m_apiEndPoints = apiEndPoints?.Value;
            m_apiEndPoints = apiEndPoints != null ? apiEndPoints.Value : null;
            m_AssociateAllocationService = associateAllocationService;
            m_ClientBillingRoleHistoryService = clientBillingRoleHistoryService;
            m_OrgService = orgService;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the ClientBillingRoles
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ClientBillingRoles>> GetAll()
        {
            ServiceListResponse<ClientBillingRoles> response = new ServiceListResponse<ClientBillingRoles>();
            var obj = await m_ProjectContext.ClientBillingRoles.ToListAsync();
            if (obj == null || obj.Count == 0) {
                response.Items = null;
                response.IsSuccessful = false;
                response.Message = "No Client Billing Roles found...";
            }
            else
            {
                response.Items = obj;
                response.IsSuccessful = true;
                response.Message = "";
            }
            return response;
        }

        #endregion

        #region GetById
        /// <summary>
        /// Get ClientBillingRoles by id
        /// </summary>
        /// <param name="id">ClientBillingRoles Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<ClientBillingRoles>> GetById(int id)
        {
            ServiceResponse<ClientBillingRoles> response = new ServiceResponse<ClientBillingRoles>();
            if (id == 0)
            {
                response.Item = null;
                response.IsSuccessful = false;
                response.Message = "Invalid Request";
            }
            else {
                var obj = await m_ProjectContext.ClientBillingRoles.FindAsync(id);
                if (obj == null)
                {
                    response.Item = null;
                    response.IsSuccessful = false;
                    response.Message = "Client Billing Role not found...";
                }
                else
                {
                    response.Item = obj;
                    response.IsSuccessful = true;
                    response.Message = "";
                }
            }
            return response;
        }

        #endregion

        #region GetAllByProjectId
        /// <summary>
        /// Get client billing roles by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>ClientBillingRoleResponse</returns>
        public async Task<ServiceListResponse<ClientBillingRole>> GetAllByProjectId(int projectId)
        {
            ServiceListResponse<ClientBillingRole> response;
            if (projectId == 0)
            {
                response = new ServiceListResponse<ClientBillingRole>()
                {
                    Items = new List<ClientBillingRole>(),
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
                return response;
            }
            else
            {
                m_Logger.LogInformation("Calling \"GetByProjectId\" method in ClientBillingRoleService.");

                m_Logger.LogInformation($"Getting client billing role for project Id:{projectId}");

                List<ClientBillingRoles> clientBillingRoles = await m_ProjectContext.ClientBillingRoles
                    .Where(cbr => cbr.ProjectId == projectId && cbr.IsActive == true).ToListAsync();

                m_Logger.LogInformation($"Fetching project record associated with project Id:{projectId}");

                Entities.Project project = await m_ProjectContext.Projects
                   .Where(proj => proj.ProjectId == projectId).FirstOrDefaultAsync();

                m_Logger.LogInformation($"Fetching associate allocation record for project Id:{projectId}");

                List<AssociateAllocation> associateAllocations = await m_ProjectContext.AssociateAllocation
                    .Where(aa => aa.ProjectId == projectId && aa.ReleaseDate == null).ToListAsync();

                m_Logger.LogInformation($"Adding record to ClientBillingRoleModels.");

                var clientBillingRoleModels =
                     (from cbr in clientBillingRoles
                      join ap in m_ProjectContext.AllocationPercentage
                        on cbr.ClientBillingPercentage equals ap.AllocationPercentageId
                      select new ClientBillingRole
                      {
                          ClientBillingRoleId = cbr.ClientBillingRoleId,
                          ClientBillingRoleName = cbr.ClientBillingRoleName,
                          ClientBillingPercentage = Convert.ToInt32(ap.Percentage),
                          NoOfPositions = cbr.NoOfPositions,
                          StartDate = cbr.StartDate,
                          EndDate = cbr.EndDate,
                          IsActive = cbr.IsActive,
                          ProjectId = project.ProjectId,
                          ProjectName = project.ProjectName,
                          Percentage = ap.Percentage,
                          AllocationCount = (from aa in associateAllocations
                                             where aa.ClientBillingRoleId == cbr.ClientBillingRoleId
                                             select aa.AssociateAllocationId).Distinct().Count()

                      }).OrderBy(cbrm => cbrm.ClientBillingRoleName).ToList();

                m_Logger.LogInformation($"Adding record to ClientBillingRoleModels.");
                response = new ServiceListResponse<ClientBillingRole>()
                {
                    Items = clientBillingRoleModels,
                    IsSuccessful = true,
                    Message = ""
                };
                return response;
            }
        }

        #endregion

        #region Create
        /// <summary>
        /// This method create Client Billing Role. 
        /// </summary>
        /// <param name="clientBillingRoleIn"></param>
        /// <returns>ClientBillingRoleResponse</returns>
        public async Task<ServiceResponse<int>> Create(ClientBillingRoles clientBillingRoleIn)
        {
            ServiceResponse<int> response;

            int isCreated;
            m_Logger.LogInformation("Calling \"Create\" method in ClientBillingRoleService");

            ClientBillingRoles existingClientBillingRoles = await GetExistingClientBillingRole(clientBillingRoleIn.ClientBillingRoleName, 
                                                                                         clientBillingRoleIn.NoOfPositions,
                                                                                         clientBillingRoleIn.ClientBillingPercentage,
                                                                                         clientBillingRoleIn.StartDate,
                                                                                         clientBillingRoleIn.ProjectId);

            if (existingClientBillingRoles != null)
            {
                response = new ServiceResponse<int>();
                response.Item = -1;
                response.IsSuccessful = false;
                response.Message = "Client Billing Role already exist.";
                return response;
            }
            else
                m_Logger.LogInformation("Client Billing Role does not already exist.");

            ClientBillingRoles clientBillingRole = new ClientBillingRoles();

            if (!clientBillingRoleIn.IsActive.HasValue)
                clientBillingRoleIn.IsActive = true;

            m_Logger.LogInformation("Assigning to automapper.");

            clientBillingRole = m_Mapper.Map<ClientBillingRoles, ClientBillingRoles>(clientBillingRoleIn);

            m_ProjectContext.ClientBillingRoles.Add(clientBillingRole);
            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ClientBillingRoleService");
            isCreated = await m_ProjectContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                response = new ServiceResponse<int>();
                response.Item = isCreated;
                response.IsSuccessful = true;
                response.Message = string.Empty;
                return response;
            }
            else
            {
                response = new ServiceResponse<int>();
                response.Item = 0;
                response.IsSuccessful = false;
                response.Message = "No Client Billing Role created.";
                return response;
            }
        }

        #endregion

        #region Update
        /// <summary>
        /// Update client billing role.
        /// </summary>
        /// <param name="clientBillingRoleIn"></param>
        /// <returns>ClientBillingRoleResponse</returns>
        public async Task<ServiceResponse<int>> Update(ClientBillingRoles clientBillingRoleIn)
        {
            ServiceResponse<int> response;
            try
            {
                int isUpdated;

                m_Logger.LogInformation("Calling \"Update\" method in ClientBillingRoleService");

                #region Status

                var statuses = await m_OrgService.GetStatusesByCategoryName("PPC");

                if (statuses == null || statuses.Items == null ||statuses.Items.Count == 0)
                {
                    response = new ServiceResponse<int>();
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Statuses not found.";
                    return response;
                }                   

                var draftedStatus = statuses.Items.Where(st => st.StatusCode.Equals("Drafted")).FirstOrDefault();
                var closedStatus = statuses.Items.Where(st => st.StatusCode.Equals("Closed")).FirstOrDefault();
                var createdStatus = statuses.Items.Where(st => st.StatusCode.Equals("Created")).FirstOrDefault();

                if (draftedStatus == null)
                {
                    response = new ServiceResponse<int>();
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Drafted statuses not found.";
                    return response;
                }

                if (closedStatus == null)
                {
                    response = new ServiceResponse<int>();
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Closed statuses not found.";
                    return response;
                }

                if (createdStatus == null)
                {
                    response = new ServiceResponse<int>();
                    response.Item = 0;
                    response.IsSuccessful = false;
                    response.Message = "Created statuses not found.";
                    return response;
                }

                #endregion

                #region Fetching Client Billing Role For Update

                var clientBillingRole = m_ProjectContext.ClientBillingRoles.Find(clientBillingRoleIn.ClientBillingRoleId);

                if (clientBillingRole == null)
                {
                    return response = new ServiceResponse<int>() {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "ClientBillingRoles not found for update."
                    };
                    //return CreateResponse(null, false, "ClientBillingRoles not found for update.");
                }
                else
                    m_Logger.LogInformation("ClientBillingRoles found for update.");

                #endregion

                #region Build Rules for Updating Client Billing

                var project = m_ProjectContext.Projects.Find(clientBillingRoleIn.ProjectId);

                if (project == null)
                {
                    return response = new ServiceResponse<int>() {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project not found."
                    };
                    //return CreateResponse(null, false, "Project not found.");
                }

                int allocationCount = 0;
                if (project.ProjectStateId != closedStatus.StatusId && clientBillingRoleIn.ProjectId.HasValue)
                {
                    var allocations = await m_AssociateAllocationService.GetByProjectId(clientBillingRoleIn.ProjectId.Value);
                    allocationCount = (allocations == null || allocations.Items == null || allocations.Items.Count ==0)? 0 : 
                                            allocations.Items.Where(aa => aa.IsBillable == true).Count();
                }
                else
                    m_Logger.LogInformation("Project status is closed, so not fetching allocations.");

                if (allocationCount >= clientBillingRoleIn.NoOfPositions &&
                     (project.ProjectStateId == draftedStatus.StatusId || project.ProjectStateId == createdStatus.StatusId))
                {
                   return response = new ServiceResponse<int>() { 
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Cannot decrease Positions."
                    };
                    //return CreateResponse(null, false, "Cannot decrease Positions.");
                }
                    

                ClientBillingRoles otherClientBillingRole = await GetExistingClientBillingRole(clientBillingRoleIn.NoOfPositions,
                                                                                               clientBillingRole.ClientBillingPercentage,
                                                                                               clientBillingRole.StartDate,
                                                                                               clientBillingRole.ProjectId,
                                                                                               clientBillingRole.ClientBillingRoleId);

                if (otherClientBillingRole == null)
                {
                    return response = new ServiceResponse<int>() {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Client billing role already exists."
                    };
                    //return CreateResponse(null, false, "Client billing role already exist");
                }

                #endregion

                #region Creatiing Client Billing Roles History

                ClientBillingRolesHistory clientBillingRolesHistory = new ClientBillingRolesHistory
                {
                    ClientBillingRoleId = clientBillingRole.ClientBillingRoleId,
                    ClientBillingRoleName = clientBillingRole.ClientBillingRoleName,
                    ProjectId = clientBillingRole.ProjectId ?? 0,
                    NoOfPositions = clientBillingRole.NoOfPositions ?? 0,
                    StartDate = clientBillingRole.StartDate,
                    EndDate = clientBillingRole.EndDate,
                    ClientBillingPercentage = clientBillingRole.ClientBillingPercentage
                };

                var response_obj = await m_ClientBillingRoleHistoryService.Create(clientBillingRolesHistory);

                if (!response_obj.IsSuccessful)
                {
                   return response = new ServiceResponse<int>() {
                        Item = 0,
                        IsSuccessful = response_obj.IsSuccessful,
                        Message = response_obj.Message
                    };
                    //return CreateResponse(null, response_obj.IsSuccessful, response.Message);
                }

                #endregion

                #region Updating Client Billing Role

                clientBillingRole.StartDate = clientBillingRoleIn.StartDate;
                clientBillingRole.EndDate = clientBillingRoleIn.EndDate;
                clientBillingRole.ClientBillingRoleName = clientBillingRoleIn.ClientBillingRoleName;
                clientBillingRole.ClientBillingPercentage = clientBillingRoleIn.ClientBillingPercentage;
                clientBillingRole.NoOfPositions = clientBillingRoleIn.NoOfPositions;

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ClientBillingRoleService");
                isUpdated = await m_ProjectContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    m_Logger.LogInformation("Client Billing Role updated successfully.");
                    return response = new ServiceResponse<int>() {
                        Item = isUpdated,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                    //return CreateResponse(clientBillingRole, true, string.Empty);
                }
                else
                {
                    m_Logger.LogError("No client billing role created.");
                    return null;
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// This method deletes notification type.
        /// </summary>
        /// <param name="clientBillingRoleId"></param>
        /// <returns>dynamic</returns>
        public async Task<ServiceResponse<bool>> Delete(int clientBillingRoleId)
        {
            int isUpdated;
            ServiceResponse<bool> response;
            m_Logger.LogInformation("Calling \"Delete\" method in ClientBillingRoleService");
            m_Logger.LogInformation("Fetching client billing roles for delete in ClientBillingRoleService");

            //Fetch client billing role for delete
            var clientBillingRole = m_ProjectContext.ClientBillingRoles.Find(clientBillingRoleId);

            //client billing role exists?
            if (clientBillingRole == null)
            {
                return response = new ServiceResponse<bool>() { 
                    Item = false,
                    IsSuccessful = false,
                    Message = "Client billing role not found for delete."
                };
            }

            m_Logger.LogInformation("Fetching client billing roles in ClientBillingRoleService");

            m_ProjectContext.ClientBillingRoles.Remove(clientBillingRole);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in ClientBillingRoleService.");

            isUpdated = await m_ProjectContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Deleted client billing role record in ClientBillingRoleService.");
                {
                    return response = new ServiceResponse<bool>()
                    {
                        Item = true,
                        IsSuccessful = true,
                        Message = "Client Billing Role is deleted successfully."
                    };
                }
            }
            else
            {
                return response = new ServiceResponse<bool>()
                {
                    Item = false,
                    IsSuccessful = true,
                    Message = "No record updated."
                };
            }
        }
        #endregion

        #region Close
        /// <summary>
        /// Close client billing role.
        /// </summary>
        /// <param name="clientBillingRoleId"></param>
        /// <returns>ClientBillingRoleResponse</returns>
        public async Task<ServiceResponse<int>> Close(int clientBillingRoleId, DateTime endDate,string reason)
        {
            ServiceResponse<int> response;
            try
            {
                int isUpdated;

                m_Logger.LogInformation("Calling \"Update\" method in ClientBillingRoleService");

                var clientBillingRole = m_ProjectContext.ClientBillingRoles.Find(clientBillingRoleId);

                if (clientBillingRole == null)
                {
                    return response = new ServiceResponse<int>() { 
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Client billing role not found for closing."
                    };
                }

                var associateAllocations = await m_AssociateAllocationService.GetByClientBillingRoleId(clientBillingRoleId);

                if (associateAllocations != null && associateAllocations.Items != null && associateAllocations.Items.Count > 0)
                {
                    return response = new ServiceResponse<int>() { 
                        Item = 0,
                        IsSuccessful = false,
                        Message = "This Role has active allocation(s). First Release associate(s)."
                    };
                }

                //Mark end date and Isactive to false
                clientBillingRole.EndDate = endDate;
                clientBillingRole.IsActive = false;
                clientBillingRole.Reason = reason;

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ClientBillingRoleService");
                isUpdated = await m_ProjectContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    m_Logger.LogInformation("Client billing role closed successfully.");
                    return response = new ServiceResponse<int>() {
                        Item = isUpdated,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("No client billing role created.");
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //Private Method
        #region GetExistingClientBillingRole
        /// <summary>
        /// This method gets resource allocation for billable resources
        /// </summary>
        /// <param name="noOfPositions"></param>
        /// <param name="clientBillingPercentage"></param>
        /// <param name="startDate"></param>
        /// <param name="projectId"></param>
        /// <returns>ResourceAllocationResponse</returns>
        private async Task<ClientBillingRoles> GetExistingClientBillingRole(string clientBillingRoleName, int? noOfPositions, int? clientBillingPercentage,
            DateTime? startDate, int? projectId)
        {
            return await m_ProjectContext.ClientBillingRoles
                        .Where(
                                cbr => cbr.ClientBillingRoleName == clientBillingRoleName
                                      && cbr.NoOfPositions == noOfPositions
                                      && cbr.ClientBillingPercentage == clientBillingPercentage
                                      && cbr.StartDate == startDate
                                      && cbr.ProjectId == projectId
                              ).FirstOrDefaultAsync();
        }
        #endregion

        #region GetExistingClientBillingRole
        /// <summary>
        /// This method gets resource allocation for billable resources
        /// </summary>
        /// <param name="noOfPositions"></param>
        /// <param name="clientBillingPercentage"></param>
        /// <param name="startDate"></param>
        /// <param name="projectId"></param>
        /// <param name="clientBillingRoleId"></param>
        /// <returns>ClientBillingRoles</returns>
        private async Task<ClientBillingRoles> GetExistingClientBillingRole(int? noOfPositions, int? clientBillingPercentage,
                                                                 DateTime? startDate, int? projectId, int clientBillingRoleId)
        {
            return await m_ProjectContext.ClientBillingRoles
                        .Where(
                                cbr => cbr.ProjectId == projectId
                                      && cbr.ClientBillingRoleId == clientBillingRoleId
                              ).FirstOrDefaultAsync();
        }
        #endregion   
    }
}
