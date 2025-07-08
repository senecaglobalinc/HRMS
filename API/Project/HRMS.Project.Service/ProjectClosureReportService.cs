using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using HRMS.Project.Database;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Request;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using HRMS.Project.Entities;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace HRMS.Project.Service
{
    public class ProjectClosureReportService : IProjectClosureReportService
    {
        #region Global Varibles

        private readonly ILogger<ProjectClosureReportService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private readonly IProjectManagerService m_projectManagerService;
        private readonly IOrganizationService m_OrgService;
        private readonly IEmployeeService m_EmployeeService;
        private readonly IMapper m_Mapper;
        private readonly IConfiguration m_configuration;
        private readonly IProjectService m_projectService;
        readonly IMapper mapper;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;

        #endregion

        #region Constructor
        public ProjectClosureReportService(ILogger<ProjectClosureReportService> logger,
            ProjectDBContext projectContext,
            IProjectManagerService projectManagerService,
            IOrganizationService orgService,
            IEmployeeService employeeService,
            IConfiguration configuration,
            IProjectService projectService,
            IMapper mapper,
            IOptions<MiscellaneousSettings> miscellaneousSettings)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            m_projectManagerService = projectManagerService;

            m_OrgService = orgService;

            m_EmployeeService = employeeService;
            m_configuration = configuration;
            m_projectService = projectService;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectClosureReportRequest, Entities.ProjectClosureReport>();
            });
            m_Mapper = config.CreateMapper();
            m_mapper = mapper;
            m_MiscellaneousSettings = miscellaneousSettings?.Value;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates projectClosureReport based on sent info.
        /// </summary>
        /// <param name="projectIn">ProjectClosureReport information</param>
        /// <returns>integer if reteun is"0" then the report is not created 
        /// if return response is Project Clsoure Id then the report is created sucessfully</returns>
        public async Task<ServiceResponse<int>> Create(int projectId)
        {
            ServiceResponse<int> response;
            try
            {
                int isCreated;
                m_Logger.LogInformation("Calling \"Create\" method in ProjectClosureReport Service");

                var projectclosure = await m_ProjectContext.ProjectClosure.Where(prj => prj.ProjectId == projectId).FirstOrDefaultAsync();

                if (projectclosure == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Project found with this project ID .."
                    };
                }
                var projectIn = new ProjectClosureReportRequest();
                projectIn.ProjectId = projectId;
                projectIn.ProjectClosureId = projectclosure.ProjectClosureId;
                var pcr = await m_ProjectContext.ProjectClosureReport.Where(prj => prj.ProjectClosureId == projectIn.ProjectClosureId).FirstOrDefaultAsync();
                if (pcr != null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "project closure report already exists with this project"
                    };
                }

                if (!projectIn.IsActive.HasValue)
                    projectIn.IsActive = true;
                ServiceResponse<Status> inProgressStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), HRMS.Common.Enums.ProjectClosureStatusCodes.InProgress.ToString());
                if (inProgressStatus.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "status not found for the  given parameters"
                    };
                }

                projectIn.StatusId = inProgressStatus.Item.StatusId;

                Entities.ProjectClosureReport project = new Entities.ProjectClosureReport();
                project = m_mapper.Map<ProjectClosureReportRequest, Entities.ProjectClosureReport>(projectIn);
                m_ProjectContext.ProjectClosureReport.Add(project);

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProjectClosureReportService");
                isCreated = await m_ProjectContext.SaveChangesAsync();
                if (isCreated > 0)
                {

                    m_Logger.LogInformation("Project Closure Report created successfully.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = projectclosure.ProjectClosureId,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
                else
                {
                    m_Logger.LogError("Error occured while creating ProjectClosureReport..");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error occured while creating ProjectClosureReport.."
                    };
                }
            }
            catch (Exception ex)
            {
                return response = new ServiceResponse<int>()
                {
                    Item = 0,
                    IsSuccessful = false,
                    Message = "Error occured while creating ProjectClosureReport.."
                };
            }
        }

        #endregion

        #region Update
        /// <summary>
        /// Update projectClosureReport based on sent info.
        /// </summary>
        /// <param name="projectIn">ProjectClosureReport information</param>
        /// <returns>returns integer "0" when update is failed
        /// when update is successful repose is "1"</returns>
        public async Task<ServiceResponse<int>> Update(ProjectClosureReportRequest projectIn)
        {
            ServiceResponse<int> response;
            try
            {
                string submitType = "submit";
                string updateType = "update";
                int isUpdated;
                m_Logger.LogInformation("Calling \"Update\" method in ProjectClosureReport Service");

                int? projectClosureId = await m_ProjectContext.ProjectClosure.Where(prj => prj.ProjectId == projectIn.ProjectId)
                  .Select(c => c.ProjectClosureId).FirstOrDefaultAsync();

                if (!projectClosureId.HasValue)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Project found with this projectID.."
                    };
                }

                ServiceResponse<Status> completedStatus = await m_OrgService.GetStatusByCategoryAndStatusCode(HRMS.Common.Enums.CategoryMaster.PPC.ToString(), HRMS.Common.Enums.ProjectClosureStatusCodes.Completed.ToString());
                if (completedStatus.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "status not found for the given parameters"
                    };
                }
                var project = m_ProjectContext.ProjectClosureReport.Where(prj => prj.ProjectClosureId == projectClosureId)
                    .FirstOrDefault();
                if (project == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project closure report not found for update."
                    };
                }
                else
                {
                    m_Logger.LogInformation("ProjectClosureReport found for update.");

                    //   project.ClientFeedback = projectIn.ClientFeedback;
                    //   project.DeliveryPerformance = projectIn.DeliveryPerformance;
                    project.ValueDelivered = projectIn.ValueDelivered;
                    project.ManagementChallenges = projectIn.ManagementChallenges;
                    project.TechnologyChallenges = projectIn.TechnologyChallenges;
                    project.EngineeringChallenges = projectIn.EngineeringChallenges;
                    project.BestPractices = projectIn.BestPractices;
                    project.LessonsLearned = projectIn.LessonsLearned;
                    project.ReusableArtifacts = projectIn.ReusableArtifacts;
                    project.ProcessImprovements = projectIn.ProcessImprovements;
                    project.Awards = projectIn.Awards;
                    project.NewTechnicalSkills = projectIn.NewTechnicalSkills;
                    project.NewTools = projectIn.NewTools;
                    project.Remarks = projectIn.Remarks;

                }
                if (projectIn.type == submitType)
                    project.StatusId = completedStatus.Item.StatusId;
                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProjectClosureReportService");
                isUpdated = await m_ProjectContext.SaveChangesAsync();

                if (isUpdated > 0)
                {
                    if (projectIn.type == updateType)
                    {
                        m_Logger.LogInformation("Project closure report Updated successfully.");

                        return response = new ServiceResponse<int>()
                        {
                            Item = 1,
                            IsSuccessful = true,
                            Message = string.Empty
                        };
                    }
                    else if (projectIn.type == submitType)
                    {
                        int notificationTypeId = Convert.ToInt32(HRMS.Common.Enums.NotificationType.TLSubmitForApproval);
                        int? departmentId = null;
                        ServiceResponse<int> notification = await NotificationConfiguration(projectIn.ProjectId, notificationTypeId, departmentId);
                        if (notification.Item == 0)
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = notification.Message
                            };
                        }
                        m_Logger.LogInformation("Project closure report submitted successfully.");
                        return response = new ServiceResponse<int>()
                        {
                            Item = 1,
                            IsSuccessful = true,
                            Message = string.Empty
                        };

                    }
                    else
                    {
                        m_Logger.LogInformation(" type was incorrect.");
                        return response = new ServiceResponse<int>()
                        {
                            Item = 1,
                            IsSuccessful = false,
                            Message = "type was incorrect."
                        };

                    }

                }
                else
                {
                    m_Logger.LogError("No project closure report updated.");
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No project closure report updated."
                    }; ;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #endregion

        #region CreateRepository
        void CreateRepository(string projectCode)
        {
            try
            {
                string filePath = m_MiscellaneousSettings.RepositoryPath;

                Common.Extensions.Extensions.CreateFolder(filePath, projectCode);
            }
            catch (Exception ex)
            {
                m_Logger.LogError($"Error occurred while creating directories for projectsClosure : {projectCode} {ex.StackTrace}");
            }
            return;
        }
        #endregion

        #region GetClosureReportByProjectId
        /// <summary>
        /// Gets Project Closure report data by ProjectId
        /// </summary>
        /// <param name="ProjectId">project Id</param>
        /// <returns>Project Closure Report data when report data found with the projectID
        /// else sends a null list when report data not found</returns>
        public async Task<ServiceListResponse<ProjectClosureReportDetails>> GetClosureReportByProjectId(int projectId)
        {
            ServiceListResponse<ProjectClosureReportDetails> response;
            if (projectId == 0)
            {
                response = new ServiceListResponse<ProjectClosureReportDetails>()
                {
                    Items = null,
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
                    response = new ServiceListResponse<ProjectClosureReportDetails>()
                    {
                        Items = null,
                        IsSuccessful = false,
                        Message = "No Such Project exists"
                    };
                }
                else
                {
                    m_Logger.LogInformation("ProjectClosureReport details Found");
                    response = new ServiceListResponse<ProjectClosureReportDetails>();
                    try
                    {
                        m_Logger.LogInformation("Fetching Data");
                        int projectClosureId = project.ProjectClosureId;
                        var pcr = await m_ProjectContext.ProjectClosureReport.Where(x => x.ProjectClosureId == projectClosureId).FirstOrDefaultAsync();
                        if (pcr == null)
                        {
                            response = new ServiceListResponse<ProjectClosureReportDetails>()
                            {
                                Items = null,
                                IsSuccessful = false,
                                Message = "Project Closure Report Does not Exists"
                            };
                        }
                        else
                        {
                            string rootPath = m_MiscellaneousSettings.RepositoryPath;
                            string clientPath = rootPath + pcr.ClientFeedbackFile;
                            string deliveryPath = rootPath + pcr.DeliveryPerformanceFile;
                            response.Items = await (from report in m_ProjectContext.ProjectClosureReport
                                                    join closure in m_ProjectContext.ProjectClosure on
                                 report.ProjectClosureId equals closure.ProjectClosureId
                                                    where (report.ProjectClosureId == projectClosureId && report.IsActive == true)
                                                    select new ProjectClosureReportDetails
                                                    {
                                                        ProjectId = closure.ProjectId,
                                                        ClientFeedback = report.ClientFeedback,
                                                        DeliveryPerformance = report.DeliveryPerformance,
                                                        ClientFeedbackFile = Path.GetFileName(clientPath),
                                                        DeliveryPerformanceFile = Path.GetFileName(deliveryPath),
                                                        ValueDelivered = report.ValueDelivered,
                                                        ManagementChallenges = report.ManagementChallenges,
                                                        TechnologyChallenges = report.TechnologyChallenges,
                                                        EngineeringChallenges = report.EngineeringChallenges,
                                                        BestPractices = report.BestPractices,
                                                        LessonsLearned = report.LessonsLearned,
                                                        Awards = report.Awards,
                                                        ReusableArtifacts = report.ReusableArtifacts,
                                                        Remarks = report.Remarks,
                                                        ProcessImprovements = report.ProcessImprovements,
                                                        NewTechnicalSkills = report.NewTechnicalSkills,
                                                        NewTools = report.NewTools,
                                                        CaseStudy = report.CaseStudy,
                                                        StatusId = report.StatusId,
                                                        RejectRemarks = report.RejectRemarks

                                                    }).ToListAsync();


                            response.IsSuccessful = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.IsSuccessful = false;
                        response.Message = "Error occured while fetching the data";
                        m_Logger.LogError($"Error occured in GetClosureReportByProjectId() method { ex.StackTrace}");
                    }

                }

            }
            return response;


        }
        #endregion

        #region NotificationConfiguration
        /// <summary>
        /// NotificationConfiguration
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <returns>Integer value, 0 - represents unsuccessful response and 1- represents successful response</returns>
        public async Task<ServiceResponse<int>> NotificationConfiguration(int projectId, int notificationTypeId, int? departmentId)
        {
            ServiceResponse<int> response;
            try
            {
                m_Logger.LogInformation("Calling \"NotificationFromTL\" method in ProjectClosureReportService");
                int smSubmit = Convert.ToInt32(HRMS.Common.Enums.NotificationType.SMSubmit);
                int tlSubmit = Convert.ToInt32(HRMS.Common.Enums.NotificationType.TLSubmitForApproval);
                int closeApprovebyDH = Convert.ToInt32(HRMS.Common.Enums.NotificationType.ClosureApprovedByDH);
                int closeRejectedbyDH = Convert.ToInt32(HRMS.Common.Enums.NotificationType.ClosureRejectedByDH);
                int categoryId = Convert.ToInt32(HRMS.Common.Enums.CategoryMaster.PPC);
                var projectManager = await m_ProjectContext.ProjectManagers.Where(prj => prj.ProjectId == projectId).FirstOrDefaultAsync();
                if (projectManager == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Project Manager not Found with the project Id"
                    };
                }
                int PMId = Convert.ToInt32(projectManager.ProgramManagerId);
                //var projectClosure = await m_ProjectContext.ProjectClosure.Where(x => x.ProjectId == projectId).FirstOrDefaultAsync();
                //if (projectClosure == null)
                //{
                //    return response = new ServiceResponse<int>()
                //    {
                //        Item = 0,
                //        IsSuccessful = false,
                //        Message = "Project Closure not Found with the project Id"
                //    };
                //}
                ServiceResponse<NotificationConfiguration> notificationConfiguration = await m_OrgService.GetNotificationConfiguration(notificationTypeId, categoryId);
                if (notificationConfiguration.Item == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "Error Occured while fetching Notification Configuration"
                    };
                }
                ServiceListResponse<Department> departments = await m_OrgService.GetAllDepartment(true);
                if (departments.Items == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "department list is empty"
                    };
                }
                var projdept = departments.Items.Where(q => q.DepartmentId == departmentId).FirstOrDefault();
                if (notificationTypeId == smSubmit)
                {

                    if (projdept == null)
                    {
                        return response = new ServiceResponse<int>()
                        {
                            Item = 0,
                            IsSuccessful = false,
                            Message = "Error Occured while fetching departments"
                        };
                    }
                }
                if (notificationConfiguration.Item != null)
                {
                    var project = await m_projectService.GetProjectById(projectId);

                    if (project != null)
                    {
                        NotificationDetail notificationDetail = new NotificationDetail();
                        StringBuilder emailContent = new StringBuilder(System.Net.WebUtility.HtmlDecode(notificationConfiguration.Item.EmailContent));

                        emailContent = emailContent.Replace("{ProjectCode}", project.Item.ProjectCode)
                                                   .Replace("{ProjectName}", project.Item.ProjectName)
                                                   .Replace("{ManagerName}", project.Item.ManagerName)
                                                   .Replace("{ProjectState}", project.Item.ProjectState)
                                                   .Replace("{Department}", projdept?.Description ?? "");

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
                        var employee = await m_EmployeeService.GetEmployeeById(PMId);
                        if (employee.Item == null)
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Failed to Fetch employee"
                            };
                        }
                        int userId = employee.Item.UserId.Value;

                        var users = await m_OrgService.GetUserById(userId);
                        if (users.Item == null)
                        {
                            return response = new ServiceResponse<int>()
                            {
                                Item = 0,
                                IsSuccessful = false,
                                Message = "Failed to Fetch User's data"
                            };
                        }
                        // notificationDetail.ToEmail = users.Item.EmailAddress;  //to-do. should replace below line with this line.
                        notificationDetail.ToEmail = notificationConfiguration.Item.EmailTo;

                        notificationDetail.CcEmail = notificationConfiguration.Item.EmailCC;

                        //Getting email id for PM
                        string pmEmailId = (await GetPMandTLEmailForClosure(projectId, "PM")).Item;
                        if (!string.IsNullOrEmpty(pmEmailId))
                        {
                            notificationDetail.ToEmail = $"{notificationDetail.ToEmail};{pmEmailId}";
                        }

                        if (notificationTypeId == tlSubmit)
                        {
                            notificationDetail.Subject = $"{project.Item.ProjectName} {notificationConfiguration.Item.EmailSubject}";
                        }
                        else if (notificationTypeId == smSubmit)
                        {
                            notificationDetail.Subject = $"{projdept.Description} {notificationConfiguration.Item.EmailSubject}";
                        }
                        else if (notificationTypeId == closeApprovebyDH)
                        {
                            //Getting email id for TL
                            string tlEmailId = (await GetPMandTLEmailForClosure(projectId, "TL")).Item;
                            if (!string.IsNullOrEmpty(tlEmailId))
                                notificationDetail.ToEmail = $"{notificationDetail.ToEmail};{tlEmailId}";
                            notificationDetail.Subject = $"{project.Item.ProjectName} {notificationConfiguration.Item.EmailSubject}";
                            project.Item.IsActive = false;
                            //projectClosure.ActualClosureDate = DateTime.Now;
                            await m_ProjectContext.SaveChangesAsync();
                        }
                        else
                        {
                            notificationDetail.Subject = $"{project.Item.ProjectName} {notificationConfiguration.Item.EmailSubject}";
                        }

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

        #region SaveFIle
        /// <summary>
        /// Save uploded file details into the specified project code folder
        /// </summary>
        /// <param name="uploadFiles"></param>
        /// <returns>method returns integer "0" when files stored sucessfully
        /// else returns "0" fails to save the files</returns>
        public async Task<ServiceResponse<int>> Save(UploadFiles uploadFilesIn)
        {
            int isCreated = 0;
            string clientFeedbackType = "ClientFeedback";
            string deliveryPerformanceType = "DeliveryPerformance";
            var response = new ServiceResponse<int>();
            m_Logger.LogInformation("ProjectClosureReportService: Calling \"Save\" method.");
            try
            {
                ProjectClosureReport uploadFile = new ProjectClosureReport();


                int? projectClosureId = await m_ProjectContext.ProjectClosure.Where(prj => prj.ProjectId == uploadFilesIn.ProjectId)
                    .Select(c => c.ProjectClosureId).FirstOrDefaultAsync();
                if (!projectClosureId.HasValue)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Project found with this project ID.."
                    };
                }
                var projectClosureReport = m_ProjectContext.ProjectClosureReport.Where(prj => prj.ProjectClosureId == projectClosureId)
                .FirstOrDefault();
                if (projectClosureReport == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Project Closure Report found with this project ID.."
                    };
                }



                var project = m_ProjectContext.Projects
                                        .Where(emp => emp.ProjectId == uploadFilesIn.ProjectId)
                                        .FirstOrDefault();
                if (project == null)
                {
                    return response = new ServiceResponse<int>()
                    {
                        Item = 0,
                        IsSuccessful = false,
                        Message = "No Project found with this project ID.."
                    };
                }
                char[] charsToTrim2 = { '*', '-', '/', ' ', '_', '+', '.', '&', '!', '@', '#', '%', '^', '(', ')' };
                string projectName = project.ProjectName.Trim(charsToTrim2);
                projectName = Regex.Replace(projectName, @"\s", "");
                string projectCode = project.ProjectCode;
                int projectId = project.ProjectId;

                //create folder structure with the project code
                CreateRepository(projectCode);

                string rootPath = m_MiscellaneousSettings.RepositoryPath;
                IFormFile file = uploadFilesIn.UploadedFiles;

                string path = string.Empty;
                string relativePath = string.Empty;
                path = @rootPath + projectCode + @"\";
                relativePath = projectCode + @"\";
                bool isExists = Directory.Exists(path);
                if (!isExists)
                {
                    response.IsSuccessful = false;
                    response.Message = "File cannot be uploaded";
                    return response;
                }

                //Give fileName and serverpath to save the file/files.
                if (uploadFilesIn.FileType == clientFeedbackType)
                {
                    var filename = projectCode.ToString() + "_" + projectName + "_csat.pdf";
                    var filePath = path + filename;
                    var fileRelativePath = relativePath + filename;
                    projectClosureReport.ClientFeedbackFile = fileRelativePath;
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                        stream.Dispose();
                    }
                }
                if (uploadFilesIn.FileType == deliveryPerformanceType)
                {

                    var filename = projectCode.ToString() + "_" + projectName + "_Workbook.xlsx";
                    var filePath = path + filename;
                    var fileRelativePath = relativePath + filename;
                    projectClosureReport.DeliveryPerformanceFile = fileRelativePath;
                    using (var stream = File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                        stream.Dispose();
                    }

                }

                projectClosureReport.IsActive = true;

                //Add file to list

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProjectClosureReportService");
                isCreated = await m_ProjectContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = isCreated;
                    m_Logger.LogInformation("File uploaded successfully");
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No file uploaded";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while uploading files";
                m_Logger.LogError($"Error occurred in \"Save\" of ProjectClosureReportService {ex.StackTrace}");
            }
            return response;
        }
        #endregion

        #region Download
        /// <summary>
        /// Download uploded file
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="projectId"></param>
        /// <returns>File path</returns>
        public async Task<ServiceResponse<String>> Download(string fileType, int projectId)
        {
            try
            {
                string clientFeedbackType = "ClientFeedback";
                string deliveryPerformanceType = "DeliveryPerformance";
                var response = new ServiceResponse<String>();
                m_Logger.LogInformation("ProjectClosureReportService: Calling \"Download\" method.");

                int? projectClosureId = await (m_ProjectContext.ProjectClosure.Where(prj => prj.ProjectId == projectId)
                    .Select(c => c.ProjectClosureId)).FirstOrDefaultAsync();
                if (!projectClosureId.HasValue)
                {
                    return response = new ServiceResponse<String>()
                    {
                        IsSuccessful = false,
                        Message = "No Project found with this projectID.."
                    };
                }
                var projectClosureReport = m_ProjectContext.ProjectClosureReport.Where(prj => prj.ProjectClosureId == projectClosureId)
                .FirstOrDefault();
                if (projectClosureReport == null)
                {
                    return response = new ServiceResponse<String>()
                    {

                        IsSuccessful = false,
                        Message = "No Project Closure Report found with this project ID.."
                    };
                }
                string path;
                string rootPath = m_MiscellaneousSettings.RepositoryPath;
                if (fileType == clientFeedbackType)
                {
                    path = projectClosureReport.ClientFeedbackFile;
                    path = rootPath + path;
                }
                else if (fileType == deliveryPerformanceType)
                {
                    path = projectClosureReport.DeliveryPerformanceFile;
                    path = rootPath + path;
                }
                else
                {
                    return response = new ServiceResponse<String>()
                    {

                        IsSuccessful = false,
                        Message = "Path not found"
                    };
                }

                return response = new ServiceResponse<String>()
                {
                    Item = path,
                    IsSuccessful = true,
                    Message = "Path found successfully"
                };


            }
            catch (Exception ex)
            {
                var response = new ServiceResponse<String>();
                m_Logger.LogError($"Error occured while fetching path {ex.StackTrace}");
                return response = new ServiceResponse<String>()
                {

                    IsSuccessful = false,
                    Message = "No Project Closure Report found with this project ID.."
                };
            }

        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete uploded file
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="projectId"></param>
        /// <returns>Boolean value</returns>
        public async Task<ServiceResponse<bool>> Delete(string fileType, int projectId)
        {
            int isDeleted = 0;
            var response = new ServiceResponse<bool>();
            string clientFeedbackType = "ClientFeedback";
            string deliveryPerformanceType = "DeliveryPerformance";
            m_Logger.LogInformation("ProjectClosureReportService: Calling \"Delete\" method.");
            try
            {
                int? projectClosureId = await (m_ProjectContext.ProjectClosure.Where(prj => prj.ProjectId == projectId)
                     .Select(c => c.ProjectClosureId)).FirstOrDefaultAsync();
                if (!projectClosureId.HasValue)
                {

                    response.IsSuccessful = false;
                    response.Message = "No Project found with this project code..";

                }

                var projectClosureReport = m_ProjectContext.ProjectClosureReport.Where(prj => prj.ProjectClosureId == projectClosureId)
             .FirstOrDefault();
                if (projectClosureReport == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "No Project Closure Report found with this project ID..";
                }
                else
                {
                    string path = string.Empty;
                    string rootPath = m_MiscellaneousSettings.RepositoryPath;
                    if (fileType == clientFeedbackType)
                    {
                        path = projectClosureReport.ClientFeedbackFile;
                        path = rootPath + path;
                    }
                    else if (fileType == deliveryPerformanceType)
                    {
                        path = projectClosureReport.DeliveryPerformanceFile;
                        path = rootPath + path;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "Path not found";
                    }

                    //Check if file exists in the location
                    if (File.Exists(path))
                    {
                        //delete the file
                        File.Delete(path);
                        if (fileType == clientFeedbackType)
                        {
                            projectClosureReport.ClientFeedbackFile = null;

                        }
                        else if (fileType == deliveryPerformanceType)
                        {
                            projectClosureReport.DeliveryPerformanceFile = null;


                        }

                        m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ProjectClosureReportService");
                        isDeleted = await m_ProjectContext.SaveChangesAsync();
                        if (isDeleted > 0)
                        {
                            response.IsSuccessful = true;
                            response.Item = true;
                            m_Logger.LogInformation("File deleted successfully");
                        }
                        else
                        {
                            response.IsSuccessful = false;
                            response.Message = "No file deleted";
                            return response;
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "File not found";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while deleting files";
                m_Logger.LogError($"Error occurred in \"Delete\" of ProjectClosureReportService {ex.StackTrace}");
            }
            return response;
        }
        #endregion

        #region GetPMandTLDetails
        private async Task<ServiceResponse<string>> GetPMandTLEmailForClosure(int projectId, string type)
        {
            ServiceResponse<string> response;
            try
            {
                m_Logger.LogInformation("Calling \"GetPMandTLEmailForClosure\" method in ProjectClosureReportService");

                var projectDtls = await m_ProjectContext.ProjectManagers.Where(pw => pw.ProjectId == projectId && pw.IsActive == true).ToListAsync();
                int id = type == "PM" ? (projectDtls.First().ProgramManagerId ?? 0) : (projectDtls.First().LeadId ?? 0);
                var employee = await m_EmployeeService.GetEmployeeById(id);
                int userId = employee.Item.UserId.Value;

                var users = await m_OrgService.GetUserById(userId);
                string emailAddress = users.Item.EmailAddress;
                // string emailAddress = "jayanth.chincholi@senecaglobal.com"; //  to-do: works only for DEV (Replace with the above line)

                return response = new ServiceResponse<string>()
                {
                    Item = emailAddress,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in getting PM Email.");

                return response = new ServiceResponse<string>()
                {
                    Item = "",
                    IsSuccessful = false,
                    Message = "Exception occured in getting PM Email."
                };
            }
        }
        #endregion
    }
}
