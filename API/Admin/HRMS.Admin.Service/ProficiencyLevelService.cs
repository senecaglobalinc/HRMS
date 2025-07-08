using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    /// <summary>
    /// service class to proficiency level details
    /// </summary>
    public class ProficiencyLevelService : IProficiencyLevelService
    {
        #region Global Variables 

        private readonly ILogger<ProficiencyLevelService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;

        #endregion

        #region Constructor
        public ProficiencyLevelService(ILogger<ProficiencyLevelService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProficiencyLevel, ProficiencyLevel>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a ProficiencyLevel
        /// </summary>
        /// <param name="proficiencyLevelIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(ProficiencyLevel proficiencyLevelIn)
        {
            m_Logger.LogInformation("Calling Create method in ProficiencyLevelService");
            int isCreated;

            //Checking if ProficiencyLevelCode already exists
            var isExists = m_AdminContext.ProficiencyLevels
                .Where(p => p.ProficiencyLevelCode.ToLower().Trim() == proficiencyLevelIn.ProficiencyLevelCode.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Proficiency level code already exists");

            ProficiencyLevel proficiencyLevel = new ProficiencyLevel();

            if (!proficiencyLevelIn.IsActive.HasValue)
                proficiencyLevelIn.IsActive = true;

            //create fields
            m_mapper.Map<ProficiencyLevel, ProficiencyLevel>(proficiencyLevelIn, proficiencyLevel);

            m_Logger.LogInformation("Add ProficiencyLevel to list");
            m_AdminContext.ProficiencyLevels.Add(proficiencyLevel);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProficiencyLevelService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("ProficiencyLevel created successfully.");
                return CreateResponse(proficiencyLevel, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No ProficiencyLevel created.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets all Proficiency levels
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<List<ProficiencyLevel>> GetAll(bool isActive = true) =>
            await m_AdminContext.ProficiencyLevels.Where(pl => pl.IsActive == isActive).OrderBy(x => x.ProficiencyLevelCode).ToListAsync();
        #endregion

        #region GetById
        /// <summary>
        /// Gets Proficiency levels based on Ids
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<List<ProficiencyLevel>> GetByIds(string proficiencyLevelIds)
        {
            List<int> ids = proficiencyLevelIds.Split(",".ToCharArray()).Select(Int32.Parse).ToList();
            return await m_AdminContext.ProficiencyLevels.Where(p => ids.Contains(p.ProficiencyLevelId)).ToListAsync();
        }

        #endregion

        #region Update
        /// <summary>
        /// Updates the ProficiencyLevel details
        /// </summary>
        /// <param name="proficiencyLevelIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(ProficiencyLevel proficiencyLevelIn)
        {
            m_Logger.LogInformation("Calling Update method in ProficiencyLevelService");

            int isUpdated;

            //Checking if ProficiencyLevelCode already exists
            var isExists = m_AdminContext.ProficiencyLevels
                          .Where(p => p.ProficiencyLevelCode.ToLower().Trim() == proficiencyLevelIn.ProficiencyLevelCode.ToLower().Trim() &&
                          p.ProficiencyLevelId != proficiencyLevelIn.ProficiencyLevelId).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Proficiency level code already exists");

            //Fetch proficiencyLevel for update
            var proficiencyLevel = m_AdminContext.ProficiencyLevels.Find(proficiencyLevelIn.ProficiencyLevelId);

            if (proficiencyLevel == null)
                return CreateResponse(null, false, "ProficiencyLevel not found for update.");

            if (!proficiencyLevelIn.IsActive.HasValue)
                proficiencyLevelIn.IsActive = proficiencyLevel.IsActive;

            proficiencyLevelIn.CreatedBy = proficiencyLevel.CreatedBy;
            proficiencyLevelIn.CreatedDate = proficiencyLevel.CreatedDate;
            //update fields
            m_mapper.Map<ProficiencyLevel, ProficiencyLevel>(proficiencyLevelIn, proficiencyLevel);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in ProficiencyLevelService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating ProficiencyLevel record in ProficiencyLevelService.");
                return CreateResponse(proficiencyLevel, true, string.Empty);
            }
            else
            {
                return CreateResponse(null, false, "No record updated.");
            }
        }
        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="proficiencyLevel"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(ProficiencyLevel proficiencyLevel, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in ProficiencyLevelService");

            dynamic response = new ExpandoObject();
            response.ProficiencyLevel = proficiencyLevel;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in ProficiencyLevelService");

            return response;
        }

        #endregion
    }
}