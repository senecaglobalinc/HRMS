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
    public class ProjectRoleDetailsService : IProjectRoleDetailService
    {
        #region Global Varibles

        private readonly ILogger<ProjectRoleDetailsService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;

        #endregion

        #region ProjectRoleDetailsService
        public ProjectRoleDetailsService(ILogger<ProjectRoleDetailsService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectRoleDetails, ProjectRoleDetails>();
            });
            m_mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints.Value;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the ProjectRoleDetails
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ProjectRoleDetails>> GetAll()
        {
            ServiceListResponse<ProjectRoleDetails> response;
            var obj = await m_ProjectContext.ProjectRoleDetails.ToListAsync();
            if (obj == null || obj.Count == 0)
            {
                response = new ServiceListResponse<ProjectRoleDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Project Role Details found.."
                };
            }
            else {
                response = new ServiceListResponse<ProjectRoleDetails>() { 
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
        /// Get ProjectRoleDetails by id
        /// </summary>
        /// <param name="id">ProjectRoleDetails Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<ProjectRoleDetails>> GetById(int id)
        {
            ServiceResponse<ProjectRoleDetails> response;
            var obj = await m_ProjectContext.ProjectRoleDetails.FindAsync(id);
            if (obj == null)
            {
                response = new ServiceResponse<ProjectRoleDetails>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "No Project Role Details found with this Id."
                };
            }
            else {
                response = new ServiceResponse<ProjectRoleDetails>()
                {
                    Item = obj,
                    IsSuccessful = true,
                    Message = ""
                };
            }

            return response;
        }

        #endregion

    }
}
