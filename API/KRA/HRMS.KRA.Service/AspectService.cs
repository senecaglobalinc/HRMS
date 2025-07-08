using AutoMapper;
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
    public class AspectService : IAspectService
    {
        #region Global Variables 

        private readonly ILogger<AspectService> m_Logger;
        private readonly KRAContext m_KRAContext;
        private readonly IMapper m_Mapper;

        #endregion

        #region Constructor
        public AspectService(ILogger<AspectService> logger, KRAContext kraContext)
        {
            m_Logger = logger;
            m_KRAContext = kraContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Aspect, Aspect>();
            });

            m_Mapper = config.CreateMapper();
        }
        #endregion

        #region GetAll
        /// <summary>
        /// This method fetches all aspect records.
        /// </summary>
        /// <returns>List<Aspect></returns>
        public async Task<List<AspectModel>> GetAllAsync()
        {
            m_Logger.LogInformation("AspectService: Calling \"GetAll\" method.");

            return await (from am in m_KRAContext.Aspects
                          select new AspectModel
                          {
                              AspectId = am.AspectId,
                              AspectName = am.AspectName
                          }).AsNoTracking().ToListAsync();

        }
        #endregion

        #region Create
        /// <summary>
        /// This method creates an aspect record. 
        /// </summary>
        /// <param name="aspectName">Aspect name information</param>
        /// <returns>dynamic</returns>
        public async Task<(bool, string)> CreateAsync(string aspectName)
        {

            if (string.IsNullOrEmpty(aspectName))
            {
                return (false, "Aspect name is required field.");
            }

            m_Logger.LogInformation("AspectService: Calling \"Create\" method."); 
            var isExist = m_KRAContext.Aspects.Any(x => x.AspectName == aspectName);

            if (isExist)
            {
                return (false, "Aspect name already exists.");
            }

            m_KRAContext.Aspects.Add(new Aspect()
            {
                AspectName = aspectName,
                IsActive = true
            });

            m_Logger.LogInformation("AspectService: Calling SaveChangesAsync method on DBContext.");
            
            var isCreated = await m_KRAContext.SaveChangesAsync() > 0;
            return (isCreated, isCreated ? "Record created Successfully" : "No aspect created.");
        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates aspect.
        /// </summary>
        /// <param name="model">aspect information</param>
        /// <returns>dynamic</returns>
        public async Task<(bool, string)> UpdateAsync(AspectModel model)
        {
            m_Logger.LogInformation("AspectService: Calling \"Update\" method.");

            var aspect = m_KRAContext.Aspects.Find(model.AspectId);

            if (aspect is null)
            {
                return (false, "Aspect not found for update.");
            }

            var isExist = m_KRAContext.Aspects.SingleOrDefault(x => x.AspectName == model.AspectName);

            if (isExist is { } && aspect.AspectId != isExist.AspectId)
            {
                return (false, "Aspect name already exists.");
            }

            aspect.AspectName = model.AspectName;
            var isUpdated = await m_KRAContext.SaveChangesAsync() > 0;
            
            return (isUpdated, isUpdated ? "Record Updated Successfully" : "No record updated.");
        }

        #endregion

        #region Delete
        /// <summary>
        /// This method deletes aspect record.
        /// </summary>
        /// <param name="aspectId"></param>
        /// <returns>dynamic</returns>
        public async Task<(bool, string)> DeleteAsync(int aspectId)
        {

            m_Logger.LogInformation("AspectService: Calling \"Delete\" method");
            var aspect = m_KRAContext.Aspects.Find(aspectId);

            if (aspect is null)
            {
                return (false, "Aspect record not found for delete.");
            }

            var isAspectReferenced = m_KRAContext.Definitions.Any(x => x.AspectId == aspectId);

            if (isAspectReferenced)
            {
                return (false, "Aspect could not be deleted, as it is referenced in KRA Definition/s.");
            }

            m_Logger.LogInformation("AspectService: Calling SaveChanges method on DB Context.");
            m_KRAContext.Aspects.Remove(aspect);
            var isUpdated = await m_KRAContext.SaveChangesAsync() > 0;

            return (isUpdated, isUpdated ? "Record deleted" : "Not deleted any record.");
        }
        #endregion
    }
}
