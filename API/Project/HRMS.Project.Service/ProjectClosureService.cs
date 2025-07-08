using AutoMapper;
using HRMS.Common.Extensions;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
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

namespace HRMS.Project.Service
{
    public class ProjectClosureService : IProjectClosureService
    {
        #region Global Varibles

        private readonly ILogger<ProjectClosureService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_closemapper;
        private readonly IProjectService m_ProjectService;
        private IProjectManagerService m_projectManagerService;
        private IOrganizationService m_OrgService;
        private IEmployeeService m_EmployeeService;
        private IAssociateAllocationService m_associateAllocationService;
        private IProjectClosureReportService m_projectClosureReportService;
        private IProjectClosureActivityService m_projectClosureActivityService;
        private readonly IMapper m_CloseMapper;

        #endregion

        #region ProjectClosureService
        public ProjectClosureService(ILogger<ProjectClosureService> logger,
            ProjectDBContext projectContext,
            IProjectService projectService,
            IProjectManagerService projectManagerService,
            IOrganizationService orgService,
            IEmployeeService employeeService,
            IAssociateAllocationService associateAllocationService,
            IProjectClosureReportService projectClosureReportService,
            IProjectClosureActivityService projectClosureActivityService
            , IMapper mapper
            )
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            m_ProjectService = projectService;
            m_projectManagerService = projectManagerService;
            m_OrgService = orgService;
            m_EmployeeService = employeeService;
            m_associateAllocationService = associateAllocationService;
            m_projectClosureReportService = projectClosureReportService;
            m_projectClosureActivityService = projectClosureActivityService;

