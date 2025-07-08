using HRMS.KRA.Database;
using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.KRA.Service
{
    public class MeasurementTypeService : IMeasurementTypeService
    {
        #region Global Varibles
        private readonly ILogger<MeasurementTypeService> m_Logger;
        private readonly KRAContext m_KRAContext;
        #endregion

        #region MeasurementTypeService
        public MeasurementTypeService(ILogger<MeasurementTypeService> logger, KRAContext kraContext)
        {
            m_Logger = logger;
            m_KRAContext = kraContext;
        }
        #endregion

        #region Get
        /// <summary>
        /// Gets KRA Measurement Type
        /// </summary>
        /// <returns></returns>
        public async Task<List<MeasurementTypeModel>> GetAllAsync()
        {
            m_Logger.LogInformation("MeasurementTypeService: Calling \"GetAll\" method.");

            return await (from mt in m_KRAContext.MeasurementTypes
                          select new MeasurementTypeModel
                          {
                              Id = mt.MeasurementTypeId,
                              MeasurementType = mt.MeasurementTypeName 
                          }).AsNoTracking().ToListAsync();
        }
        #endregion

        #region Create
        /// <summary>
        /// Create New Measurement Type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<(bool, string)> CreateAsync(string name)
        {
            try
            {
                m_Logger.LogInformation("MeasurementTypeService: Calling \"Create\" method.");
                var isExist = m_KRAContext.MeasurementTypes.Any(x => x.MeasurementTypeName == name);

                if (isExist)
                {
                    return (false, "Record already exist.");
                }

                m_KRAContext.MeasurementTypes.Add(new MeasurementType() { MeasurementTypeName = name });
                var isCreated = await m_KRAContext.SaveChangesAsync() > 0;

                return (isCreated, isCreated ? "Record's created successfully!." : "Failed to insert record.");
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in DefinitionDetails " + ex.StackTrace);
                return (false, $"Failed to insert record. { ex?.Message}");
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update KRA Measurement Type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateAsync(MeasurementTypeModel model)
        {
            m_Logger.LogInformation("MeasurementTypeService: Calling \"Update\" method.");
            var measurementType = m_KRAContext.MeasurementTypes.Find(model.Id);

            if (measurementType is null)
            {
                return (false, "Measurement Type not found for update.");
            }

            //Fetch DefinitionDetails table
            //var isExists = m_KRAContext.DefinitionDetails.Any(x => x.MeasurementTypeId == (model.Id));

            //if (isExists)
            //    return (isUpdated, "Can't update it is being mapped to DefinitionDetails table");

            m_Logger.LogInformation("MeasurementTypeService: Measurement Type exists?");

            var isExist = m_KRAContext.MeasurementTypes.SingleOrDefault(x => x.MeasurementTypeName == model.MeasurementType);

            if (isExist is { } && measurementType.MeasurementTypeId != isExist.MeasurementTypeId)
            {
                return (false, "Measurement Type already exists.");
            }

            measurementType.MeasurementTypeName = model.MeasurementType;
            var isUpdated = await m_KRAContext.SaveChangesAsync() > 0;
            
            return (isUpdated, isUpdated ? "Updated successfully" : "Update failed");
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete KRA Measurement Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteAsync(int id)
        {
            m_Logger.LogInformation("MeasurementTypeService: Calling \"Delete\" method");

            var measurementType = m_KRAContext.MeasurementTypes.Find(id);
            
            if (measurementType is null)
            {
                return (false, "Measurement Type not found for delete.");
            }

            //Fetch DefinitionDetails table
            var isMeasurementTypeReferenced = m_KRAContext.DefinitionDetails.Any(x => x.MeasurementTypeId == id);

            if (isMeasurementTypeReferenced)
            {
                return (false, "Can't delete it, as it is being mapped to DefinitionDetails table");
            }

            m_Logger.LogInformation("MeasurementTypeService: Calling SaveChanges method on DB Context.");
            m_KRAContext.MeasurementTypes.Remove(measurementType);

            var isdeleted = await m_KRAContext.SaveChangesAsync() > 0;
            return (isdeleted, isdeleted ? "Record deleted" : "Not deleted any record.");
        }
        #endregion
    }
}
