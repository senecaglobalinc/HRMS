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
    public class ScaleService : IScaleService
    {
        #region Global Varibles
        private readonly ILogger<ScaleService> m_Logger;
        private readonly KRAContext m_KRAContext;
        #endregion

        #region ScaleService
        public ScaleService(ILogger<ScaleService> logger, KRAContext kraContext)
        {
            m_Logger = logger;
            m_KRAContext = kraContext;
        }

        #endregion

        #region Get
        /// <summary>
        /// Gets KRA ScaleMaster Master
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScaleModel>> GetAllAsync()
        {
            m_Logger.LogInformation("ScaleService: Calling \"GetAll\" method.");

            return await (from sm in m_KRAContext.Scales
                          select new ScaleModel
                          {
                              ScaleID = sm.ScaleId,
                              MinimumScale = sm.MinimumScale,
                              MaximumScale = sm.MaximumScale,
                              //ScaleLevel = sm.MinimumScale + " - " + sm.MaximumScale,
                              ScaleTitle = sm.ScaleTitle
                          }).AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetAll Scale Details
        /// <summary>
        /// This method fetches all Scale Details List.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScaleDetailsModel>> GetScaleDetailsAsync()
        {
            m_Logger.LogInformation("KRAService: Calling \"GetScaleDetails\" method.");

            return await (from sd in m_KRAContext.ScaleDetails
                          select new ScaleDetailsModel
                          {
                              ScaleDetailId = sd.ScaleDetailId,
                              ScaleValue = sd.ScaleValue,
                              ScaleID = sd.ScaleId,
                              ScaleDescription = sd.ScaleDescription
                          }).AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetScaleDetailsById
        /// <summary>
        /// Get KRAScale Details By MasterID
        /// </summary>
        /// <param name="ScaleID"></param>
        /// <returns></returns>
        public async Task<List<ScaleDetailsModel>> GetScaleDetailsByIdAsync(int ScaleID)
        {
            m_Logger.LogInformation("ScaleService: Calling \"GetScaleDetailsById\" method.");

            return await (from dm in m_KRAContext.ScaleDetails
                          where dm.ScaleId == ScaleID
                          select new ScaleDetailsModel
                          {
                              ScaleDetailId = dm.ScaleDetailId,
                              ScaleValue = dm.ScaleValue,
                              ScaleDescription = dm.ScaleDescription
                          }).ToListAsync();
        }
        #endregion

        #region Create
        /// <summary>
        /// Create New ScaleMaster and Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> CreateAsync(ScaleModel model)
        {
            m_Logger.LogInformation("ScaleService: Calling \"Create\" method.");
            var DistinctItems = model.ScaleDetails.GroupBy(scale_inner => scale_inner.ScaleDescription.ToLower().Trim()).Select(description => description.First());

            if (DistinctItems.Count() != model.MaximumScale)
            {
                return (false, "Duplicate scale description.");
            }

            var isExist = m_KRAContext.Scales.Any(x => x.ScaleTitle == model.ScaleTitle);

            if (isExist || string.IsNullOrEmpty(model.ScaleTitle))
            {
                return (false, "Duplicate or Null scale title.");
            }

            var scale = new Scale()
            {
                MinimumScale = model.MinimumScale,
                MaximumScale = model.MaximumScale,
                ScaleTitle = model.ScaleTitle
            };

            m_KRAContext.Scales.Add(scale);

            var isCreated = await m_KRAContext.SaveChangesAsync() > 0;

            if (isCreated)
            {
                List<ScaleDetails> list = new List<ScaleDetails>();
                model.ScaleDetails.ForEach(x =>
                {
                    ScaleDetails ScaleDetails = new ScaleDetails();
                    ScaleDetails.ScaleId = scale.ScaleId;
                    ScaleDetails.ScaleValue = x.ScaleValue;
                    ScaleDetails.ScaleDescription = x.ScaleDescription;
                    list.Add(ScaleDetails);
                });
                m_KRAContext.ScaleDetails.AddRange(list);

                isCreated = await m_KRAContext.SaveChangesAsync() > 0;
            }
            return (isCreated, "Record's created successfully!.");
        }
        #endregion

        #region Update
        /// <summary>
        /// Update a ScaleMaster
        /// </summary>
        /// <param name="ScaleMasterIn"></param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateAsync(ScaleModel model)
        {
            m_Logger.LogInformation("Update method calling ");

            var isDetailsNull = model.ScaleDetails.Any(x => string.IsNullOrEmpty(x.ScaleDescription.Trim()));
            
            if (isDetailsNull)
            {
                return (false, "Mandatory fields cannot be empty");
            }

            var distinctItems = model.ScaleDetails.GroupBy(scale => scale.ScaleDescription.ToLower().Trim()).Select(description => description.First());

            if (distinctItems.Count() != model.MaximumScale)
            {
                return (false, "duplicate scale description.");
            }

            var ScaleDetailsIds = model.ScaleDetails.Select(x => x.ScaleDetailId).Distinct().ToList();

            m_KRAContext.ScaleDetails.Where(x => ScaleDetailsIds.Contains(x.ScaleDetailId)).ToList().ForEach(x =>
                x.ScaleDescription = model.ScaleDetails.Find(k => k.ScaleDetailId == x.ScaleDetailId).ScaleDescription);

            bool isUpdated = await m_KRAContext.SaveChangesAsync() > 0;

            return (isUpdated, isUpdated ? "Updated successfully" : "Update failed");
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete a ScaleMaster
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteAsync(int id)
        {
            m_Logger.LogInformation("ScaleService: Calling \"Delete\" method");

            //Fetch scale master for delete
            var scale = m_KRAContext.Scales.Find(id);

            if (scale == null)
            {
                return (false, "Scale not found for delete.");
            }

            var isScaleReferenced = m_KRAContext.DefinitionDetails.Any(x => x.ScaleId == id);

            if (isScaleReferenced)
            {
                return (false, "Scale could not be deleted, as it is referenced in KRA Definition/s.");
            }

            var scaledetails = m_KRAContext.ScaleDetails.Where(x => x.Scale.ScaleId == id).ToList();

            m_Logger.LogInformation("ScaleService: Calling Remove method on DB Context.");

            if (scaledetails is { })
            {
                m_KRAContext.ScaleDetails.RemoveRange(scaledetails);
            }

            m_KRAContext.Scales.Remove(scale);
            var isDeleted = await m_KRAContext.SaveChangesAsync() > 0;
            
            return (isDeleted, isDeleted ? "Record deleted" : "Not deleted any record.");
        }
        #endregion
    }
}
