using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    /// <summary>
    /// Service class to get the competency area
    /// </summary>
    public class CompetencyAreaService : ICompetencyAreaService
    {
        #region Global Variables

        private readonly ILogger<CompetencyAreaService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly ISkillService m_SkillService;
        private readonly ISkillGroupService m_SkillGroupService;
        private readonly IMapper m_Mapper;

        #endregion

        #region Constructor
        public CompetencyAreaService(ILogger<CompetencyAreaService> logger, AdminContext adminContext,
                                     ISkillService skillService, ISkillGroupService skillGroupService)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
            m_SkillService = skillService;
            m_SkillGroupService = skillGroupService;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompetencyArea, CompetencyArea>();
            });

            m_Mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// This method create competency area. 
        /// </summary>
        /// <param name="competencyAreaIn">Competency area detail information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Create(CompetencyArea competencyAreaIn)
        {
            int isCreated;
            m_Logger.LogInformation("Calling \"Create\" method in CompetencyAreaService");

            m_Logger.LogInformation("Competency area already exists?");
            CompetencyArea competencyAreaAlreadyExits =
                await GetByCompetencyAreaCode(competencyAreaIn.CompetencyAreaCode);

            if (competencyAreaAlreadyExits != null)
                return CreateResponse(null, false, "Competency area code already exists.");
            else
                m_Logger.LogInformation("Competency area does not already exists.");

            m_Logger.LogInformation("Update feilds.");

            CompetencyArea competencyArea = new CompetencyArea();

            if (!competencyAreaIn.IsActive.HasValue)
                competencyAreaIn.IsActive = true;

            m_Logger.LogInformation("Assigning to automapper.");

            m_Mapper.Map<CompetencyArea, CompetencyArea>(competencyAreaIn, competencyArea);

            m_Logger.LogInformation("Add competency to list");
            m_AdminContext.CompetencyAreas.Add(competencyArea);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in CompetencyAreaService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Competency area created successfully.");
                return CreateResponse(competencyArea, true, string.Empty);

            }
            else
                return CreateResponse(null, false, "No competency area created.");
        }
        #endregion

        #region Delete
        /// <summary>
        /// This method deactivates competency area.
        /// </summary>
        /// <param name="competencyAreaID">competency area ID</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Delete(int competencyAreaID)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling \"Delete\" method in CompetencyAreaService");

            //Fetch competency area for update
            var competencyArea = m_AdminContext.CompetencyAreas.Find(competencyAreaID);

            //Competency area exists?
            if (competencyArea == null)
                return CreateResponse(null, false, "Competency area not found for delete.");

            //Verify Skill or Skill Group exist for competency area.
            var response = await VerifySkillOrSkillGroupExists(competencyAreaID);
            if (!response.IsSuccessful)
                return response;

            competencyArea.IsActive = false;

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in CompetencyAreaService.");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating competency area record in CompetencyAreaService.");
                return CreateResponse(null, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// This method fetches all competency area based on isActive flag.
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns>List<CompetencyArea></returns>
        public async Task<List<CompetencyArea>> GetAll(bool? isActive)
        {
            if (!isActive.HasValue)
                return await m_AdminContext.CompetencyAreas.OrderBy(x => x.CompetencyAreaCode).ToListAsync();
            else
                return await m_AdminContext.CompetencyAreas.Where(ca => ca.IsActive == isActive).OrderBy(x => x.CompetencyAreaCode).ToListAsync();
        }
        #endregion

        #region GetByCompetencyAreaCode
        /// <summary>
        /// this method fetches competency area based on competency code.
        /// </summary>
        /// <param name="competencyAreaCode">competency area code</param>
        /// <returns>CompetencyArea</returns>
        public async Task<CompetencyArea> GetByCompetencyAreaCode(string competencyAreaCode) =>
                        await m_AdminContext.CompetencyAreas.Where(ca => ca.CompetencyAreaCode.ToLower().Trim() == competencyAreaCode.ToLower().Trim())
                        .FirstOrDefaultAsync();

        #endregion

        #region Update
        /// <summary>
        /// This method updates competency area.
        /// </summary>
        /// <param name="competencyAreaIn">competency area detail information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Update(CompetencyArea competencyAreaIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling \"Update\" method in CompetencyAreaService");

            //Fetch competency area for update
            var competencyArea = m_AdminContext.CompetencyAreas.Find(competencyAreaIn.CompetencyAreaId);

            //Competency area exists?
            if (competencyArea == null)
                return CreateResponse(null, false, "Competency area not found for update.");

            //Verify Skill or Skill Group exist for competency area.
            var response = await VerifySkillOrSkillGroupExists(competencyAreaIn.CompetencyAreaId);
            if (!response.IsSuccessful)
                return response;

            CompetencyArea competencyAreaAlreadyExits =
               await GetByCompetencyAreaCode(competencyAreaIn.CompetencyAreaCode);

            //Competency area already exists?
            if (competencyAreaAlreadyExits != null &&
                competencyAreaAlreadyExits.CompetencyAreaId != competencyArea.CompetencyAreaId)
                return CreateResponse(null, false, "Competency area code already exists.");

            //Update feilds
            if (!competencyAreaIn.IsActive.HasValue)
                competencyAreaIn.IsActive = competencyArea.IsActive;

            competencyAreaIn.CreatedBy = competencyArea.CreatedBy;
            competencyAreaIn.CreatedDate = competencyArea.CreatedDate;

            m_Mapper.Map<CompetencyArea, CompetencyArea>(competencyAreaIn, competencyArea);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in CompetencyAreaService.");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating competency area record in CompetencyAreaService.");
                return CreateResponse(competencyArea, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated.");
        }

        #endregion

        //Helpers
        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="competencyArea"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(CompetencyArea competencyArea, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in CompetencyAreaService");

            dynamic response = new ExpandoObject();
            response.CompetencyArea = competencyArea;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in CompetencyAreaService");

            return response;
        }

        #endregion

        #region VerifySkillOrSkillGroupExists
        /// <summary>
        /// this method fetches verifies wheather skill or skill area exists for competency area.
        /// </summary>
        /// <param name="competencyAreaID">competency area code</param>
        /// <returns>Boolean</returns>
        private async Task<dynamic> VerifySkillOrSkillGroupExists(int competencyAreaID)
        {
            m_Logger.LogInformation("Calling VerifySkillOrSkillGroupExists method in CompetencyAreaService");

            var skills = await m_SkillService.GetByCompetencyAreaId(competencyAreaID);

            m_Logger.LogInformation("Skill Count:" + (skills != null ? skills.Count() : 0));
            if (skills != null && skills.Where(sk => sk.IsActive.HasValue && sk.IsActive.Value).Count() > 0)
            {
                m_Logger.LogInformation("Active Skill Count:" +
                    skills.Where(sk => sk.IsActive.HasValue && sk.IsActive.Value).Count());

                return CreateResponse(null, false, "Skills exists under this competency area.");
            }

            var skillGroups = await m_SkillGroupService.GetByCompetencyAreaId(competencyAreaID);

            m_Logger.LogInformation("Skill Groups Count:" + (skillGroups != null ? skillGroups.Count() : 0));
            if (skillGroups != null && skillGroups.Where(sg => sg.IsActive.HasValue && sg.IsActive.Value).Count() > 0)
            {
                m_Logger.LogInformation("Active Skill Groups Count:" +
                    skillGroups.Where(sg => sg.IsActive.HasValue && sg.IsActive.Value).Count());

                return CreateResponse(null, false, "Skill Groups exists under this competency area.");
            }

            m_Logger.LogInformation("No active Skill or Skill Group found for select competency area.");

            return CreateResponse(null, true, string.Empty);
        }

        #endregion
    }
}
