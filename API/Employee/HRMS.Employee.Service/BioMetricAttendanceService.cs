using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
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
using Microsoft.Graph;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static OfficeOpenXml.ExcelErrorValue;

namespace HRMS.Employee.Service
{
   public class BioMetricAttendanceService: IBioMetricAttendanceService
    {
        #region Global Varibles
        private readonly EmployeeDBContext _employeeDBContext;
        private readonly IProjectService _projectService;
        private readonly IOrganizationService _organizationService;
        private readonly ILogger<AttendanceReportService> _logger;
        private readonly IConfiguration _configuration;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;
        #endregion

        #region Constructor
        public BioMetricAttendanceService(EmployeeDBContext employeeDBContext,
                                  IProjectService projectService,
                                  IOrganizationService organizationService,
                                  ILogger<AttendanceReportService> logger,
                                  IConfiguration configuration,
                                  IOptions<MiscellaneousSettings> miscellaneousSettings)
        {
            _employeeDBContext = employeeDBContext;
            _projectService = projectService;
            _organizationService = organizationService;
            _logger = logger;
            _configuration = configuration;
            m_MiscellaneousSettings = miscellaneousSettings?.Value;
        }
        #endregion

        #region GetAttendanceMaxDate
        public async Task<DateTime?> GetAttendanceMaxDate()
        {
            var listDates = await _employeeDBContext.BioMetricAttendenceDetail.Where(d => d.Punch1_Date != null).Select(d => d.Punch1_Date).ToListAsync();
            DateTime? date = listDates.Max();
            return date;
        }
        #endregion

        #region IsDeliveryDepartment
        public async Task<bool> IsDeliveryDepartment(int employeeId)
        {
            bool result = false;

            int deptId = await _employeeDBContext.Employees.Where(c => c.EmployeeId == employeeId).Select(d => d.DepartmentId.Value).FirstOrDefaultAsync();
            if (deptId == 1) result = true;
            return result;
        }
        #endregion

        #region GetAttendanceSummaryReport
        public async Task<List<AttendanceReport>> GetAttendanceSummaryReport(AttendanceReportFilter filter)
        {
            List<AttendanceReport> details = new List<AttendanceReport>();
            try
            {
                List<AttendanceReport> attendanceReport = new List<AttendanceReport>();
                List<BioMetricAttendance> attendance = new List<BioMetricAttendance>();
                List<string> employeeCodes = new List<string>();
                List<string> projectEmployeeCodes = new List<string>();
                string FromDate = string.Empty;
                string ToDate = string.Empty;
                int? departmentId = await _employeeDBContext.Employees.Where(c => c.EmployeeId == (filter.ManagerId != 0 ? filter.ManagerId : filter.EmployeeId)).Select(d => d.DepartmentId).FirstOrDefaultAsync();

                var holidays = GetHolidays();
                int totalWorkingDays = 0;
                int totalHolidays = 0;
                List<string> wfhAssociatesList = new List<string>();
                string wfhAssociates = m_MiscellaneousSettings.WFHAssociates;
                if (!string.IsNullOrWhiteSpace(wfhAssociates))
                {
                    wfhAssociatesList = wfhAssociates?.Split(',').ToList();
                }

                for (var day = filter.FromDate.Date; day.Date <= filter.ToDate.Date; day = day.AddDays(1))
                {
                    if (day.Date.DayOfWeek != System.DayOfWeek.Sunday && day.Date.DayOfWeek != System.DayOfWeek.Saturday)
                    {
                        if (holidays!=null && holidays.Contains(day.Date))
                        {
                            totalHolidays++;
                        }
                        else
                        {
                            totalWorkingDays++;
                        }
                    }
                }

                if (filter.EmployeeId != 0 && string.IsNullOrWhiteSpace(filter.EmployeeCode))
                {
                    filter.EmployeeCode = await _employeeDBContext.Employees.Where(c => c.EmployeeId == filter.EmployeeId).Select(x => x.EmployeeCode).FirstOrDefaultAsync();
                }

                if (filter.RoleName == "Associate")
                {
                    employeeCodes.Add(filter.EmployeeCode.ToLower());
                }

                var employees = GetAssociateInfo(filter.EmployeeCode, filter.ManagerId, filter.RoleName, departmentId.Value, filter.ProjectId ?? 0, filter.IsLeadership ?? false).Result.Items;

                if (filter.RoleName == "Program Manager")
                {
                    employeeCodes.AddRange(employees.Where(c => c.ProgramManagerId.HasValue && c.ProgramManagerId == filter.ManagerId).Select(d => d.AssociateCode.ToLower()).ToList());
                }
                else if (filter.RoleName == "Reporting Manager")
                {
                    employeeCodes.AddRange(employees.Where(c => c.ReportingManagerId != 0 && c.ReportingManagerId == filter.ManagerId).Select(d => d.AssociateCode.ToLower()).ToList());
                }
                else if (filter.RoleName == "Delivery")
                {
                    employeeCodes.AddRange(employees.Where(c => c.DepartmentId.HasValue && c.DepartmentId == 1).Select(d => d.AssociateCode.ToLower()).ToList());
                }
                else if (filter.RoleName == "Department Head" && filter.IsLeadership == true)
                {
                    var lemps = await _employeeDBContext.LeadershipAssociates.Where(e => e.IsActive == true).ToListAsync();

                    employeeCodes.AddRange(employees.Join(lemps, emp => emp.AssociateId, lemp => lemp.AssociateId,
                                             (emp, lemp) => new { emp, lemp }).Where(e => e.lemp.IsActive == true)
                                             .Select(d => (d.emp.AssociateCode).ToLower()).ToList());
                }
                else if (filter.RoleName == "Department Head" && filter.IsLeadership == false)
                {
                    employeeCodes.AddRange(employees.Where(c => c.DepartmentId.HasValue && c.DepartmentId == departmentId).Select(d => d.AssociateCode.ToLower()).ToList());
                }
                else if (filter.RoleName == "Team Lead")
                {
                    employeeCodes.AddRange(employees.Where(c => c.ReportingManagerId != 0 && c.ReportingManagerId == filter.ManagerId).Select(d => d.AssociateCode.ToLower()).ToList());
                }
                else if (filter.RoleName == "HRA" || filter.RoleName == "HRM")
                {
                    employeeCodes.AddRange(employees.Select(d => d.AssociateCode.ToLower()).ToList());
                }

                if (filter.ProjectId.HasValue && filter.ProjectId > 0)
                {
                    projectEmployeeCodes.AddRange(employees.Where(c => c.ProjectId.HasValue && c.ProjectId == filter.ProjectId).Select(d => d.AssociateCode.ToLower()).ToList());
                }

                if (!string.IsNullOrWhiteSpace(filter.EmployeeCode))
                {
                    employeeCodes = new List<string>
                    {
                        filter.EmployeeCode.ToLower()
                    };
                }

                if (!string.IsNullOrWhiteSpace(filter.EmployeeCode) && (filter.ProjectId.HasValue == false || filter.ProjectId.Value == 0))
                {
                    var bioMetricAttendances = await _employeeDBContext.BioMetricAttendenceDetail.Where(c => filter.EmployeeCode.ToLower() == c.AsscociateId.ToLower()).ToListAsync();
                    attendance = bioMetricAttendances.Where(c => (c.Punch1_Date != null && c.Punch1_Date?.Date >= filter.FromDate.Date && c.Punch1_Date?.Date < filter.ToDate.AddDays(1).Date)
                                 || (c.Punch1_Date == null && c.Punch2_Date?.Date >= filter.FromDate.Date && c.Punch2_Date?.Date < filter.ToDate.AddDays(1).Date)).ToList();
                }
                else if (string.IsNullOrWhiteSpace(filter.EmployeeCode) && (filter.ProjectId.HasValue == false || filter.ProjectId.Value == 0))
                {
                    var bioMetricAttendances = await _employeeDBContext.BioMetricAttendenceDetail.Where(c => employeeCodes.Contains(c.AsscociateId.ToLower())).ToListAsync();
                    attendance = bioMetricAttendances.Where(c => (c.Punch1_Date != null && c.Punch1_Date?.Date >= filter.FromDate.Date && c.Punch1_Date?.Date < filter.ToDate.AddDays(1).Date)
                                || (c.Punch1_Date == null && c.Punch2_Date?.Date >= filter.FromDate.Date && c.Punch2_Date?.Date < filter.ToDate.AddDays(1).Date)).ToList();
                }
                else if (!string.IsNullOrWhiteSpace(filter.EmployeeCode) && (filter.ProjectId.HasValue && filter.ProjectId.Value > 0))
                {
                    var bioMetricAttendances = await _employeeDBContext.BioMetricAttendenceDetail.Where(c => filter.EmployeeCode.ToLower() == c.AsscociateId.ToLower()).ToListAsync();
                    attendance = bioMetricAttendances.Where(c => (c.Punch1_Date != null && c.Punch1_Date?.Date >= filter.FromDate.Date && c.Punch1_Date?.Date < filter.ToDate.AddDays(1).Date)
                    || (c.Punch1_Date == null && c.Punch2_Date?.Date >= filter.FromDate.Date && c.Punch2_Date?.Date < filter.ToDate.AddDays(1).Date)).ToList();
                }
                else if (string.IsNullOrWhiteSpace(filter.EmployeeCode) && (filter.ProjectId.HasValue && filter.ProjectId.Value > 0))
                {
                    var bioMetricAttendances = await _employeeDBContext.BioMetricAttendenceDetail.Where(c => employeeCodes.Contains(c.AsscociateId.ToLower()) && projectEmployeeCodes.Contains(c.AsscociateId.ToLower())).ToListAsync();
                    attendance = bioMetricAttendances.Where(c => (c.Punch1_Date != null && c.Punch1_Date?.Date >= filter.FromDate.Date && c.Punch1_Date?.Date < filter.ToDate.AddDays(1).Date)
                    || (c.Punch1_Date == null && c.Punch2_Date?.Date >= filter.FromDate.Date && c.Punch2_Date?.Date < filter.ToDate.AddDays(1).Date)).ToList();
                }

                if (filter.ProjectId.HasValue && filter.ProjectId > 0 && string.IsNullOrEmpty(filter.EmployeeCode))
                {
                    foreach (string code in projectEmployeeCodes)
                    {
                        AttendanceReport associate = new AttendanceReport();
                        var associateAttendance = attendance.Where(x => x.AsscociateId.ToLower() == code.ToLower()).ToList();
                        decimal TotalWFHDays = 0;
                        decimal TotalWFODays = 0;
                        decimal TotalLeaves = 0;
                        if (associateAttendance.Count()>0)
                        {

                            associateAttendance = RemoveDuplicateAttendanceDate(associateAttendance);
                            (TotalWFHDays, TotalWFODays, TotalLeaves) = GetDaysCalculations(code, filter.FromDate.Date, filter.ToDate.Date, associateAttendance);
                        }
                        EmployeeInfo info = employees.Where(c => c.AssociateCode.ToLower() == code.ToLower()).FirstOrDefault();
                        associate.EmployeeName = info.AssociateName;
                        associate.EmployeeCode = info.AssociateCode;
                        associate.ProjectName = info.ProjectName;
                        associate.DepartmentName = info.DepartmentName;
                        associate.ReportingManagerName = info.ReportingManagerName;
                        associate.TotalWorkingDays = totalWorkingDays;
                        associate.TotalHolidays = totalHolidays;
                        associate.WorkFromHome = false;
                        associate.TotalWFHDays = TotalWFHDays;
                        associate.TotalWFODays = TotalWFODays;
                        associate.TotalLeaves = TotalLeaves;
                        associate.TotalDaysWorked = TotalWFHDays + TotalWFODays;
                        if (wfhAssociatesList.Contains(info.AssociateCode))
                            associate.WorkFromHome = true;
                        details.Add(associate);
                    }
                }
                else
                {
                    foreach (string code in employeeCodes)
                    {
                        decimal TotalWFHDays = 0;
                        decimal TotalWFODays = 0;
                        decimal TotalLeaves = 0;
                        AttendanceReport associate = new AttendanceReport();
                        var associateAttendance = attendance.Where(x => x.AsscociateId.ToLower() == code.ToLower()).ToList();
                        if (associateAttendance.Count() > 0)
                        {
                            associateAttendance = RemoveDuplicateAttendanceDate(associateAttendance);
                            (TotalWFHDays, TotalWFODays, TotalLeaves) = GetDaysCalculations(code, filter.FromDate.Date, filter.ToDate.Date, associateAttendance);
                        }
                        EmployeeInfo info = employees.Where(c => c.AssociateCode.ToLower() == code.ToLower()).FirstOrDefault();
                        associate.EmployeeName = info.AssociateName;
                        associate.EmployeeCode = info.AssociateCode;
                        associate.DepartmentName = info.DepartmentName;
                        associate.ProjectName = info.ProjectName;
                        associate.ReportingManagerName = info.ReportingManagerName;                       
                        associate.TotalWorkingDays = totalWorkingDays;
                        associate.TotalHolidays = totalHolidays;
                        associate.WorkFromHome = false;
                        associate.TotalWFHDays = TotalWFHDays;
                        associate.TotalWFODays = TotalWFODays;
                        associate.TotalLeaves = TotalLeaves;
                        associate.TotalDaysWorked = TotalWFHDays + TotalWFODays;
                        associate.CompliancePrecentage = Math.Floor((TotalWFODays / (totalWorkingDays - TotalLeaves)) * 100);
                        if (wfhAssociatesList.Contains(info.AssociateCode))
                            associate.WorkFromHome = true;
                        details.Add(associate);
                    }
                }

                if (details != null && details.Count > 0)
                    details = details.OrderBy(c => c.EmployeeCode).ToList();
                return details;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error = > " + ex.Message);
                _logger.LogInformation("Error = > " + ex.StackTrace);
            }
            return details;
        }
        #endregion

