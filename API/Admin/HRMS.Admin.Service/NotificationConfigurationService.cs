using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
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
    /// Service class to get the notification configuration
    /// </summary>
    public class NotificationConfigurationService : INotificationConfigurationService
    {
        #region Global Variables 

        private readonly ILogger<NotificationConfigurationService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly ICategoryMasterService m_CategoryMasterService;
        private readonly INotificationTypeService m_NotificationTypeService;

        private readonly IMapper m_Mapper;

        #endregion

        #region Constructor
        public NotificationConfigurationService(ILogger<NotificationConfigurationService> logger, AdminContext adminContext,
            ICategoryMasterService categoryMasterService, INotificationTypeService notificationTypeService)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
            m_CategoryMasterService = categoryMasterService;
            m_NotificationTypeService = notificationTypeService;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NotificationConfiguration, NotificationConfiguration>();
            });

            m_Mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// This method create notification configuration. 
        /// </summary>
        /// <param name="notificationConfigurationIn"></param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Create(NotificationConfiguration notificationConfigurationIn)
        {
            try
            {
                int isCreated;
                m_Logger.LogInformation("Calling \"Create\" method in NotificationConfigurationService");
                m_Logger.LogInformation("Verifying Category master exists? in NotificationConfigurationService.");

                CategoryMaster category = await m_CategoryMasterService.GetByCategoryMasterId(notificationConfigurationIn.CategoryMasterId);

                if (category == null)
                    return CreateResponse(null, false, "Category not found.");
                else
                    m_Logger.LogInformation("Category found.");

                m_Logger.LogInformation("Verifying Notification type exists? in NotificationConfigurationService.");

                NotificationType notificationType =
                    await m_NotificationTypeService.GetByNotificationTypeId(notificationConfigurationIn.NotificationTypeId);

                if (notificationType == null)
                    return CreateResponse(null, false, "Notification Type not found.");
                else
                    m_Logger.LogInformation("Notification Type found.");

                m_Logger.LogInformation("Verifying notification configuration already exists? in NotificationConfigurationService.");

                NotificationConfiguration notificationConfigurationAlreadyExits =
                    await GetByNotificationTypeAndCategory(notificationConfigurationIn.NotificationTypeId, notificationConfigurationIn.CategoryMasterId);

                //Notification code already exists?
                if (notificationConfigurationAlreadyExits != null)
                    return CreateResponse(null, false, "Notification configuration already exists.");
                else
                    m_Logger.LogInformation("Notification configuration does not already exists.");

                m_Logger.LogInformation("Update feilds.");

                NotificationConfiguration notificationConfiguration = new NotificationConfiguration();

                if (!notificationConfigurationIn.IsActive.HasValue)
                    notificationConfigurationIn.IsActive = true;

                m_Logger.LogInformation("Assigning to automapper.");

                m_Mapper.Map<NotificationConfiguration, NotificationConfiguration>(notificationConfigurationIn, notificationConfiguration);

                m_Logger.LogInformation("Add notification configuration to list.");

                m_AdminContext.NotificationConfigurations.Add(notificationConfiguration);

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in NotificationConfigurationService.");

                isCreated = await m_AdminContext.SaveChangesAsync();

                if (isCreated > 0)
                {
                    m_Logger.LogInformation("Notification configuration created successfully.");

                    return CreateResponse(notificationConfiguration, true, string.Empty);

                }
                else
                    return CreateResponse(null, false, "No notification configuration created.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// This method fetches all notification configuration based on isActive flag.
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns>List<NotificationConfiguration></returns>
        public async Task<List<NotificationConfiguration>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("Calling \"GetAll\" method in NotificationConfigurationService.");

            if (!isActive.HasValue)
                return await m_AdminContext.NotificationConfigurations.Include(nc => nc.NotificationType).ToListAsync();
            else
                return await m_AdminContext.NotificationConfigurations.Include(nc => nc.NotificationType)
                    .Where(nc => nc.IsActive == isActive).ToListAsync();
        }
        #endregion

        #region GetByNotificationTypeAndCategory
        /// <summary>
        /// this method fetches notification configuration based on notification type id and category id.
        /// </summary>
        /// <param name="notificationTypeId"></param>
        /// <param name="categoryMasterId"></param>
        /// <returns>NotificationConfiguration</returns>
        public async Task<NotificationConfiguration> GetByNotificationTypeAndCategory(int? notificationTypeId, int? categoryMasterId)
        {
            return await m_AdminContext.NotificationConfigurations.Include(nc => nc.NotificationType)
                .Where(nc => nc.NotificationTypeId == notificationTypeId && nc.CategoryMasterId == categoryMasterId)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region GetNotificationConfiguration
        /// <summary>
        /// this method fetches notification configuration based on notification type code and category id.
        /// </summary>
        /// <param name="notificationCode"></param>
        /// <param name="categoryMasterId"></param>
        /// <returns>NotificationConfiguration</returns>
        public async Task<NotificationConfiguration> GetNotificationConfiguration(string notificationCode, int? categoryMasterId)
        {
            return await m_AdminContext.NotificationConfigurations.Include(nc => nc.NotificationType)
                .Where(nc => nc.NotificationType.NotificationCode == notificationCode && nc.CategoryMasterId == categoryMasterId)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region GetByNotificationTypeAndEmailTo
        /// <summary>
        /// this method fetches notification configuration based on notification type id and email to.
        /// </summary>
        /// <param name="notificationTypeId"></param>
        /// <param name="emailTo"></param>
        /// <returns>NotificationConfiguration</returns>
        public async Task<NotificationConfiguration> GetByNotificationTypeAndEmailTo(int? notificationTypeId, string emailTo)
        {
            return await m_AdminContext.NotificationConfigurations.Include(nc => nc.NotificationType)
                                         .Where(nc => nc.NotificationTypeId == notificationTypeId && nc.EmailTo.Contains(emailTo))
                                         .FirstOrDefaultAsync();

        }

        #endregion

        #region GetEmployeeWorkEmails
        /// <summary>
        /// this method fetch Work Emails.
        /// </summary>        
        /// <returns>List<string></returns>
        public async Task<List<string>> GetEmployeeWorkEmails()
        {
            return await m_AdminContext.Users
                                         .Where(u => u.IsActive == true)
                                         .Select(u => u.EmailAddress)
                                         .OrderBy(u => u)
                                         .ToListAsync();

        }

        #endregion        

        #region Update
        /// <summary>
        /// This method updates notification configuration.
        /// </summary>
        /// <param name="notificationConfigurationIn">notification configuration information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Update(NotificationConfiguration notificationConfigurationIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling \"Update\" method in NotificationConfigurationService.");

            m_Logger.LogInformation("Verifying Category master exists? in NotificationConfigurationService.");
            CategoryMaster category = await m_CategoryMasterService.GetByCategoryMasterId(notificationConfigurationIn.CategoryMasterId);

            if (category == null)
                return CreateResponse(null, false, "Category not found.");
            else
                m_Logger.LogInformation("Category found.");

            m_Logger.LogInformation("Verifying Notification type exists? in NotificationConfigurationService.");

            NotificationType notificationType =
                await m_NotificationTypeService.GetByNotificationTypeId(notificationConfigurationIn.NotificationTypeId);

            if (notificationType == null)
                return CreateResponse(null, false, "Notification Type not found.");
            else
                m_Logger.LogInformation("Notification Type found.");

            m_Logger.LogInformation("Fetching notification configuration for update in NotificationConfigurationService.");

            var notificationConfiguration = m_AdminContext.NotificationConfigurations
                                                 .Where(nc => nc.NotificationTypeId == notificationConfigurationIn.NotificationTypeId &&
                                                        nc.CategoryMasterId == notificationConfigurationIn.CategoryMasterId).FirstOrDefault();

            m_Logger.LogInformation("Notification configuration exists?");

            if (notificationConfiguration == null)
                return CreateResponse(null, false, "Notification configuration not found for update.");
            else
                m_Logger.LogInformation("Notification configuration found.");

            m_Logger.LogInformation("Update feilds.");

            if (!notificationConfigurationIn.IsActive.HasValue)
                notificationConfigurationIn.IsActive = notificationConfiguration.IsActive;

            notificationConfigurationIn.NotificationConfigurationId = notificationConfiguration.NotificationConfigurationId;
            notificationConfigurationIn.CreatedBy = notificationConfiguration.CreatedBy;
            notificationConfigurationIn.CreatedDate = notificationConfiguration.CreatedDate;

            m_Logger.LogInformation("Assigning to automapper.");

            m_Mapper.Map<NotificationConfiguration, NotificationConfiguration>(notificationConfigurationIn, notificationConfiguration);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in NotificationTypeService.");

            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating notification configuration record in NotificationConfigurationService.");

                return CreateResponse(notificationConfiguration, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated.");
        }

        #endregion

        //Helpers

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="notificationConfiguration"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(NotificationConfiguration notificationConfiguration, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in NotificationConfigurationService");

            dynamic response = new ExpandoObject();
            response.NotificationConfiguration = notificationConfiguration;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in NotificationConfigurationService");

            return response;
        }

        #endregion

    }
}
