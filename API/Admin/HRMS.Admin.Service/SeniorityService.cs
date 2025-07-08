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
    public class SeniorityService : ISeniorityService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<SeniorityService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public SeniorityService(AdminContext adminContext, ILogger<SeniorityService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SGRolePrefix, SGRolePrefix>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a Seniority
        /// </summary>
        /// <param name="seniorityIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(SGRolePrefix seniorityIn)
        {
            m_Logger.LogInformation("Calling Create Seniority method in SeniorityService");

            //Checking if  already exists
            var isExists = m_AdminContext.SGRolePrefixes.Where(p => p.PrefixName.ToLower().Trim() == seniorityIn.PrefixName.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Seniority Name already exists");

            int isCreated;

            SGRolePrefix seniority = new SGRolePrefix();

            if (!seniorityIn.IsActive.HasValue)
                seniorityIn.IsActive = true;

            //Add fields
            m_mapper.Map<SGRolePrefix, SGRolePrefix>(seniorityIn, seniority);

            m_Logger.LogInformation("Add seniority to list");
            m_AdminContext.SGRolePrefixes.Add(seniority);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in SeniorityService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Seniority created successfully.");
                return CreateResponse(seniority, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No seniority created");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the Domain details
        /// </summary>
        /// <returns></returns>
        public async Task<List<SGRolePrefix>> GetAll(bool isActive = true) =>
                       await m_AdminContext.SGRolePrefixes.Where(sp => sp.IsActive == isActive).OrderBy(x => x.PrefixName).ToListAsync();
        #endregion

        #region Update
        /// <summary>
        /// Updates the Seniority details
        /// </summary>
        /// <param name="seniorityIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(SGRolePrefix seniorityIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling Update seniority method in SeniorityService");

            //Checking if  already exists
            var isExists = m_AdminContext.SGRolePrefixes.Where(p => p.PrefixName.ToLower().Trim() == seniorityIn.PrefixName.ToLower().Trim()
                                            && p.PrefixID != seniorityIn.PrefixID).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Seniority Name already exists");

            //Fetch seniority for update
            var seniority = m_AdminContext.SGRolePrefixes.Find(seniorityIn.PrefixID);

            if (seniority == null)
                return CreateResponse(null, false, "Seniority not found for update");

            if (!seniorityIn.IsActive.HasValue)
                seniorityIn.IsActive = seniority.IsActive;

            seniorityIn.CreatedBy = seniority.CreatedBy;
            seniorityIn.CreatedDate = seniority.CreatedDate;

            //update fields
            m_mapper.Map<SGRolePrefix, SGRolePrefix>(seniorityIn, seniority);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in SeniorityService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating seniority record in SeniorityService");
                return CreateResponse(seniority, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated");
        }
        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="seniority"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(SGRolePrefix seniority, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in SeniorityService");

            dynamic response = new ExpandoObject();
            response.Seniority = seniority;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in SeniorityService");

            return response;
        }

        #endregion
    }
}