            var configure = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectClosureInitiationResponse, Entities.ProjectClosure>();
            });
            m_CloseMapper = configure.CreateMapper();

            m_closemapper = mapper;
        }
        #endregion

        #region ProjectClosureInitiation
        /// <summary>
        /// Initiates a Project Closure Process by taking projectData as paramenter
        /// </summary>
        /// <param name="projectData">Project information</param>
        /// <returns>Integer value 0-represent unsucessful response and >1-represent successful response</returns>
        public async Task<ServiceResponse<int>> ProjectClosureInitiation(ProjectClosureInitiationResponse projectData)
        {
            ServiceResponse<int> response;
            try
            {
                int isClosed = 0;

                m_Logger.LogInformation("Calling \"ProjectClosureInitiation\" method in ProjectService");

                ServiceResponse<Status> ClosedInitiatedStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), HRMS.Common.Enums.ProjectClosureStatusCodes.ClosureInitiated.ToString());
                if (ClosedInitiatedStatus.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Closure Initiated Status Not Found"
                    };
                }
                var project = m_ProjectContext.Projects.Find(projectData.ProjectId);

                if (project == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project not found for close."
                    };
                }
                else
                    m_Logger.LogInformation("Project - Project found for closure Initiation.");
                if (projectData.Remarks == null || projectData.Remarks == "")
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project Closure Initiation - No Reason is provided."
                    };
                }
                if (projectData.ActualEndDate.HasValue)
                {
                    var allocations = await m_associateAllocationService.GetByProjectId(projectData.ProjectId);

                    if (allocations.Items != null)
                    {
                        if (allocations.Items.Where(aa => !aa.ReleaseDate.HasValue).Count() > 0)
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Project Closure Initiation - Project has allocations can't close."
                            };
                        }

                    }
                    projectData.IsActive = true;
                    projectData.IsTransitionRequired = false;
                    project.ActualEndDate = projectData.ActualEndDate;
                    projectData.ExpectedClosureDate = projectData.ActualEndDate;
                    project.ProjectStateId = ClosedInitiatedStatus.Item.StatusId;
                    projectData.StatusId = ClosedInitiatedStatus.Item.StatusId;
                    Entities.ProjectClosure projectClosure = new Entities.ProjectClosure();
                    projectClosure = m_closemapper.Map<ProjectClosureInitiationResponse, Entities.ProjectClosure>(projectData);
                    m_ProjectContext.ProjectClosure.Add(projectClosure);
                }
                else
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project Closure Initiation - No End Date is provided."
                    };

                }
                m_Logger.LogInformation("Project Closure Initiation - Calling SaveChangesAsync method on DBContext in ProjectService");
                isClosed = await m_ProjectContext.SaveChangesAsync();


                if (isClosed > 0)
                {
                    var projectId = projectData.ProjectId;

                    var pmCreated = await m_projectClosureReportService.Create(projectId);
                    if (pmCreated.Item == 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Error occurred while creating project report."
                        };
                    }
                    var actCreated = await m_projectClosureActivityService.CreateActivityChecklist(projectId);
                    if (actCreated.Item == 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Error occurred while creating project activity checklist."
                        };
                    }
                    m_Logger.LogInformation("Project closure Initiated successfully.");
                    m_Logger.LogInformation("Sending Notifications.");
                    ServiceResponse<int> notification = await NotifyAll(projectData.ProjectId);
                    if (notification.Item == 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Error Occurred While Sending Notifications."
                        };
                    }
                    return response = new ServiceResponse<int>()
                    {
                        Item = isClosed,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("Project cannot be closed.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = isClosed,
                        IsSuccessful = false,
                        Message = string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Project Closure Initiation - Project cannot be closed.");
                throw ex;
            }

        }


        #endregion

        #region NotifyAll
        /// <summary>
        /// Notification for all Service Departments and Team Lead at the time of Project Closure Initiation
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Integer value 0-represents Unsuccessful Response and 1-Represents Successful Response</returns>
        public async Task<ServiceResponse<int>> NotifyAll(int projectId)
        {
            ServiceResponse<int> response;
            try
            {
                m_Logger.LogInformation("Calling \"NotificationConfigurations\" method in ProjectService");
                int notificationTypeId = Convert.ToInt32(HRMS.Common.Enums.NotificationType.ClosureInitiated);
                int categoryId = Convert.ToInt32(HRMS.Common.Enums.CategoryMaster.PPC);
                int? LeadId = m_ProjectContext.ProjectManagers.Where(st => st.ProjectId == projectId && st.IsActive == true).Select(st => st.LeadId).FirstOrDefault();
                if (LeadId == 0 || LeadId == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error Occured while fetching Lead Id"
                    };
                }
                var employee = await m_EmployeeService.GetEmployeeById((int)LeadId);
                if (employee.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error Occured while fetching Employee"
                    };
                }
                int userId = employee.Item.UserId.Value;
                if (userId == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = $"No User Found With Project Id {projectId}"
                    };
                }
                var users = await m_OrgService.GetUserById(userId);
                if (users.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = $"No Users Found With Project Id {projectId}"
                    };
                }
                ServiceResponse<NotificationConfiguration> notificationConfiguration = await m_OrgService.GetNotificationConfiguration(notificationTypeId, categoryId);
                if (notificationConfiguration.Item != null)
                {
                    var project = await m_ProjectService.GetProjectById(projectId);

                    if (project != null)
                    {
                        NotificationDetail notificationDetail = new NotificationDetail();
                        StringBuilder emailContent = new StringBuilder(System.Net.WebUtility.HtmlDecode(notificationConfiguration.Item.EmailContent));

                        emailContent = emailContent.Replace("{ProjectCode}", project.Item.ProjectCode)
                                                   .Replace("{ProjectName}", project.Item.ProjectName)
                                                   .Replace("{ManagerName}", project.Item.ManagerName)
                                                   .Replace("{ProjectState}", project.Item.ProjectState)
                                                   .Replace("{ProjectEndDate}", project.Item.ActualEndDate.Value.ToShortDateString());



                        if (string.IsNullOrEmpty(notificationConfiguration.Item.EmailFrom))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email From cannot be blank"
                            };
                        }
                        notificationDetail.FromEmail = notificationConfiguration.Item.EmailFrom;
                        if (string.IsNullOrEmpty(notificationConfiguration.Item.EmailTo))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email To cannot be blank"
                            };
                        }
                        // notificationDetail.ToEmail = $"{notificationConfiguration.Item.EmailTo};{users.Item.EmailAddress};   //To-do. Should be uncommented at the time of production
                        notificationDetail.ToEmail = notificationConfiguration.Item.EmailTo;  //to-do. Only for Dev purpose, should be removed in production.
                        notificationDetail.CcEmail = notificationConfiguration.Item.EmailCC;
                        notificationDetail.Subject = $"{project.Item.ProjectName} {notificationConfiguration.Item.EmailSubject}";
                        notificationDetail.EmailBody = emailContent.ToString();
                        m_OrgService.SendEmail(notificationDetail);
                    }
                }
                return response = new ServiceResponse<int>()
                {
                    Item = 1,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in NotificationConfiguration.");
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Exception occured in NotificationConfiguration."
                };
            }



        }
        #endregion

        #region SubmitForClosureApproval
        /// <summary>
        /// Submits a project for closure approval By DH.
        /// </summary>
        /// <param name="submitForClosureApproval"></param>
        /// <returns>Integer value, 0-Represents Unsuccessful Response and 1-Represents Successful Response</returns>
        public async Task<ServiceResponse<int>> SubmitForClosureApproval(SubmitForClosureApprovalRequest submitForClosureApproval)
        {
            ServiceResponse<int> response;

            if (submitForClosureApproval.projectId == 0 || string.IsNullOrEmpty(submitForClosureApproval.userRole) || submitForClosureApproval.employeeId == 0)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Invalid Request"
                };
            }
            if (submitForClosureApproval.userRole != HRMS.Common.Enums.Roles.ProgramManager.GetEnumDescription())
            {
                var approveOrRejectByDH = await ApproveOrRejectProjectClosureByDH(submitForClosureApproval.projectId, submitForClosureApproval.employeeId);
                if (approveOrRejectByDH.Item == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = approveOrRejectByDH.Message
                    };
                }
                else
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = approveOrRejectByDH.Item,
                        IsSuccessful = approveOrRejectByDH.IsSuccessful,
                        Message = approveOrRejectByDH.Message
                    };
                }
            }
            else
            {
                var category = await m_OrgService.GetCategoryByName(HRMS.Common.Enums.CategoryMaster.PPC.ToString());

                if (category == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Category not found."
                    };
                }
                //var notificationType = await m_OrgService.GetNotificationTypeByCode(Convert.ToString(HRMS.Common.Enums.NotificationType.SubmittedForClosureApproval));
                //if (notificationType == null)
                //{
                //    return response = new ServiceResponse<int>()
                //    {
                //        Item = 0,
                //        IsSuccessful = false,
                //        Message = notificationType.Message
                //    };
                //}

                var notificationConfig = await m_OrgService.GetByNotificationTypeAndCategoryId(Convert.ToInt32(HRMS.Common.Enums.NotificationType.SubmittedForClosureApproval), category.Item.CategoryMasterId);

                if (notificationConfig == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Notification Config not found."
                    };
                }

                var user = await m_OrgService.GetUserByEmail(notificationConfig.Item.EmailTo);

                if (user == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "User not found."
                    };
                }

                var emp = await m_EmployeeService.GetEmployeeByUserId(user.Item.UserId);
                if (emp == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Employee not found."
                    };
                }

                var status = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, HRMS.Common.Enums.ProjectClosureStatusCodes.SubmittedForClosureApproval.ToString());
                if (status == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Status not found."
                    };
                }
                var projectClosure = await (m_ProjectContext.ProjectClosure.Where(q => q.ProjectId == submitForClosureApproval.projectId)).FirstOrDefaultAsync();
                if (projectClosure == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No records found in project closure table"
                    };
                }
                int projectClosureId = projectClosure.ProjectClosureId;
                using (var trans = m_ProjectContext.Database.BeginTransaction())
                {
                    ProjectClosureWorkflow p_workflow = new ProjectClosureWorkflow()
                    {
                        SubmittedBy = submitForClosureApproval.employeeId,
                        SubmittedTo = emp.Item.EmployeeId,
                        SubmittedDate = DateTime.Now,
                        WorkflowStatus = status.Item.StatusId,
                        ProjectClosureId = projectClosureId,
                        Comments = null,
                        IsActive = true
                    };

                    m_ProjectContext.ProjectClosureWorkflow.Add(p_workflow);

                    var project = await (m_ProjectContext.Projects.Where(q => q.ProjectId == submitForClosureApproval.projectId)).FirstOrDefaultAsync();
                    if (project == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = $"No Project Found with the Project ID : {submitForClosureApproval.projectId}"
                        };
                    }
                    project.ProjectStateId = status.Item.StatusId;

                    var created = m_ProjectContext.SaveChanges();
                    if (created == 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = $"Error Occured while updating Database with the Project ID : {submitForClosureApproval.projectId}"
                        };
                    }
                    trans.Commit();

                    response = new ServiceResponse<int>();
                    response.Item = 1;
                    response.IsSuccessful = true;
                    response.Message = "Project submitted for Closure Approval successfully.";

                    ServiceResponse<int> notification = await NotificationConfiguration(submitForClosureApproval.projectId);
                    if (notification.Item == 0)
                    {
                        response.Item = 0;
                        response.IsSuccessful = false;
                        response.Message = "Error Occured While Sending Notifications";
                    }
                    return response;
                }
            }
        }
        #endregion

        #region RejectClosure
        /// <summary>
        /// Reject a Closure Report sent by TL.
        /// </summary>
        /// <param name="rejectClosureReport"></param>
        /// <returns>Integer Value 0-Represents Unsuccessful Response and 1-Represents Successful Response</returns>
        public async Task<ServiceResponse<int>> RejectClosure(RejectClosureReport rejectClosureReport)
        {
            ServiceResponse<int> response;
            var project = await m_ProjectContext.ProjectClosure.Where(st => st.ProjectId == rejectClosureReport.ProjectId).FirstOrDefaultAsync();
            if (project == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = $"Project Closure Not Found with Project Id { rejectClosureReport.ProjectId}"
                };
            }
            int projectClosureId = project.ProjectClosureId;
            var projectClosureReport = await m_ProjectContext.ProjectClosureReport.Where(st => st.ProjectClosureId == projectClosureId).FirstOrDefaultAsync();
            if (projectClosureReport == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = $"Project Closure Report Not Found for Project Id {rejectClosureReport.ProjectId}"
                };
            }
            try
            {
                m_Logger.LogInformation($"Project Closure Report Found for Project Id {rejectClosureReport.ProjectId}");
                ServiceResponse<Status> inProgressStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), HRMS.Common.Enums.ProjectClosureStatusCodes.InProgress.ToString());
                if (inProgressStatus.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No In Progress Status Found"
                    };
                }
                projectClosureReport.StatusId = inProgressStatus.Item.StatusId;
                projectClosureReport.RejectRemarks = rejectClosureReport.RejectRemarks;
                m_Logger.LogInformation("Updating the database");
                int isUpdated = await m_ProjectContext.SaveChangesAsync();
                if (isUpdated > 0)
                {
                    ServiceResponse<int> notification = await RejectionNotificationForTL(rejectClosureReport.ProjectId);
                    if (notification.Item == 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Error Occured While Sending Notifications"
                        };
                    }
                    return response = new ServiceResponse<int>()
                    {
                        Item = 1,
                        IsSuccessful = true,
                        Message = "Project Closure Report Rejection Successful"
                    };
                }
                else
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project Closure Report Couldn't be Submitted"
                    };
                }
            }
            catch (Exception ex)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = $"Error occured while Project Closure Report Rejection for Project Id {rejectClosureReport.ProjectId}"
                };
            }

        }
        #endregion

        #region NotificationConfiguration
        /// <summary>
        /// Notification to Delivery Head for Closure Approval
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Integer Value 0-Represents Unsuccessful Response and 1-Represents Successful Response</returns>
        public async Task<ServiceResponse<int>> NotificationConfiguration(int projectId)
        {
            ServiceResponse<int> response;
            try
            {
                m_Logger.LogInformation("Calling \"NotificationConfiguration\" method in ProjectClosureService");
                int notificationTypeId = Convert.ToInt32(HRMS.Common.Enums.NotificationType.SubmittedForClosureApproval);
                int categoryId = Convert.ToInt32(HRMS.Common.Enums.CategoryMaster.PPC);
                ServiceResponse<NotificationConfiguration> notificationConfiguration = await m_OrgService.GetNotificationConfiguration(notificationTypeId, categoryId);

                if (notificationConfiguration.Item != null)
                {
                    var project = await m_ProjectService.GetProjectById(projectId);

                    if (project != null)
                    {
                        NotificationDetail notificationDetail = new NotificationDetail();
                        StringBuilder emailContent = new StringBuilder(System.Net.WebUtility.HtmlDecode(notificationConfiguration.Item.EmailContent));



                        emailContent = emailContent.Replace("{ProjectCode}", project.Item.ProjectCode)
                                                   .Replace("{ProjectName}", project.Item.ProjectName)
                                                   .Replace("{ManagerName}", project.Item.ManagerName)
                                                   .Replace("{ProjectState}", project.Item.ProjectState)
                                                   .Replace("{ProjectEndDate}", project.Item.ActualEndDate.Value.ToShortDateString());

                        if (string.IsNullOrEmpty(notificationConfiguration.Item.EmailFrom))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email From cannot be blank"
                            };
                        }
                        notificationDetail.FromEmail = notificationConfiguration.Item.EmailFrom;

                        if (string.IsNullOrEmpty(notificationConfiguration.Item.EmailTo))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email To cannot be blank"
                            };
                        }
                        notificationDetail.ToEmail = notificationConfiguration.Item.EmailTo;

                        notificationDetail.CcEmail = notificationConfiguration.Item.EmailCC;



                        notificationDetail.Subject = $"{project.Item.ProjectName} {notificationConfiguration.Item.EmailSubject}";
                        notificationDetail.EmailBody = emailContent.ToString();
                        m_OrgService.SendEmail(notificationDetail);
                    }
                }
                return response = new ServiceResponse<int>()
                {
                    Item = 1,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in NotificationConfiguration.");

                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Exception occured in NotificationConfiguration."
                };
            }



        }
        #endregion 

        #region ApproveOrRejectProjectClosureByDH
        /// <summary>
        /// Approve Project Closure By Department Head
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="employeeId">Employee Id of Delivery Head</param>
        /// <returns>Integer Value 0-Represents Unsuccessful Response and 1-Represents Successful Response</returns>
        public async Task<ServiceResponse<int>> ApproveOrRejectProjectClosureByDH(int projectId, int employeeId)
        {
            ServiceResponse<int> response;

            if (projectId == 0 || employeeId == 0)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Invalid Request"
                };
            }

            var category = await m_OrgService.GetCategoryByName(HRMS.Common.Enums.CategoryMaster.PPC.ToString());

            if (category.Item == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = category.Message
                };
            }

            var workFlowstatus = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, HRMS.Common.Enums.ProjectClosureStatusCodes.SubmittedForClosureApproval.ToString());
            if (workFlowstatus.Item == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = workFlowstatus.Message
                };
            }

            var projectClosure = await (m_ProjectContext.ProjectClosure.Where(q => q.ProjectId == projectId)).FirstOrDefaultAsync();
            if (projectClosure == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Project Closure not found"
                };
            }
            int projectClosureId = projectClosure.ProjectClosureId;
            var projectClosureWorkFlow = await m_ProjectContext.ProjectClosureWorkflow
                                        .Where(q => q.ProjectClosureId == projectClosureId && q.WorkflowStatus == workFlowstatus.Item.StatusId)
                                        .FirstOrDefaultAsync();
            if (projectClosureWorkFlow == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Project Closure Workflow not found"
                };
            }

            var toUserEmp = await m_EmployeeService.GetActiveEmployeeById(projectClosureWorkFlow.SubmittedBy);
            if (toUserEmp.Item == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = toUserEmp.Message
                };
            }


            var user = await m_OrgService.GetUserById(toUserEmp.Item.UserId.Value);
            if (user.Item == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = user.Message
                };
            }

            var submittedToEmp = await m_EmployeeService.GetEmployeeByUserId(toUserEmp.Item.UserId.Value);
            if (submittedToEmp.Item == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = submittedToEmp.Message
                };
            }

            var wfStatusByDH = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, HRMS.Common.Enums.ProjectClosureStatusCodes.ClosureApprovedByDH.ToString());
            if (wfStatusByDH.Item == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = wfStatusByDH.Message
                };
            }

            var projectState = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, HRMS.Common.Enums.ProjectClosureStatusCodes.Closed.ToString());
            if (projectState.Item == null)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = projectState.Message
                };
            }
            using (var trans = m_ProjectContext.Database.BeginTransaction())
            {
                ProjectClosureWorkflow p_workflow = new ProjectClosureWorkflow()
                {
                    SubmittedBy = employeeId,
                    SubmittedTo = submittedToEmp.Item.EmployeeId,
                    SubmittedDate = DateTime.Now,
                    WorkflowStatus = wfStatusByDH.Item.StatusId,
                    ProjectClosureId = projectClosureId,
                    Comments = null,
                    IsActive = true
                };

                m_ProjectContext.ProjectClosureWorkflow.Add(p_workflow);

                var project = await (m_ProjectContext.Projects.Where(q => q.ProjectId == projectId)).FirstOrDefaultAsync();
                if (project == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = $"No Project Found with Project ID {projectId}"
                    };
                }
                project.ProjectStateId = projectState.Item.StatusId;

                var created = m_ProjectContext.SaveChanges();
                trans.Commit();
                if (created == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Failed to approve a project closure.."
                    };
                }
                else
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = created,
                        IsSuccessful = true,
                        Message = "Project Closure Approved successfully.",
                    };
                }
            }
        }
        #endregion

        #region ApproveOrRejectClosureByDH
        /// <summary>
        /// Approves or Rejects a project by DH
        /// </summary>
        /// <param name="approveOrRejectClosureRequest"></param>
        /// <returns>Integer Value 0-Represents Unsuccessful Response and 1-Represents Successful Response</returns>
        public async Task<ServiceResponse<int>> ApproveOrRejectClosureByDH(ApproveOrRejectClosureRequest approveOrRejectClosureRequest)
        {
            ServiceResponse<int> response;
            string approveStatus = "Approve";
            if (approveOrRejectClosureRequest.projectId == 0 || string.IsNullOrEmpty(approveOrRejectClosureRequest.status) || approveOrRejectClosureRequest.employeeId == 0)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Invalid Request"
                };
            }
            if (approveOrRejectClosureRequest.status == approveStatus)
            {
                var approveOrRejectByDH = await ApproveOrRejectProjectClosureByDH(approveOrRejectClosureRequest.projectId, approveOrRejectClosureRequest.employeeId);
                if (approveOrRejectByDH.Item == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = approveOrRejectByDH.Message
                    };
                }
                else
                {
                    ServiceResponse<int> notification = await m_projectClosureReportService.NotificationConfiguration(approveOrRejectClosureRequest.projectId, Convert.ToInt32(HRMS.Common.Enums.NotificationType.ClosureApprovedByDH), null);
                    if (notification.Item == 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Error Occured While Sending Notification in ApproveOrRejectClosureByDH() method"
                        };
                    }
                    return response = new ServiceResponse<int>()
                    {
                        Item = approveOrRejectByDH.Item,
                        IsSuccessful = approveOrRejectByDH.IsSuccessful,
                        Message = approveOrRejectByDH.Message
                    };
                }
            }
            else
            {


                var category = await m_OrgService.GetCategoryByName(HRMS.Common.Enums.CategoryMaster.PPC.ToString());

                if (category.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = category.Message
                    };
                }


                var notificationType = await m_OrgService.GetNotificationTypeByCode(HRMS.Common.Enums.NotificationType.ClosureRejectedByDH.ToString());
                if (notificationType.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = notificationType.Message
                    };
                }


                var notificationConfig = await m_OrgService.GetByNotificationTypeAndCategoryId(notificationType.Item.NotificationTypeId, category.Item.CategoryMasterId);

                if (notificationConfig.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = notificationConfig.Message
                    };
                }


                var user = await m_OrgService.GetUserByEmail(notificationConfig.Item.EmailTo);

                if (user.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = user.Message
                    };
                }


                var emp = await m_EmployeeService.GetEmployeeByUserId(user.Item.UserId);
                if (emp.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = emp.Message
                    };
                }

                var workFlowByDH = await m_OrgService.GetStatusByCategoryIdAndStatusCode(category.Item.CategoryMasterId, HRMS.Common.Enums.ProjectClosureStatusCodes.ClosureRejectedByDH.ToString());
                if (workFlowByDH.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = workFlowByDH.Message
                    };
                }

                using (var trans = m_ProjectContext.Database.BeginTransaction())
                {
                    var projectClosure = await (m_ProjectContext.ProjectClosure.Where(q => q.ProjectId == approveOrRejectClosureRequest.projectId).FirstOrDefaultAsync());
                    if (projectClosure == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = $"No Project Closure Found with Project Id {approveOrRejectClosureRequest.projectId}"
                        };
                    }
                    int projectClosureId = projectClosure.ProjectClosureId;
                    ProjectClosureWorkflow p_workflow = new ProjectClosureWorkflow()
                    {
                        SubmittedBy = approveOrRejectClosureRequest.employeeId,
                        SubmittedTo = emp.Item.EmployeeId,
                        SubmittedDate = DateTime.Now,
                        WorkflowStatus = workFlowByDH.Item.StatusId,
                        ProjectClosureId = projectClosureId,
                        Comments = null,
                        IsActive = true
                    };

                    m_ProjectContext.ProjectClosureWorkflow.Add(p_workflow);


                    var project = await (m_ProjectContext.Projects.Where(q => q.ProjectId == approveOrRejectClosureRequest.projectId)).FirstOrDefaultAsync();
                    if (project == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = $"No Project with Project Id {approveOrRejectClosureRequest.projectId}"
                        };
                    }
                    project.ProjectStateId = workFlowByDH.Item.StatusId;


                    var created = m_ProjectContext.SaveChanges();

                    trans.Commit();
                    if (created == 0)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Failed to approve a project.."
                        };
                    }
                    else
                    {
                        ServiceResponse<int> notification = await m_projectClosureReportService.NotificationConfiguration(approveOrRejectClosureRequest.projectId, Convert.ToInt32(HRMS.Common.Enums.NotificationType.ClosureRejectedByDH), null);
                        if (notification.Item == 0)
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Error Occured While Sending Notification in ApproveOrRejectClosureByDH() method"
                            };
                        }
                        return response = new ServiceResponse<int>()
                        {
                            Item = created,
                            IsSuccessful = true,
                            Message = "Project Rejected successfully.",
                        };
                    }
                }
            }
        }
        #endregion

        #region RejectionNotificationForTL
        /// <summary>
        /// Notification to Team Lead for Closure Report Rejection and ReWork
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Integer Value 0-Represents Unsuccessful Response and 1-Represents Successful Response</returns>
        public async Task<ServiceResponse<int>> RejectionNotificationForTL(int projectId)
        {
            ServiceResponse<int> response;
            try
            {
                m_Logger.LogInformation("Calling \"NotificationConfiguration\" method in ProjectClosureService");

                int notificationTypeId = Convert.ToInt32(HRMS.Common.Enums.NotificationType.ClosureReportRejected);

                int categoryId = Convert.ToInt32(HRMS.Common.Enums.CategoryMaster.PPC);

                int? LeadId = m_ProjectContext.ProjectManagers.Where(st => st.ProjectId == projectId && st.IsActive == true).Select(st => st.LeadId).FirstOrDefault();
                if (LeadId == 0 || LeadId == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error Occured while fetching Lead Id"
                    };
                }
                var employee = await m_EmployeeService.GetEmployeeById((int)LeadId);
                if (employee.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error Occured while fetching Employee"
                    };
                }
                int userId = employee.Item.UserId.Value;
                if (userId == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = $"No User Found With Project Id {projectId}"
                    };
                }
                var users = await m_OrgService.GetUserById(userId);
                if (users.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = $"No Users Found With Project Id {projectId}"
                    };
                }

                ServiceResponse<NotificationConfiguration> notificationConfiguration = await m_OrgService.GetNotificationConfiguration(notificationTypeId, categoryId);

                if (notificationConfiguration.Item != null)
                {
                    var project = await m_ProjectService.GetProjectById(projectId);

                    if (project != null)
                    {
                        NotificationDetail notificationDetail = new NotificationDetail();
                        StringBuilder emailContent = new StringBuilder(System.Net.WebUtility.HtmlDecode(notificationConfiguration.Item.EmailContent));



                        emailContent = emailContent.Replace("{ProjectCode}", project.Item.ProjectCode)
                                                   .Replace("{ProjectName}", project.Item.ProjectName)
                                                   .Replace("{ManagerName}", project.Item.ManagerName)
                                                   .Replace("{ProjectState}", project.Item.ProjectState)
                                                    .Replace("{ProjectEndDate}", project.Item.ActualEndDate.Value.ToShortDateString());



                        if (string.IsNullOrEmpty(notificationConfiguration.Item.EmailFrom))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email From cannot be blank"
                            };
                        }
                        notificationDetail.FromEmail = notificationConfiguration.Item.EmailFrom;



                        if (string.IsNullOrEmpty(notificationConfiguration.Item.EmailTo))
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Email To cannot be blank"
                            };
                        }
                        // notificationDetail.ToEmail = users.Item.EmailAddress;   //to-do. Should be uncommented at the time of production
                        notificationDetail.ToEmail = notificationConfiguration.Item.EmailTo;  //to-do. Only for Dev purpose, should be removed in production.



                        notificationDetail.CcEmail = notificationConfiguration.Item.EmailCC;



                        notificationDetail.Subject = $"{notificationConfiguration.Item.EmailSubject} Project Name : {project.Item.ProjectName} Project End Date : {project.Item.ActualEndDate.Value.ToShortDateString()}";
                        notificationDetail.EmailBody = emailContent.ToString();
                        m_OrgService.SendEmail(notificationDetail);
                    }
                }
                return response = new ServiceResponse<int>()
                {
                    Item = 1,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in NotificationConfiguration.");



                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Exception occured in NotificationConfiguration."
                };
            }



        }
        #endregion
    }
}
