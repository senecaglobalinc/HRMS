using HRMS.Admin.Database;
using HRMS.Admin.Infrastructure.Models;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class FinancialYearService : IFinancialYearService

    {
        #region Global Variables

        private readonly ILogger<FinancialYearService> m_Logger;
        private readonly AdminContext m_AdminContext;

        #endregion

        #region Constructor
        public FinancialYearService(ILogger<FinancialYearService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// This method gets the Financial years list.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<FinancialYearModel>> GetAll()
        {
            var response = new ServiceListResponse<FinancialYearModel>();
            try
            {
                var financialYearList = await m_AdminContext.FinancialYears.OrderByDescending(fy => fy.FromYear).ToListAsync();

                var financialYears = (from fy in financialYearList
                                      select new FinancialYearModel
                                      {
                                          Id = fy.Id,
                                          FinancialYearName = Convert.ToString(fy.FromYear) + " - " + Convert.ToString(fy.ToYear),
                                          IsActive = fy.IsActive ?? false
                                      }).ToList();

                response.IsSuccessful = true;
                response.Items = financialYears;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;

                response.Message = "Error occured while fetching financial years";
                m_Logger.LogError("Error occured while fetching financial years" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetByIdAsync
        /// <summary>
        /// Get FinancialYear by id
        /// </summary>
        /// <param name="financialYearId">FinancialYear Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<FinancialYearModel>> GetByIdAsync(int financialYearId)
        {
            var response = new ServiceResponse<FinancialYearModel>();
            try
            {

                var financialYear = await (from fy in m_AdminContext.FinancialYears
                                           where fy.Id == financialYearId
                                           select new FinancialYearModel
                                           {
                                               Id = fy.Id,
                                               FinancialYearName = Convert.ToString(fy.FromYear) + " - " + Convert.ToString(fy.ToYear),
                                               IsActive = fy.IsActive ?? false
                                           }).FirstOrDefaultAsync();

                response.IsSuccessful = true;
                response.Item = financialYear;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;

                response.Message = "Error occured while fetching financial years";
                m_Logger.LogError("Error occured while fetching financial years" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetCurrentFinancialYear
        /// <summary>
        /// This method gets the current financial year.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<FinancialYearModel>> GetCurrentFinancialYear()
        {
            var response = new ServiceResponse<FinancialYearModel>();
            try
            {
                var financialYears = await (from fy in m_AdminContext.FinancialYears
                                            where fy.IsActive == true
                                            select new FinancialYearModel
                                            {
                                                Id = fy.Id,
                                                FinancialYearName = Convert.ToString(fy.FromYear) + " - " + Convert.ToString(fy.ToYear),
                                                IsActive = fy.IsActive ?? false
                                            }).FirstOrDefaultAsync();

                response.IsSuccessful = true;
                response.Item = financialYears;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;

                response.Message = "Error occured while fetching financial years";
                m_Logger.LogError("Error occured while fetching financial years" + ex.StackTrace);
            }
            return response;
        }
        #endregion
    }
}
