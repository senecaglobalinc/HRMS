using HRMS.Common;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service that manages attendance regularization requests and approvals.
    /// </summary>
    public class AttendanceRegularizationService : IAttendanceRegularizationService
    {
        #region Global Variables

        private readonly EmployeeDBContext m_employeeDBContext;
        private readonly IOrganizationService m_organizationService;
        private readonly IProjectService m_projectService;
        private readonly ILogger<AttendanceRegularizationService> m_Logger;
        private readonly EmailConfigurations m_EmailConfigurations;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <see cref="AttendanceRegularizationService"/>.
        /// </summary>
        /// <param name="context">Employee database context.</param>
        /// <param name="logger">Application logger.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="organizationService">Organization service instance.</param>
        /// <param name="projectService">Project service instance.</param>
        /// <param name="emailConfigurations">Email configuration options.</param>
        public AttendanceRegularizationService(EmployeeDBContext context, ILogger<AttendanceRegularizationService> logger, IConfiguration configuration,
            IOrganizationService organizationService,IProjectService projectService, IOptions<EmailConfigurations> emailConfigurations)
        {
            m_employeeDBContext = context;
            m_Logger = logger;
            m_organizationService = organizationService;
            m_projectService = projectService;
            m_EmailConfigurations = emailConfigurations.Value;
            _configuration = configuration;
        }
        #endregion

        #region GetNotPunchInDates
        /// <summary>
        /// Retrieves dates on which an associate did not punch in.
        /// </summary>
        /// <param name="attendanceRegularizationFilter">Filter containing associate and date range.</param>
        /// <returns>List of dates that require regularization.</returns>
        public async Task<ServiceResponse<AttendanceRegularization>> GetNotPunchInDates( AttendanceRegularizationFilter attendanceRegularizationFilter)
        {
            var response = new ServiceResponse<AttendanceRegularization>();
            try
            {
                var associateAttendance = await m_employeeDBContext.BioMetricAttendenceDetail.Where(attendance => attendance.AsscociateId == attendanceRegularizationFilter.AssociateId).ToListAsync();

                associateAttendance = associateAttendance.Where(attendance => (attendance.Punch1_Date != null && attendance.Punch1_Date?.Date >= attendanceRegularizationFilter.FromDate.Date && attendance.Punch1_Date?.Date <= attendanceRegularizationFilter.ToDate.Date) 
                                      || (attendance.Punch1_Date == null && attendance.Punch2_Date?.Date >= attendanceRegularizationFilter.FromDate.Date && attendance.Punch2_Date?.Date <= attendanceRegularizationFilter.ToDate.Date)).ToList();
                var existingDateRanges = associateAttendance.Where(attendance=>attendance.Punch1_Date!=null).Select(attendance => attendance.Punch1_Date).ToList();
                    existingDateRanges.AddRange(associateAttendance.Where(attendance => attendance.Punch1_Date == null && attendance.Punch2_Date!=null).Select(attendance => attendance.Punch2_Date).ToList());              
                var leaveData = m_employeeDBContext.AssociateLeave.Where(leave => leave.EmployeeCode==attendanceRegularizationFilter.AssociateId).ToList();
                var leaveAppliedData= leaveData.Where(leave => leave.FromDate.Date >= attendanceRegularizationFilter.FromDate.Date && leave.ToDate <= attendanceRegularizationFilter.ToDate.Date).ToList();
                var leaveDates = GetLeaveDates(leaveAppliedData);
                var halfDayLeaveDates = leaveAppliedData.Where(leave => leave.Session1Id == leave.Session2Id).Select(leave =>leave.ToDate.Date.ToString("dd-MM-yyyy")).ToList();
                var excludeLeaveDates = leaveDates.Where(i => !halfDayLeaveDates.Contains(i)).ToList();
                List<DateTime?> attendanceMissedDates =new List<DateTime?>();
                DateTime currentDate = attendanceRegularizationFilter.FromDate.Date;
                DateTime endDate = attendanceRegularizationFilter.ToDate.Date;
                AttendanceRegularization attendanceRegularization = null;
                var holidays = m_organizationService.GetAllHolidays().Result.Items.Select(x => x.HolidayDate).ToList();
                var submittedRegularization = m_employeeDBContext.AttendanceRegularizationWorkFlow.Where(x => x.SubmittedBy == attendanceRegularizationFilter.AssociateId
                  &&(x.Status == Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationSubmittedForApproval))).Select(x => x.RegularizationAppliedDate.Date).ToList();
                while (currentDate <=endDate )
                {
                    if (!(existingDateRanges.Contains(currentDate) || currentDate.DayOfWeek == DayOfWeek.Sunday || currentDate.DayOfWeek == DayOfWeek.Saturday || holidays.Contains(currentDate) || submittedRegularization.Contains(currentDate) || excludeLeaveDates.Contains(currentDate.ToString("dd-MM-yyyy"))))
                        attendanceMissedDates.Add(currentDate);
                    //Increment date
                    currentDate = currentDate.AddDays(1); 
                }               
              
                // Get RM detials of associate
                var employee = m_employeeDBContext.Employees.Where(x => x.EmployeeCode == attendanceRegularizationFilter.AssociateId && x.IsActive==true).FirstOrDefault();
                string RM = m_employeeDBContext.Employees.Where(emp => emp.EmployeeId == employee.ReportingManager && emp.IsActive==true).Select(x => x.FirstName +" "+ x.LastName).FirstOrDefault();
               
                     attendanceRegularization = new AttendanceRegularization
                    {
                        ReportingManager = RM,
                        ReportingManagerId = employee.ReportingManager,
                        AttendanceRegularizationDates =attendanceMissedDates.Count>0?attendanceMissedDates.Select(x => x.Value.ToString("yyyy-MM-dd")).ToList() : new List<string>()

                    };
                    response.IsSuccessful = true;
                    response.Item = attendanceRegularization;               
                
            }
            catch(Exception e)
            {
                m_Logger.LogError("Error occured while fetching data in GetNotPunchInDates method " + e.Message);
                response.Message = "Error occured while fetching data";
                response.IsSuccessful = false;
            }
            return response;
        }
        #endregion

        #region SaveAttendanceRegularizationDetails
        /// <summary>
        /// Saves new attendance regularization requests submitted by an associate.
        /// </summary>
        /// <param name="attendanceRegularizationWorkFlow">List of regularization requests.</param>
        /// <returns>True when data is saved successfully.</returns>
        public async Task<ServiceResponse<bool>> SaveAttendanceRegularizationDetails(List<AttendanceRegularizationWorkFlow> attendanceRegularizationWorkFlow)
        {
            var response =new ServiceResponse<bool>();
            try
            {
                foreach (AttendanceRegularizationWorkFlow attendance in attendanceRegularizationWorkFlow)
                {
                    var isExist = m_employeeDBContext.AttendanceRegularizationWorkFlow.Where(x => x.SubmittedBy == attendance.SubmittedBy && x.RegularizationAppliedDate == attendance.RegularizationAppliedDate.Date && x.Status!= Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationRejected)).FirstOrDefault();
                    if (isExist == null)
                    {
                        attendance.Status = Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationSubmittedForApproval);
                        m_employeeDBContext.AttendanceRegularizationWorkFlow.Add(attendance);
                    }
                }
                var saved = (await m_employeeDBContext.SaveChangesAsync());
                if (saved == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "Failed to save";
                    return response;

                }
                else
                {
                   
                    RegularizationOnSubmitNotification(attendanceRegularizationWorkFlow );
                }
                response.IsSuccessful = true;
                response.Item = true;
            }
            catch(Exception e)
            {
                m_Logger.LogError("Error occured while Saving data in SaveAttendanceRegularizationDetails method " + e.Message);
                response.Message = "Error occured while Saving data";
                response.IsSuccessful = false;
            }
            return response;
        }
        #endregion

        #region ApproveAttendanceRegularizationDetails
        /// <summary>
        /// Approves or rejects submitted attendance regularization requests.
        /// </summary>
        /// <param name="regularizationWorkFlow">Details of the approval or rejection.</param>
        /// <returns>True when the operation succeeds.</returns>
        public async Task<ServiceResponse<bool>> ApproveOrRejectAttendanceRegularizationDetails(AttendanceRegularizationWorkFlowDetails regularizationWorkFlow)
        {
            var response = new ServiceResponse<bool>();
            int? IsSaved=0;
            using (var dbContext = m_employeeDBContext.Database.BeginTransaction())
            {
                try
                {
                    var employee = m_employeeDBContext.Employees.Where(x => x.EmployeeCode == regularizationWorkFlow.SubmittedBy && x.IsActive == true).FirstOrDefault();
                    string AssociateName = employee.FirstName + " " + employee.LastName;
                    if (regularizationWorkFlow.IsApproved)
                    {                      
                        foreach (var regularizationDate in regularizationWorkFlow.RegularizationDates)
                        {
                            var attendance= m_employeeDBContext.AttendanceRegularizationWorkFlow.Where(x => x.SubmittedBy == regularizationWorkFlow.SubmittedBy && x.RegularizationAppliedDate == regularizationDate.Date).FirstOrDefault();
                            if (attendance != null)
                            {
                                attendance.Status = Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationApproved);
                                attendance.ApprovedBy = regularizationWorkFlow.ApprovedBy;
                                attendance.ApprovedDate = DateTime.Now.Date;
                                attendance.RemarksByRM = regularizationWorkFlow.RemarksByRM;
                                m_employeeDBContext.AttendanceRegularizationWorkFlow.Update(attendance);
                                IsSaved = await m_employeeDBContext.SaveChangesAsync();
                            }
                            if (IsSaved > 0)
                            {
                                BioMetricAttendance bioMetricAttendence = new BioMetricAttendance();
                               
                                bioMetricAttendence.AsscociateId = attendance.SubmittedBy;
                                bioMetricAttendence.AsscociateName = AssociateName;
                                bioMetricAttendence.InTime = attendance.InTime;
                                bioMetricAttendence.OutTime = attendance.OutTime;
                                //string appliedDate = attendance.RegularizationAppliedDate.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                //bioMetricAttendence.Punch1_Date = appliedDate;
                                //bioMetricAttendence.Punch2_Date = appliedDate;
                                bioMetricAttendence.Punch1_Date = attendance.RegularizationAppliedDate.Date;
                                bioMetricAttendence.Punch2_Date = attendance.RegularizationAppliedDate.Date;
                                bioMetricAttendence.WorkTime_HHMM = GetTotalWorkingHours(attendance.InTime, attendance.OutTime);
                                bioMetricAttendence.Location = attendance.Location;
                                bioMetricAttendence.IsRegularized = true;
                                m_employeeDBContext.BioMetricAttendenceDetail.Add(bioMetricAttendence);
                               IsSaved= m_employeeDBContext.SaveChanges();                            
                            }
                        }
                        if (IsSaved > 0)
                        {
                            RegularizationApproveOrRejectNotification(AssociateName, employee.WorkEmailAddress, regularizationWorkFlow.RegularizationDates, Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationApproved));
                        }
                    }
                    else
                    {                       
                        foreach (var regularizationDate in regularizationWorkFlow.RegularizationDates)
                        {
                            var attendance = m_employeeDBContext.AttendanceRegularizationWorkFlow.Where(x => x.SubmittedBy == regularizationWorkFlow.SubmittedBy && x.RegularizationAppliedDate == regularizationDate.Date && x.Status!= Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationRejected)).FirstOrDefault();
                            if (attendance != null)
                            {
                                attendance.Status = Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationRejected);
                                attendance.RemarksByRM = regularizationWorkFlow.RemarksByRM;
                                attendance.RejectedBy = regularizationWorkFlow.RejectedBy;
                                attendance.RejectedDate = regularizationWorkFlow.RejectedDate;
                                m_employeeDBContext.AttendanceRegularizationWorkFlow.Update(attendance);
                                IsSaved = await m_employeeDBContext.SaveChangesAsync();
                               
                            }
                        }
                        if (IsSaved > 0)
                        {
                            RegularizationApproveOrRejectNotification(AssociateName, employee.WorkEmailAddress, regularizationWorkFlow.RegularizationDates, Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationRejected));
                        }

                    }
                    await dbContext.CommitAsync();
                    response.IsSuccessful = true;
                }
                catch (Exception e)
                {
                    dbContext.Rollback();
                    response.IsSuccessful = false;
                    response.Message = "Error occure while approving attendance " ;

                }
            }

            return response;
        }
        #endregion

        #region GetAllAssociateSubmittedAttendanceRegularization
        /// <summary>
        /// Retrieves all attendance regularization requests submitted to a manager.
        /// </summary>
        /// <param name="managerId">Manager employee id.</param>
        /// <param name="roleName">Role of the manager.</param>
        /// <returns>List of submitted regularization details.</returns>
        public async Task<ServiceListResponse<AttendanceRegularizationWorkFlowDetails>> GetAllAssociateSubmittedAttendanceRegularization(int managerId, string roleName)
        {
            var response = new ServiceListResponse<AttendanceRegularizationWorkFlowDetails>();
            try
            {
                List<string> excludedEmployees = new List<string>();
                string excludedAssociates = _configuration.GetSection("ExcludeAssociates")?.Value;

                if (!string.IsNullOrWhiteSpace(excludedAssociates))
                {
                    excludedEmployees = excludedAssociates?.Split(',').ToList();
                }
                List<AttendanceRegularizationWorkFlowDetails> regularizedAssociate = new List<AttendanceRegularizationWorkFlowDetails>();
                if (roleName.ToLower() == "reporting manager" || roleName.ToLower() == "team lead")
                {
                    regularizedAssociate = await (from attendance in m_employeeDBContext.AttendanceRegularizationWorkFlow
                                                  join emp in m_employeeDBContext.Employees on attendance.SubmittedBy equals emp.EmployeeCode
                                                  where attendance.SubmittedTo == managerId && attendance.Status == Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationSubmittedForApproval)
                                                  && emp.IsActive == true
                                                  group new { attendance, emp } by new { attendance.SubmittedBy, emp.FirstName, emp.LastName  , emp.EmployeeId } into grp
                                                  select new AttendanceRegularizationWorkFlowDetails
                                                  {
                                                      EmployeeId = grp.Key.EmployeeId,
                                                      SubmittedBy = grp.Key.SubmittedBy,
                                                      AssociateName = grp.Key.FirstName +" "+grp.Key.LastName,
                                                      RegularizationCount=grp.Count()
                                                  }).ToListAsync();

                }
                else if (roleName.ToLower() == "Program Manager".ToLower())
                {
                    var employeeIds = new List<int>();
                    var allocations = await m_projectService.GetActiveAllocations();
                    if (allocations != null && allocations.Items != null)
                    {
                        employeeIds = (from allocation in allocations.Items
                                       where allocation.ProgramManagerId == managerId
                                       select (int)allocation.AssociateId
                                   ).Distinct().ToList();
                    }
                    regularizedAssociate = await (from attendance in m_employeeDBContext.AttendanceRegularizationWorkFlow
                                                  join emp in m_employeeDBContext.Employees on attendance.SubmittedBy equals emp.EmployeeCode
                                                  where employeeIds.Contains(emp.EmployeeId) && attendance.Status == Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationSubmittedForApproval)
                                                 && emp.IsActive == true
                                                  select new AttendanceRegularizationWorkFlowDetails
                                                  {
                                                      EmployeeId=emp.EmployeeId,
                                                      SubmittedBy=attendance.SubmittedBy,
                                                      AssociateName=emp.FirstName +" "+emp.LastName,
                                                      RegularizationAppliedDate= attendance.RegularizationAppliedDate
                                                  }).ToListAsync();
                    regularizedAssociate.ToList().ForEach(attendance =>
                    {
                        var date = GetAttendanceDates(attendance.RegularizationAppliedDate);
                        if (!date)
                        {
                            regularizedAssociate.Remove(attendance);
                        }
                    });
                    regularizedAssociate = (from attendance in regularizedAssociate
                                                  group new { attendance.EmployeeId, attendance.SubmittedBy,attendance.AssociateName } by new { attendance.AssociateName, attendance.EmployeeId, attendance.SubmittedBy } into grp
                                                  select new AttendanceRegularizationWorkFlowDetails
                                                  {
                                                      EmployeeId = grp.Key.EmployeeId,
                                                      SubmittedBy = grp.Key.SubmittedBy,
                                                      AssociateName = grp.Key.AssociateName,
                                                      RegularizationCount = grp.Count()
                                                  }).ToList();
                }
                else if (roleName.ToLower() == "HRA".ToLower() || roleName.ToLower() == "HRM".ToLower())
                {
                    var employeesIds = await m_employeeDBContext.Employees.Where(emp => emp.IsActive == true && emp.Nationality != "US" && emp.Nationality != "us" && emp.Nationality != "USA"
                     && emp.Nationality != "usa" && !excludedEmployees.Contains(emp.EmployeeCode))
                      .Select(emp => emp.EmployeeId).ToListAsync();
                    regularizedAssociate = await (from attendance in m_employeeDBContext.AttendanceRegularizationWorkFlow
                                                  join emp in m_employeeDBContext.Employees on attendance.SubmittedBy equals emp.EmployeeCode
                                                  where employeesIds.Contains(emp.EmployeeId) && attendance.Status == Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationSubmittedForApproval)
                                                  && emp.IsActive == true
                                                  group new { attendance, emp } by new { attendance.SubmittedBy, emp.FirstName, emp.LastName, emp.EmployeeId } into grp
                                                  select new AttendanceRegularizationWorkFlowDetails
                                                  {
                                                      EmployeeId = grp.Key.EmployeeId,
                                                      SubmittedBy = grp.Key.SubmittedBy,
                                                      AssociateName = grp.Key.FirstName + " " + grp.Key.LastName,
                                                      RegularizationCount = grp.Count()
                                                  }).ToListAsync();



                }
                if (regularizedAssociate.Count > 0)
                {
                    var empIds = string.Join(",", regularizedAssociate.Select(x => x.EmployeeId).ToArray());
                var allocationDetails = m_projectService.GetAllocationsByEmpIds(empIds).Result;
                var associatewithAllocation = new List<AssociateAllocation>();
                if(allocationDetails.Items!=null)
                {
                    regularizedAssociate.Select(x => x.EmployeeId).ToList().ForEach(emp =>
                    {
                      var allocation= allocationDetails.Items.Where(allocation => allocation.EmployeeId == emp && allocation.IsActive == true && allocation.IsPrimary==true).FirstOrDefault();
                     if(allocation!=null)
                        {
                            associatewithAllocation.Add(allocation);
                        }                      

                    });
                }
                               
                    if (associatewithAllocation.Count > 0)
                    {
                        var projectDetails = m_projectService.GetAllProjects().Result;
                        var associateProjectDetails = (from allocation in associatewithAllocation
                                                       join project in projectDetails.Items on allocation.ProjectId equals project.ProjectId
                                                       where allocation.IsActive == true && allocation.IsPrimary == true
                                                       select new
                                                       {
                                                           EmployeeId = allocation.EmployeeId,
                                                           ProjectName = project.ProjectName
                                                       }).ToList();


                        regularizedAssociate.ForEach(associate =>
                        {
                            if (associatewithAllocation.Select(x => x.EmployeeId).Contains(associate.EmployeeId))
                            {
                                associate.ProjectName = associateProjectDetails.Where(x => x.EmployeeId == associate.EmployeeId).FirstOrDefault().ProjectName;
                            }
                        });
                    }
                    
                }              
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No data found";
                    return response;
                }
                response.Items = regularizedAssociate;
                response.IsSuccessful = true;
                
            }
            catch (Exception e)
            {
                m_Logger.LogError("Error occured while fetching data in GetAllAssociateSubmittedAttendanceRegularization method " + e.Message);
                response.Message = "Error occured while fetching data";
                response.IsSuccessful = false;
            }

            return response;
        }
        #endregion

        #region GetAssociateSubmittedAttendanceRegularization
        /// <summary>
        /// Retrieves regularization requests submitted by a specific associate.
        /// </summary>
        /// <param name="AssociateId">Associate employee code.</param>
        /// <param name="roleName">Role of the requesting user.</param>
        /// <returns>List of regularization workflow records.</returns>
        public async Task<ServiceListResponse<AttendanceRegularizationWorkFlow>> GetAssociateSubmittedAttendanceRegularization(string AssociateId, string roleName)
        {
            var response = new ServiceListResponse<AttendanceRegularizationWorkFlow>();
            try
            {
               
                var attendanceRegularizations = await m_employeeDBContext.AttendanceRegularizationWorkFlow.Where(x => x.SubmittedBy== AssociateId 
                && x.Status==Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationSubmittedForApproval)).ToListAsync();
                if (roleName.ToLower() == "Program Manager".ToLower())
                {
                    attendanceRegularizations.ToList().ForEach(attendance =>
                    {
                        var date = GetAttendanceDates(attendance.RegularizationAppliedDate);
                        if (!date)
                        {
                            attendanceRegularizations.Remove(attendance);
                        }
                    });
                }
                if (attendanceRegularizations.Count == 0)
                {
                    response.IsSuccessful = false;
                    response.Message = "No data found";
                    return response;
                }
                response.Items = attendanceRegularizations;
                response.IsSuccessful = true;
            }
            catch (Exception e)
            {
                m_Logger.LogError("Error occured while fetching data in GetAssociateSubmittedAttendanceRegularization method " + e.Message);
                response.Message = "Error occured while fetching data";
                response.IsSuccessful = false;
            }

            return response;
        }
        #endregion

        #region Private methods
        private string GetTotalWorkingHours(string InTime, string OutTime)
        {
            // Calculate total work hours
            string TotalTime = string.Empty;
            if (string.IsNullOrWhiteSpace(OutTime) == false && string.IsNullOrWhiteSpace(InTime) == false)
            {
                var inTime = InTime.Split(':');
                var outTime =OutTime.Split(':');
                TotalTime = inTime.Count() > 1 && outTime.Count() > 1 ?
                                                        (new TimeSpan(Convert.ToInt32(outTime[0]), Convert.ToInt32(outTime[1]), 0) -
                                                         new TimeSpan(Convert.ToInt32(inTime[0]), Convert.ToInt32(inTime[1]), 0)).ToString(@"hh\:mm") : "";
            }
            return TotalTime;
        }

        private bool GetAttendanceDates(DateTime date)
        {
            int count = 0;
            DateTime? newDate=null;
            for (var day = date.AddDays(1); count <= 2; day = day.AddDays(1))
            {
                if (day.Date.DayOfWeek != DayOfWeek.Sunday && day.Date.DayOfWeek != DayOfWeek.Saturday)
                {
                    newDate = day;
                    count++;
                }
            }
            if(DateTime.Now.Date>= newDate)
            {
                return true;
            }
            return false;
        }
        private string GetHTMlTable(List<AttendanceRegularizationWorkFlow> attendanceRegularizationWorkFlow)
        {
            string tableHtml = "";
            DataTable dt = new DataTable("AttendanceRegularization");
            dt.Columns.Add(new DataColumn("AttendanceDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("ActualFirstIn", typeof(string)));
            dt.Columns.Add(new DataColumn("ActualLastOut", typeof(string)));
            dt.Columns.Add(new DataColumn("EmployeeFirstIn", typeof(string)));
            dt.Columns.Add(new DataColumn("EmployeeLastOut", typeof(string)));
            dt.Columns.Add(new DataColumn("Remarks", typeof(string)));           
            int i = 1;
            foreach (var attendance in attendanceRegularizationWorkFlow)
            {

                DataRow dr = dt.NewRow();
                dr["AttendanceDate"] = attendance.RegularizationAppliedDate;
                dr["ActualFirstIn"] = "---NA---";
                dr["ActualLastOut"] = "---NA---";
                dr["EmployeeFirstIn"] = attendance.InTime;
                dr["EmployeeLastOut"] = attendance.OutTime;
                dr["Remarks"] = attendance.Remarks;
                dt.Rows.Add(dr);
                i++;
            }

            //tableHtml += "<table>";
           // tableHtml += "<tr  style=white-space:nowrap><th>S.no</th><th>Skill Name</th><th>Current Proficiency Level</th><th>New Proficiency Level</th><th>Current Last Used</th><th>New Last Used</th><th>Current Experience</th><th>New Experience</th></tr>";

            foreach (DataRow drAttendace in dt.Rows)
            {
                tableHtml += "<tr><td>" + drAttendace["AttendanceDate"] + "</td><td>" + drAttendace["ActualFirstIn"] + "</td><td>" + drAttendace["ActualLastOut"] + "</td><td>" + drAttendace["EmployeeFirstIn"] + "</td>" +
                 "<td>" + drAttendace["EmployeeLastOut"] + " </td ><td> " + drAttendace["Remarks"] + " </td></tr>";
            }
            //tableHtml += "</table>";

          
            return tableHtml;

        }

        private List<string> GetLeaveDates(List<AssociateLeave> associateLeaves)
        {
            List<string> leaveDate = new List<string>();
            foreach (var associate in associateLeaves)
            {
                var fromDate = associate.FromDate;
                var toDate = associate.ToDate;
                if (fromDate == toDate)
                {
                    leaveDate.Add(fromDate.ToString("dd-MM-yyyy"));
                }
                else
                {
                    while (fromDate <= toDate)
                    {
                        leaveDate.Add(fromDate.ToString("dd-MM-yyyy"));
                        fromDate = fromDate.AddDays(1);
                    }
                }
            }
            return leaveDate;
        }
        #endregion

        #region Send Notification
        /// <summary>
        /// Sends notification to reporting manager when an associate submits a regularization request.
        /// </summary>
        /// <param name="attendanceRegularizationWorkFlows">Submitted workflows.</param>
        public void RegularizationOnSubmitNotification( List<AttendanceRegularizationWorkFlow> attendanceRegularizationWorkFlows)
        {
            string AssociateId = attendanceRegularizationWorkFlows.First().SubmittedBy;
            var employee = m_employeeDBContext.Employees.Where(emp => emp.EmployeeCode == AssociateId && emp.IsActive == true).FirstOrDefault();
            string associateName = employee.FirstName + " " + employee.LastName;
            var reportingManagerDetails = m_employeeDBContext.Employees.Where(emp => emp.EmployeeId == employee.ReportingManager && emp.IsActive == true).FirstOrDefault();
            string reportingManagerName = reportingManagerDetails.FirstName + " " + reportingManagerDetails.LastName;
            
            var htmlTable = GetHTMlTable(attendanceRegularizationWorkFlows);

            string FilePath = Utility.GetNotificationTemplatePath(NotificationTemplatePaths.currentDirectory,NotificationTemplatePaths.subDirectories_Regularization_Notification);
            StreamReader stream = new StreamReader(FilePath);
            string MailText = stream.ReadToEnd();

            stream.Close();
            MailText = MailText.Replace("{ReportingManager}", reportingManagerName).Replace("{AssociateName}", associateName)
                .Replace("{AssociateCode}", AssociateId).Replace("{TableData}", htmlTable);
            NotificationDetail notificationDetail = new NotificationDetail();
            notificationDetail.EmailBody = MailText;                      
            notificationDetail.Subject = "Attendance Regularization Request from"+ associateName+" "+ AssociateId;
            notificationDetail.ToEmail = reportingManagerDetails.WorkEmailAddress;
            notificationDetail.FromEmail = m_EmailConfigurations.FromEmail;
            m_organizationService.SendEmail(notificationDetail);
        }

        /// <summary>
        /// Sends notification to associate on approval or rejection of their request.
        /// </summary>
        /// <param name="associateName">Associate name.</param>
        /// <param name="associateEmail">Associate email address.</param>
        /// <param name="regularizationDates">Dates of regularization.</param>
        /// <param name="RegularizationStatus">Status value.</param>
        public void RegularizationApproveOrRejectNotification(string associateName,string associateEmail , List<DateTime> regularizationDates, int RegularizationStatus)
        {
            var attendanceRegularizationDates =string.Join(",", regularizationDates.Select(x => x.ToString("yyyy-MM-dd")).ToList());
            NotificationDetail notificationDetail = new NotificationDetail();
            if (Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationApproved) == RegularizationStatus)
            {
                notificationDetail.Subject = "Your attendance regularization request has been approved.";
                notificationDetail.EmailBody = "<html><p> Dear "+associateName + "</p> <p>Your attendance regularization request for "+ attendanceRegularizationDates + " has been approved.</p><br/><p>Thank you.</p> </html>";
            }
            else if (Convert.ToInt32(AttendanceRegularizationStatusCode.AttendanceRegularizationRejected) == RegularizationStatus)
            {
                notificationDetail.Subject = "Your attendance regularization request has been rejected.";
               notificationDetail.EmailBody = "<html><p> Dear " + associateName + "</p> <p>Your attendance regularization request for " + attendanceRegularizationDates + " has been rejected.</p><br/><p>Thank you.</p> </html>";
            }
            notificationDetail.FromEmail = m_EmailConfigurations.FromEmail;
            notificationDetail.ToEmail = associateEmail;
            m_organizationService.SendEmail(notificationDetail);
        }

        #endregion
    }
}