        #region GetAttendanceDetailReport
        public async Task<List<AttendanceDetailReport>> GetAttendanceDetailReport(AttendanceReportFilter filter)
        {
            List<AttendanceDetailReport> attendanceReport = new List<AttendanceDetailReport>();
            List<BioMetricAttendance> attendance = new List<BioMetricAttendance>();
            try
            {

                var holidays = GetHolidays();
                var bioMetricAttendances = await _employeeDBContext.BioMetricAttendenceDetail.Where(c => c.AsscociateId.ToLower() == filter.EmployeeCode.ToLower()).ToListAsync();
                attendance = bioMetricAttendances.Where(c => (c.Punch1_Date != null && c.Punch1_Date?.Date >= filter.FromDate.Date && c.Punch1_Date?.Date < filter.ToDate.AddDays(1).Date) ||
                            (c.Punch1_Date == null && c.Punch2_Date?.Date >= filter.FromDate.Date && c.Punch2_Date?.Date < filter.ToDate.AddDays(1).Date)).ToList();
                if (attendance.Count() > 0)
                {
                    //Remove Duplicate Date (Remove WFH Record if found duplicates)
                    attendance = RemoveDuplicateAttendanceDate(attendance);
                }
                var leaveData = _employeeDBContext.AssociateLeave.Where(x => x.EmployeeCode.ToLower() == filter.EmployeeCode.ToLower()).ToList();
                var leavedetails = leaveData.Where(c => c.FromDate.Date >= filter.FromDate.Date && c.ToDate.Date <= filter.ToDate.Date).ToList();
                var leaveDates = GetLeaveDates(leavedetails);
                if ((attendance.Count > 0 || holidays != null || leaveDates.Count > 0))
                {
                    for (var date = filter.FromDate.Date; date.Date <= filter.ToDate.Date; date = date.AddDays(1))
                    {
                        List<BioMetricAttendance> dayAttendance = attendance.Where(c => (c.Punch1_Date != null && c.Punch1_Date?.Date == date.Date) || (c.Punch1_Date == null && c.Punch2_Date?.Date == date.Date)).ToList();

                        if (dayAttendance != null && dayAttendance.Count > 0)
                        {
                            foreach (BioMetricAttendance associate in dayAttendance)
                            {

                                if (leaveDates.Select(leave => leave.LeaveDate).Contains(date.ToString()))
                                {
                                    var leaveDatail = leaveDates.Where(leave => leave.LeaveDate == date.ToString()).First();
                                    AttendanceDetailReport leave = new AttendanceDetailReport();
                                    leave.date = date.Date.ToString("yyyy-MM-dd");
                                    leave.title = $"{leaveDatail.LeaveType} ({leaveDatail.Session})";
                                    attendanceReport.Add(leave);
                                }
                                AttendanceDetailReport checkIn = new AttendanceDetailReport();
                                string checkinTime = string.IsNullOrWhiteSpace(associate.InTime) ? "N/A" : associate.InTime.Substring(0, 5);
                                checkIn.date = associate.Punch1_Date != null ? associate.Punch1_Date?.Date.ToString("yyyy-MM-dd") : associate.Punch2_Date?.Date.ToString("yyyy-MM-dd");
                                checkIn.title = $"Checkin Time: {checkinTime}";
                                attendanceReport.Add(checkIn);

                                string checkoutTime = string.IsNullOrWhiteSpace(associate.OutTime) ? "N/A" : associate.OutTime.Substring(0, 5);
                                AttendanceDetailReport checkOut = new AttendanceDetailReport();
                                checkOut.date = associate.Punch1_Date != null ? associate.Punch1_Date?.Date.ToString("yyyy-MM-dd") : associate.Punch2_Date?.Date.ToString("yyyy-MM-dd");
                                checkOut.title = $"Checkout Time: {checkoutTime}";
                                attendanceReport.Add(checkOut);

                                AttendanceDetailReport totalHours = new AttendanceDetailReport();
                                string total = associate.WorkTime_HHMM == null || associate.WorkTime_HHMM == "" || associate.WorkTime_HHMM == "00:00" ? "0" : associate.WorkTime_HHMM;
                                totalHours.date = associate.Punch1_Date != null ? associate.Punch1_Date?.Date.ToString("yyyy-MM-dd") : associate.Punch2_Date?.Date.ToString("yyyy-MM-dd");
                                totalHours.title = $"Total Hours: {total}";
                                attendanceReport.Add(totalHours);

                                AttendanceDetailReport location = new AttendanceDetailReport();
                                location.date = associate.Punch1_Date != null ? associate.Punch1_Date?.Date.ToString("yyyy-MM-dd") : associate.Punch2_Date?.Date.ToString("yyyy-MM-dd");
                                location.title = associate.Location;
                                attendanceReport.Add(location);


                                AttendanceDetailReport summary = new AttendanceDetailReport();
                                summary.date = associate.Punch1_Date != null ? associate.Punch1_Date?.Date.ToString("yyyy-MM-dd") : associate.Punch2_Date?.Date.ToString("yyyy-MM-dd");
                                summary.title = associate.SUMMARY != null && associate.SUMMARY.Contains(":") ? associate.SUMMARY.Split(":")[1] : associate.SUMMARY;
                                attendanceReport.Add(summary);

                                if (associate.IsRegularized == true)
                                {
                                    AttendanceDetailReport regularization = new AttendanceDetailReport();
                                    regularization.date = associate.Punch1_Date != null ? associate.Punch1_Date?.Date.ToString("yyyy-MM-dd") : associate.Punch2_Date?.Date.ToString("yyyy-MM-dd");
                                    regularization.title = "Regularized";
                                    attendanceReport.Add(regularization);
                                }


                            }
                        }

                        else if (date.DayOfWeek == System.DayOfWeek.Saturday || date.DayOfWeek == System.DayOfWeek.Sunday)
                        {
                            AttendanceDetailReport weekend = new AttendanceDetailReport();
                            weekend.date = date.Date.ToString("yyyy-MM-dd");
                            weekend.title = $"Weekend";
                            attendanceReport.Add(weekend);
                        }
                        else if (holidays.Contains(date.Date))
                        {
                            AttendanceDetailReport holiday = new AttendanceDetailReport();
                            holiday.date = date.Date.ToString("yyyy-MM-dd");
                            holiday.title = $"Holiday";
                            attendanceReport.Add(holiday);
                        }
                        else if (leaveDates.Select(leave => leave.LeaveDate).Contains(date.ToString()))
                        {
                            var leaveDatail = leaveDates.Where(leave => leave.LeaveDate == date.ToString()).First();
                            AttendanceDetailReport leave = new AttendanceDetailReport();
                            leave.date = date.Date.ToString("yyyy-MM-dd");
                            leave.title = leaveDatail.Session == null ? leaveDatail.LeaveType : $"{leaveDatail.LeaveType} ({leaveDatail.Session})";
                            attendanceReport.Add(leave);
                        }
                        else 
                        {                           
                            AttendanceDetailReport leave = new AttendanceDetailReport();
                            leave.date = date.Date.ToString("yyyy-MM-dd");
                            leave.title = "Absent";
                            attendanceReport.Add(leave);
                        }
                      
                    }
                }
              
            }
            catch(Exception e)
            {
                
            }
            return attendanceReport;
        }
        #endregion

