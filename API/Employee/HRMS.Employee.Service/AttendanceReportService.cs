using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class AttendanceReportService : IAttendanceReportService
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
        public AttendanceReportService(EmployeeDBContext employeeDBContext,
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
        public async Task<DateTime> GetAttendanceMaxDate()
        {
            DateTime date = await _employeeDBContext.AttendanceDetail.Select(d => d.Date).MaxAsync();

            return date;
        }
        #endregion

        #region IsDeliveryDepartment
        public async Task<bool> IsDeliveryDepartment(int employeeId)
        {
            bool result = false;

            int deptId = await _employeeDBContext.Employees.Where(c => c.EmployeeId == employeeId).Select(d => d.DepartmentId.Value).FirstOrDefaultAsync();
            if(deptId == 1) result = true;
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
                List<AttendanceDetail> attendance = new List<AttendanceDetail>();
                List<string> employeeCodes = new List<string>();
                List<string> projectEmployeeCodes = new List<string>();
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
                    if (day.Date.DayOfWeek != DayOfWeek.Sunday && day.Date.DayOfWeek != DayOfWeek.Saturday)
                    {
                        if (holidays.Contains(day.Date))
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
                    attendance = await _employeeDBContext.AttendanceDetail.Where(c => c.Date >= filter.FromDate.Date && c.Date < filter.ToDate.AddDays(1).Date && string.IsNullOrWhiteSpace(c.InTime) == false && filter.EmployeeCode.ToLower() == c.AsscociateId.ToLower()).ToListAsync();
                }
                else if (string.IsNullOrWhiteSpace(filter.EmployeeCode) && (filter.ProjectId.HasValue == false || filter.ProjectId.Value == 0))
                {
                    attendance = await _employeeDBContext.AttendanceDetail.Where(c => c.Date >= filter.FromDate.Date && c.Date < filter.ToDate.AddDays(1).Date && string.IsNullOrWhiteSpace(c.InTime) == false && employeeCodes.Contains(c.AsscociateId.ToLower())).ToListAsync();
                }
                else if (!string.IsNullOrWhiteSpace(filter.EmployeeCode) && (filter.ProjectId.HasValue && filter.ProjectId.Value > 0))
                {
                    attendance = await _employeeDBContext.AttendanceDetail.Where(c => c.Date >= filter.FromDate.Date && c.Date < filter.ToDate.AddDays(1).Date && string.IsNullOrWhiteSpace(c.InTime) == false && filter.EmployeeCode.ToLower() == c.AsscociateId.ToLower()).ToListAsync();
                }
                else if (string.IsNullOrWhiteSpace(filter.EmployeeCode) && (filter.ProjectId.HasValue && filter.ProjectId.Value > 0))
                {
                    attendance = await _employeeDBContext.AttendanceDetail.Where(c => c.Date >= filter.FromDate.Date && c.Date < filter.ToDate.AddDays(1).Date && string.IsNullOrWhiteSpace(c.InTime) == false && employeeCodes.Contains(c.AsscociateId.ToLower()) && projectEmployeeCodes.Contains(c.AsscociateId.ToLower())).ToListAsync();
                }

                if (attendance != null && attendance.Count > 0)
                {
                    attendanceReport = attendance.GroupBy(x => new { x.AsscociateId, x.AsscociateName })
                        .Select(c => new AttendanceReport
                        {
                            EmployeeCode = c.Key.AsscociateId,
                            EmployeeName = c.Key.AsscociateName,
                            TotalDaysWorked = c.Select(l => l.Date.Date).Distinct().Count()
                        }).ToList();
                }

                if (filter.ProjectId.HasValue && filter.ProjectId > 0 && string.IsNullOrEmpty(filter.EmployeeCode))
                {
                    foreach (string code in projectEmployeeCodes)
                    {
                        AttendanceReport associate = new AttendanceReport();

                        EmployeeInfo info = employees.Where(c => c.AssociateCode.ToLower() == code.ToLower()).FirstOrDefault();
                        associate.EmployeeName = info.AssociateName;
                        associate.EmployeeCode = info.AssociateCode;
                        associate.ProjectName = info.ProjectName;
                        associate.DepartmentName = info.DepartmentName;
                        associate.ReportingManagerName = info.ReportingManagerName;
                        decimal? totalDays = 0;
                        if (attendance != null && attendance.Count > 0)
                            totalDays = attendanceReport.Where(c => c.EmployeeCode.ToLower() == code.ToLower())
                            .Select(d => d.TotalDaysWorked).FirstOrDefault();
                        associate.TotalDaysWorked = totalDays ?? 0;
                        associate.TotalWorkingDays = totalWorkingDays;
                        associate.TotalHolidays = totalHolidays;
                        associate.WorkFromHome = false;
                        if (wfhAssociatesList.Contains(info.AssociateCode))
                            associate.WorkFromHome = true;
                        details.Add(associate);
                    }
                }
                else
                {
                    foreach (string code in employeeCodes)
                    {
                        AttendanceReport associate = new AttendanceReport();

                        EmployeeInfo info = employees.Where(c => c.AssociateCode.ToLower() == code.ToLower()).FirstOrDefault();
                        associate.EmployeeName = info.AssociateName;
                        associate.EmployeeCode = info.AssociateCode;
                        associate.DepartmentName = info.DepartmentName;
                        associate.ProjectName = info.ProjectName;
                        associate.ReportingManagerName = info.ReportingManagerName;
                        decimal? totalDays = 0;
                        if (attendance != null && attendance.Count > 0)
                            totalDays = attendanceReport.Where(c => c.EmployeeCode.ToLower() == code.ToLower())
                            .Select(d => d.TotalDaysWorked).FirstOrDefault();
                        associate.TotalDaysWorked = totalDays ?? 0;
                        associate.TotalWorkingDays = totalWorkingDays;
                        associate.TotalHolidays = totalHolidays;
                        associate.WorkFromHome = false;
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

            }
            return details;
        }
        #endregion

        #region GetAttendanceDetailReport
        public async Task<List<AttendanceDetailReport>> GetAttendanceDetailReport(AttendanceReportFilter filter)
        {
            List<AttendanceDetailReport> attendanceReport = new List<AttendanceDetailReport>();
            List<AttendanceDetail> attendance = new List<AttendanceDetail>();
            var holidays = GetHolidays();
            attendance = await _employeeDBContext.AttendanceDetail.Where(c => c.Date >= filter.FromDate.Date && c.Date < filter.ToDate.AddDays(1).Date && c.AsscociateId.ToLower() == filter.EmployeeCode.ToLower()).ToListAsync();

            if (attendance != null && attendance.Count > 0)
            {
                for (var date = filter.FromDate.Date; date.Date <= filter.ToDate.Date; date = date.AddDays(1))
                {
                    List<AttendanceDetail> dayAttendance = attendance.Where(c => c.Date.Date == date.Date && string.IsNullOrWhiteSpace(c.InTime) == false).ToList();

                    if (dayAttendance != null && dayAttendance.Count > 0)
                    {
                        if (dayAttendance != null && dayAttendance.Count > 0)
                        {
                            foreach (AttendanceDetail associate in dayAttendance)
                            {
                                if (!string.IsNullOrWhiteSpace(associate.InTime))
                                {
                                    AttendanceDetailReport checkIn = new AttendanceDetailReport();
                                    string checkinTime = string.IsNullOrWhiteSpace(associate.InTime) ? "" : associate.InTime;
                                    checkIn.date = associate.Date.Date.ToString("yyyy-MM-dd");
                                    checkIn.title = $"Checkin Time: {checkinTime}";
                                    attendanceReport.Add(checkIn);

                                    string checkoutTime = string.IsNullOrWhiteSpace(associate.OutTime) ? "" : associate.OutTime;
                                    AttendanceDetailReport checkOut = new AttendanceDetailReport();
                                    checkOut.date = associate.Date.Date.ToString("yyyy-MM-dd");
                                    checkOut.title = $"Checkout Time: {checkoutTime}";
                                    attendanceReport.Add(checkOut);

                                    AttendanceDetailReport totalHours = new AttendanceDetailReport();
                                    string total = string.IsNullOrWhiteSpace(associate.TotalTime) ? "" : associate.TotalTime;
                                    totalHours.date = associate.Date.Date.ToString("yyyy-MM-dd");
                                    totalHours.title = $"Total Hours: {total}";
                                    attendanceReport.Add(totalHours);
                                }
                            }
                        }
                    }
                    else if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
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
                }
            }
            return attendanceReport;
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

                List<GenericType> associates = GetAssociatesReportingToManager( employeeId,  roleName,  projectId ,  isLeadership).Result.Items;
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
                        if(projectId > 0)
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

        #region Private Methods
        private List<DateTime> GetHolidays()
        {
            var holidays = _organizationService.GetAllHolidays().Result.Items;

            return holidays?.Count > 0 ? holidays.Select(c => c.HolidayDate).ToList() : null;
        }
        #endregion
    }
}
