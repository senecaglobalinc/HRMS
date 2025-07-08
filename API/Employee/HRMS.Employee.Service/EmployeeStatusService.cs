using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service class to get EmployeeResign Status and to make employee inActive.
    /// </summary>
    public class EmployeeStatusService : IEmployeeStatusService
    {
        #region Global Varibles

        private readonly ILogger<EmployeeStatusService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IProjectService m_ProjectService;
        private readonly IOrganizationService m_OrgService;
        private readonly IConfiguration m_configuration;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;
        private readonly EmailConfigurations m_EmailConfigurations;

        #endregion

        #region Constructor

        public EmployeeStatusService(EmployeeDBContext employeeDBContext,
            ILogger<EmployeeStatusService> logger,
            IProjectService projectService,
            IOrganizationService orgService,
            IConfiguration configuration,
            IOptions<MiscellaneousSettings> miscellaneousSettings,
            IOptions<EmailConfigurations> emailConfigurations)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_ProjectService = projectService;
            m_OrgService = orgService;
            m_configuration = configuration;
            m_MiscellaneousSettings = miscellaneousSettings?.Value;
            m_EmailConfigurations = emailConfigurations.Value;
        }

        #endregion

        #region UpdateEmployeeStatus
        /// <summary>
        /// This method is to make employee inactive.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<Employee.Entities.Employee>> UpdateEmployeeStatus(EmployeeDetails employee)
        {
            var response = new ServiceResponse<Employee.Entities.Employee>();
            try
            {
                Employee.Entities.Employee Employee = m_EmployeeContext.Employees.
                                                  Where(emp => emp.EmployeeId == employee.EmpId).FirstOrDefault();
                string employeeName = Employee.FirstName + " " + Employee.LastName;
                // if the employee is reporting manager then we can't inactive the employee
                var isEmployeeRM = m_EmployeeContext.Employees.
                                                  Where(emp => emp.ReportingManager == employee.EmpId && emp.IsActive == true).ToList();

                var projectRMDetails = m_ProjectService.GetProjectRMData(employee.EmpId).Result.Items;

                List<EmployeeDetails> project_managerDetails = new List<EmployeeDetails>();
                if (isEmployeeRM != null && isEmployeeRM.Count > 0)
                {

                    if (projectRMDetails.Count > 0)
                    {
                        project_managerDetails = new List<EmployeeDetails>();
                        projectRMDetails.ForEach(manager =>
                        {
                            var employee = m_EmployeeContext.Employees.Find(manager.ProgramManagerId);
                            EmployeeDetails programmanager = new EmployeeDetails();
                            programmanager.ProgramManagerName = employee.FirstName + " " + employee.LastName;
                            programmanager.WorkEmailAddress = employee.WorkEmailAddress;
                            programmanager.ProjectName = manager.ProjectName;
                            project_managerDetails.Add(programmanager);
                        });
                        //send notification to program manager to release from reporting manager role.
                        NotificationToPMForRelease(employeeName, "ReportingManager", project_managerDetails);

                        response.IsSuccessful = false;
                        response.Message = "This associate is allocated as Reporting manager. So you can't inactive him. Sent notification to Program Manager";
                        return response;
                    }
                    else if (projectRMDetails.Count == 0)
                    {
                        //if the RM is not available in projectmanager table but assigned to associate as RM then get PM of his allocations and send notification

                        List<AssociateAllocation> associateAllocations = m_ProjectService.GetAssociateAllocationsByEmployeeId(employee.EmpId).Result.Items;
                        List<AssociateAllocation> allocations = new List<AssociateAllocation>();

                        if (associateAllocations != null && associateAllocations.Count > 0)
                        {
                            PracticeArea practicearea = m_OrgService.GetPracticeAreaByCode("Talent Pool").Result.Item;
                            List<int> talent_pool_projectIds = m_ProjectService.GetAllProjects().Result.Items.
                                Where(project => project.PracticeAreaId == practicearea.PracticeAreaId).Select(project => project.ProjectId).ToList();

                            allocations = associateAllocations.Where(allocation => !talent_pool_projectIds.Contains((int)allocation.ProjectId)).ToList();
                        }
                        if (allocations.Count > 0)
                        {
                            var project_managerDetail = GetProjectAndProgramManagerDetails(allocations);
                            //Send notification to Program Manager to release him from Reporting manager role.
                            NotificationToPMForRelease(employeeName, "Reporting manager", project_managerDetail);
                            response.IsSuccessful = false;
                            response.Message = "This associate is allocated as Reporting manager. So you can't inactive him. Sent notification to Program Manager";
                            return response;
                        }
                    }
                    response.IsSuccessful = false;
                    response.Message = "This associate is allocated as Reporting manager. So you can't inactive him.";
                    return response;
                }

                if (Employee.DepartmentId == 1)
                {
                    return await UpdateDeliveryEmployeeStatus(employee);
                }
                else
                {
                    return await UpdateNonDeliveryEmployeeStatus(employee);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while employee status";
                m_Logger.LogError("Error occurred in \"UpdateEmployeeStatus\" of EmployeeStatusService" + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region UpdateNonDeliveryEmployeeStatus
        private async Task<ServiceResponse<Employee.Entities.Employee>> UpdateNonDeliveryEmployeeStatus(EmployeeDetails associateDetails)
        {
            int isUpdated = 0;
            var response = new ServiceResponse<Employee.Entities.Employee>();

            Employee.Entities.Employee employee = m_EmployeeContext.Employees.
                                                Where(emp => emp.EmployeeId == associateDetails.EmpId).FirstOrDefault();
            var userID = employee.UserId;
            employee.IsActive = false;
            employee.UserId = null;
            employee.StatusId = (int)AssociateExitStatusCodesNew.Resigned;
            employee.ModifiedDate = DateTime.Now;
            employee.RelievingDate = Utility.GetDateTimeInIST(associateDetails.LastWorkingDate);
            isUpdated = await m_EmployeeContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                var emp = m_EmployeeContext.Employees.Find(associateDetails.EmpId);

                //Deactivate the User(make the User reacord IsActive as false) if Associate is Deactivated
                if (emp.IsActive == false && emp.UserId == null)
                {
                    var result = await m_OrgService.UpdateUser((int)userID);
                    if (result)
                    {
                        response.IsSuccessful = true;
                        response.Item = employee;
                        return response;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                    }
                }
            }
            else
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while updating employee status";
            }
            return response;
        }
        #endregion

        #region UpdateDeliveryEmployeeStatus
        private async Task<ServiceResponse<Employee.Entities.Employee>> UpdateDeliveryEmployeeStatus(EmployeeDetails associateDetails)
        {
            int isUpdated = 0;
            var response = new ServiceResponse<Employee.Entities.Employee>();
            var project_managerDetails = new List<EmployeeDetails>();
            Employee.Entities.Employee employee = m_EmployeeContext.Employees.
                                               Where(emp => emp.EmployeeId == associateDetails.EmpId).FirstOrDefault();
            var userID = employee.UserId;
            string employeeName = employee.FirstName + " " + employee.LastName;

            //Cross communication call to get project managers by employee id
            var ProjectManagers = await m_ProjectService.GetProjectManagersByEmployeeId(associateDetails.EmpId);
            if (!ProjectManagers.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.Message = ProjectManagers.Message;
                return response;
            }
            if (ProjectManagers.Items != null)
            {
                var isProgramManager = ProjectManagers.Items.Where(pm => pm.ProgramManagerId == associateDetails.EmpId).ToList().Count() > 0 ? true : false;

                // if the employee is ProgramManager, then we can't inactive the employee
                if (isProgramManager)
                {
                    response.IsSuccessful = false;
                    response.Message = "This associate is allocated as Program Manager.So you can't inactive him.";
                    return response;
                }
            }
            //check if the associate has the lead role in projectmanager table
            var managerDetails = m_ProjectService.GetProjectLeadData(associateDetails.EmpId).Result.Items;

            if (managerDetails.Count > 0)
            {
                project_managerDetails = new List<EmployeeDetails>();
                managerDetails.ForEach(manager =>
                {
                    var employee = m_EmployeeContext.Employees.Find(manager.ProgramManagerId);
                    EmployeeDetails programmanager = new EmployeeDetails();
                    programmanager.ProgramManagerName = employee.FirstName + " " + employee.LastName;
                    programmanager.WorkEmailAddress = employee.WorkEmailAddress;
                    programmanager.ProjectName = manager.ProjectName;
                    project_managerDetails.Add(programmanager);
                });

                //send notification to program manager to release from reporting manager rolr and project allocations
                NotificationToPMForRelease(employeeName, "Lead", project_managerDetails);
                response.IsSuccessful = false;

                response.Message = "This associate is allocated as Lead. So you can't inactive him. Sent notification to Program Manager";

                return response;

            }

            //check if associate is assigned as RM/Lead/PM in associate allocations.
            var isprojectManagersFromAllocations = m_ProjectService.GetProjectManagerFromAllocations(associateDetails.EmpId).Result;
            if (isprojectManagersFromAllocations)
            {
                response.IsSuccessful = false;
                response.Message = "This associate is allocated as RM/Lead/PM in allocations. So you can't inactive him.";
                return response;
            }
            //Cross communication call to get associate allocations by employee id
            var AssociateAllocations = await m_ProjectService.GetAssociateAllocationsByEmployeeId(associateDetails.EmpId);
            if (!AssociateAllocations.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.Message = AssociateAllocations.Message;
                return response;
            }

            employee.IsActive = false;
            employee.UserId = null;
            employee.StatusId = (int)AssociateExitStatusCodesNew.Resigned;
            employee.RelievingDate = Utility.GetDateTimeInIST(associateDetails.LastWorkingDate);

            List<AssociateAllocation> allocations = AssociateAllocations.Items;

            if (allocations != null && allocations.Count == 1)
            {
                //cross communication call to get project by Id
                //If associate has an allocations, then throw error - Release associate from project(s)
                var project = await m_ProjectService.GetProjectById(new List<int> { allocations.SingleOrDefault().ProjectId.Value });
                if (!project.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = project.Message;
                    return response;
                }
                if (project.Items.First() != null && project.Items.First().ProjectTypeId != 6)
                {
                    //send notification to program manager to release from project allocations
                    project_managerDetails = new List<EmployeeDetails>();
                    project_managerDetails = GetProjectAndProgramManagerDetails(allocations);

                    NotificationToPMForRelease(employeeName, "Associate", project_managerDetails);

                    response.IsSuccessful = false;
                    response.Message = "This associate has an allocation(s). Release associate from project(s). Sent notification to Program Manager.";
                    return response;
                }
                else if (project.Items.First() != null && project.Items.First().ProjectTypeId == 6)
                {
                    //allocate associate to talent pool based on competency group

                    TalentPoolDetails tpDetails = new TalentPoolDetails();
                    tpDetails.EmployeeId = associateDetails.EmpId;
                    tpDetails.projectId = allocations.SingleOrDefault().ProjectId.Value;
                    tpDetails.ReleaseDate = Utility.GetDateTimeInIST(associateDetails.LastWorkingDate);

                    var result = await m_ProjectService.ReleaseFromTalentPool(tpDetails);
                    if (!result.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = result.Message;
                        return response;
                    }

                    employee.UserId = null;
                }
            }
            else
            {
                if (allocations == null || allocations.Count == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "This associate has zero Allocations";
                    return response;
                }
                if (allocations != null && allocations.Count > 1)
                {
                    //send notification to program manager to release from project allocations
                    project_managerDetails = new List<EmployeeDetails>();
                    project_managerDetails = GetProjectAndProgramManagerDetails(allocations);

                    NotificationToPMForRelease(employeeName, "Associate", project_managerDetails);
                    response.IsSuccessful = false;
                    response.Message = "This associate has an allocation(s). Release associate from project(s). Sent notification to Program Manager";
                    return response;
                }
            }

            isUpdated = await m_EmployeeContext.SaveChangesAsync();
            if (isUpdated > 0)
            {
                var emp = m_EmployeeContext.Employees.Find(associateDetails.EmpId);

                //Deactivate the User(make the User reacord IsActive as false) if Associate is Deactivated
                if (emp.IsActive == false && emp.UserId == null)
                {
                    var result = await m_OrgService.UpdateUser((int)userID);
                    if (result)
                    {
                        response.IsSuccessful = true;
                        response.Item = employee;
                    }
                    else
                    {
                        response.IsSuccessful = false;
                    }

                }
            }
            else
            {
                response.IsSuccessful = false;
                response.Message = "No record updated";
                return response;
            }
            return response;
        }
        #endregion

        #region GetProjectAndProgramManagerDetails
        public List<EmployeeDetails> GetProjectAndProgramManagerDetails(List<AssociateAllocation> associateAllocations)
        {
            List<EmployeeDetails> project_managerDetails = new List<EmployeeDetails>();
            if (associateAllocations != null)
            {
                var practicearea = m_OrgService.GetPracticeAreaByCode("Talent Pool").Result.Item;
                var talent_pool_projectIds = m_ProjectService.GetAllProjects().Result.Items.
                    Where(project => project.PracticeAreaId == practicearea.PracticeAreaId).Select(project => project.ProjectId).ToList();

                List<int> projectIDs = associateAllocations.Where(project => !talent_pool_projectIds.Contains((int)project.ProjectId)).Select(project => project.ProjectId).Cast<int>().ToList();

                List<Project> projectDetails = m_ProjectService.GetProjectById(projectIDs).Result.Items;

                List<int> projectManagerIds = associateAllocations.Select(allocation => allocation.ProgramManagerId).Cast<int>().Distinct().ToList();

                List<Employee.Entities.Employee> projectManagers = new List<Employee.Entities.Employee>();
                projectManagerIds.ForEach(id =>
                {
                    var employee1 = m_EmployeeContext.Employees.Find(id);
                    projectManagers.Add(employee1);
                });

                project_managerDetails = (from associate in associateAllocations
                                          join proj in projectDetails on associate.ProjectId equals proj.ProjectId
                                          join pm in projectManagers on associate.ProgramManagerId equals pm.EmployeeId
                                          //where associate.ProjectId==proj.ProjectId && associate.ReportingManagerId==pm.EmployeeId
                                          select new EmployeeDetails
                                          {
                                              ProjectName = proj.ProjectName,
                                              ProgramManagerName = pm.FirstName + " " + pm.LastName,
                                              WorkEmailAddress = pm.WorkEmailAddress
                                          }).ToList();
            }
            return project_managerDetails;
        }
        #endregion

        #region Notification
        public bool NotificationToPMForRelease(string EmpName, string empRole, List<EmployeeDetails> project_managerDetails)
        {

            //Notifying to respective program manager to release from RM role
            NotificationDetail notificationConfig = new NotificationDetail();
            notificationConfig.FromEmail = m_EmailConfigurations.FromEmail;

            if (m_MiscellaneousSettings.Environment == "PROD")
            {
                notificationConfig.ToEmail = string.Join(";", project_managerDetails.Select(projectManager => projectManager.WorkEmailAddress).Distinct().ToArray());
                notificationConfig.CcEmail = m_EmailConfigurations.ToEmail;
            }
            if (m_MiscellaneousSettings.Environment == "QA")
            {
                notificationConfig.CcEmail = m_EmailConfigurations.CcEmail;
                notificationConfig.ToEmail = m_EmailConfigurations.ToEmail;
            }

            notificationConfig.EmailBody = NotifyingPMtoReleaseRole_AllocationConfig(EmpName, empRole, project_managerDetails);

            if (empRole == "Associate")
            {
                notificationConfig.Subject = "Release request of an Associate from allocation(s)";

            }
            else if (empRole == "ReportingManager")
            {
                notificationConfig.Subject = "Release request of an Associate from  Reporting Manager role ";

            }
            else if (empRole == "Lead")
            {
                notificationConfig.Subject = "Release request of an Associate from Lead role ";
            }

            bool IsNotification = m_EmailConfigurations.SendEmail;

            bool emailStatus = false;
            if (IsNotification)
            {
                emailStatus = m_OrgService.SendEmail(notificationConfig).Result.IsSuccessful;
            }

            return emailStatus;
        }

        public string NotifyingPMtoReleaseRole_AllocationConfig(string employeeName, string empRole, List<EmployeeDetails> project_managerDetails)
        {           
            string filePath = Utility.GetNotificationTemplatePath(NotificationTemplatePaths.currentDirectory,NotificationTemplatePaths.subDirectories_Associate_Release_Request); 
            StreamReader stream = new StreamReader(filePath);
            string MailText = stream.ReadToEnd();
            stream.Close();
            string programManager = string.Join(", ", project_managerDetails.Select(project => project.ProgramManagerName).Distinct().ToArray());
            string generatedHTMLtable = "";
            MailText = MailText.Replace("{ProgramManager}", programManager);
            var bodyInfo = "";

            if (empRole == "Associate")
            {
                generatedHTMLtable = GenerateHTMLTable(project_managerDetails);
                bodyInfo = "<strong>" + employeeName + "</strong> needs to be deactivated today, but this associate has project allocation(s), please release from allocations.";
                MailText = MailText.Replace("{tableheading}", "<p>Below are the allocation details.</p>");
                MailText = MailText.Replace("{table}", generatedHTMLtable + "<br/>");

            }
            else if (empRole == "ReportingManager")
            {
                bodyInfo = "<strong>" + employeeName + "</strong>  needs to be deactivated today, but this associate allocated as Reporting Manager role, please release this associate from RM role.";
                MailText = MailText.Replace("{tableheading}", "");
                MailText = MailText.Replace("{table}", "");

            }
            else if (empRole == "Lead")
            {
                bodyInfo = "<strong>" + employeeName + "</strong>  needs to be deactivated today,  but this associate allocated as Lead, please release this associate from lead role.";
                MailText = MailText.Replace("{tableheading}", "");
                MailText = MailText.Replace("{table}", "");

            }
            MailText = MailText.Replace("{bodyInfo}", bodyInfo);

            return MailText;
        }

        public static string GenerateHTMLTable(List<EmployeeDetails> project_managerDetails)
        {
            string tableHtml = "";
            DataTable dt = new DataTable("ProjectManagers");
            dt.Columns.Add(new DataColumn("S.No", typeof(int)));
            dt.Columns.Add(new DataColumn("Project", typeof(string)));
            dt.Columns.Add(new DataColumn("ProgramManager", typeof(string)));
            int i = 1;
            foreach (var pm in project_managerDetails)
            {

                DataRow dr = dt.NewRow();
                dr["S.No"] = i;
                dr["Project"] = pm.ProjectName;
                dr["ProgramManager"] = pm.ProgramManagerName;
                dt.Rows.Add(dr);
                i++;
            }

            tableHtml += "<table>";
            tableHtml += "<tr><th>S.No</th><th>Project</th><th>ProgramManager</th></tr>";

            foreach (DataRow pm in dt.Rows)
            {
                tableHtml += "<tr><td>" + pm["S.No"] + "</td><td>" + pm["Project"] + "</td><td>" + pm["ProgramManager"] + "</td></tr>";
            }
            tableHtml += "</table>";
            return tableHtml;
        }
        #endregion
    }
}
