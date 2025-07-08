using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to Maintain Exit Activity Details
    /// To Create Associate Exit Activities
    /// To Update Associate Exit Activities
    /// </summary>
    public class AssociateExitActivityService : IAssociateExitActivityService
    {
        #region Global Varibles
        private readonly ILogger<AssociateExitActivityService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IOrganizationService m_OrgService;
        private readonly IAssociateExitService m_AssociateExitService;
        #endregion

        #region Constructor
        public AssociateExitActivityService(EmployeeDBContext employeeDBContext,
            ILogger<AssociateExitActivityService> logger,
            IOrganizationService orgService,
            IAssociateExitService associateExitService)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_OrgService = orgService;
            m_AssociateExitService = associateExitService;
        }

        #endregion

        #region CreateActivityChecklist
        /// <summary>
        /// This method Creates list of ActivityChecklist by employeeId
        /// </summary>
        /// <param name="employeeId">employeeId</param>
        /// <returns>Integer value 0-represents Unsuccessful Submission and 1-represents Successful Submission</returns>
        public async Task<ServiceResponse<int>> CreateActivityChecklist(int employeeId, int hraId)
        {
            ServiceResponse<int> response;
            try
            {
                int isCreated = 0;
                AssociateExitActivity associateExitActivity = null;

                m_Logger.LogInformation("Calling \"CreateActivityChecklist\" method in AssociateExitActivityService");

                var empDtls = await m_EmployeeContext.Employees.FirstOrDefaultAsync(pa => pa.EmployeeId == employeeId && pa.IsActive == true);
                var deliveryDepartment = await m_OrgService.GetDepartmentByCode("delivery");
                var exitDtls = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(emp => emp.EmployeeId == employeeId && emp.IsActive == true);
                var abscondDtls = await m_EmployeeContext.AssociateAbscond.FirstOrDefaultAsync(emp => emp.AssociateId == employeeId && emp.IsActive == true);
                if (exitDtls == null && abscondDtls == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Details Not Found!"
                    };
                }

                if (exitDtls != null)
                {
                    exitDtls.StatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress);

                    var exitWFList = await m_EmployeeContext.AssociateExitWorkflow.Where(x => x.AssociateExitId == exitDtls.AssociateExitId).ToListAsync();

                    if (exitWFList != null)
                    {
                        var exitWf = exitWFList.FirstOrDefault(x => x.WorkflowStatus == Convert.ToInt32(AssociateExitStatusCodesNew.ResignationApproved));
                        exitWf.SubmittedTo = hraId;
                    }

                    associateExitActivity = await m_EmployeeContext.AssociateExitActivity.FirstOrDefaultAsync(prj => prj.AssociateExitId == exitDtls.AssociateExitId);
                }
                else
                {
                    associateExitActivity = await m_EmployeeContext.AssociateExitActivity.FirstOrDefaultAsync(prj => prj.AssociateAbscondId == abscondDtls.AssociateAbscondId);
                }

                if (associateExitActivity != null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Activity already exists with the given EmployeeId "
                    };
                }

                ServiceListResponse<Activity> activities = await m_OrgService.GetExitActivitiesByDepartment();
                if (!activities.IsSuccessful)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error Occured while fetching Exit Activities"
                    };
                }

                List<int> departmentId = activities.Items.Where(st => st.Department != "Quality and Information Security").Select(st => st.DepartmentId).Distinct().ToList();

                if (deliveryDepartment?.Item.DepartmentId != empDtls?.DepartmentId)
                {
                    departmentId = activities.Items.Where(st => st.DepartmentId != deliveryDepartment?.Item.DepartmentId).Select(st => st.DepartmentId).Distinct().ToList();
                }

                if (departmentId.Count == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error Occured while fetching Department ID's"
                    };
                }

                ServiceResponse<Status> statuses = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.DepartmentActivityInProgress.ToString());
                if (!statuses.IsSuccessful)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "In Progress status not found "
                    };
                }

                foreach (int Id in departmentId)
                {
                    AssociateExitActivity exitActivity = new AssociateExitActivity();

                    if (exitDtls != null)
                    {
                        exitActivity.AssociateExitId = exitDtls.AssociateExitId;
                        exitActivity.AssociateAbscondId = null;
                    }
                    else
                    {
                        exitActivity.AssociateExitId = null;
                        exitActivity.AssociateAbscondId = abscondDtls.AssociateAbscondId;
                    }

                    exitActivity.DepartmentId = Id;
                    exitActivity.StatusId = statuses.Item.StatusId;
                    exitActivity.IsActive = true;

                    await m_EmployeeContext.AssociateExitActivity.AddAsync(exitActivity);
                }

                isCreated = await m_EmployeeContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    if (exitDtls != null)
                    {
                        ServiceResponse<int> notification = await m_AssociateExitService.AssociateExitSendNotification(employeeId, Convert.ToInt32(NotificationType.ActivitiesInProgress), null, null);
                        if (!notification.IsSuccessful)
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = notification.Message
                            };
                        }
                    }

                    m_Logger.LogInformation("Associate Exit Activity created successfully.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 1,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("No Associate exit Activity created.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Associate Exit Activity created."
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
        /// Updates Activity Checklist By projectId and takes ExitActivityInformation as Parameter
        /// </summary>
        /// <param name="activityIn">ExitActivityInformation</param>
        /// <returns>Integer value 0-represents Unsuccessful Submission and 1-represents Successful Submission</returns>
        public async Task<ServiceResponse<int>> UpdateActivityChecklist(ActivityChecklist activityIn)
        {
            ServiceResponse<int> response;
            try
            {
                int isUpdated = 0;
                string submitType = "submit";
                Status statusState = null;
                AssociateExitActivity exitActivity = null;

                m_Logger.LogInformation("Calling \"UpdateActivityChecklist\" method in AssociateExitActivityService");

                var exitDtls = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(c => c.EmployeeId == activityIn.EmployeeId && c.IsActive == true);
                var abscondDtls = await m_EmployeeContext.AssociateAbscond.FirstOrDefaultAsync(emp => emp.AssociateId == activityIn.EmployeeId && emp.IsActive == true);
                if (exitDtls == null && abscondDtls == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Details Not Found!"
                    };
                }

                ServiceListResponse<Status> statuses = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
                if (!statuses.IsSuccessful)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = statuses.Message
                    };
                }

                exitActivity = exitDtls != null
                    ? await m_EmployeeContext.AssociateExitActivity.FirstOrDefaultAsync(c => c.AssociateExitId == exitDtls.AssociateExitId && c.DepartmentId == activityIn.DepartmentId)
                    : await m_EmployeeContext.AssociateExitActivity.FirstOrDefaultAsync(c => c.AssociateAbscondId == abscondDtls.AssociateAbscondId && c.DepartmentId == activityIn.DepartmentId);

                exitActivity.Remarks = activityIn.Remarks;
                exitActivity.DepartmentId = activityIn.DepartmentId;
                if (exitDtls != null)
                {
                    exitActivity.AssociateExitId = exitDtls.AssociateExitId;
                    exitActivity.AssociateAbscondId = null;
                }
                else
                {
                    exitActivity.AssociateExitId = null;
                    exitActivity.AssociateAbscondId = abscondDtls.AssociateAbscondId;
                }
                exitActivity.IsActive = true;

                if (activityIn.Type == submitType)
                {
                    statusState = statuses.Items?.FirstOrDefault(x => x.StatusCode.ToLower().Trim() == AssociateExitStatusCodesNew.DepartmentActivityCompleted.ToString().ToLower());
                    exitActivity.StatusId = statusState != null ? statusState.StatusId : 0;
                }
                else
                {
                    statusState = statuses.Items?.FirstOrDefault(x => x.StatusCode.ToLower().Trim() == AssociateExitStatusCodesNew.DepartmentActivityInProgress.ToString().ToLower());
                    exitActivity.StatusId = statusState != null ? statusState.StatusId : 0;
                }

                if (exitActivity == null)
                {
                    await m_EmployeeContext.AssociateExitActivity.AddAsync(exitActivity);
                }

                m_Logger.LogInformation("Saving Exit Activity table details");
                await m_EmployeeContext.SaveChangesAsync();

                exitActivity = exitDtls != null
                   ? await m_EmployeeContext.AssociateExitActivity.FirstOrDefaultAsync(c => c.AssociateExitId == exitDtls.AssociateExitId && c.DepartmentId == activityIn.DepartmentId)
                   : await m_EmployeeContext.AssociateExitActivity.FirstOrDefaultAsync(c => c.AssociateAbscondId == abscondDtls.AssociateAbscondId && c.DepartmentId == activityIn.DepartmentId);

                foreach (UpdateActivityChecklist activityDetails in activityIn.ActivityDetails)
                {
                    AssociateExitActivityDetail exitActivityDetail = await m_EmployeeContext.AssociateExitActivityDetail.FirstOrDefaultAsync(s => s.ActivityId == activityDetails.ActivityId && s.AssociateExitActivityId == exitActivity.AssociateExitActivityId);

                    if (exitActivityDetail != null)
                    {
                        exitActivityDetail.ActivityId = activityDetails.ActivityId;
                        exitActivityDetail.AssociateExitActivityId = exitActivity.AssociateExitActivityId;
                        exitActivityDetail.CreatedBy = activityDetails.CreatedBy;
                        exitActivityDetail.IsActive = true;
                        exitActivityDetail.Remarks = activityDetails.Remarks;
                        exitActivityDetail.SystemInfo = activityDetails.SystemInfo;
                        exitActivityDetail.ActivityValue = activityDetails.ActivityValue;
                    }
                    else
                    {
                        AssociateExitActivityDetail exitActivityInsert = new AssociateExitActivityDetail
                        {
                            ActivityId = activityDetails.ActivityId,
                            AssociateExitActivityId = exitActivity.AssociateExitActivityId,
                            CreatedBy = activityDetails.CreatedBy,
                            IsActive = true,
                            Remarks = activityDetails.Remarks,
                            SystemInfo = activityDetails.SystemInfo,
                            ActivityValue = activityDetails.ActivityValue
                        };
                        await m_EmployeeContext.AssociateExitActivityDetail.AddAsync(exitActivityInsert);
                    }
                }

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in AssociateExitActivity");
                isUpdated = await m_EmployeeContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    if (exitDtls != null)
                    {
                        //Get Clearance Status by checking all the stake holders
                        ServiceResponse<bool> readyForCleranceResponse = await m_AssociateExitService.AssociateClearanceStatus(activityIn.EmployeeId);
                        if (readyForCleranceResponse.IsSuccessful && readyForCleranceResponse.Item)
                        {
                            m_Logger.LogInformation("Updating AssociateExit - Status column to ReadyForClearance. Got clerance from all stakeholders");
                            exitDtls.StatusId = statuses.Items.FirstOrDefault(x => x.StatusCode.ToLower().Trim() == AssociateExitStatusCodesNew.ReadyForClearance.ToString().ToLower()).StatusId;
                            _ = await m_EmployeeContext.SaveChangesAsync();
                        }

                        if (activityIn.Type == submitType)
                        {
                            ServiceResponse<int> notification = await m_AssociateExitService.AssociateExitSendNotification(exitDtls.EmployeeId, Convert.ToInt32(NotificationType.Completed), activityIn.DepartmentId, null);
                            if (!notification.IsSuccessful)
                            {
                                return response = new ServiceResponse<int>()
                                {
                                    Item = 0,
                                    IsSuccessful = false,
                                    Message = notification.Message
                                };
                            }
                        }
                    }
                    else
                    {
                        int cnt = 0;
                        var activitesResult = await m_EmployeeContext.AssociateExitActivity.Where(w => w.AssociateAbscondId == abscondDtls.AssociateAbscondId).Select(x => x).ToListAsync();

                        if (activitesResult?.Count > 0)
                        {
                            foreach (var activity in activitesResult)
                            {
                                cnt += activity.StatusId == Convert.ToInt32(AssociateExitStatusCodesNew.DepartmentActivityCompleted) ? 0 : 1;
                            }
                        }

                        if (cnt == 0)
                        {
                            abscondDtls.StatusId = statuses.Items.FirstOrDefault(x => x.StatusCode.ToLower().Trim() == AssociateExitStatusCodesNew.ReadyForClearance.ToString().ToLower().Trim()).StatusId;
                            _ = await m_EmployeeContext.SaveChangesAsync();
                        }

                    }

                    m_Logger.LogInformation("Associate Exit Activity updated successfully.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = isUpdated,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("No Associate Exit Activity Updated.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Associate Exit Activity Updated."
                    };
                }
            }
            catch (Exception ex)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Error Occured While Updating Database"
                };
            }
        }
        #endregion

        #region GetDepartmentActivitiesByProjectId
        /// <summary>
        /// This method Gets Department Activities by employeeId and departmentId
        /// </summary>
        /// <param name="employeeId">employeeId</param>
        /// <param name="departmentId">departmentId</param>
        /// <returns>DepartmentActivities</returns>
        public async Task<ServiceResponse<Activities>> GetDepartmentActivitiesByProjectId(int employeeId, int? departmentId = null)
        {
            ServiceResponse<Activities> response;
            ServiceListResponse<Activity> activities = null;
            ServiceListResponse<Department> departments = null;

            ServiceListResponse<Status> statuses = null;
            List<GetActivityChecklist> activityChecklists = null;
            if (employeeId == 0)
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
                m_Logger.LogInformation("Checking if EmployeeExit Details exists");

                int resignedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);
                int abscondedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Absconded);

                var exitDtls = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(pa => pa.EmployeeId == employeeId && ((pa.IsActive == true)
                                                                                        || (pa.StatusId == resignedStatus && pa.IsActive == false)));
                var abscondDtls = await m_EmployeeContext.AssociateAbscond.FirstOrDefaultAsync(pa => pa.AssociateId == employeeId && ((pa.IsActive == true)
                                                                                        || (pa.StatusId == abscondedStatus && pa.IsActive == false)));
                if (exitDtls == null && abscondDtls == null)
                {
                    response = new ServiceResponse<Activities>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = "No Such Record Exists !"
                    };
                }
                else
                {
                    m_Logger.LogInformation("Employee Exit details Found");
                    response = new ServiceResponse<Activities>();
                    try
                    {
                        m_Logger.LogInformation("Fetching Data");

                        activities = await m_OrgService.GetExitActivitiesByDepartment();
                        if (!activities.IsSuccessful)
                        {
                            return response = new ServiceResponse<Activities>()
                            {
                                Item = null,
                                IsSuccessful = false,
                                Message = "Error Occured While Fetching Exit Activities"
                            };
                        }

                        departments = await m_OrgService.GetAllDepartments();
                        if (!departments.IsSuccessful)
                        {
                            return response = new ServiceResponse<Activities>()
                            {
                                Item = null,
                                IsSuccessful = false,
                                Message = "Error Occured While Fetching Departments"
                            };
                        }

                        statuses = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
                        if (!statuses.IsSuccessful)
                        {
                            return response = new ServiceResponse<Activities>()
                            {
                                Item = null,
                                IsSuccessful = false,
                                Message = "Error Occured While Fetching Statuses of Associate Category"
                            };
                        }

                        if (activities.Items == null || (activities.Items != null && activities.Items.Count <= 0))
                        {
                            response.Item = null;
                            response.IsSuccessful = false;
                            response.Message = "Activities for AssociateExit category not found.";
                        }

                        if (departmentId != null)
                        {
                            var exitActivity = exitDtls != null
                                ? await m_EmployeeContext.AssociateExitActivity.FirstOrDefaultAsync(st => st.AssociateExitId == exitDtls.AssociateExitId && st.DepartmentId == departmentId)
                                : await m_EmployeeContext.AssociateExitActivity.FirstOrDefaultAsync(st => st.AssociateAbscondId == abscondDtls.AssociateAbscondId && st.DepartmentId == departmentId);
                            if (exitActivity == null)
                            {
                                return response = new ServiceResponse<Activities>()
                                {
                                    Item = null,
                                    IsSuccessful = false,
                                    Message = "Error Occured While Fetching Exit Activities"
                                };
                            }

                            Department projdept = null;
                            Status projstatus = null;

                            Activities ac = new Activities
                            {
                                EmployeeId = employeeId,
                                DepartmentId = exitActivity.DepartmentId,
                                Remarks = exitActivity.Remarks,
                                StatusId = exitActivity.StatusId
                            };

                            projdept = departments.Items.FirstOrDefault(q => q.DepartmentId == departmentId);
                            if (projdept == null)
                            {
                                return response = new ServiceResponse<Activities>()
                                {
                                    Item = null,
                                    IsSuccessful = false,
                                    Message = "Error Occured While Fetching Departments"
                                };
                            }

                            projstatus = statuses.Items.FirstOrDefault(q => q.StatusId == ac.StatusId);
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
                            ac.StatusCode = projstatus.StatusCode;

                            activityChecklists = await (from deptact in m_EmployeeContext.AssociateExitActivity
                                                        join act in m_EmployeeContext.AssociateExitActivityDetail
                                                        on deptact.AssociateExitActivityId equals act.AssociateExitActivityId
                                                        where (exitDtls != null ? deptact.AssociateExitId == exitDtls.AssociateExitId
                                                        : deptact.AssociateAbscondId == abscondDtls.AssociateAbscondId)
                                                        && deptact.DepartmentId == departmentId
                                                        select new GetActivityChecklist()
                                                        {
                                                            ActivityId = act.ActivityId,
                                                            ActivityValue = act.ActivityValue,
                                                            Remarks = act.Remarks,
                                                        }).OrderBy(x => x.ActivityId).ToListAsync();

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

        #region GetDepartmentActivitiesForHRA
        /// <summary>
        /// Gets Department Activities by employeeId
        /// </summary>
        /// <param name="employeeId">employeeId</param>
        /// <returns>DepartmentActivities</returns>
        public async Task<ServiceListResponse<Activities>> GetDepartmentActivitiesForHRA(int employeeId)
        {
            ServiceListResponse<Activities> response;
            List<Activities> activities = new List<Activities>();

            if (employeeId == 0)
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
                    int resignedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);
                    int abscondedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Absconded);

                    List<int> departmentIds = await m_EmployeeContext.AssociateExitActivity
                       .Where(x => (x.AssociateExit.EmployeeId == employeeId && ((x.AssociateExit.IsActive == true)
                       || (x.AssociateExit.StatusId == resignedStatus && x.AssociateExit.IsActive == false)))
                       || (x.AssociateAbscond.AssociateId == employeeId && ((x.AssociateAbscond.IsActive == true)
                       || (x.AssociateAbscond.StatusId == resignedStatus && x.AssociateAbscond.IsActive == false))))
                       .Select(x => x.DepartmentId).ToListAsync();

                    if (departmentIds == null || departmentIds.Count == 0)
                    {
                        return response = new ServiceListResponse<Activities>()
                        {
                            Items = null,
                            IsSuccessful = false,
                            Message = "Department Id's Not Found"
                        };
                    }
                    else
                    {
                        foreach (int Id in departmentIds)
                        {
                            Activities ac = new Activities();
                            ServiceResponse<Activities> activity = await GetDepartmentActivitiesByProjectId(employeeId, Id);

                            ac = activity.Item;
                            activities.Add(ac);
                        }
                    }

                    response = new ServiceListResponse<Activities>
                    {
                        Items = activities,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                catch (Exception ex)
                {
                    response = new ServiceListResponse<Activities>
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "Error occured while fetching the data"
                    };
                    m_Logger.LogError($"Error occured in GetDepartmentActivitiesForPM() method {ex.StackTrace}");
                }
            }

            return response;
        }
        #endregion
    }
}
