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
    public class KeyFunctionService : IKeyFunctionService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<KeyFunctionService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public KeyFunctionService(AdminContext adminContext, ILogger<KeyFunctionService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SGRole, SGRole>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a KeyFunction
        /// </summary>
        /// <param name="keyFunctionIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(SGRole keyFunctionIn)
        {
            m_Logger.LogInformation("Calling Create keyFunctionIn method in KeyFunctionService");

            //Checking if Key Function already exists
            var isExists = m_AdminContext.SGRoles.Where(p => p.SGRoleName.ToLower().Trim() == keyFunctionIn.SGRoleName.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Key Function Name already exists");

            int isCreated;

            SGRole keyFunction = new SGRole();

            if (!keyFunctionIn.IsActive.HasValue)
                keyFunctionIn.IsActive = true;

            //Add fields
            m_mapper.Map<SGRole, SGRole>(keyFunctionIn, keyFunction);

            m_Logger.LogInformation("Add keyFunction to list");
            m_AdminContext.SGRoles.Add(keyFunction);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in KeyFunctionService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("KeyFunction created successfully.");
                return CreateResponse(keyFunction, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No keyFunction created");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the Domain details
        /// </summary>
        /// <returns></returns>
        public async Task<List<SGRole>> GetAll(bool isActive = true) =>
                       await m_AdminContext.SGRoles
                                            .Include(r => r.Department)
                                            .Where(sp => sp.IsActive == isActive).OrderBy(x => x.SGRoleName).ToListAsync();
        #endregion

        #region Update
        /// <summary>
        /// Updates the KeyFunction details
        /// </summary>
        /// <param name="keyFunctionIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(SGRole keyFunctionIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling Update keyFunction method in KeyFunctionService");

            //Checking if Key Function already exists
            var isExists = m_AdminContext.SGRoles.Where(p => p.SGRoleName.ToLower().Trim() == keyFunctionIn.SGRoleName.ToLower().Trim()
                                && p.SGRoleID != keyFunctionIn.SGRoleID).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Key Function Name already exists");

            //Fetch keyFunction for update
            var keyFunction = m_AdminContext.SGRoles.Find(keyFunctionIn.SGRoleID);

            if (keyFunction == null)
                return CreateResponse(null, false, "KeyFunction not found for update");

            if (!keyFunctionIn.IsActive.HasValue)
                keyFunctionIn.IsActive = keyFunction.IsActive;

            keyFunctionIn.CreatedBy = keyFunction.CreatedBy;
            keyFunctionIn.CreatedDate = keyFunction.CreatedDate;

            //update fields
            m_mapper.Map<SGRole, SGRole>(keyFunctionIn, keyFunction);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in KeyFunctionService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating keyFunction record in KeyFunctionService");
                return CreateResponse(keyFunction, true, string.Empty);
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
        /// <param name="keyFunction"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(SGRole keyFunction, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in KeyFunctionService");

            dynamic response = new ExpandoObject();
            response.KeyFunction = keyFunction;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in KeyFunctionService");

            return response;
        }

        #endregion
    }
}


