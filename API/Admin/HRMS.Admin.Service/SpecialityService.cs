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
    /// service class to get Domain details
    /// </summary>
    public class SpecialityService : ISpecialityService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<SpecialityService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public SpecialityService(AdminContext adminContext, ILogger<SpecialityService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SGRoleSuffix, SGRoleSuffix>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a Speciality
        /// </summary>
        /// <param name="specialityIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(SGRoleSuffix specialityIn)
        {
            m_Logger.LogInformation("Calling CreateSpeciality method in SpecialityService");

            //Checking if  already exists
            var isExists = m_AdminContext.SGRoleSuffixes.Where(p => p.SuffixName.ToLower().Trim() == specialityIn.SuffixName.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Speciality Name already exists");

            int isCreated;

            SGRoleSuffix speciality = new SGRoleSuffix();

            if (!specialityIn.IsActive.HasValue)
                specialityIn.IsActive = true;

            //Add fields
            m_mapper.Map<SGRoleSuffix, SGRoleSuffix>(specialityIn, speciality);

            m_Logger.LogInformation("Add speciality to list");
            m_AdminContext.SGRoleSuffixes.Add(speciality);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in SpecialityService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Speciality created successfully.");
                return CreateResponse(speciality, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No speciality created");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the Domain details
        /// </summary>
        /// <returns></returns>
        public async Task<List<SGRoleSuffix>> GetAll(bool isActive = true) =>
                       await m_AdminContext.SGRoleSuffixes.Where(ss => ss.IsActive == isActive).OrderBy(x => x.SuffixName).ToListAsync();
        #endregion

        #region Update
        /// <summary>
        /// Updates the Speciality details
        /// </summary>
        /// <param name="specialityIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(SGRoleSuffix specialityIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling Update speciality method in SpecialityService");

            //Checking if  already exists
            var isExists = m_AdminContext.SGRoleSuffixes.Where(p => p.SuffixName.ToLower().Trim() == specialityIn.SuffixName.ToLower().Trim()
                                                   && p.SuffixID != specialityIn.SuffixID).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Speciality Name already exists");

            //Fetch speciality for update
            var speciality = m_AdminContext.SGRoleSuffixes.Find(specialityIn.SuffixID);

            if (speciality == null)
                return CreateResponse(null, false, "Speciality not found for update");

            if (!specialityIn.IsActive.HasValue)
                specialityIn.IsActive = speciality.IsActive;

            specialityIn.CreatedBy = speciality.CreatedBy;
            specialityIn.CreatedDate = speciality.CreatedDate;

            //update fields
            m_mapper.Map<SGRoleSuffix, SGRoleSuffix>(specialityIn, speciality);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in SpecialityService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating speciality record in SpecialityService");
                return CreateResponse(speciality, true, string.Empty);
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
        /// <param name="speciality"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(SGRoleSuffix speciality, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in SpecialityService");

            dynamic response = new ExpandoObject();
            response.Speciality = speciality;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in SpecialityService");

            return response;
        }

        #endregion
    }
}
