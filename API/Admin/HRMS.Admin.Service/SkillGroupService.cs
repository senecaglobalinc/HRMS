using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    /// <summary>
    /// Service class to get the Client details
    /// </summary>
    public class SkillGroupService : ISkillGroupService
    {
        #region Global Variables

        private readonly ILogger<SkillGroupService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;

        #endregion

        #region Constructor
        public SkillGroupService(ILogger<SkillGroupService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SkillGroup, SkillGroup>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Create skillGroup
        /// </summary>
        /// <param name="skillGroupIn">skillGroup information</param>
        /// <returns></returns>
        public async Task<dynamic> Create(SkillGroup skillGroupIn)
        {
            m_Logger.LogInformation("Calling CreateSkillGroup method in SkillGroupService");

            int isCreated;

            //Checking if Skill Group Name already exists
            var isExists = m_AdminContext.SkillGroups.Where(p => p.SkillGroupName.ToLower().Trim() == skillGroupIn.SkillGroupName.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "SkillGroupName already exists");

            SkillGroup skillGroup = new SkillGroup();

            if (!skillGroupIn.IsActive.HasValue)
                skillGroupIn.IsActive = true;

            m_mapper.Map<SkillGroup, SkillGroup>(skillGroupIn, skillGroup);

            m_AdminContext.SkillGroups.Add(skillGroup);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in SkillGroupService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
                return CreateResponse(skillGroup, true, string.Empty);
            else
                return CreateResponse(null, false, "No SkillGroup created.");
        }

        #endregion

        #region Delete
        /// <summary>
        /// This method deactivates skill group.
        /// </summary>
        /// <param name="skillGroupId">Skill Group Id</param>
        /// <returns>bool</returns>
        public async Task<dynamic> Delete(int skillGroupId)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling DeleteSkillGroup in SkillGroupService");

            //Fetch skill group for delete
            var skillGroup = m_AdminContext.SkillGroups.Find(skillGroupId);

            //SkillGroup exists?
            if (skillGroup == null)
                return CreateResponse(null, false, "skillGroup not found for delete.");

            skillGroup.IsActive = false;

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in SkillGroupService.");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating Skill Group record in SkillGroupService.");
                return CreateResponse(null, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the skillGroups
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<SkillGroup>> GetAll(bool? isActive)
        {
            return await (from sg in m_AdminContext.SkillGroups
                          join ca in m_AdminContext.CompetencyAreas on sg.CompetencyAreaId equals ca.CompetencyAreaId
                          where sg.IsActive == true && ca.IsActive == true
                          select new SkillGroup
                          {
                              SkillGroupId = sg.SkillGroupId,
                              SkillGroupName = sg.SkillGroupName,
                              CompetencyAreaId = sg.CompetencyAreaId,
                              IsActive = sg.IsActive,
                              Description = sg.Description,
                              CompetencyArea = ca
                          }).OrderBy(x => x.SkillGroupName).ToListAsync();
        }
        #endregion

        #region GetByCompetencyAreaId
        /// <summary>
        /// this method fetches skill group based on competency ID.
        /// </summary>
        /// <param name="competencyAreaID">competency area ID</param>
        /// <returns>SkillGroup</returns>
        public async Task<List<SkillGroup>> GetByCompetencyAreaId(int competencyAreaID) =>
            await m_AdminContext.SkillGroups.Where(sg => sg.CompetencyAreaId == competencyAreaID)
                        .ToListAsync();
        #endregion

        #region GetByCompetencyAreaCode
        /// <summary>
        /// this method fetches skill group based on competency area code.
        /// </summary>
        /// <param name="competencyAreaCode">competency area code</param>
        /// <returns>SkillGroup</returns>
        public async Task<List<SkillGroup>> GetByCompetencyAreaCode(string competencyAreaCode) =>
            await (from skillGroup in m_AdminContext.SkillGroups
                   join competency in m_AdminContext.CompetencyAreas
                   on skillGroup.CompetencyAreaId equals competency.CompetencyAreaId
                   where competency.CompetencyAreaCode == competencyAreaCode
                   select new SkillGroup
                   {
                       SkillGroupId = skillGroup.SkillGroupId,
                       SkillGroupName = skillGroup.SkillGroupName,
                       Description = skillGroup.Description,
                   }).ToListAsync();
        #endregion

        #region Update
        /// <summary>
        /// Updates the skillGroup information
        /// </summary>
        /// <param name="skillGroupIn">skillGroup information</param>
        /// <returns></returns>
        public async Task<dynamic> Update(SkillGroup skillGroupIn)
        {
            int isCreated;

            m_Logger.LogInformation("Calling UpdateSkillGroup method in SkillGroupService");

            //Checking if Skill Group Name already exists
            var isExists = m_AdminContext.SkillGroups.Where(p => p.SkillGroupName.ToLower().Trim() == skillGroupIn.SkillGroupName.ToLower().Trim()
            && p.SkillGroupId != skillGroupIn.SkillGroupId).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "SkillGroupName already exists");

            var skillGroup = m_AdminContext.SkillGroups.Find(skillGroupIn.SkillGroupId);

            if (skillGroup == null)
                return CreateResponse(null, false, "SkillGroup not found for update.");

            if (!skillGroupIn.IsActive.HasValue)
                skillGroupIn.IsActive = skillGroup.IsActive;

            skillGroupIn.CreatedBy = skillGroup.CreatedBy;
            skillGroupIn.CreatedDate = skillGroup.CreatedDate;

            m_mapper.Map<SkillGroup, SkillGroup>(skillGroupIn, skillGroup);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in SkillGroupService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
                return CreateResponse(skillGroup, true, string.Empty);
            else
                return CreateResponse(null, false, "No record updated.");
        }
        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="skillGroup"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(SkillGroup skillGroup, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in SkillGroupService");

            dynamic response = new ExpandoObject();
            response.SkillGroup = skillGroup;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in SkillGroupService");

            return response;
        }

        #endregion
    }
}
