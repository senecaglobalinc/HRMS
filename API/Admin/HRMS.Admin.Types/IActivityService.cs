using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IActivityService
    {
        //Task<List<Activity>> GetActivitiesByCategoryName(string CategoryName);
        Task<dynamic> Create(Activity activityIn);
        Task<dynamic> Update(Activity activityIn);
        Task<List<Activity>> GetAll(bool isActive = true);
        Task<Activity> GetByActivityId(int activityId);
        Task<List<GenericType>> GetActivitiesForDropdown();        
        Task<List<ActivityDetails>> GetExitActivitiesByDepartment(int? departmentId = null);
        Task<List<ActivityDetails>> GetClosureActivitiesByDepartment(int? departmentId = null);
        Task<List<TransitionPlanDetails>> GetTransitionPlanActivities();
    }
}