        #region GetAdvanceAttendanceReport
        public async Task<byte[]> GetAdvanceAttendanceReport(AttendanceReportFilter filter)
        {
            byte[] xlsAdvanceReport = null;
            try
            {
                int? departmentId = await _employeeDBContext.Employees.Where(c => c.EmployeeId == (filter.ManagerId != 0 ? filter.ManagerId : filter.EmployeeId)).Select(d => d.DepartmentId).FirstOrDefaultAsync();
                List<DateTime> holidays = GetHolidays();
                List<AssociateAdvanceAttendanceDetails> associatesDetailsList = new List<AssociateAdvanceAttendanceDetails>();
                List<EmployeeInfo> employeeInfos = GetAssociateInfo(filter.EmployeeCode, filter.ManagerId, filter.RoleName, departmentId.Value, filter.ProjectId ?? 0, filter.IsLeadership ?? false).Result.Items;
                associatesDetailsList = GetAssociateAdvanceDetailsForHR(filter, employeeInfos);
                if(associatesDetailsList != null)
                {
                    List<AttendanceReport> attendanceSummaryReport = await GetAttendanceSummaryReport(filter);
                    var uniqueDates = associatesDetailsList.Select(d => $"{d.Date}/{d.DayOfWeek}").Distinct().OrderBy(d => d).ToList();
                    var groupedData = GroupAssociatesWithDates(associatesDetailsList,uniqueDates,holidays,attendanceSummaryReport);
                    DataTable advanceDataTable = LinqToDataTable(groupedData, uniqueDates);
                    xlsAdvanceReport = CreateAdvanceXLSAttachment(advanceDataTable);
                    return xlsAdvanceReport;
                }
            }
            catch (Exception e)
            {

            }
            return xlsAdvanceReport;
        }
        #endregion

        #region GetAssociateInfo
        /// <summary>
        /// Get the Employee info 
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeInfo>> GetAssociateInfo(int departmentId)
        {

            var response = new ServiceListResponse<EmployeeInfo>();
            try
            {
                var departments = await _organizationService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.Message = departments.Message;
                    response.IsSuccessful = false;
                    return response;
                }

                var projects = await _projectService.GetAllProjects();
                if (!projects.IsSuccessful)
                {
                    response.Message = projects.Message;
                    response.IsSuccessful = false;
                    return response;
                }
                List<string> excludedEmployees = new List<string>();

                string excludedAssociates = m_MiscellaneousSettings.ExcludeAssociates;
                if (!string.IsNullOrWhiteSpace(excludedAssociates))
                {
                    excludedEmployees = excludedAssociates?.Split(',').ToList();
                }

                var employees = _employeeDBContext.Employees.Where(e => e.DepartmentId == departmentId && e.IsActive == true && e.Nationality != "US" && e.Nationality != "USA"
                && !excludedEmployees.Contains(e.EmployeeCode))
                    .Select(d => new EmployeeInfo
                    {
                        AssociateId = d.EmployeeId,
                        AssociateCode = d.EmployeeCode,
                        AssociateName = d.FirstName + " " + d.LastName,
                        DepartmentId = d.DepartmentId,
                        ReportingManagerId = d.ReportingManager ?? 0,
                        ProjectId = 0,
                        ProgramManagerId = 0
                    }).ToList();

                if (departmentId == 1)
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (!allocations.IsSuccessful)
                    {
                        response.Message = allocations.Message;
                        response.IsSuccessful = false;
                        return response;
                    }

                    if (allocations != null && allocations.Items.Count > 0)
                    {
                        foreach (EmployeeInfo info in employees)
                        {
                            info.ProgramManagerId = allocations.Items.Where(c => c.AssociateId == info.AssociateId).Select(d => d.ProgramManagerId).FirstOrDefault();
                            info.ProjectId = allocations.Items.Where(c => c.AssociateId == info.AssociateId).Select(d => d.ProjectId).FirstOrDefault();
                        }
                    }
                }

