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
    /// service class to get Holiday details
    /// </summary>
    public class HolidayService : IHolidayService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<HolidayService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public HolidayService(AdminContext adminContext, ILogger<HolidayService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Holiday, Holiday>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a Holiday
        /// </summary>
        /// <param name="holidayIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(Holiday holidayIn)
        {
            m_Logger.LogInformation("Calling CreateHoliday method in HolidayService");

            int isCreated;

            //Checking if  already exists
            var isExists = m_AdminContext.Holidays.Where(p => p.HolidayDate == holidayIn.HolidayDate).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Holiday already exists");

            Holiday holiday = new Holiday();

           
            //Add fields
            m_mapper.Map<Holiday, Holiday>(holidayIn, holiday);

            m_Logger.LogInformation("Add Holiday to list");
            m_AdminContext.Holidays.Add(holiday);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in HolidayService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Holiday created successfully.");
                return CreateResponse(holiday, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No Holiday created");
        }
        #endregion
        
        #region GetAll
        /// <summary>
        /// Gets the Holiday details
        /// </summary>
        /// <returns></returns>
        public async Task<List<Holiday>> GetAll() =>
                       await m_AdminContext.Holidays.OrderBy(x => x.HolidayDate).ToListAsync();
        #endregion
        
        #region Update
        /// <summary>
        /// Updates the Holiday details
        /// </summary>
        /// <param name="Holiday"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(Holiday holidayIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling UpdateHoliday method in HolidayService");

            //Checking if  already exists
            var isExists = m_AdminContext.Holidays.Where(p => p.HolidayDate == holidayIn.HolidayDate
                                            && p.Id != holidayIn.Id).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Holiday Name already exists");

            //Fetch Holiday for update
            Holiday holiday = m_AdminContext.Holidays.Find(holidayIn.Id);

            if (holiday == null)
                return CreateResponse(null, false, "Holiday not found for update");

                        
            //update fields
            m_mapper.Map<Holiday, Holiday>(holidayIn, holiday);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in HolidayService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating CreateHoliday record in HolidayService");
                return CreateResponse(holiday, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated");
        }
        #endregion

        #region GetHolidaysForDropdown
        /// <summary>
        /// Get Holidays For Dropdown
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetHolidaysForDropdown() =>
                        await m_AdminContext.Holidays
                              .Select(c => new GenericType()
                                 { Id = c.Id,
                                   Name = c.Occasion })
                        .OrderBy(x => x.Name)
                        .ToListAsync();

        #endregion        

        #region GetByHolidayId
        /// <summary>
        /// Gets the Holiday by Id
        /// </summary>
        /// <param name="holidayId"></param>
        /// <returns></returns>
        public async Task<Holiday> GetByHolidayId(int holidayId) =>
                        await m_AdminContext.Holidays.Where(et => et.Id == holidayId)
                        .FirstOrDefaultAsync();

        #endregion        

        #region GetHolidayIdByName
        /// <summary>
        /// GetHolidayIdByName
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<int>> GetHolidayIdByName(string occasion)
        {
            var response = new ServiceResponse<int>();
            try
            {
                var holiday = await m_AdminContext.Holidays
                                        .Where(hd => hd.Occasion.ToLower().Contains(occasion.ToLower()))
                                        .Select(et => et.Id)
                                        .FirstOrDefaultAsync();

                response.IsSuccessful = true;
                response.Item = holiday;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Holiday";
                m_Logger.LogError("Error occured while fetching GetHolidayIdByName() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="Holiday"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(Holiday Holiday, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in HolidayService");

            dynamic response = new ExpandoObject();
            response.Holiday = Holiday;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in HolidayService");

            return response;
        }

        #endregion
    }
}
