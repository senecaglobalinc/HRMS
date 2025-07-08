using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
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
    /// service class to get Reason details
    /// </summary>
    public class ReasonService : IReasonService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<ReasonService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public ReasonService(AdminContext adminContext, ILogger<ReasonService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reason, Reason>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a Reason
        /// </summary>
        /// <param name="Reason"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(Reason reasonIn)
        {
            m_Logger.LogInformation("Calling CreateReason method in ReasonService");

            int isCreated;

            //Checking if  already exists
            var isExists = m_AdminContext.Reasons.Where(p => p.ReasonTypeId == reasonIn.ReasonTypeId &&  p.ReasonCode.ToLower().Trim() == reasonIn.ReasonCode.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Reason already exists");

            Reason reason = new Reason();

            if (!reasonIn.IsActive.HasValue)
                reasonIn.IsActive = true;

            //Add fields
            m_mapper.Map<Reason, Reason>(reasonIn , reason);

            m_Logger.LogInformation("Add Reason to list");
            m_AdminContext.Reasons.Add(reason);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ReasonService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Reason created successfully.");
                return CreateResponse(reason, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No Reason created");
        }
        #endregion
        
        #region GetAll
        /// <summary>
        /// Gets the Reason details
        /// </summary>
        /// <returns></returns>
        public async Task<List<Reason>> GetAll(bool isActive = true) =>
                       await m_AdminContext.Reasons.Where(et => et.IsActive == isActive).OrderBy(x => x.Description).ToListAsync();
        #endregion
        
        #region Update
        /// <summary>
        /// Updates the Reason details
        /// </summary>
        /// <param name="Reason"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(Reason reasonIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling UpdateReason method in ReasonService");

            //Checking if  already exists
            var isExists = m_AdminContext.Reasons.Where(p => p.ReasonTypeId == reasonIn.ReasonTypeId && p.ReasonCode.ToLower().Trim() == reasonIn.ReasonCode.ToLower().Trim()
                                            && p.ReasonId != reasonIn.ReasonId).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Reason already exists");

            //Fetch Reason for update
            Reason reason = m_AdminContext.Reasons.Find(reasonIn.ReasonId);

            if (reason == null)
                return CreateResponse(null, false, "Reason not found for update");

            if (!reasonIn.IsActive.HasValue)
                reason.IsActive = reasonIn.IsActive;
            
            //update fields
            m_mapper.Map<Reason, Reason>(reasonIn, reason);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in ReasonService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating CreateReason record in ReasonService");
                return CreateResponse(reason, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated");
        }
        #endregion

        #region GetReasonsForDropdown
        /// <summary>
        /// Get Reasons For Dropdown
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetReasonsForDropdown() =>
                        await m_AdminContext.Reasons.Where(et => et.IsActive == true)
                              .Select(c => new GenericType()
                                 { Id = c.ReasonId,
                                   Name = c.Description })
                        .OrderBy(x => x.Name)
                        .ToListAsync();

        #endregion

        #region GetVoluntaryExitReasons
        /// <summary>
        /// Get Voluntary Exit Reasons
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetVoluntaryExitReasons() =>
                        await m_AdminContext.Reasons.Where(et => et.IsActive == true && et.ReasonTypeId == 1)
                              .Select(c => new GenericType()
                              {
                                  Id = c.ReasonId,
                                  Name = c.Description
                              })
                        .OrderBy(x => x.Name)
                        .ToListAsync();

        #endregion

        #region GetOtherExitReasons
        /// <summary>
        /// Get Other Exit Reasons
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetOtherExitReasons() =>
                        await m_AdminContext.Reasons.Where(et => et.IsActive == true && et.ReasonTypeId == 2)
                              .Select(c => new GenericType()
                              {
                                  Id = c.ReasonId,
                                  Name = c.Description
                              })
                        .OrderBy(x => x.Name)
                        .ToListAsync();

        #endregion 

        #region GetByReasonId
        /// <summary>
        /// Gets the Reason by Id
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        public async Task<Reason> GetByReasonId(int reasonId) =>
                        await m_AdminContext.Reasons.Where(et => et.ReasonId == reasonId)
                        .FirstOrDefaultAsync();

        #endregion        

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="Reason"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(Reason Reason, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in ReasonService");

            dynamic response = new ExpandoObject();
            response.Reason = Reason;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in ReasonService");

            return response;
        }

        #endregion
    }
}
