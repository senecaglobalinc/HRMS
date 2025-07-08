using AutoMapper;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to get Prospective Associate details
    /// To Create Prospective Associate
    /// To Update prospective Associate
    /// </summary>
    public class TransitionPlanService : ITransitionPlanService
    {
        #region Global Varibles

        private readonly ILogger<TransitionPlanService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;
        private readonly IAssociateExitService m_AssociateExitService;
        #endregion

        #region Constructor
        public TransitionPlanService(EmployeeDBContext employeeDBContext,
            ILogger<TransitionPlanService> logger,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints,
            IOrganizationService orgService,
            IProjectService projectService,
            IAssociateExitService associateExitService)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssociateExit, AssociateExit>();
                cfg.CreateMap<HRMS.Employee.Entities.Employee, HRMS.Employee.Entities.Employee>();
                cfg.CreateMap<TransitionDetail, TransitionPlan>();
            });
            m_mapper = config.CreateMapper();
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
            m_OrgService = orgService;
            m_ProjectService = projectService;
            m_AssociateExitService = associateExitService;
        }

        #endregion

        #region UpdateTransitionPlan
        /// <summary>
        /// Updates Transition Plan
        /// </summary>
        /// <param name="projectIn"></param>
        /// <returns>Integer value 0-represents Unsuccessful Submission and 1-represents Successful Submission</returns>
        public async Task<ServiceResponse<int>> UpdateTransitionPlan(TransitionDetail projectIn)
        {
            ServiceResponse<int> response;
            try
            {
                int isUpdated = 0;

                m_Logger.LogInformation("Calling \"UpdateTransitionPlan\" method in TransitionPlanService");

                var exitDtls = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(c => c.EmployeeId == projectIn.EmployeeId && c.IsActive == true);
                if (exitDtls == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        IsSuccessful = false,
                        Message = "No Such Employee Exit exists"
                    };
                }

                if (projectIn.TransitionNotRequired == true)
                {
                    exitDtls.TransitionRequired = false;
                    exitDtls.TransitionRemarks = projectIn.Remarks;
                    exitDtls.StatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress);
                }
                else
                {
                    var activityIdCount = projectIn.UpdateTransitionDetail.GroupBy(x => x.ActivityId)
                                         .Where(g => g.Count() > 1)
                                         .Select(y => y.Key)
                                         .ToList();

                    if (activityIdCount.Count > 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Duplicate tasks added in the KT plan."
                        };
                    }

                    TransitionPlan transitionplan = await m_EmployeeContext.TransitionPlan.FirstOrDefaultAsync(c => c.AssociateExitId == exitDtls.AssociateExitId);
                    if (transitionplan == null)
                    {
                        transitionplan = new TransitionPlan();
                        m_mapper.Map<TransitionDetail, TransitionPlan>(projectIn, transitionplan);
                        transitionplan.AssociateExitId = exitDtls.AssociateExitId;
                        transitionplan.IsActive = true;
                        await m_EmployeeContext.TransitionPlan.AddAsync(transitionplan);
                    }
                    else
                    {
                        m_Logger.LogInformation("Transition Plan found for update.");
                        m_mapper.Map<TransitionDetail, TransitionPlan>(projectIn, transitionplan);
                    }
                    m_Logger.LogInformation("Saving Transition Plan details");

                    isUpdated = await m_EmployeeContext.SaveChangesAsync();

                    foreach (UpdateTransitionDetail activityDetails in projectIn.UpdateTransitionDetail)
                    {
                        TransitionPlanDetail transitionPlanDtls = m_EmployeeContext.TransitionPlanDetail.Where(s => s.ActivityId == activityDetails.ActivityId && s.TransitionPlanId == transitionplan.TransitionPlanId).FirstOrDefault();

                        if (transitionPlanDtls != null)
                        {
                            transitionPlanDtls.TransitionPlanId = transitionplan.TransitionPlanId;
                            transitionPlanDtls.ActivityId = activityDetails.ActivityId;
                            transitionPlanDtls.CreatedBy = activityDetails.CreatedBy;
                            transitionPlanDtls.IsActive = true;
                            transitionPlanDtls.Remarks = activityDetails.Remarks;
                            transitionPlanDtls.SystemInfo = activityDetails.SystemInfo;
                            transitionPlanDtls.StartDate = activityDetails.StartDate;
                            transitionPlanDtls.EndDate = activityDetails.EndDate;
                            transitionPlanDtls.ActivityDescription = activityDetails.ActivityDescription;
                            transitionPlanDtls.Status = activityDetails.Status;
                        }
                        else
                        {
                            TransitionPlanDetail transitionPlanInsert = new TransitionPlanDetail
                            {
                                TransitionPlanId = transitionplan.TransitionPlanId,
                                ActivityId = activityDetails.ActivityId,
                                CreatedBy = activityDetails.CreatedBy,
                                IsActive = true,
                                Remarks = activityDetails.Remarks,
                                SystemInfo = activityDetails.SystemInfo,
                                StartDate = activityDetails.StartDate,
                                EndDate = activityDetails.EndDate,
                                ActivityDescription = activityDetails.ActivityDescription,
                                Status = activityDetails.Status
                            };

                            await m_EmployeeContext.TransitionPlanDetail.AddAsync(transitionPlanInsert);
                        }
                    }

                    m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in TransitionPlan");

                    if (projectIn.Type == "TLSubmit" && transitionplan.StatusId == 0)
                    {
                        ServiceResponse<Status> statuses = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.KTPlanInProgress.ToString());
                        if (!statuses.IsSuccessful)
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "KTPlan In Progress status not found "
                            };
                        }

                        exitDtls.StatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress);
                        transitionplan.StatusId = statuses.Item.StatusId;
                    }
                    else if (projectIn.Type == "AssociateSubmit")
                    {
                        ServiceResponse<Status> statuses = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.KTPlanSubmitted.ToString());
                        if (!statuses.IsSuccessful)
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "KTPlan Submitted status not found "
                            };
                        }

                        exitDtls.StatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress);
                        transitionplan.StatusId = statuses.Item.StatusId;
                    }
                    else if (projectIn.Type == "TLComplete")
                    {
                        ServiceResponse<Status> statuses = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.KTPlanCompleted.ToString());
                        if (!statuses.IsSuccessful)
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "KTPlan completed status not found "
                            };
                        }

                        exitDtls.StatusId = Convert.ToInt32(AssociateExitStatusCodesNew.ResignationInProgress);
                        transitionplan.StatusId = statuses.Item.StatusId;
                    }
                }

                isUpdated += await m_EmployeeContext.SaveChangesAsync();

                if (isUpdated > 0 && projectIn.Type == "TLComplete")
                {
                    //Get Clearance Status by checking all the stake holders
                    ServiceResponse<bool> readyForCleranceResponse = await m_AssociateExitService.AssociateClearanceStatus(projectIn.EmployeeId);
                    if (readyForCleranceResponse.IsSuccessful && readyForCleranceResponse.Item)
                    {
                        m_Logger.LogInformation("Updating AssociateExit - Status column to ReadyForClearance. Got clerance from all stakeholders");

                        ServiceResponse<Status> statuses = await m_OrgService.GetStatusByCategoryAndStatusCode(CategoryMaster.AssociateExit.ToString(), AssociateExitStatusCodesNew.ReadyForClearance.ToString());
                        exitDtls.StatusId = statuses.Item.StatusId;
                        _ = await m_EmployeeContext.SaveChangesAsync();
                    }
                }

                if (isUpdated > 0)
                {
                    if (projectIn.Type == "TLSubmit" && projectIn.TransitionNotRequired == false)
                    {
                        ServiceResponse<int> notification = await m_AssociateExitService.AssociateExitSendNotification(exitDtls.EmployeeId, Convert.ToInt32(NotificationType.KTPlanInProgress), null, exitDtls.ProjectId);
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
                    else if (projectIn.Type == "AssociateSubmit")
                    {
                        ServiceResponse<int> notification = await m_AssociateExitService.AssociateExitSendNotification(exitDtls.EmployeeId, Convert.ToInt32(NotificationType.KTPlanSubmitted), null, exitDtls.ProjectId);
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
                    else if (projectIn.Type == "TLComplete" && projectIn.TransitionNotRequired == false)
                    {
                        ServiceResponse<int> notification = await m_AssociateExitService.AssociateExitSendNotification(exitDtls.EmployeeId, Convert.ToInt32(NotificationType.KTPlanCompleted), null, exitDtls.ProjectId);
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

                    m_Logger.LogInformation("Transition plan updated successfully.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = isUpdated,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("No tarnsition plan updated.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Tarnsition Plan Updated."
                    };
                }
            }
            catch (Exception ex)
            {
                if (((PostgresException)ex.GetBaseException()).ConstraintName == StringConstants.TransitionPlanDetailUniqueConstraint)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Duplicate tasks added in the KT plan."
                    };
                }

                throw ex;
            }
        }
        #endregion

        #region GetTransitionPlanByAssociateIdandProjectId
        /// <summary>
        /// This method Gets Transition Plan Details by employeeId and projectId
        /// </summary> 
        /// <param name="employeeId">employeeId</param>
        /// <param name="projectId">projectId</param>
        /// <returns>TransitionDetails</returns>
        public async Task<ServiceResponse<TransitionDetail>> GetTransitionPlanByAssociateIdandProjectId(int employeeId, int projectId)
        {
            ServiceResponse<TransitionDetail> response;
            List<UpdateTransitionDetail> activityChecklists = null;
            if (employeeId == 0)
            {
                response = new ServiceResponse<TransitionDetail>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            else
            {
                m_Logger.LogInformation("Checking if AssociateExit exists");

                int resignedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);
                var exitDtls = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(pa => pa.EmployeeId == employeeId && ((pa.IsActive == true) || (pa.StatusId == resignedStatus && pa.IsActive == false)));
                if (exitDtls == null)
                {
                    response = new ServiceResponse<TransitionDetail>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = "No Such AssociateExit exists"
                    };
                }
                else
                {
                    m_Logger.LogInformation("Transition details Found");
                    response = new ServiceResponse<TransitionDetail>();
                    try
                    {
                        m_Logger.LogInformation("Fetching Data");

                        TransitionPlan transitionPlan = await m_EmployeeContext.TransitionPlan.FirstOrDefaultAsync(st => st.AssociateExitId == exitDtls.AssociateExitId);
                        if (transitionPlan == null)
                        {
                            return response = new ServiceResponse<TransitionDetail>()
                            {
                                Item = null,
                                IsSuccessful = false,
                                Message = "Error Occured While Fetching Transition Plan Details"
                            };
                        }

                        ServiceResponse<Project> project = await m_ProjectService.GetProjectByID(exitDtls.ProjectId??0);
                        var exitStatus = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
                        if (exitStatus.Items == null)
                        {
                            return response = new ServiceResponse<TransitionDetail>()
                            {
                                Item = null,
                                IsSuccessful = false,
                                Message = "Error Occured While Fetching Statuses"
                            };
                        }
                        var status = exitStatus.Items.FirstOrDefault(st => st.StatusId == transitionPlan.StatusId);

                        TransitionDetail ac = new TransitionDetail
                        {
                            EmployeeId = employeeId,
                            ProjectId = exitDtls.ProjectId
                        };

                        if (projectId != 0)
                            ac.ProjectName = project.Item.ProjectName;
                        ac.KnowledgeTransferredRemarks = transitionPlan.KnowledgeTransaferredRemarks;
                        ac.Status = status.StatusCode;
                        ac.StatusDesc = status.StatusDescription;
                        ac.KnowledgeTransferred = transitionPlan.KnowledgeTransferred;
                        ac.Others = transitionPlan.Others;
                        ac.TransitionFrom = transitionPlan.TransitionFrom;
                        ac.TransitionTo = transitionPlan.TransitionTo;
                        ac.StartDate = transitionPlan.StartDate;
                        ac.EndDate = transitionPlan.EndDate;

                        activityChecklists = (from deptact in m_EmployeeContext.TransitionPlan
                                              join act in m_EmployeeContext.TransitionPlanDetail
                                              on deptact.TransitionPlanId equals act.TransitionPlanId
                                              where (deptact.AssociateExitId == exitDtls.AssociateExitId)
                                              select new UpdateTransitionDetail()
                                              {
                                                  ActivityId = act.ActivityId,
                                                  Remarks = act.Remarks,
                                                  StartDate = act.StartDate,
                                                  EndDate = act.EndDate,
                                                  ActivityDescription = act.ActivityDescription,
                                                  Status = act.Status
                                              }).ToList();

                        ac.UpdateTransitionDetail = activityChecklists;
                        response.Item = ac;
                        response.IsSuccessful = true;
                        response.Message = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        response.Item = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the data";
                        m_Logger.LogError($"Error occured in GetTransitionPlanByAssociateIdandProjectId() method {ex.StackTrace}");
                    }
                }
            }
            return response;
        }
        #endregion

        #region GetTransitionPlanByAssociateId
        /// <summary>
        /// This method Gets Transition Plan Details by employeeId 
        /// </summary> 
        /// <param name="employeeId">employeeId</param>
        /// <returns>TransitionDetails</returns>
        public async Task<ServiceListResponse<TransitionDetail>> GetTransitionPlanByAssociateId(int employeeId)
        {
            ServiceListResponse<TransitionDetail> response;
            List<UpdateTransitionDetail> activityChecklists = null;
            if (employeeId == 0)
            {
                response = new ServiceListResponse<TransitionDetail>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            else
            {
                m_Logger.LogInformation("Checking if AssociateExit exists");
                int resignedStatus = Convert.ToInt32(AssociateExitStatusCodesNew.Resigned);
                var exitDtls = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(pa => pa.EmployeeId == employeeId && ((pa.IsActive == true)
                                                                                        || (pa.StatusId == resignedStatus && pa.IsActive == false)));
                if (exitDtls == null)
                {
                    response = new ServiceListResponse<TransitionDetail>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Such AssociateExit exists"
                    };
                }
                else
                {
                    m_Logger.LogInformation("Transition details Found");
                    response = new ServiceListResponse<TransitionDetail>();
                    try
                    {
                        m_Logger.LogInformation("Fetching Data");

                        var empDtls = await m_EmployeeContext.Employees.FirstOrDefaultAsync(c => c.EmployeeId == employeeId);
                        if (empDtls != null)
                        {
                            var transitionPlans = await m_EmployeeContext.TransitionPlan.Where(st => st.AssociateExitId == exitDtls.AssociateExitId).ToListAsync();
                            if (transitionPlans.Count == 0)
                            {
                                return response = new ServiceListResponse<TransitionDetail>()
                                {
                                    Items = new List<TransitionDetail>(),
                                    IsSuccessful = true,
                                    Message = "No Transition Plan Found"
                                };
                            }

                            var exitStatus = await m_OrgService.GetStatusesByCategoryName(CategoryMaster.AssociateExit.ToString());
                            if (exitStatus.Items == null)
                            {
                                return response = new ServiceListResponse<TransitionDetail>()
                                {
                                    Items = null,
                                    IsSuccessful = false,
                                    Message = "Error Occured While Fetching Statuses"
                                };
                            }

                            List<TransitionDetail> transitionDetails = new List<TransitionDetail>();
                            foreach (TransitionPlan plan in transitionPlans)
                            {
                                ServiceResponse<Project> project = await m_ProjectService.GetProjectByID(exitDtls.ProjectId??0);
                                if (project.Item == null && empDtls.DepartmentId == Convert.ToInt32(DepartmentCodes.TrainingDepartment))
                                {
                                    return response = new ServiceListResponse<TransitionDetail>()
                                    {
                                        Items = null,
                                        IsSuccessful = false,
                                        Message = project.Message
                                    };
                                }
                                var status = exitStatus.Items.FirstOrDefault(st => st.StatusId == plan.StatusId);

                                TransitionDetail ac = new TransitionDetail
                                {
                                    EmployeeId = employeeId,
                                    ProjectId = exitDtls.ProjectId??0
                                };

                                if (ac.ProjectId != 0)
                                    ac.ProjectName = project.Item.ProjectName;
                                ac.KnowledgeTransferredRemarks = plan.KnowledgeTransaferredRemarks;
                                ac.Status = status.StatusCode;
                                ac.StatusDesc = status.StatusDescription;
                                ac.KnowledgeTransferred = plan.KnowledgeTransferred;
                                ac.Others = plan.Others;
                                ac.TransitionFrom = plan.TransitionFrom;
                                ac.TransitionTo = plan.TransitionTo;
                                ac.StartDate = plan.StartDate;
                                ac.EndDate = plan.EndDate;

                                activityChecklists = await (from deptact in m_EmployeeContext.TransitionPlan
                                                            join act in m_EmployeeContext.TransitionPlanDetail
                                                            on deptact.TransitionPlanId equals act.TransitionPlanId
                                                            where (deptact.AssociateExitId == exitDtls.AssociateExitId)
                                                            select new UpdateTransitionDetail()
                                                            {
                                                                ActivityId = act.ActivityId,
                                                                Remarks = act.Remarks,
                                                                StartDate = act.StartDate,
                                                                EndDate = act.EndDate,
                                                                ActivityDescription = act.ActivityDescription,
                                                                Status = act.Status
                                                            }).ToListAsync();

                                ac.UpdateTransitionDetail = activityChecklists;
                                transitionDetails.Add(ac);
                            }

                            response.Items = transitionDetails;
                            response.IsSuccessful = true;
                            response.Message = string.Empty;
                        }
                        else
                        {
                            response = new ServiceListResponse<TransitionDetail>()
                            {
                                Items = null,
                                IsSuccessful = false,
                                Message = "No Such Associate exists"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Items = null;
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the data";
                        m_Logger.LogError($"Error occured in GetTransitionPlanByAssociateIdandProjectId() method {ex.StackTrace}");
                    }
                }
            }
            return response;
        }
        #endregion

        #region DeleteTransitionActivity
        /// <summary>
        /// This method Deletes the Transition Activities
        /// </summary> 
        /// <param name="employeeId">employeeId</param>
        /// <param name="projectId">projectId</param>
        /// <param name="activityId">activityId</param>
        /// <returns>Integer value</returns>
        public async Task<ServiceResponse<int>> DeleteTransitionActivity(int employeeId, int projectId, int activityId)
        {
            ServiceResponse<int> response;

            if (employeeId == 0 || activityId == 0)
            {
                response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            else
            {
                m_Logger.LogInformation("Checking if AssociateExit exists");
                var exitDtls = await m_EmployeeContext.AssociateExit.FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.IsActive == true);
                if (exitDtls == null)
                {
                    response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Such AssociateExit exists"
                    };
                }
                else
                {
                    m_Logger.LogInformation("Associate Exit details Found");
                    response = new ServiceResponse<int>();
                    try
                    {
                        var empDtls = await m_EmployeeContext.Employees.FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.IsActive == true);
                        if (empDtls != null)
                        {
                            if (projectId == 0 && empDtls.DepartmentId != Convert.ToInt32(DepartmentCodes.TrainingDepartment))
                            {
                                response = new ServiceResponse<int>()
                                {
                                    Item = 0,
                                    IsSuccessful = false,
                                    Message = "No Such project exists"
                                };
                            }
                            m_Logger.LogInformation("Fetching Data");

                            var transitionPlan = await m_EmployeeContext.TransitionPlan.FirstOrDefaultAsync(st => st.AssociateExitId == exitDtls.AssociateExitId);
                            if (transitionPlan == null)
                            {
                                return response = new ServiceResponse<int>()
                                {
                                    Item = 0,
                                    IsSuccessful = false,
                                    Message = "Error Occured While Fetching Transition Plan Details"
                                };
                            }

                            var transitionPlanDetail = await m_EmployeeContext.TransitionPlanDetail.FirstOrDefaultAsync(st => st.TransitionPlanId == transitionPlan.TransitionPlanId
                                                                                                                              && st.ActivityId == activityId);
                            if (transitionPlanDetail == null)
                            {
                                return response = new ServiceResponse<int>()
                                {
                                    Item = 0,
                                    IsSuccessful = false,
                                    Message = $"No Such Transition Plan Found for Activity Id {activityId}"
                                };
                            }

                            m_EmployeeContext.TransitionPlanDetail.Remove(transitionPlanDetail);
                            int isDeleted = await m_EmployeeContext.SaveChangesAsync();
                            if (isDeleted > 0)
                            {
                                return response = new ServiceResponse<int>()
                                {
                                    Item = 1,
                                    IsSuccessful = true,
                                    Message = $"Deleted Successfully"
                                };
                            }
                            else
                            {
                                return response = new ServiceResponse<int>()
                                {
                                    Item = 0,
                                    IsSuccessful = false,
                                    Message = $"Error Occured While Deleting data"
                                };
                            }
                        }
                        else
                        {
                            response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "No Such project exists"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Item = 0;
                        response.IsSuccessful = false;
                        response.Message = "Error occured Deleting the data";
                        m_Logger.LogError($"Error occured in DeleteTransitionActivity() method {ex.StackTrace}");
                    }
                }
            }
            return response;


        }
        #endregion
    }
}
