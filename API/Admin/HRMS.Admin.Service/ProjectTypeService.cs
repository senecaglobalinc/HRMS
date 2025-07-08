using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class ProjectTypeService : IProjectTypeService
    {
        #region Global Variables 

        private readonly ILogger<ProjectTypeService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;

        #endregion

        #region Constructor
        public ProjectTypeService(ILogger<ProjectTypeService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectType, ProjectType>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a ProjectType
        /// </summary>
        /// <param name="projectTypeIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(ProjectType projectTypeIn)
        {
            m_Logger.LogInformation("Calling Create method in ProjectTypeService");
            int isCreated;

            //Checking if ProjectTypeCode already exists
            var isExists = m_AdminContext.ProjectTypes.Where(p => p.ProjectTypeCode.ToLower().Trim() == projectTypeIn.ProjectTypeCode.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "ProjectTypeCode already exists");

            ProjectType projectType = new ProjectType();

            if (!projectTypeIn.IsActive.HasValue)
                projectTypeIn.IsActive = true;

            //Add fields
            m_mapper.Map<ProjectType, ProjectType>(projectTypeIn, projectType);

            m_Logger.LogInformation("Add ProjectType to list");
            m_AdminContext.ProjectTypes.Add(projectType);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProjectTypeService");
            isCreated = await m_AdminContext.SaveChangesAsync();
            if (isCreated > 0)
            {
                m_Logger.LogInformation("ProjectType created successfully.");
                return CreateResponse(projectType, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No ProjectType created.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets all ProjectTypes
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<List<ProjectType>> GetAll(bool isActive = true) {
            
           return await m_AdminContext.ProjectTypes.Where(pt => pt.IsActive == isActive ).OrderBy(x => x.ProjectTypeCode).ToListAsync();
        }
            #endregion

        #region GetByProjectTypeCode
        /// <summary>
        /// Gets all ProjectTypes
        /// </summary>
        /// <param name="projectTypeCode"></param>
        /// <returns></returns>
        public async Task<ProjectType> GetByProjectTypeCode(string projectTypeCode) =>
            await m_AdminContext.ProjectTypes.Where(pt => pt.ProjectTypeCode.ToLower().Trim() == projectTypeCode.ToLower().Trim())
            .FirstOrDefaultAsync();
        #endregion

        #region GetByProjectTypeId
        /// <summary>
        /// Gets ProjectType by Id.
        /// </summary>
        /// <param name="projectTypeId"></param>
        /// <returns></returns>
        public async Task<ProjectType> GetProjectTypeById(int projectTypeId) =>
            await m_AdminContext.ProjectTypes.Where(pt => pt.ProjectTypeId == projectTypeId)
            .FirstOrDefaultAsync();
        #endregion

        #region GetByProjectTypeIds
        /// <summary>
        /// Gets ProjectTypes by Ids.
        /// </summary>
        /// <param name="projectTypeIds"></param>
        /// <returns></returns>
        public async Task<List<ProjectType>> GetByProjectTypeIds(int[] projectTypeIds) =>
            await m_AdminContext.ProjectTypes.Where(pt => projectTypeIds.Contains(pt.ProjectTypeId))
            .ToListAsync();
        #endregion

        #region Update
        /// <summary>
        /// Updates the ProjectType details
        /// </summary>
        /// <param name="projectTypeIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(ProjectType projectTypeIn)
        {
            m_Logger.LogInformation("Calling Update method in ProjectTypeService");
            int isUpdated;

            //Checking if ProjectTypeCode already exists
            var isExists = m_AdminContext.ProjectTypes.Where(p => p.ProjectTypeCode.ToLower().Trim() == projectTypeIn.ProjectTypeCode.ToLower().Trim()
            && p.ProjectTypeId != projectTypeIn.ProjectTypeId).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "ProjectTypeCode already exists");

            //Fetch projectType for update
            var projectType = m_AdminContext.ProjectTypes.Find(projectTypeIn.ProjectTypeId);

            if (!projectTypeIn.IsActive.HasValue)
                projectTypeIn.IsActive = projectType.IsActive;

            projectTypeIn.CreatedBy = projectType.CreatedBy;
            projectTypeIn.CreatedDate = projectType.CreatedDate;

            //update fields
            m_mapper.Map<ProjectType, ProjectType>(projectTypeIn, projectType);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in ProjectTypeService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating ProjectType record in ProjectTypeService");
                return CreateResponse(projectType, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No ProjectType record updated.");
        }
        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="projectType"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(ProjectType projectType, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in ProjectTypeService");

            dynamic response = new ExpandoObject();
            response.ProjectType = projectType;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in ProjectTypeService");

            return response;
        }

        #endregion

    }
}
