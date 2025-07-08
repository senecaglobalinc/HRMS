using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class StatusService : IStatusService
    {
        #region Global Variables

        private readonly ILogger<StatusService> m_Logger;
        private readonly AdminContext m_AdminContext;
       

        #endregion

        #region UserGradeService
        public StatusService(ILogger<StatusService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the designation details
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<Status>> GetAll(bool? isActive = true) 
        {

            return await (from st in m_AdminContext.Statuses
                          join ct in m_AdminContext.Categories on st.CategoryMasterId equals ct.CategoryMasterId
                          where st.IsActive == true && ct.IsActive == true
                          select new Status
                          {
                              StatusId = st.StatusId,
                              StatusCode = st.StatusCode,
                              StatusDescription = st.StatusDescription,
                              IsActive = st.IsActive,
                              CategoryMasterId = ct.CategoryMasterId,
                              CategoryMaster = ct
                          }).OrderBy(x => x.StatusCode).ToListAsync();
        }
        #endregion

        #region GetStatusById
        /// <summary>
        /// GetStatusById
        /// </summary>
        /// <param name="statusId">statusCode</param>
        public async Task<Status> GetStatusById(int statusId) =>
                        await m_AdminContext.Statuses.Where(st => st.StatusId == statusId)
                        .FirstOrDefaultAsync();

        #endregion

        #region GetStatusByCode
        /// <summary>
        /// GetStatusByCode
        /// </summary>
        /// <param name="statusCode">statusCode</param>
        public async Task<Status> GetStatusByCode(string statusCode) =>
                        await m_AdminContext.Statuses.Where(st => st.StatusCode == statusCode)
                        .FirstOrDefaultAsync();

        #endregion

        #region GetByCategoryAndStatusCode
        /// <summary>
        /// GetByCategoryAndStatusCode
        /// </summary>
        /// <param name="category"></param>
        /// <param name="statusCode"></param>
        /// <returns>int</returns>
        public async Task<Status> GetByCategoryAndStatusCode(string category, string statusCode)
        {
            return await (from Status in m_AdminContext.Statuses
                          join Category in m_AdminContext.Categories on Status.CategoryMasterId equals Category.CategoryMasterId
                          where Status.StatusCode.ToLower().Trim() == statusCode.ToLower().Trim()
                                && Category.CategoryName.ToLower().Trim() == category.ToLower().Trim()
                                && Status.IsActive == true
                          select new Status
                          {
                              StatusId = Status.StatusId,
                              StatusCode = Status.StatusCode,
                              StatusDescription = Status.StatusDescription
                          }).FirstOrDefaultAsync();
        }
        #endregion

		#region GetByCategory
        /// <summary>
        /// GetByCategoryAndStatusCode
        /// </summary>
        /// <param name="category"></param>
        /// <param name="statusCode"></param>
        /// <returns>int</returns>
        public async Task<List<Status>> GetByCategory(string category)
        {
            return await (from Status in m_AdminContext.Statuses
                          join Category in m_AdminContext.Categories on Status.CategoryMasterId equals Category.CategoryMasterId
                          where Category.CategoryName.ToLower().Trim() == category.ToLower().Trim()
                                && Status.IsActive == true
                          select new Status
                          {
                              StatusId = Status.StatusId,
                              StatusCode = Status.StatusCode,
                              StatusDescription = Status.StatusDescription
                          }).ToListAsync();
        }
        #endregion

        #region GetByCategoryIdAndStatusCode
        /// <summary>
        /// GetByCategoryIdAndStatusCode
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="statusCode"></param>
        /// <returns>int</returns>
        public async Task<Status> GetByCategoryIdAndStatusCode(int categoryId, string statusCode)
        {
            return await (from Status in m_AdminContext.Statuses
                          join Category in m_AdminContext.Categories on Status.CategoryMasterId equals Category.CategoryMasterId
                          where Status.StatusCode.ToLower().Trim() == statusCode.ToLower().Trim()
                                && Status.CategoryMasterId == categoryId && Status.IsActive == true
                          select new Status
                          {
                              StatusId = Status.StatusId,
                              StatusCode = Status.StatusCode,
                              StatusDescription = Status.StatusDescription
                          }).FirstOrDefaultAsync();
        }
        #endregion

        #region GetProjectStatuses
        /// <summary>
        /// Gets the Project Statuses
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<Status>> GetProjectStatuses()
        {
            List<Status> response;
            var category = await m_AdminContext.Categories
                                        .Where(q => q.CategoryName.ToLower() == "ppc").FirstOrDefaultAsync();
            if (category == null) {
                response = new List<Status>();
                response = null;
                return response;
            }
            var statuses = await (from st in m_AdminContext.Statuses
                          where st.CategoryMasterId == category.CategoryMasterId
                          select new Status
                          {
                              StatusId = st.StatusId,
                              StatusCode = st.StatusCode,
                              StatusDescription = st.StatusDescription,
                              IsActive = st.IsActive,
                              CategoryMasterId = category.CategoryMasterId
                          }).OrderBy(x=>x.StatusCode).ToListAsync();
            if (statuses.Count == 0)
            {
                response = new List<Status>();
                response = null;
                return response;
            }
            else {
                response = new List<Status>();
                response = statuses;
                return response;
            }
        }
        #endregion

        #region GetstatusMasterDetails
        /// <summary>
        /// Gets Status Master details
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<StatusMaster>> GetStatusMasterDetails(bool? isActive = true)
        {
            List<StatusMaster> response;
            var statusList = await (from st in m_AdminContext.Statuses
                          join ct in m_AdminContext.Categories on st.CategoryMasterId equals ct.CategoryMasterId
                          where st.IsActive == true && ct.IsActive == true
                          select new StatusMaster
                          {
                              StatusCode = st.StatusCode,
                              StatusDescription = st.StatusDescription,
                              IsActive = st.IsActive.HasValue == true ? "Yes" : "No",
                              StatusId = st.StatusId,
                              CategoryID = st.CategoryMasterId,
                              CategoryName = ct.CategoryName
                          }).OrderBy(s => s.StatusCode).ToListAsync();
            if (statusList.Count == 0) {
                response = null;
                return response;
            }
            if (isActive.HasValue) {
                statusList = statusList.Where(i => i.IsActive == "Yes").ToList();
            }
            response = new List<StatusMaster>();
            if (statusList.Count > 0)
            {
                response = statusList;
                return response;
            }

            else
                return response;

        }
        #endregion
    }
}
