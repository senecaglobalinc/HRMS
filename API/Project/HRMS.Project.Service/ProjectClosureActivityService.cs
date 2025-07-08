using AutoMapper;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HRMS.Common.Enums;
using HRMS.Common.Extensions;

namespace HRMS.Project.Service
{
    public class ProjectClosureActivityService : IProjectClosureActivityService
    {
        #region Global Varibles

        private readonly ILogger<ProjectClosureActivityService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private IOrganizationService m_OrgService;
        private readonly IMapper m_Mapper;
        private readonly IProjectService m_projectService;
        private readonly IProjectClosureReportService m_projectClosureReport;
        private readonly IEmployeeService m_EmployeeService;
        IMapper mapper;

        #endregion

        #region DepartmentActivityService
        public ProjectClosureActivityService(ILogger<ProjectClosureActivityService> logger,
            ProjectDBContext projectContext,
            IOrganizationService orgService,
            IProjectService projectService,
            IProjectClosureReportService projectClosureReportService,
            IEmployeeService employeeService
            , IMapper mapper
            )
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            m_OrgService = orgService;
            m_projectService = projectService;
            m_projectClosureReport = projectClosureReportService;
            m_EmployeeService = employeeService;

             //CreateMapper
             var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Entities.Project, Entities.Project>();
            });
            m_Mapper = config.CreateMapper();

            m_mapper = mapper;
        }
        #endregion

        #region GetDepartmentActivitiesByProjectId
        /// <summary>
        /// This method Gets Department Activities by ProjectId and Department ID
        /// </summary>
        /// <param name="projectId">project Id</param>
        /// <returns>DepartmentActivities</returns>
        public async Task<ServiceResponse<Activities>> GetDepartmentActivitiesByProjectId(int projectId, int? departmentId = null)
        {
            ServiceResponse<Activities> response;
            ServiceListResponse<Activity> activities = null;
            ServiceListResponse<Department> departments = null;
            ServiceListResponse<Status> statuses = null;
            List<ProjectClosureActivity> projects = null;
            List<GetActivityChecklist> activityChecklists = null;
            if (projectId == 0)
            {
                response = new ServiceResponse<Activities>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            else
            {
                m_Logger.LogInformation("Checking if project exists");
                var project = m_ProjectContext.ProjectClosure.FirstOrDefault(c => c.ProjectId == projectId);
                if (project == null)
                {
                    response = new ServiceResponse<Activities>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = "No Such Project exists"
                    };
                }
                else
                {
                    m_Logger.LogInformation("Project details Found");
                    response = new ServiceResponse<Activities>();
                    try
                    {
                        m_Logger.LogInformation("Fetching Data");
                        int projectClosureId = project.ProjectClosureId;
                        activities = await m_OrgService.GetClosureActivitiesByDepartment();
                        if(!activities.IsSuccessful)
                        {
                            return response = new ServiceResponse<Activities>()
                            {
                                Item = null,
                                IsSuccessful = false,
                                Message = "Error Occured While Fetching Closure Activities"
                            };
                        }
                        departments = await m_OrgService.GetAllDepartment(true);
                        if (!departments.IsSuccessful)
                        {
                            return response = new ServiceResponse<Activities>()
                            {
                                Item = null,
                                IsSuccessful = false,
                                Message = "Error Occured While Fetching Departments"
                            };
                        }
                        statuses = await m_OrgService.GetStatusesByCategoryName(HRMS.Common.Enums.CategoryMaster.PPC.ToString());
                        if (!statuses.IsSuccessful)
                        {
                            return response = new ServiceResponse<Activities>()
                            {
                                Item = null,
                                IsSuccessful = false,
                                Message = "Error Occured While Fetching Statuses of PPC Category"
                            };
                        }
                        if (activities.Items == null || (activities.Items != null && activities.Items.Count <= 0))
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Activities for project closure category not found.";
                        }
                        
                        if (departmentId != null)
                        {
                            var projectdept = await m_ProjectContext.ProjectClosureActivity.Where(st => st.ProjectClosureId == projectClosureId && st.DepartmentId == departmentId).FirstOrDefaultAsync();
                            if (projectdept == null)
                            {
                                return response = new ServiceResponse<Activities>()
                                {
                                    Item = null,
                                    IsSuccessful = false,
                                    Message = "Error Occured While Fetching Project Closure Activities"
                                };
                            }
                            Activities ac = new Activities();
                            Department projdept = null;
                            Status projstatus = null;
                            ac.ProjectId = projectId;
                            ac.DepartmentId = projectdept.DepartmentId;
                            ac.Remarks = projectdept.Remarks;
                            ac.StatusId = projectdept.StatusId;
                            projdept = departments.Items.Where(q => q.DepartmentId == departmentId).FirstOrDefault();
                            if (projdept == null)
                            {
                                return response = new ServiceResponse<Activities>()
                                {
                                    Item = null,
                                    IsSuccessful = false,
                                    Message = "Error Occured While Fetching Departments"
                                };
                            }
                            projstatus = statuses.Items.Where(q => q.StatusId == ac.StatusId).FirstOrDefault();
                            if (projstatus == null)
                            {
                                return response = new ServiceResponse<Activities>()
                                {
                                    Item = null,
                                    IsSuccessful = false,
                                    Message = "Error Occured While Fetching Statuses"
                                };
                            }
                            ac.DepartmentName = projdept.Description;
                            ac.StatusDescription = projstatus.StatusDescription;
                            activityChecklists = await (from deptact in m_ProjectContext.ProjectClosureActivity
                                                        join act in m_ProjectContext.ProjectClosureActivityDetail
                                          on deptact.ProjectClosureActivityId equals act.ProjectClosureActivityId
                                                        where (deptact.ProjectClosureId == projectClosureId && deptact.DepartmentId == departmentId)
                                                        select new GetActivityChecklist()
                                                        {
                                                            ActivityId = act.ActivityId,
                                                            Value = act.Value,
                                                            Remarks = act.Remarks,
                                                        }).ToListAsync();
                            ac.ActivityDetails = activityChecklists;
                            response.Item = ac;
                            response.IsSuccessful = true;
                            response.Message = string.Empty;
                        }
                        


                    }
                    catch (Exception ex)
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the data";
                        m_Logger.LogError($"Error occured in GetDepartmentActivitiesByProjectId() method {ex.StackTrace}");
                    }

                }

            }
            return response;


        }


        #endregion

        #region CreateActivityChecklist
        /// <summary>
        /// This method Creates list of ActivityChecklist by projectId
        /// </summary>
        /// <param name="projectId">project Id of the project that is initiated for closure</param>
        /// <returns>Created projectClosureActivityId</returns>
        public async Task<ServiceResponse<int>> CreateActivityChecklist(int projectId)
        {
            ServiceResponse<int> response;
            try
            {
                int isCreated;
                string dept = "Technology & Delivery";

                m_Logger.LogInformation("Calling \"CreateActivityChecklist\" method in ProjectClosureActivityService");

                var project = await m_ProjectContext.ProjectClosure.Where(prj => prj.ProjectId == projectId).FirstOrDefaultAsync();

                if (project == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Project found with this project code.."
                    };
                }
                int projectClosureId = project.ProjectClosureId;

                var projectActivity = await m_ProjectContext.ProjectClosureActivity.Where(prj => prj.ProjectClosureId == project.ProjectClosureId ).FirstOrDefaultAsync();
                if (projectActivity != null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Activity already exists with the given departmentId "
                    };
                }

                ServiceResponse<Status> statuses = await m_OrgService.GetStatusByCategoryAndStatusCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), HRMS.Common.Enums.ProjectClosureStatusCodes.InProgress.ToString());
                if (!statuses.IsSuccessful)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "In Progress status not found "
                    };
                }
                ServiceListResponse<Activity> activities = await m_OrgService.GetClosureActivitiesByDepartment();
                if (!activities.IsSuccessful)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error Occured while fetching Closure Activities"
                    };
                }
                //saving in ProjectClosureActivity
                List<int> departmentId = activities.Items.Where(st => st.Department != dept).Select(st => st.DepartmentId).Distinct().ToList();
                if(departmentId.Count == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error Occured while fetching Department ID's"
                    };
                }
                foreach (int Id in departmentId)
                {
                    Entities.ProjectClosureActivity Activity = new Entities.ProjectClosureActivity();
                    Activity.ProjectClosureId = projectClosureId;
                    Activity.DepartmentId = Id;
                    Activity.StatusId = statuses.Item.StatusId;
                    Activity.IsActive = true;
                    m_ProjectContext.ProjectClosureActivity.Add(Activity);
                }

                isCreated = await m_ProjectContext.SaveChangesAsync();
                if (isCreated > 0)
                {

                    m_Logger.LogInformation("Project Closure Activity created successfully.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 1,

                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("No Project Closure Activity created.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Project Closure Activity created."
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region UpdateActivityChecklist
        /// <summary>
        /// Updates Activity Checklist By projectId and takes Project Closure Report Data as Parameter
        /// </summary>
        /// <param name="projectIn">ProjectClosureActivityInformation</param>
        /// <returns>Updated projectClosureActivityChecklist Id</returns>
        public async Task<ServiceResponse<int>> UpdateActivityChecklist(ActivityChecklist projectIn)
        {
            ServiceResponse<int> response;
            try
            {
                int isUpdated;
                string submitType = "submit";
                m_Logger.LogInformation("Calling \"UpdateActivityChecklist\" method in ProjectClosureActivityService");

                var project = m_ProjectContext.ProjectClosure.FirstOrDefault(c => c.ProjectId == projectIn.ProjectId);
                if (project == null)
                {
                   return response = new ServiceResponse<int>()
                    {
                        
                        IsSuccessful = false,
                        Message = "No Such Project exists"
                    };
                }
                int projectClosureId = project.ProjectClosureId;
                var projectactivity = m_ProjectContext.ProjectClosureActivity.FirstOrDefault(c => c.ProjectClosureId == projectClosureId && c.DepartmentId == projectIn.DepartmentId);
                if (projectactivity == null)
                {
                    ProjectClosureActivity projectClosureActivity = new ProjectClosureActivity();
                    projectClosureActivity.Remarks = projectIn.Remarks;
                    projectClosureActivity.DepartmentId = projectIn.DepartmentId;
                    projectClosureActivity.ProjectClosureId = project.ProjectClosureId;
                    projectClosureActivity.IsActive = true;
                    
                    if (projectIn.Type == submitType)
                    {
                        ServiceResponse<Status> completedStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), HRMS.Common.Enums.ProjectClosureStatusCodes.Completed.ToString());
                        if (!completedStatus.IsSuccessful)
                        {
                            return response = new ServiceResponse<int>()
                            {

                                IsSuccessful = false,
                                Message = $"Could not fetch Completed Status code"
                            };
                        }
                        projectactivity.StatusId = completedStatus.Item.StatusId;
                    }
                    else
                    {
                        ServiceResponse<Status> inProgressStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), HRMS.Common.Enums.ProjectClosureStatusCodes.InProgress.ToString());
                        if (!inProgressStatus.IsSuccessful)
                        {
                            return response = new ServiceResponse<int>()
                            {

                                IsSuccessful = false,
                                Message = $"Could not fetch In Progress Status code"
                            };
                        }
                        projectactivity.StatusId = inProgressStatus.Item.StatusId;
                    }
                }
                else
                {
                    m_Logger.LogInformation("Project Closure Activity found for update.");
                    //saving in ProjectClosureActivity
                    projectactivity.Remarks = projectIn.Remarks;
                    projectactivity.DepartmentId = projectIn.DepartmentId;
                    ServiceResponse<Status> completedStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), HRMS.Common.Enums.ProjectClosureStatusCodes.Completed.ToString());
                    if (!completedStatus.IsSuccessful)
                    {
                        return response = new ServiceResponse<int>()
                        {

                            IsSuccessful = false,
                            Message = $"Could not fetch Completed Status code"
                        };
                    }
                    if (projectIn.Type == submitType)
                    {
                        projectactivity.StatusId = completedStatus.Item.StatusId;
                    }
                }
                    m_Logger.LogInformation("Saving Project Activity table details");
                    m_ProjectContext.SaveChanges();
                    foreach (ProjectClosureActivityDetail activityDetails in projectIn.ActivityDetails)
                    {
                        var projectdetail = await m_ProjectContext.ProjectClosureActivityDetail.Where(s => s.ActivityId == activityDetails.ActivityId && s.ProjectClosureActivityId == projectactivity.ProjectClosureActivityId).FirstOrDefaultAsync();
                        if (projectdetail != null)
                        {
                            
                            projectdetail.ActivityId = activityDetails.ActivityId;
                            projectdetail.CreatedBy = activityDetails.CreatedBy;
                            projectdetail.IsActive = true;
                            projectdetail.Remarks = activityDetails.Remarks;
                            projectdetail.SystemInfo = activityDetails.SystemInfo;
                            projectdetail.Value = activityDetails.Value;
                            m_ProjectContext.SaveChanges();
                        }
                        else
                        {
                            ProjectClosureActivityDetail pa = new ProjectClosureActivityDetail();

                            pa.ActivityId = activityDetails.ActivityId;
                            pa.CreatedBy = activityDetails.CreatedBy;
                            pa.IsActive = true;
                            pa.Remarks = activityDetails.Remarks;
                            pa.SystemInfo = activityDetails.SystemInfo;
                            pa.ProjectClosureActivityId = projectactivity.ProjectClosureActivityId;
                            pa.Value = activityDetails.Value;
                            

                            m_ProjectContext.ProjectClosureActivityDetail.Add(pa);
                        }
                    }
                
                
                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProjectClosureActivityDetail");

                    isUpdated = await m_ProjectContext.SaveChangesAsync();
                    
                    
                if (isUpdated > 0)
                    {
                    if (projectIn.Type == submitType)
                    {
                        await m_projectClosureReport.NotificationConfiguration(projectIn.ProjectId, Convert.ToInt32(HRMS.Common.Enums.NotificationType.SMSubmit), projectIn.DepartmentId);
                    }


                    m_Logger.LogInformation("Project Closure Activity updated successfully.");
                    return response = new ServiceResponse<int>()
                    {
                        
                        Item = isUpdated,
                            IsSuccessful = true,
                            Message = string.Empty
                        };

                    }
                    else
                    {
                        m_Logger.LogError("No Project Closure Activity Updated.");
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "No Project Closure Activity Updated."
                        };
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region GetDepartmentActivitiesForPM
        /// <summary>
        /// Gets Department Activities by ProjectId
        /// </summary>
        /// <param name="projectId">project Id</param>
        /// <returns>DepartmentActivities</returns>
        public async Task<ServiceListResponse<Activities>> GetDepartmentActivitiesForPM(int projectId)
        {
            ServiceListResponse<Activities> response;
            ServiceListResponse<Activity> activities = null;
            List<Activities> projects = new List<Activities>();
            
            if (projectId == 0)
            {
                response = new ServiceListResponse<Activities>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            else
            {
                try
                {
                    activities = await m_OrgService.GetClosureActivitiesByDepartment();
                    string dept = "Technology & Delivery";
                    if (activities.IsSuccessful == false)
                    {
                        return response = new ServiceListResponse<Activities>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "Error Occured while fetching Project Closure Activities"
                        };
                    }
                    List<int> DepartmentId = activities.Items.Where(st => st.Department != dept).Select(st => st.DepartmentId).Distinct().ToList();
                    if (DepartmentId.Count == 0)
                    {
                        return response = new ServiceListResponse<Activities>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "Department Id's Not Found"
                        };
                    }
                    foreach (int Id in DepartmentId)
                    {
                        Activities ac = new Activities();
                        ServiceResponse<Activities> activity = await GetDepartmentActivitiesByProjectId(projectId, Id);
                        if(activity.Item == null)
                        {
                            return response = new ServiceListResponse<Activities>()
                            {
                                Items = null,
                                IsSuccessful = false,
                                Message = "Error Occured while fetching activity details"
                            };
                        }
                        ac = activity.Item;
                        projects.Add(ac);
                    }
                    response = new ServiceListResponse<Activities>();
                    response.Items = projects;
                    response.IsSuccessful = true;
                    response.Message = string.Empty;



                }
                catch (Exception ex)
                    {
                    response = new ServiceListResponse<Activities>();
                    response.Items = null;
                    response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the data";
                        m_Logger.LogError($"Error occured in GetDepartmentActivitiesForPM() method {ex.StackTrace}");
                    }

                }

            
            return response;


        }


        #endregion
    }
}
