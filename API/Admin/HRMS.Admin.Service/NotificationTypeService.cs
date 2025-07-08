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
    /// Service class to get the notification type
    /// </summary>
    public class NotificationTypeService : INotificationTypeService
    {
        #region Global Variables 

        private readonly ILogger<NotificationTypeService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly ICategoryMasterService m_CategoryMasterService;
        private readonly IMapper m_Mapper;

        #endregion

        #region Constructor
        public NotificationTypeService(ILogger<NotificationTypeService> logger, AdminContext adminContext,
                                       ICategoryMasterService categoryMasterService)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
            m_CategoryMasterService = categoryMasterService;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NotificationType, NotificationType>();
            });

            m_Mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// This method create notification type. 
        /// </summary>
        /// <param name="notificationTypeIn">notification type information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Create(NotificationType notificationTypeIn)
        {
            try
            {
                int isCreated;
                m_Logger.LogInformation("Calling \"Create\" method in NotificationTypeService");
                m_Logger.LogInformation("Verifying notification type already exists? in NotificationTypeService.");

                NotificationType notificationTypeAlreadyExits =
                    await GetByNotificationCode(notificationTypeIn.NotificationCode);

                //Notification code already exists?
                if (notificationTypeAlreadyExits != null)
                    return CreateResponse(null, false, "Notification code already exists.");
                else
                    m_Logger.LogInformation("Notification type does not already exists.");

                m_Logger.LogInformation("Verifying Category master exists? in NotificationTypeService.");

                CategoryMaster category = null;
                int? CategoryMasterId = notificationTypeIn?.CategoryMaster?.CategoryMasterId;
                if (!CategoryMasterId.HasValue)
                    CategoryMasterId = notificationTypeIn.CategoryMasterId;

                if (CategoryMasterId.HasValue)
                {
                    m_Logger.LogInformation("CategoryMasterId:" + CategoryMasterId);

                    category = await m_CategoryMasterService.GetByCategoryMasterId(CategoryMasterId.Value);
                }
                else
                    m_Logger.LogInformation("CategoryMasterId not found.");

                if (category == null)
                    return CreateResponse(null, false, "Category not found.");
                else
                    m_Logger.LogInformation("Category found.");

                m_Logger.LogInformation("Update feilds.");

                NotificationType notificationType = new NotificationType();

                if (!notificationTypeIn.IsActive.HasValue)
                    notificationTypeIn.IsActive = true;

                //notificationTypeIn.CategoryMaster = category;
                notificationTypeIn.CategoryMasterId = category.CategoryMasterId;

                m_Logger.LogInformation("Assigning to automapper.");

                m_Mapper.Map<NotificationType, NotificationType>(notificationTypeIn, notificationType);

                m_Logger.LogInformation("Add notification type to list.");

                m_AdminContext.NotificationTypes.Add(notificationType);

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in NotificationTypeService.");

                isCreated = await m_AdminContext.SaveChangesAsync();

                if (isCreated > 0)
                {
                    m_Logger.LogInformation("Notification type created successfully.");

                    return CreateResponse(notificationType, true, string.Empty);

                }
                else
                    return CreateResponse(null, false, "No notification type created.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// This method deletes notification type.
        /// </summary>
        /// <param name="notificationTypeID">notification ID</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Delete(int notificationTypeID)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling \"Delete\" method in NotificationTypeService");
            m_Logger.LogInformation("Fetching notification type for delete in NotificationTypeService");

            //Fetch notification type for delete
            var notificationType = m_AdminContext.NotificationTypes.Find(notificationTypeID);

            //Notification type exists?
            if (notificationType == null)
                return CreateResponse(null, false, "Notification type not found for delete.");

            m_Logger.LogInformation("Fetching notification configuration in NotificationTypeService");

            List<NotificationConfiguration> notificationConfigurations =
                (from nc in m_AdminContext.NotificationConfigurations
                 where nc.NotificationTypeId == notificationType.NotificationTypeId
                 select nc).ToList();

            if (notificationConfigurations != null && notificationConfigurations.Count > 0)
            {
                m_Logger.LogInformation("Deleting notification configuration in NotificationTypeService");

                m_AdminContext.NotificationConfigurations.RemoveRange(notificationConfigurations);
            }
            else
                m_Logger.LogInformation("Notification configuration not found in NotificationTypeService");

            m_Logger.LogInformation("Deleting notification type in NotificationTypeService");

            m_AdminContext.NotificationTypes.Remove(notificationType);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in NotificationTypeService.");

            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating notification type record in NotificationTypeService.");
                return CreateResponse(null, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// This method fetches all notification type based on isActive flag.
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns>List<NotificationType></returns>
        public async Task<List<NotificationType>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("Calling \"GetAll\" method in NotificationTypeService.");

            if (!isActive.HasValue)
                return await m_AdminContext.NotificationTypes.Include(nt => nt.CategoryMaster).ToListAsync();
            else
                return await m_AdminContext.NotificationTypes.Include(nt => nt.CategoryMaster)
                    .Where(nt => nt.IsActive == isActive).ToListAsync();
        }
        #endregion

        #region GetByNotificationCode
        /// <summary>
        /// this method fetches notification type based on notification code.
        /// </summary>
        /// <param name="notificationCode">notification Code</param>
        /// <returns>NotificationType</returns>
        public async Task<NotificationType> GetByNotificationCode(string notificationCode) =>
                        await m_AdminContext.NotificationTypes.Where(nt => nt.NotificationCode.ToLower().Trim() == notificationCode.ToLower().Trim())
                        .FirstOrDefaultAsync();

        #endregion

        #region GetByNotificationId
        /// <summary>
        /// this method fetches notification type based on notification id.
        /// </summary>
        /// <param name="notificationTypeId"></param>
        /// <returns>NotificationType</returns>
        public async Task<NotificationType> GetByNotificationTypeId(int? notificationTypeId) =>
                        await m_AdminContext.NotificationTypes.Include(nt => nt.CategoryMaster)
                               .Where(nt => nt.NotificationTypeId == notificationTypeId)
                               .FirstOrDefaultAsync();

        #endregion

        #region Update
        /// <summary>
        /// This method updates notification type.
        /// </summary>
        /// <param name="notificationTypeIn">notification type information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Update(NotificationType notificationTypeIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling \"Update\" method in NotificationTypeService.");
            m_Logger.LogInformation("Fetching notification type for update in NotificationTypeService.");

            var notificationType = m_AdminContext.NotificationTypes.Find(notificationTypeIn.NotificationTypeId);

            m_Logger.LogInformation("Notification type exists?");

            if (notificationType == null)
                return CreateResponse(null, false, "Notification type not found for update.");
            else
                m_Logger.LogInformation("Notification type found.");

            m_Logger.LogInformation("Notification code already exists?");

            NotificationType notificationTypeAlreadyExits =
               await GetByNotificationCode(notificationTypeIn.NotificationCode);

            if (notificationTypeAlreadyExits != null &&
                notificationTypeAlreadyExits.NotificationTypeId != notificationType.NotificationTypeId)
                return CreateResponse(null, false, "Notification code already exists.");
            else
                m_Logger.LogInformation("Notification code does not already exists");

            m_Logger.LogInformation("Verifying Category master exists? in NotificationTypeService.");
            CategoryMaster category = null;
            int? CategoryMasterId = notificationTypeIn?.CategoryMaster?.CategoryMasterId;
            if (!CategoryMasterId.HasValue)
                CategoryMasterId = notificationTypeIn.CategoryMasterId;

            if (CategoryMasterId.HasValue)
            {
                m_Logger.LogInformation("CategoryMasterId:" + CategoryMasterId);

                category = await m_CategoryMasterService.GetByCategoryMasterId(CategoryMasterId.Value);
            }
            else
                m_Logger.LogInformation("CategoryMasterId not found.");

            if (category == null)
                return CreateResponse(null, false, "Category not found.");
            else
                m_Logger.LogInformation("Category found.");

            m_Logger.LogInformation("Update feilds.");

            notificationType.CategoryMasterId = category.CategoryMasterId;

            if (!notificationTypeIn.IsActive.HasValue)
                notificationTypeIn.IsActive = notificationType.IsActive;

            notificationTypeIn.CategoryMasterId = category.CategoryMasterId;

            m_Logger.LogInformation("Assigning to automapper.");

            notificationTypeIn.NotificationTypeId = notificationType.NotificationTypeId;
            //notificationType.NotificationTypeId = notificationTypeIn.NotificationTypeId;
            notificationTypeIn.CreatedBy = notificationType.CreatedBy;
            notificationTypeIn.CreatedDate = notificationType.CreatedDate;

            m_Mapper.Map<NotificationType, NotificationType>(notificationTypeIn, notificationType);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in NotificationTypeService.");

            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating notification type record in NotificationTypeService.");

                return CreateResponse(notificationType, true, string.Empty);
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
        /// <param name="notificationType"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(NotificationType notificationType, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in NotificationTypeService");

            dynamic response = new ExpandoObject();
            response.NotificationType = notificationType;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in NotificationTypeService");

            return response;
        }

        #endregion
    }
}
