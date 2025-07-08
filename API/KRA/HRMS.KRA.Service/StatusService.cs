using HRMS.KRA.Database;
using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.KRA.Service
{
    public class StatusService : IStatusService
    {
        #region Global Varibles
        private readonly ILogger<StatusService> m_Logger;
        private readonly KRAContext m_KRAContext;
        #endregion

        #region StatusService
        public StatusService(ILogger<StatusService> logger, KRAContext kraContext)
        {
            m_Logger = logger;
            m_KRAContext = kraContext;
        }

        #endregion

        #region Get
        /// <summary>
        /// Gets Status
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatusModel>> GetAllAsync()
        {
            m_Logger.LogInformation("StatusService: Calling \"GetAll\" method.");

            return await (from sm in m_KRAContext.Statuses
                          select new StatusModel
                          {
                              StatusId = sm.StatusId,
                              StatusText = sm.StatusText,
                              StatusDescription = sm.StatusDescription,
                          }).AsNoTracking().ToListAsync();
        }
        #endregion

        #region CreateAsync
        /// <summary>
        /// Create New Status
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> CreateAsync(StatusModel model)
        {
            m_Logger.LogInformation("StatusService: Calling \"Create\" method.");
           
            var isExist = m_KRAContext.Statuses.Any(x => x.StatusText == model.StatusText);

            if (isExist || string.IsNullOrEmpty(model.StatusText))
            {
                return (false, "Duplicate or Null scale title.");
            }

            var status = new Status()
            {
                StatusText = model.StatusText,
                StatusDescription = model.StatusDescription,
            };

            m_KRAContext.Statuses.Add(status);
            
            var isCreated = await m_KRAContext.SaveChangesAsync() > 0;
           
            return (isCreated, "Record's created successfully!.");
        }

        #endregion
       
    }
}