                List<int> managerIds = new List<int>();

                managerIds.AddRange(employees.Where(d => d.ReportingManagerId > 0).Select(c => c.ReportingManagerId).ToList());

                if (departmentId == 1)
                    managerIds.AddRange(employees.Where(d => d.ProgramManagerId.HasValue).Select(c => c.ProgramManagerId.Value).ToList());

                List<GenericType> managers = _employeeDBContext.Employees.Where(c => managerIds.Contains(c.EmployeeId))
                    .Select(d => new GenericType
                    {
                        Id = d.EmployeeId,
                        Name = d.FirstName + " " + d.LastName
                    }).ToList();

                foreach (EmployeeInfo info in employees)
                {
                    if (info.ReportingManagerId > 0)
                        info.ReportingManagerName = managers.Where(c => c.Id == info.ReportingManagerId).Select(d => d.Name).FirstOrDefault();

                    if (departmentId == 1 && info.ProgramManagerId.HasValue)
                        info.ProgramManagerName = managers.Where(c => c.Id == info.ProgramManagerId).Select(d => d.Name).FirstOrDefault();

                    if (info.DepartmentId.HasValue)
                        info.DepartmentName = departments.Items.Where(c => c.DepartmentId == info.DepartmentId).Select(d => d.Description).FirstOrDefault();

                    if (departmentId == 1 && info.ProjectId.HasValue)
                        info.ProjectName = projects.Items.Where(c => c.ProjectId == info.ProjectId).Select(d => d.ProjectName).FirstOrDefault();
                }

