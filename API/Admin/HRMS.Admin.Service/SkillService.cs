using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    /// <summary>
    /// Service class to get the skill detail
    /// </summary>
    public class SkillService : ISkillService
    {
        #region Global Variables

        private readonly ILogger<SkillService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;

        #endregion

        #region Constructor
        public SkillService(ILogger<SkillService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Skill, Skill>();
            });
            m_mapper = config.CreateMapper();

        }
        #endregion

        #region Create
        /// <summary>
        /// Create skill
        /// </summary>
        /// <param name="skillIn">skill information</param>
        /// <returns></returns>
        public async Task<dynamic> Create(Skill skillIn)
        {
            int isCreated;
            m_Logger.LogInformation("Calling Create method in SkillService");

            Skill skillAlreadyExits = await GetByCode(skillIn.SkillCode);

            //skill code already exists?
            if (skillAlreadyExits != null)
                return CreateResponse(null, false, "skill code already exists.");

            Skill skill = new Skill();

            if (!skillIn.IsActive.HasValue)
                skillIn.IsActive = true;

            if (!skillIn.IsApproved.HasValue)
                skillIn.IsApproved = true;

            m_mapper.Map<Skill, Skill>(skillIn, skill);

            m_AdminContext.Skills.Add(skill);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in SkillService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
                return CreateResponse(skill, true, string.Empty);
            else
                return CreateResponse(null, false, "No skill created.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the designation details
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<Skill>> GetAll(bool? isActive)
        {
            return await (from sk in m_AdminContext.Skills
                          join ca in m_AdminContext.CompetencyAreas
                            on sk.CompetencyAreaId equals ca.CompetencyAreaId into skills
                          from Skill in skills.DefaultIfEmpty()
                          join sg in m_AdminContext.SkillGroups
                            on sk.SkillGroupId equals sg.SkillGroupId into skillgroups
                          from skillgroup in skillgroups.DefaultIfEmpty()
                          select new Skill
                          {
                              SkillId = sk.SkillId,
                              SkillCode = sk.SkillCode,
                              SkillName = sk.SkillName,
                              SkillDescription = sk.SkillDescription,
                              IsApproved = sk.IsApproved,
                              IsActive = sk.IsActive,
                              CompetencyAreaId = sk.CompetencyAreaId,
                              SkillGroupId = sk.SkillGroupId,
                              SkillGroup = new SkillGroup()
                              {
                                  SkillGroupName = skillgroup.SkillGroupName,
                                  SkillGroupId = (int)sk.SkillGroupId
                              },
                              CompetencyArea = new CompetencyArea()
                              {
                                  CompetencyAreaId = (int)sk.CompetencyAreaId,
                                  CompetencyAreaCode = Skill.CompetencyAreaCode,
                              },
                          }).OrderBy(x => x.SkillCode).ToListAsync();


        }
        #endregion

        #region GetActiveSkillsForDropdown
        /// <summary>
        /// Get Active Skills For Dropdown 
        /// </summary>              
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetActiveSkillsForDropdown()
        {
            ServiceListResponse<GenericType> response = new ServiceListResponse<GenericType>();

            try
            {
                List<GenericType> masterDetails = new List<GenericType>();

                masterDetails.AddRange(await m_AdminContext.Skills
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new GenericType
                                        {
                                            Id = c.SkillId,
                                            Name = c.SkillName
                                        }).OrderBy(s => s.Name)
                                        .ToListAsync());


                return response = new ServiceListResponse<GenericType>()
                {
                    Items = masterDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<GenericType>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion

        #region GetByCode
        /// <summary>
        /// Get skill code
        /// </summary>
        /// <param name="skillCode">skill code</param>
        public async Task<Skill> GetByCode(string skillCode) =>
                        await m_AdminContext.Skills.Where(sk => sk.SkillCode == skillCode)
                        .FirstOrDefaultAsync();

        #endregion

        #region GetById
        /// <summary>
        /// Get skills By GetById 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="skillGroupIds"></param>
        /// <returns></returns>
        public async Task<List<Skill>> GetById(string skillIds)
        {
            List<int> ids = skillIds.Split(",".ToCharArray()).Select(Int32.Parse).ToList();
            return await (from sk in m_AdminContext.Skills
                          join ca in m_AdminContext.CompetencyAreas
                            on sk.CompetencyAreaId equals ca.CompetencyAreaId into skills
                          from Skill in skills.DefaultIfEmpty()
                          join sg in m_AdminContext.SkillGroups
                            on sk.SkillGroupId equals sg.SkillGroupId into skillgroups
                          from skillgroup in skillgroups.DefaultIfEmpty()
                          where ids.Contains(sk.SkillId)
                          select new Skill
                          {
                              SkillId = sk.SkillId,
                              SkillCode = sk.SkillCode,
                              SkillName = sk.SkillName,
                              SkillDescription = sk.SkillDescription,
                              IsApproved = sk.IsApproved,
                              IsActive = sk.IsActive,
                              CompetencyAreaId = sk.CompetencyAreaId,
                              SkillGroupId = sk.SkillGroupId,
                              SkillGroup = new SkillGroup()
                              {
                                  SkillGroupName = skillgroup.SkillGroupName,
                                  SkillGroupId = (int)sk.SkillGroupId
                              },
                              CompetencyArea = new CompetencyArea()
                              {
                                  CompetencyAreaId = (int)sk.CompetencyAreaId,
                                  CompetencyAreaCode = Skill.CompetencyAreaCode,
                              },
                          }).ToListAsync();


        }
        #endregion

        #region GetByCompetencyAreaId
        /// <summary>
        /// this method fetches skills based on competency ID.
        /// </summary>
        /// <param name="competencyAreaID">competency area ID</param>
        /// <returns>Skill</returns>
        public async Task<List<Skill>> GetByCompetencyAreaId(int competencyAreaID) =>
            await m_AdminContext.Skills.Where(sk => sk.CompetencyAreaId == competencyAreaID).OrderBy(x => x.SkillCode).ToListAsync();

        #endregion

        #region GetBySkillGroupId
        /// <summary>
        /// Get skills By SkillGroupId 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="skillGroupIds"></param>
        /// <returns></returns>
        public async Task<List<Skill>> GetBySkillGroupId(string skillGroupIds)
        {
            List<int> ids = skillGroupIds.Split(",".ToCharArray()).Select(Int32.Parse).ToList();
            return await (from sk in m_AdminContext.Skills
                          join ca in m_AdminContext.CompetencyAreas
                            on sk.CompetencyAreaId equals ca.CompetencyAreaId into skills
                          from Skill in skills.DefaultIfEmpty()
                          join sg in m_AdminContext.SkillGroups
                            on sk.SkillGroupId equals sg.SkillGroupId into skillgroups
                          from skillgroup in skillgroups.DefaultIfEmpty()
                          where ids.Contains(skillgroup.SkillGroupId)
                          select new Skill
                          {
                              SkillId = sk.SkillId,
                              SkillCode = sk.SkillCode,
                              SkillName = sk.SkillName,
                              SkillDescription = sk.SkillDescription,
                              IsApproved = sk.IsApproved,
                              IsActive = sk.IsActive,
                              CompetencyAreaId = sk.CompetencyAreaId,
                              SkillGroupId = sk.SkillGroupId,
                              SkillGroup = new SkillGroup()
                              {
                                  SkillGroupName = skillgroup.SkillGroupName,
                                  SkillGroupId = (int)sk.SkillGroupId
                              },
                              CompetencyArea = new CompetencyArea()
                              {
                                  CompetencyAreaId = (int)sk.CompetencyAreaId,
                                  CompetencyAreaCode = Skill.CompetencyAreaCode,
                              },
                          }).ToListAsync();


        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the skill information
        /// </summary>
        /// <param name="skillIn">skill information</param>
        /// <returns></returns>
        public async Task<dynamic> Update(Skill skillIn)
        {
            int isUpdated;
            m_Logger.LogInformation("Calling UpdateGrade method in SkillService");

            var skill = m_AdminContext.Skills.Find(skillIn.SkillId);
            if (skill == null)
                return CreateResponse(null, false, "Skill not found for update.");

            Skill skillAlreadyExits =
                await GetByCode(skillIn.SkillCode);

            if (skillAlreadyExits != null &&
                skillAlreadyExits.SkillId != skill.SkillId)
                return CreateResponse(null, false, "Skill code already exists");

            if (!skillIn.IsActive.HasValue)
                skillIn.IsActive = skill.IsActive;

            if (!skillIn.IsApproved.HasValue)
                skillIn.IsApproved = skill.IsApproved;

            m_mapper.Map<Skill, Skill>(skill, skillIn);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in SkillService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
                return CreateResponse(skill, true, string.Empty);
            else
                return CreateResponse(null, false, "No record updated.");
        }
        #endregion

        #region GetskillsBySkillGroupId

        /// <summary>
        /// Get skills By skillgroupid 
        /// </summary>
        
        /// <param name="skillGroupIds"></param>
        /// <returns></returns>
        public async Task<List<Skill>> GetskillsBySkillGroupId(int skillgroupid)
        {           
        return await   ( from sk in m_AdminContext.Skills where sk.SkillGroupId==skillgroupid            
            select new Skill
            {
                SkillId = sk.SkillId,
                SkillCode = sk.SkillCode,
                SkillName = sk.SkillName,
                SkillDescription = sk.SkillDescription,
                IsApproved = sk.IsApproved,
                IsActive = sk.IsActive,
                CompetencyAreaId = sk.CompetencyAreaId,
                SkillGroupId = sk.SkillGroupId,               
            }).OrderBy(x => x.SkillCode).ToListAsync();

        }
        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(Skill skill, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in SkillService");

            dynamic response = new ExpandoObject();
            response.Skill = skill;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in SkillService");

            return response;
        }

        #endregion

        #region GetSkillsBySearchString

        /// <summary>
        /// GetSkillsBySearchString
        /// </summary>
        /// <param name="skillsearchstring"></param>
        /// <returns></returns>
        public async Task<List<SkillSearchResponse>> GetSkillsBySearchString(string skillsearchstring)
        {

            List<Skill> skills = await m_AdminContext.Skills.Where(x => x.SkillName.ToLower().Contains(skillsearchstring.ToLower())).ToListAsync();
            List<SkillSearchResponse> Skills = (from sk in skills
                               join skillGroup in m_AdminContext.SkillGroups on sk.SkillGroupId equals skillGroup.SkillGroupId
                               join CompetencyArea in m_AdminContext.CompetencyAreas on skillGroup.CompetencyAreaId equals CompetencyArea.CompetencyAreaId
                               select new SkillSearchResponse
                               {
                                   SkillId = sk.SkillId,
                                   SkillName = sk.SkillName,
                                   SkillGroupId = sk.SkillGroupId,
                                   SkillGroupName=skillGroup.SkillGroupName,
                                   CompetencyAreaId=sk.CompetencyAreaId,
                                   CompetencyAreaName= CompetencyArea.CompetencyAreaCode
                               }
                             ).ToList();
            return Skills;
        }

        #endregion
    }
}
