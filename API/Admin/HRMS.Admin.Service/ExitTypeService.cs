using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
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
    /// service class to get ExitType details
    /// </summary>
    public class ExitTypeService : IExitTypeService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<ExitTypeService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public ExitTypeService(AdminContext adminContext, ILogger<ExitTypeService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ExitType, ExitType>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a ExitType
        /// </summary>
        /// <param name="exitTypeIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(ExitType exitTypeIn)
        {
            m_Logger.LogInformation("Calling CreateExitType method in ExitTypeService");

            int isCreated;

            //Checking if  already exists
            var isExists = m_AdminContext.ExitTypes.Where(p => p.Description.ToLower().Trim() == exitTypeIn.Description.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "ExitType already exists");

            ExitType exitType = new ExitType();

            if (!exitTypeIn.IsActive.HasValue)
                exitTypeIn.IsActive = true;

            //Add fields
            m_mapper.Map<ExitType, ExitType>(exitTypeIn , exitType);

            m_Logger.LogInformation("Add ExitType to list");
            m_AdminContext.ExitTypes.Add(exitType);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ExitTypeService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("ExitType created successfully.");
                return CreateResponse(exitType, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No ExitType created");
        }
        #endregion
        
        #region GetAll
        /// <summary>
        /// Gets the ExitType details
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExitType>> GetAll(bool isActive = true) =>
                       await m_AdminContext.ExitTypes.Where(et => et.IsActive == isActive).OrderBy(x => x.Description).ToListAsync();
        #endregion
        
        #region Update
        /// <summary>
        /// Updates the ExitType details
        /// </summary>
        /// <param name="ExitType"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(ExitType exitTypeIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling UpdateExitType method in ExitTypeService");

            //Checking if  already exists
            var isExists = m_AdminContext.ExitTypes.Where(p => p.Description.ToLower().Trim() == exitTypeIn.Description.ToLower().Trim()
                                            && p.ExitTypeId != exitTypeIn.ExitTypeId).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "ExitType Name already exists");

            //Fetch ExitType for update
            ExitType exitType = m_AdminContext.ExitTypes.Find(exitTypeIn.ExitTypeId);

            if (exitType == null)
                return CreateResponse(null, false, "ExitType not found for update");

            if (!exitTypeIn.IsActive.HasValue)
                exitType.IsActive = exitTypeIn.IsActive;
            
            //update fields
            m_mapper.Map<ExitType, ExitType>(exitTypeIn, exitType);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in ExitTypeService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating CreateExitType record in ExitTypeService");
                return CreateResponse(exitType, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated");
        }
        #endregion

        #region GetExitTypesForDropdown
        /// <summary>
        /// Get ExitTypes For Dropdown
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetExitTypesForDropdown() =>
                        await m_AdminContext.ExitTypes.Where(et => et.IsActive == true)
                              .Select(c => new GenericType()
                                 { Id = c.ExitTypeId,
                                   Name = c.Description })
                        .OrderBy(x => x.Name)
                        .ToListAsync();

        #endregion        

        #region GetByExitTypeId
        /// <summary>
        /// Gets the ExitType by Id
        /// </summary>
        /// <param name="exitTypeId"></param>
        /// <returns></returns>
        public async Task<ExitType> GetByExitTypeId(int exitTypeId) =>
                        await m_AdminContext.ExitTypes.Where(et => et.ExitTypeId == exitTypeId)
                        .FirstOrDefaultAsync();

        #endregion        

        #region GetExitTypeIdByName
        /// <summary>
        /// GetExitTypeIdByName
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> GetExitTypeIdByName(string exitTypeName)
        {
            var response = new ServiceResponse<int>();
            try
            {
                var exitType = await m_AdminContext.ExitTypes
                                        .Where(et => et.IsActive == true
                                      && et.Description.ToLower().Contains(exitTypeName.ToLower()))
                                        .Select(et => et.ExitTypeId)
                                        .FirstOrDefaultAsync();

                response.IsSuccessful = true;
                response.Item = exitType;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching exit type";
                m_Logger.LogError("Error occured while fetching GetExitTypeIdByName() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="ExitType"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(ExitType ExitType, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in ExitTypeService");

            dynamic response = new ExpandoObject();
            response.ExitType = ExitType;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in ExitTypeService");

            return response;
        }

        #endregion
    }
}