                response.IsSuccessful = true;
                response.Items = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Employee information";
                _logger.LogError(ex.StackTrace);
            }
            return response;

        }

        public async Task<ServiceListResponse<EmployeeInfo>> GetAssociateInfo(string employeeCode, int employeeId, string roleName, int departmentId, int projectId = 0, bool isLeadership = false)
        {
            var response = new ServiceListResponse<EmployeeInfo>();


            try
            {
                var departments = await _organizationService.GetAllDepartments();
                if (!departments.IsSuccessful)
                {
                    response.Message = departments.Message;
                    response.IsSuccessful = false;
                    return response;
                }

                var projects = await _projectService.GetAllProjects();
                if (!projects.IsSuccessful)
                {
                    response.Message = projects.Message;
                    response.IsSuccessful = false;
                    return response;
                }



                List<string> excludedEmployees = new List<string>();
                string excludedAssociates = _configuration.GetSection("ExcludeAssociates")?.Value;

                if (!string.IsNullOrWhiteSpace(excludedAssociates))
                {
                    excludedEmployees = excludedAssociates?.Split(',').ToList();
                }

                List<GenericType> associates = GetAssociatesReportingToManager(employeeId, roleName, projectId, isLeadership).Result.Items;
                List<int> employeeList = associates.Select(c => c.Id).ToList();

                var employees = new List<EmployeeInfo>();
                if (!string.IsNullOrWhiteSpace(employeeCode))
                {
                    employees = _employeeDBContext.Employees.Where(e => e.EmployeeCode == employeeCode && e.IsActive == true && e.Nationality != "US" && e.Nationality != "USA")
                        .Select(d => new EmployeeInfo
                        {
                            AssociateId = d.EmployeeId,
                            AssociateCode = d.EmployeeCode,
                            AssociateName = d.FirstName + " " + d.LastName,
                            DepartmentId = d.DepartmentId,
                            ReportingManagerId = d.ReportingManager ?? 0,
                            ProjectId = 0,
                            ProgramManagerId = 0
                        }).ToList();
                }
                else
                {
                    employees = _employeeDBContext.Employees.Where(e => employeeList.Contains(e.EmployeeId) && e.IsActive == true && e.Nationality != "US" && e.Nationality != "USA"
                    && !excludedEmployees.Contains(e.EmployeeCode))
                        .Select(d => new EmployeeInfo
                        {
                            AssociateId = d.EmployeeId,
                            AssociateCode = d.EmployeeCode,
                            AssociateName = d.FirstName + " " + d.LastName,
                            DepartmentId = d.DepartmentId,
                            ReportingManagerId = d.ReportingManager ?? 0,
                            ProjectId = 0,
                            ProgramManagerId = 0
                        }).ToList();
                }

                if (departmentId == 1)
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (!allocations.IsSuccessful)
                    {
                        response.Message = allocations.Message;
                        response.IsSuccessful = false;
                        return response;
                    }

                    if (allocations != null && allocations.Items.Count > 0)
                    {
                        foreach (EmployeeInfo info in employees)
                        {
                            info.ProgramManagerId = allocations.Items.Where(c => c.AssociateId == info.AssociateId).Select(d => d.ProgramManagerId).FirstOrDefault();
                            info.ProjectId = allocations.Items.Where(c => c.AssociateId == info.AssociateId).Select(d => d.ProjectId).FirstOrDefault();
                        }
                    }
                }

                List<int> managerIds = new List<int>();

                managerIds.AddRange(employees.Where(d => d.ReportingManagerId > 0).Select(c => c.ReportingManagerId).ToList());

                if (departmentId == 1)
                    managerIds.AddRange(employees.Where(d => d.ProgramManagerId.HasValue).Select(c => c.ProgramManagerId.Value).ToList());

                List<GenericType> managers = _employeeDBContext.Employees.Where(c => managerIds.Contains(c.EmployeeId))
                    .Select(d => new GenericType
                    {
                        Id = d.EmployeeId,
                        Name = d.FirstName + " " + d.LastName
                    }).ToList();

                foreach (EmployeeInfo info in employees)
                {
                    if (info.ReportingManagerId > 0)
                        info.ReportingManagerName = managers.Where(c => c.Id == info.ReportingManagerId).Select(d => d.Name).FirstOrDefault();

                    if (departmentId == 1 && info.ProgramManagerId.HasValue)
                        info.ProgramManagerName = managers.Where(c => c.Id == info.ProgramManagerId).Select(d => d.Name).FirstOrDefault();

                    if (info.DepartmentId.HasValue)
                        info.DepartmentName = departments.Items.Where(c => c.DepartmentId == info.DepartmentId).Select(d => d.Description).FirstOrDefault();

                    if (departmentId == 1 && info.ProjectId.HasValue)
                        info.ProjectName = projects.Items.Where(c => c.ProjectId == info.ProjectId).Select(d => d.ProjectName).FirstOrDefault();
                }

                response.IsSuccessful = true;
                response.Items = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching Employee information";
                _logger.LogError(ex.StackTrace);
            }
            return response;

        }
        #endregion

        #region GetAssociatesReportingToManager
        public async Task<ServiceListResponse<GenericType>> GetAssociatesReportingToManager(int employeeId, string roleName, int projectId = 0, bool isLeadership = false)
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<string> excludedEmployees = new List<string>();
                string excludedAssociates = _configuration.GetSection("ExcludeAssociates")?.Value;

                if (!string.IsNullOrWhiteSpace(excludedAssociates))
                {
                    excludedEmployees = excludedAssociates?.Split(',').ToList();
                }

                List<GenericType> employees = new List<GenericType>();
                List<int> employeeIds = new List<int>();
                int deptId = await _employeeDBContext.Employees.Where(c => c.EmployeeId == employeeId).Select(d => d.DepartmentId ?? 0).FirstOrDefaultAsync();

                if (roleName.ToLower() == "Program Manager".ToLower())
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (allocations != null && allocations.Items != null)
                    {
                        if (projectId > 0)
                            employeeIds = (from allocation in allocations.Items
                                           where allocation.ProgramManagerId == employeeId
                                           && allocation.ProjectId == projectId
                                           select (int)allocation.AssociateId
                                           ).Distinct().ToList();
                        else
                            employeeIds = (from allocation in allocations.Items
                                           where allocation.ProgramManagerId == employeeId
                                           select (int)allocation.AssociateId
                                       ).Distinct().ToList();
                    }
                }
                else if ((roleName.ToLower() == "Team Lead".ToLower() ||
                    roleName.ToLower() == "Reporting Manager".ToLower()) && deptId == 1)
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (allocations != null && allocations.Items != null)
                    {
                        if (projectId > 0)
                            employeeIds = (from allocation in allocations.Items
                                           where allocation.ReportingManagerId == employeeId
                                            && allocation.ProjectId == projectId
                                           select (int)allocation.AssociateId
                                       ).Distinct().ToList();
                        else
                            employeeIds = (from allocation in allocations.Items
                                           where allocation.ReportingManagerId == employeeId
                                           select (int)allocation.AssociateId
                                       ).Distinct().ToList();
                    }
                }
                else if (roleName.ToLower() == "Delivery".ToLower())
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (allocations != null && allocations.Items != null)
                    {
                        if (projectId > 0)
                            employeeIds = (from allocation in allocations.Items
                                           where allocation.ProjectId == projectId
                                           select (int)allocation.AssociateId
                                       ).Distinct().ToList();
                        else
                            employeeIds = (from allocation in allocations.Items
                                           select (int)allocation.AssociateId
                                      ).Distinct().ToList();
                    }
                }

                if (employeeIds != null && employeeIds.Count > 0)
                {
                    employees = await _employeeDBContext.Employees.Where(emp => employeeIds.Contains(emp.EmployeeId) && emp.IsActive == true && !excludedEmployees.Contains(emp.EmployeeCode))
                    .Select(emp => new GenericType { Name = emp.EmployeeCode + " - " + emp.FirstName + " " + emp.LastName, Id = emp.EmployeeId })
                    .OrderBy(c => c.Name).ToListAsync();

                    response.IsSuccessful = true;
                    response.Items = employees;
                }
                else if (roleName.ToLower() == "Department Head".ToLower() && isLeadership)
                {
                    employees = await _employeeDBContext.Employees
                        .Join(_employeeDBContext.LeadershipAssociates, emp => emp.EmployeeId, lemp => lemp.AssociateId, (emp, lemp) => new { emp, lemp })
                        .Where(e => e.emp.IsActive == true && e.emp.DepartmentId == deptId && e.lemp.IsActive == true && !excludedEmployees.Contains(e.emp.EmployeeCode))
                        .Select(e => new GenericType { Name = e.emp.EmployeeCode + " - " + e.emp.FirstName + " " + e.emp.LastName, Id = e.emp.EmployeeId })
                        .OrderBy(c => c.Name).ToListAsync();

                    response.IsSuccessful = true;
                    response.Items = employees;
                }
                else if (roleName.ToLower() == "Department Head".ToLower() && !isLeadership)
                {
                    if (projectId > 0)
                    {
                        var allocations = await _projectService.GetActiveAllocations();
                        if (allocations != null && allocations.Items != null)
                        {
                            employeeIds = (from allocation in allocations.Items
                                           where allocation.ProjectId == projectId
                                           select (int)allocation.AssociateId
                                       ).Distinct().ToList();

                            if (employeeIds != null && employeeIds.Count > 0)
                            {
                                employees = await _employeeDBContext.Employees.Where(emp => employeeIds.Contains(emp.EmployeeId) && emp.IsActive == true && !excludedEmployees.Contains(emp.EmployeeCode))
                                .Select(emp => new GenericType { Name = emp.EmployeeCode + " - " + emp.FirstName + " " + emp.LastName, Id = emp.EmployeeId })
                                .OrderBy(c => c.Name).ToListAsync();

                                response.IsSuccessful = true;
                                response.Items = employees;
                            }
                        }
                    }
                    else
                    {
                        employees = await _employeeDBContext.Employees.Where(emp => emp.IsActive == true && emp.DepartmentId == deptId && !excludedEmployees.Contains(emp.EmployeeCode))
                        .Select(emp => new GenericType { Name = emp.EmployeeCode + " - " + emp.FirstName + " " + emp.LastName, Id = emp.EmployeeId })
                        .OrderBy(c => c.Name).ToListAsync();
                    }

                    response.IsSuccessful = true;
                    response.Items = employees;
                }
                else if ((roleName.ToLower() == "Team Lead".ToLower() ||
                    roleName.ToLower() == "Reporting Manager".ToLower()) && deptId != 1)
                {
                    employees = await _employeeDBContext.Employees.Where(emp => emp.IsActive == true && emp.ReportingManager == employeeId && !excludedEmployees.Contains(emp.EmployeeCode))
                     .Select(emp => new GenericType { Name = emp.EmployeeCode + " - " + emp.FirstName + " " + emp.LastName, Id = emp.EmployeeId })
                     .OrderBy(c => c.Name).ToListAsync();

                    response.IsSuccessful = true;
                    response.Items = employees;
                }
                else if (roleName.ToLower() == "HRA".ToLower() || roleName.ToLower() == "HRM".ToLower())
                {
                    employees = await _employeeDBContext.Employees.Where(emp => emp.IsActive == true && emp.Nationality != "US" && emp.Nationality != "us" && emp.Nationality != "USA"
                    && emp.Nationality != "usa" && !excludedEmployees.Contains(emp.EmployeeCode))
                     .Select(emp => new GenericType { Name = emp.EmployeeCode + " - " + emp.FirstName + " " + emp.LastName, Id = emp.EmployeeId })
                     .OrderBy(c => c.Name).ToListAsync();

                    response.IsSuccessful = true;
                    response.Items = employees;
                }
                else
                {
                    response.IsSuccessful = true;
                    response.Items = employees;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in Attendance Report Service - GetAssociates method.", ex.StackTrace);
                response.Message = "Exception occured in Attendance Report Service - GetAssociates method.";
                response.IsSuccessful = false;
                return response;
            }
        }

        #endregion

        #region GetProjectsByManager
        public async Task<ServiceListResponse<GenericType>> GetProjectsByManager(int employeeId, string roleName)
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> projects = new List<GenericType>();
                List<int> employeeIds = new List<int>();
                int deptId = _employeeDBContext.Employees.Where(c => c.EmployeeId == employeeId)
                                    .Select(d => d.DepartmentId ?? 0).FirstOrDefault();

                if (roleName.ToLower() == "Program Manager".ToLower())
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (allocations != null && allocations.Items != null)
                    {
                        projects = (from allocation in allocations.Items
                                    where allocation.ProgramManagerId == employeeId && allocation.ProjectId.HasValue == true
                                    && string.IsNullOrWhiteSpace(allocation.ProjectName) == false
                                    orderby allocation.ProjectName
                                    select new GenericType { Name = allocation.ProjectName, Id = allocation.ProjectId.Value }).Distinct().ToList();
                    }
                }
                else if (roleName.ToLower() == "Reporting Manager".ToLower())
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (allocations != null && allocations.Items != null)
                    {
                        projects = (from allocation in allocations.Items
                                    where allocation.ReportingManagerId == employeeId && allocation.ProjectId.HasValue == true
                                    && string.IsNullOrWhiteSpace(allocation.ProjectName) == false
                                    orderby allocation.ProjectName
                                    select new GenericType { Name = allocation.ProjectName, Id = allocation.ProjectId.Value }).Distinct().ToList();
                    }
                }
                else if (roleName.ToLower() == "Team Lead".ToLower())
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (allocations != null && allocations.Items != null)
                    {
                        projects = (from allocation in allocations.Items
                                    where allocation.ReportingManagerId == employeeId && allocation.ProjectId.HasValue == true
                                    && string.IsNullOrWhiteSpace(allocation.ProjectName) == false
                                    orderby allocation.ProjectName
                                    select new GenericType { Name = allocation.ProjectName, Id = allocation.ProjectId.Value }).Distinct().ToList();
                    }
                }
                else if (roleName.ToLower() == "Delivery".ToLower() && deptId == 1)
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (allocations != null && allocations.Items != null)
                    {
                        projects = (from allocation in allocations.Items
                                    where allocation.ProjectId.HasValue == true
                                    && string.IsNullOrWhiteSpace(allocation.ProjectName) == false
                                    orderby allocation.ProjectName
                                    select new GenericType { Name = allocation.ProjectName, Id = allocation.ProjectId.Value }).Distinct().ToList();
                    }
                }
                else if (roleName.ToLower() == "Department Head".ToLower() && deptId == 1)
                {
                    var allocations = await _projectService.GetActiveAllocations();
                    if (allocations != null && allocations.Items != null)
                    {
                        projects = (from allocation in allocations.Items
                                    where allocation.ProjectId.HasValue == true
                                    && string.IsNullOrWhiteSpace(allocation.ProjectName) == false
                                    orderby allocation.ProjectName
                                    select new GenericType { Name = allocation.ProjectName, Id = allocation.ProjectId.Value }).Distinct().ToList();
                    }
                }

                response.IsSuccessful = true;
                response.Items = projects;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in Associate Allocation Service - GetProjects method.", ex.StackTrace);
                response.Message = "Exception occured in Associate Allocation Service - GetProjects method.";
                response.IsSuccessful = false;
            }
            return response;
        }

        #endregion

        #region GetAttendanceMuster
        public async Task<ServiceResponse<DataTable>> GetAttendanceMuster(int year,int month)
        {
            var response = new ServiceResponse<DataTable>();
            List<AttendanceDetailsWithDates> attendanceWithDates = new List<AttendanceDetailsWithDates>();
            DataTable dTable = new DataTable();
            try
            {
                var designation = (await _organizationService.GetAllDesignations());
                if(!designation.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    return response;
                }
                var associatesDetails = (from emp in _employeeDBContext.Employees.Where(emp=>emp.IsActive==true).ToList()
                                         join des in designation.Items on emp.DesignationId equals des.DesignationId
                                         select new AttendanceMusterDetails
                                         {
                                             AssociateId=emp.EmployeeCode,
                                             AssociateName=emp.FirstName+" "+emp.LastName,
                                             Designation=des.DesignationName                                             
                                         }).ToList();

                int currentMonth = month; // DateTime.Now.Month;
                int currentYear = year;
                int numberOfDaysInMonth = DateTime.DaysInMonth(currentYear,currentMonth);
                DateTime fromDate =new DateTime(currentYear,currentMonth,1);
                DateTime ToDate = fromDate.AddMonths(1).AddDays(-1);

                var bioMetricAttendances = await _employeeDBContext.BioMetricAttendenceDetail.Where(c =>string.IsNullOrWhiteSpace(c.InTime) == false).ToListAsync();
                var attendance = bioMetricAttendances.Where(c => c.Punch1_Date?.Date >= fromDate.Date && c.Punch1_Date?.Date <= ToDate.Date).ToList();
                var leaveData = _employeeDBContext.AssociateLeave.ToList();
                var leavedetails = leaveData.Where(c => c.FromDate >= fromDate && c.ToDate <= ToDate).ToList();

                var holidays = GetHolidays();
                associatesDetails.ForEach(associate => {
                    var associateAttendanceDates = new AttendanceDetailsWithDates();
                    associateAttendanceDates.AssociateId = associate.AssociateId;
                    var leaveDates = GetLeaveDates(leavedetails.Where(x=>x.EmployeeCode==associate.AssociateId).ToList());
                    var isBiometric = bioMetricAttendances.Where(bio => bio.AsscociateId == associate.AssociateId).ToList();
                    var isLeave = leavedetails.Where(leave => leave.EmployeeCode == associate.AssociateId).ToList();

                    for (var date = fromDate; date.Date <= ToDate; date = date.AddDays(1))
                    {
                        var bio = attendance.Where(c => c.AsscociateId == associate.AssociateId && c.Punch1_Date?.Date == date.Date && string.IsNullOrWhiteSpace(c.InTime) == false).FirstOrDefault();

                        if (bio!=null && !string.IsNullOrWhiteSpace(bio.InTime))
                        {
                            bool isLeav = false;
                            bool isBio = true;
                            LeaveDetails leaveDatail=null;
                            if (leaveDates.Select(leave => leave.LeaveDate).Contains(date.ToString()))
                            {
                                 leaveDatail = leaveDates.Where(leave => leave.LeaveDate == date.ToString()).First();
                                if (leaveDatail != null)
                                {
                                    //associate.Day.Add(date.Day.ToString(), leaveDatail.LeaveType);
                                    isLeav = true;
                                }
                            }
                            if (isLeav && isBio)
                            {
                                associateAttendanceDates.Day.Add(date.Day.ToString(), "P:" + leaveDatail.LeaveType);
                            }
                            else
                            {
                                associateAttendanceDates.Day.Add(date.Day.ToString(), "P");
                            }
                        }

                        else if (date.DayOfWeek == System.DayOfWeek.Saturday || date.DayOfWeek == System.DayOfWeek.Sunday)
                        {
                            associateAttendanceDates.Day.Add(date.Day.ToString(), "OFF");
                        }
                        else if (holidays.Contains(date.Date))
                        {
                            associateAttendanceDates.Day.Add(date.Day.ToString(), "H");
                        }
                        else if (leaveDates.Select(leave => leave.LeaveDate).Contains(date.ToString()))
                        {
                            var leaveDatail = leaveDates.Where(leave => leave.LeaveDate == date.ToString()).First();
                            associateAttendanceDates.Day.Add(date.Day.ToString(), leaveDatail.LeaveType);
                        }
                        else
                        {
                            associateAttendanceDates.Day.Add(date.Day.ToString(), "A");
                        }
                    }

                    attendanceWithDates.Add(associateAttendanceDates);
                });
                List<AttendanceTotalDaysDetails> attendanceTotalDaysDetails = new List<AttendanceTotalDaysDetails>();

                attendanceWithDates.ForEach(associateAttendance =>
                {
                    var attendanceTotalDaysDetail = new AttendanceTotalDaysDetails();

                    attendanceTotalDaysDetail.AssociateId = associateAttendance.AssociateId;
                    attendanceTotalDaysDetail.Present = associateAttendance.Day.Where(day => day.Value == "P").Count();
                    attendanceTotalDaysDetail.Leave = associateAttendance.Day.Where(day => day.Value != "P" && day.Value != "A" && day.Value != "H" && day.Value != "OFF").Count();
                    var halfdayPresentAndLeave = associateAttendance.Day.Where(day => day.Value.Contains("P:")).Count();
                    if(halfdayPresentAndLeave>0)
                    {
                        var presentAndLeave= Decimal.Divide(halfdayPresentAndLeave, 2);
                        attendanceTotalDaysDetail.Present =  attendanceTotalDaysDetail.Present + presentAndLeave;
                        attendanceTotalDaysDetail.Leave = attendanceTotalDaysDetail.Leave - presentAndLeave;
                    }
                    attendanceTotalDaysDetail.Absent = associateAttendance.Day.Where(day => day.Value == "A").Count();
                    attendanceTotalDaysDetail.Holiday = associateAttendance.Day.Where(day => day.Value == "H").Count();
                    attendanceTotalDaysDetail.Total = associateAttendance.Day.Count();                   

                    attendanceTotalDaysDetails.Add(attendanceTotalDaysDetail);
                });
               
                DataTable dt =(DataTable) JsonConvert.DeserializeObject(JsonConvert.SerializeObject(associatesDetails), typeof(DataTable));//Convert JSON to DT
                DataTable associateDT =(DataTable) JsonConvert.DeserializeObject(JsonConvert.SerializeObject(attendanceTotalDaysDetails), typeof(DataTable));//Convert JSON to DT
                var formatedData = CreateXLSAttchment(dt, attendanceWithDates, associateDT, numberOfDaysInMonth);
                response.IsSuccessful = true;
                response.Item = formatedData;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured in Associate Allocation Service - GetProjects method.", ex.StackTrace);
                response.Message = "Exception occured in Associate Allocation Service - GetProjects method.";
                response.IsSuccessful = false;
            }
            return response;
        }

        #endregion

        #region Private Methods
        private List<DateTime> GetHolidays()
        {
            var holidays = _organizationService.GetAllHolidays().Result.Items;

            return holidays?.Count > 0 ? holidays.Select(c => c.HolidayDate).ToList() : null;
        }
       
        private List<LeaveDetails> GetLeaveDates(List<AssociateLeave> associateLeaves)
        {
            List<LeaveDetails> leaveDetails = new List<LeaveDetails>();
            foreach (var associate in associateLeaves)
            {
                var fromDate = associate.FromDate;
                var toDate = associate.ToDate;
                if (fromDate == toDate)
                {
                    var leave = new LeaveDetails
                    {
                        LeaveDate = fromDate.ToString(),
                        Session = associate.Session1Id == associate.Session2Id ? associate.Session1Name : null,
                        LeaveType = associate.LeaveType
                    };
                    leaveDetails.Add(leave);
                }
                else
                {                  
                    while (fromDate <= toDate)
                    {
                        var leave = new LeaveDetails();
                        if (fromDate == toDate)
                        {
                            leave.LeaveDate = fromDate.ToString();
                            leave.Session = associate.Session1Id == associate.Session2Id ? associate.Session1Name : null;
                            leave.LeaveType = associate.LeaveType;
                        }
                        else
                        {
                            leave.LeaveDate = fromDate.ToString();
                            leave.LeaveType = associate.LeaveType;
                        }
                        leaveDetails.Add(leave);
                        fromDate = fromDate.Date.AddDays(1);
                    }
                }
            }
            return leaveDetails.GroupBy(x => x.LeaveDate).Select(y => y.First()).ToList();
        }

        private decimal GetTotalLeaves(List<AssociateLeave> associateLeaves)
        {
            decimal total = 0;

            foreach (var associate in associateLeaves)
            {
                 total += associate.NumberOfDays;
            }               
            
            return total;
        }
        private List<AssociateAdvanceAttendanceDetails> GetAssociateAdvanceDetailsForHR(AttendanceReportFilter filter,List<EmployeeInfo> employeeInfos)
        {
            List<AssociateAdvanceAttendanceDetails> associatesDetailsList = new List<AssociateAdvanceAttendanceDetails>();

            try
            {
                var connectionString = _configuration.GetSection("ConnectionStrings:Default").Value;
                using (var con = new NpgsqlConnection(connectionString))
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;

                    string employeeJson = JsonConvert.SerializeObject(employeeInfos);
                    cmd.Parameters.AddWithValue("employeeData", NpgsqlTypes.NpgsqlDbType.Jsonb, employeeJson);

                    DateTime currentMonthStart = filter.FromDate;
                    DateTime lastDay = filter.ToDate;

                    while (currentMonthStart <= lastDay)
                    {
                        DateTime currentMonthEnd = new DateTime(currentMonthStart.Year, currentMonthStart.Month, 1)
                                                        .AddMonths(1)
                                                        .AddDays(-1);
                        if (currentMonthEnd > filter.ToDate)
                            currentMonthEnd = filter.ToDate;

                        string query = @$"SELECT 
                            date_series::date AS ""Date"",
                            to_char(date_series, 'Day') AS ""DayOfWeek"",
                            e.""AssociateId"",
                            e.""AssociateCode"",
                            e.""AssociateName"",
                            e.""ProgramManagerId"",
                            e.""ProgramManagerName"",
                            e.""ProjectName"",
                            e.""ReportingManagerName"",
                            CASE
                                WHEN ba.""Id"" IS NOT NULL THEN ba.""Location""
                                WHEN al.""associateleaveid"" IS NOT NULL THEN al.""leavetype""
                                WHEN arwf.""WorkFlowId"" IS NOT NULL THEN 'Regularization Applied'
                                ELSE 'Absent'
                            END AS ""AttendanceStatus""
                            FROM(
                                SELECT gs::date AS date_series FROM generate_series(
                                    '{currentMonthStart:yyyy-MM-dd}'::date,  
                                    '{currentMonthEnd:yyyy-MM-dd}'::date,
                                    '1 day'::interval) gs
                                WHERE EXTRACT(DOW FROM gs) NOT IN (0,6)  -- Exclude Sundays (0) & Saturdays (6)
                            ) date_filtered
                            CROSS JOIN  (
                                SELECT * FROM jsonb_to_recordset(@employeeData) AS x(
                                    ""AssociateId"" int,
                                    ""AssociateCode"" text,
                                    ""AssociateName"" text,
                                    ""ReportingManagerId"" int,
                                    ""ReportingManagerName"" text,
                                    ""ProgramManagerId"" int,
                                    ""ProgramManagerName"" text,
                                    ""ProjectId"" int,
                                    ""ProjectName"" text,
                                    ""DepartmentId"" int,
                                    ""DepartmentName"" text
                                 )
                            ) e

                            LEFT JOIN ""BiometricAttendance"" ba 
                            ON ba.""UserID"" = e.""AssociateCode""
                            AND (
                                date_filtered.date_series = ba.""Punch1_Date""
                                OR date_filtered.date_series = ba.""Punch2_Date""
                            )
                            LEFT JOIN ""AssociateLeave"" al 
                            ON al.""employeecode"" = e.""AssociateCode""
                            AND date_filtered.date_series BETWEEN al.""fromdate"" AND al.""todate""
                            LEFT JOIN ""AttendanceRegularizationWorkFlow"" arwf 
                            ON arwf.""SubmittedBy"" = e.""AssociateCode""
                            AND date_filtered.date_series = arwf.""RegularizationAppliedDate""
                            AND arwf.""Status"" = 1
                            ORDER BY e.""AssociateCode"", date_filtered.date_series";

                        cmd.CommandText = query;
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    AssociateAdvanceAttendanceDetails associateDetails = new AssociateAdvanceAttendanceDetails
                                    {
                                        Date = Convert.ToDateTime(reader["Date"]).ToString("yyyy-MM-dd"),
                                        DayOfWeek = reader["DayOfWeek"].ToString(),
                                        EmployeeId = reader.GetInt32("AssociateId"),
                                        EmployeeCode = reader["AssociateCode"].ToString(),
                                        EmployeeName = reader["AssociateName"].ToString(),
                                        ProjectName = reader["ProjectName"].ToString(),
                                        ProgramManager = reader.IsDBNull(reader.GetOrdinal("ProgramManagerName")) ? "No Manager Assigned" : reader["ProgramManagerName"].ToString(),
                                        ReportingManager = reader.IsDBNull(reader.GetOrdinal("ReportingManagerName")) ? "No Manager Assigned" : reader["ReportingManagerName"].ToString(),
                                        AttendanceStatus = reader["AttendanceStatus"].ToString(),
                                    };
                                    associatesDetailsList.Add(associateDetails);
                                }
                            }
                        }

                        currentMonthStart = currentMonthEnd.AddDays(1);
                    }
                    cmd.Dispose();
                }
                return associatesDetailsList;
            }
            catch
            {
                return associatesDetailsList;
            }
        }
        private List<AdvanceAttendanceReportModel> GroupAssociatesWithDates(List<AssociateAdvanceAttendanceDetails> list, List<string> uniqueDates ,List<DateTime> holidays, List<AttendanceReport> attendanceReports)
        {
            try
            {
                var groupedAssociates = list.GroupBy(a => new { a.EmployeeId, a.EmployeeCode, a.EmployeeName,a.ReportingManager,a.ProgramManager,a.ProjectName })
                        .Select(g => new AdvanceAttendanceReportModel
                        {
                            EmployeeCode = g.Key.EmployeeCode,
                            EmployeeName = g.Key.EmployeeName,
                            ReportingManager = g.Key.ReportingManager,
                            ProjectName = g.Key.ProjectName,
                            CompliancePercentage =  attendanceReports.FirstOrDefault(a => a.EmployeeCode == g.Key.EmployeeCode)?.CompliancePrecentage??0,
                            AttendanceByDate = uniqueDates.ToDictionary(
                                date => date,
                                date =>
                                {
                                    var dateSection = date.Split("/")[0];
                                    var record = g.FirstOrDefault(a => $"{a.Date}/{a.DayOfWeek}" == date);
                                    if (record != null)
                                    {
                                        if (record.AttendanceStatus == "Absent" && holidays.Contains(Convert.ToDateTime(record.Date)))
                                        {
                                            return "Holiday";
                                        }
                                        else
                                        {
                                            return record.AttendanceStatus;
                                        }
                                    }
                                    return "";
                                }
                                )
                        }).ToList();

                return groupedAssociates;
            }
            catch
            {
                return new List<AdvanceAttendanceReportModel>();
            }
        }
        private static DataTable LinqToDataTable(List<AdvanceAttendanceReportModel> data, List<string> uniqueDates)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("EmployeeCode", typeof(string));
                dt.Columns.Add("EmployeeName", typeof(string));
                dt.Columns.Add("ProjectName", typeof(string));
                dt.Columns.Add("ReportingManager", typeof(string));
                dt.Columns.Add("CompliancePercentage", typeof(decimal));

                foreach (var date in uniqueDates)
                {
                    dt.Columns.Add(date, typeof(string));
                }

                foreach (var record in data)
                {
                    var row = dt.NewRow();
                    row["EmployeeCode"] = record.EmployeeCode;
                    row["EmployeeName"] = record.EmployeeName;
                    row["ProjectName"] = record.ProjectName;
                    row["ReportingManager"] = record.ReportingManager;
                    row["CompliancePercentage"] = record.CompliancePercentage;

                    foreach (var date in uniqueDates)
                    {
                        row[date] = record.AttendanceByDate[date];
                    }

                    dt.Rows.Add(row);
                }

                return dt;
            }
            catch
            {
                return dt;
            }
        }

        private static byte[] CreateAdvanceXLSAttachment(DataTable dt)
        {
            using(var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("AdvanceAttendanceReport");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                }

                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        worksheet.Cell(row + 2, col + 1).Value = dt.Rows[row][col]?.ToString() ?? "N/A";
                    }
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
            
        }

        public static DataTable CreateXLSAttchment(DataTable dt, List<AttendanceDetailsWithDates> attendanceWithDates,DataTable attendanceTotalDays   , int numberOfDays)
        {
            // Days of month 
                    for (int i = 1; i <= numberOfDays; i++)
                    {
                      dt.Columns.Add(i.ToString());
                    }

            //Total days Present/Absent/Leave           
            foreach (DataColumn dc in attendanceTotalDays.Columns)
            {
                if (!dt.Columns.Contains(dc.ColumnName))
                {
                    dt.Columns.Add(dc.ToString());
                }
            }

                for (int i = 0; i < dt.Rows.Count; i++)
                    {
                    var id = dt.Columns.IndexOf("AssociateId");
                    var attendance = attendanceWithDates.Where(x => x.AssociateId == dt.Rows[i][id].ToString()).FirstOrDefault();
                    foreach (var day in attendance.Day)
                    {

                        if (dt.Columns.Contains(day.Key))
                            {
                            int index = dt.Columns.IndexOf(day.Key);
                            dt.Rows[i][index] = day.Value;
                           }
                      }
                foreach (DataColumn col in attendanceTotalDays.Columns)
                {
                    if (col.ColumnName != "AssociateId")
                    {
                        if (dt.Columns.Contains(col.ColumnName))
                        {
                            int index = dt.Columns.IndexOf(col.ColumnName);
                            int index2 = attendanceTotalDays.Columns.IndexOf(col.ColumnName);
                            dt.Rows[i][index] = attendanceTotalDays.Rows[i][index2];
                        }
                    }
                }
            }
            return dt;
        }
        private (decimal, decimal, decimal) GetDaysCalculations(string empCode, DateTime fromDate, DateTime toDate, List<BioMetricAttendance> bioMetricAttendances)
        {
            decimal totalWFHDays = 0; 
            decimal totalWFODays=0;
            if (bioMetricAttendances != null && bioMetricAttendances.Count > 0)
            {
                totalWFHDays = bioMetricAttendances.Where(x => x.Location == "WFH").Select(l => l.Punch1_Date).Distinct().Count();
                totalWFODays = bioMetricAttendances.Where(x => x.Location == "WFO").Select(l => l.Punch1_Date).Distinct().Count();
            }

            //Leave data
            decimal TotalLeaves = 0;
            List<AssociateLeave> leaveData = _employeeDBContext.AssociateLeave.Where(x => x.EmployeeCode.ToLower() == empCode.ToLower()).ToList();
            if (leaveData.Count() > 0)
            {
                List<AssociateLeave> leavedetails = leaveData.Where(c => c.FromDate >= fromDate && c.ToDate <= toDate).GroupBy(x => x.AssociateLeaveId).Select(x => x.First()).ToList().ToList();
                if (leavedetails.Count() > 0)
                {
                    List<LeaveDetails> leaveDates = GetLeaveDates(leavedetails);
                    TotalLeaves = GetTotalLeaves(leavedetails);

                    //Get half day leave details
                    var halfSession = leaveDates.Where(x => x.Session != null).ToList();
                    if (halfSession.Count() > 0)
                    {
                        halfSession.ForEach(halfday =>
                        {
                            var isDateAvailableInAttendance = bioMetricAttendances.Where(x => x.AsscociateId.ToLower() == empCode.ToLower() && x.Punch1_Date == Common.Utility.GetDateTimeInIST(DateTime.Parse(halfday.LeaveDate))).FirstOrDefault();
                            if (isDateAvailableInAttendance != null)
                            {
                                if (isDateAvailableInAttendance.Location == "WFH")
                                {
                                    totalWFHDays = totalWFHDays - Decimal.Divide(1, 2);
                                }
                                else
                                {
                                    totalWFODays = totalWFODays - Decimal.Divide(1, 2);
                                }
                            }
                        });
                    }
                }
            }
            return (totalWFHDays, totalWFODays, TotalLeaves);
        }

        private List<BioMetricAttendance> RemoveDuplicateAttendanceDate(List<BioMetricAttendance> bioMetricAttendances)
        {
            List<DateTime?> duplicateAttendanceDates = bioMetricAttendances.Where(k => k.Punch1_Date != null).GroupBy(s => s.Punch1_Date)
                              .Where(g => g.Count() > 1)
                              .Select(g => g.Key).ToList();
            foreach (var date in duplicateAttendanceDates)
            {
                // when WFH/WFO both records are captured then consider both In-Times and Out-Times and show it as Biometric only.

                BioMetricAttendance WFHAttendanceDate = bioMetricAttendances.Where(x => x.Punch1_Date == date.Value.Date && x.Location == "WFH").FirstOrDefault();
                BioMetricAttendance WFOAttendanceDate = bioMetricAttendances.Where(x => x.Punch1_Date == date.Value.Date && x.Location == "WFO").FirstOrDefault();
                if (WFHAttendanceDate != null && WFOAttendanceDate != null)
                {
                    if (WFHAttendanceDate.InTime != null && WFOAttendanceDate.InTime != null)
                    {
                        WFOAttendanceDate.InTime = ConvertToTimeSpan(WFHAttendanceDate.InTime)
                                             <= ConvertToTimeSpan(WFOAttendanceDate.InTime)
                                             ? WFHAttendanceDate.InTime : WFOAttendanceDate.InTime;
                    }
                    if (WFHAttendanceDate.OutTime != null && WFOAttendanceDate.OutTime != null)
                    {
                        WFOAttendanceDate.OutTime = ConvertToTimeSpan(WFHAttendanceDate.OutTime)
                                              >= ConvertToTimeSpan(WFOAttendanceDate.OutTime)
                                              ? WFHAttendanceDate.OutTime : WFOAttendanceDate.OutTime;
                    }                    
                    WFOAttendanceDate.WorkTime_HHMM = CalculateTotalTime(WFOAttendanceDate.OutTime, WFOAttendanceDate.InTime);
                    bioMetricAttendances.Remove(WFHAttendanceDate);
                }
            }
            return bioMetricAttendances;
        }

        public string CalculateTotalTime(string OutTime, string InTime)
        {
            string TotalTime = string.Empty;
            if (string.IsNullOrWhiteSpace(OutTime) == false && string.IsNullOrWhiteSpace(InTime) == false)
            {
                var inTime = InTime.Split(':');
                var outTime = OutTime.Split(':');


                TotalTime = inTime.Count() > 1 && outTime.Count() > 1 ?
                                (new TimeSpan(Convert.ToInt32(outTime[0]), Convert.ToInt32(outTime[1]), 0) -
                                 new TimeSpan(Convert.ToInt32(inTime[0]), Convert.ToInt32(inTime[1]), 0)).ToString(@"hh\:mm") : "";
            }
            return TotalTime;
        }

        public TimeSpan ConvertToTimeSpan(string time)
        {
            TimeSpan convertedTime = new TimeSpan();
            if (!string.IsNullOrWhiteSpace(time))
            {
                var timeParts = time.Split(':');
                if (timeParts.Length >= 2)
                {
                    convertedTime = new TimeSpan(Convert.ToInt32(timeParts[0]), Convert.ToInt32(timeParts[1]), 0);
                }
            }
            return convertedTime;
        }
        #endregion
    }
}
