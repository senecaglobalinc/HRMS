using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Infrastructure.Models.Domain;
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
    /// service class to get Activity details
    /// </summary>
    public class ActivityService : IActivityService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<ActivityService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public ActivityService(AdminContext adminContext, ILogger<ActivityService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;


            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Activity, Activity>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a Activity
        /// </summary>
        /// <param name="Activity"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(Activity activityIn)
        {
            m_Logger.LogInformation("Calling CreateActivity method in ActivityService");

            int isCreated;

            //Checking if  already exists
            var isExists = m_AdminContext.Activities.Where(p => p.Description.ToLower().Trim() == activityIn.Description.ToLower().Trim()
             && p.DepartmentId == activityIn.DepartmentId && p.ActivityTypeId == activityIn.ActivityTypeId).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Activity already exists");

            Activity Activity = new Activity();

            if (!activityIn.IsActive.HasValue)
                activityIn.IsActive = true;

            //Add fields
            m_mapper.Map<Activity, Activity>(activityIn, Activity);

            m_Logger.LogInformation("Add Activity to list");
            m_AdminContext.Activities.Add(Activity);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ActivityService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Activity created successfully.");
                return CreateResponse(Activity, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No Activity created");
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the Activity details
        /// </summary>
        /// <param name="Activity"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(Activity activityIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling UpdateActivity method in ActivityService");

            //Checking if  already exists
            var isExists = m_AdminContext.Activities.Where(p => p.Description.ToLower().Trim() == activityIn.Description.ToLower().Trim()
                                         && p.DepartmentId == activityIn.DepartmentId && p.ActivityTypeId == activityIn.ActivityTypeId
                                         && p.ActivityId != activityIn.ActivityId).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Activity already exists");

            //Fetch Activity for update
            Activity activity = m_AdminContext.Activities.Find(activityIn.ActivityId);

            if (activity == null)
                return CreateResponse(null, false, "Activity not found for update");

            if (!activityIn.IsActive.HasValue)
                activity.IsActive = activityIn.IsActive;

            //update fields
            m_mapper.Map<Activity, Activity>(activityIn, activity);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in ActivityService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating CreateActivity record in ActivityService");
                return CreateResponse(activity, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the Activity details
        /// </summary>
        /// <returns></returns>
        public async Task<List<Activity>> GetAll(bool isActive = true) =>
                       await m_AdminContext.Activities.Where(et => et.IsActive == isActive).OrderBy(x => x.Description).ToListAsync();
        #endregion

        #region GetByActivityId
        /// <summary>
        /// Gets the Activity by Id
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public async Task<Activity> GetByActivityId(int activityId) =>
                        await m_AdminContext.Activities.Where(et => et.ActivityId == activityId)
                        .FirstOrDefaultAsync();

        #endregion        

        #region GetActivitiesForDropdown
        /// <summary>
        /// Get Activitys For Dropdown
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetActivitiesForDropdown() =>
                        await m_AdminContext.Activities.Where(et => et.IsActive == true)
                              .Select(c => new GenericType()
                              {
                                  Id = c.ActivityId,
                                  Name = c.Description
                              })
                        .OrderBy(x => x.Name)
                        .ToListAsync();

        #endregion

        #region GetExitActivitiesByDepartment
        /// <summary>
        /// GetExitActivitiesByDepartment
        /// </summary>        
        /// <returns></returns>
        public async Task<List<ActivityDetails>> GetExitActivitiesByDepartment(int? departmentId = null) =>
                        await (from at in m_AdminContext.Activities
                               join ac in m_AdminContext.ActivityTypes on at.ActivityTypeId equals ac.ActivityTypeId
                               join dt in m_AdminContext.Departments on at.DepartmentId equals dt.DepartmentId
                               where at.IsActive == true
                                && at.DepartmentId == (departmentId ?? at.DepartmentId)
                                && ac.ParentId == 1
                               select new ActivityDetails()
                               {
                                   ActivityId = at.ActivityId,
                                   Department = dt.Description,
                                   DepartmentId = dt.DepartmentId,
                                   ActivityType = ac.Description,
                                   Description = at.Description,
                                   IsRequired=at.IsRequired
                               })
                        .OrderBy(x => x.Department).ThenBy(y => y.ActivityType)
                        .ThenBy(z => z.Description).ToListAsync();

        #endregion  

        #region GetClosureActivitiesByDepartment
        /// <summary>
        /// GetClosureActivitiesByDepartment
        /// </summary>        
        /// <returns></returns>
        public async Task<List<ActivityDetails>> GetClosureActivitiesByDepartment(int? departmentId = null) =>

                       await (from at in m_AdminContext.Activities
                              join ac in m_AdminContext.ActivityTypes on at.ActivityTypeId equals ac.ActivityTypeId
                              join dt in m_AdminContext.Departments on at.DepartmentId equals dt.DepartmentId
                              where at.IsActive == true
                               && at.DepartmentId == (departmentId ?? at.DepartmentId)
                               && ac.ParentId == 2
                              select new ActivityDetails()
                              {
                                  ActivityId = at.ActivityId,
                                  Department = dt.Description,
                                  DepartmentId = dt.DepartmentId,
                                  ActivityType = ac.Description,
                                  Description = at.Description
                              })
                        .OrderBy(x => x.Department).ThenBy(y => y.ActivityType)
                        .ThenBy(z => z.Description).ToListAsync();

        #endregion

        #region GetTransitionPlanActivities
        /// <summary>
        /// GetTransitionPlanActivities
        /// </summary>        
        /// <returns></returns>
        public async Task<List<TransitionPlanDetails>> GetTransitionPlanActivities()
        {
            var category = await (from at in m_AdminContext.ActivityTypes
                                  where at.IsActive == true
                                   && at.ParentId == 12
                                  select at).Distinct().OrderBy(x => x.ActivityTypeId).ToListAsync();
            List<TransitionPlanDetails> transition = new List<TransitionPlanDetails>();

            foreach (ActivityType activityType in category)
            {
                TransitionPlanDetails transDetails = new TransitionPlanDetails();
                transDetails.ActivityTypeId = activityType.ActivityTypeId;
                transDetails.ActivityType = activityType.Description;
                var act = await (from at in m_AdminContext.Activities
                                 join ac in m_AdminContext.ActivityTypes on at.ActivityTypeId equals ac.ActivityTypeId
                                 where at.IsActive == true
                                  && ac.ParentId == 12 && at.ActivityTypeId == activityType.ActivityTypeId
                                 select new TransitionDetail()
                                 {
                                     ActivityId = at.ActivityId,
                                     Description = at.Description
                                 }
                                      ).Distinct().OrderBy(x => x.ActivityId).ToListAsync();
                transDetails.TransitionActivityDetails = act;
                transition.Add(transDetails);
            }
            return transition;
        }
        #endregion

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="Activity"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(Activity Activity, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in ActivityService");

            dynamic response = new ExpandoObject();
            response.Activity = Activity;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in ActivityService");

            return response;
        }

        #endregion

        //#region GetActivitiesByCategoryName
        ///// <summary>
        ///// GetActivitiesByCategoryName
        ///// </summary>
        ///// <param name="CategoryName"></param>
        ///// <returns>int</returns>
        //public async Task<List<Activity>> GetActivitiesByCategoryName(string CategoryName)
        //{
        //    return await (from activities in m_AdminContext.Activity
        //                  join categories in m_AdminContext.Categories on activities.CategoryMasterId equals categories.CategoryMasterId
        //                  where activities.IsActive == true && categories.CategoryName.ToLower().Trim() == CategoryName.ToLower().Trim()
        //                  select new Activity
        //                  {
        //                      Id = activities.Id,
        //                      Description = activities.Description,
        //                      DepartmentId = activities.DepartmentId

        //                  }).ToListAsync();
        //}
        //#endregion
    }
}
