using AutoMapper;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class ProjectRolesService: IProjectRolesService
    {
        #region Global Varibles

        private readonly ILogger<ProjectRolesService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;

        #endregion

        #region ProjectRolesService
        public ProjectRolesService(ILogger<ProjectRolesService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectRoles, ProjectRoles>();
            });
            m_mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints.Value;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the ProjectRoles
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectRoles>> GetAll()
        {
            ServiceListResponse<ProjectRoles> response;
            var obj = await m_ProjectContext.ProjectRoles.ToListAsync();
            if (obj == null || obj.Count == 0)
            {
                response = new ServiceListResponse<ProjectRoles>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Project Roles found.."
                };
            }
            else {
                response = new ServiceListResponse<ProjectRoles>()
                {
                    Items = obj,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            return response;
        }

        #endregion

        #region GetById
        /// <summary>
        /// Get ProjectRoles by id
        /// </summary>
        /// <param name="id">ProjectRoles Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<ProjectRoles>> GetById(int id)
        {
            ServiceResponse<ProjectRoles> response;
            if (id == 0)
            {
                response = new ServiceResponse<ProjectRoles>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request..."
                };
            }
            else { 
               var obj = await m_ProjectContext.ProjectRoles.FindAsync(id);
                if (obj == null)
                {
                    response = new ServiceResponse<ProjectRoles>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = "No Project Roles found with this Id.."
                    };
                }
                else
                {
                    response = new ServiceResponse<ProjectRoles>()
                    {
                        Item = obj,
                        IsSuccessful = true,
                        Message = ""
                    };
                }
            }
            return response;
        }

        #endregion
    }
}
