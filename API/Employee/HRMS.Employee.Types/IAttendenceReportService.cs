using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAttendanceReportService
    {
        Task<List<AttendanceReport>> GetAttendanceSummaryReport(AttendanceReportFilter filter);
        Task<List<AttendanceDetailReport>> GetAttendanceDetailReport(AttendanceReportFilter filter);
        Task<DateTime> GetAttendanceMaxDate();
        Task<ServiceListResponse<GenericType>> GetAssociatesReportingToManager(int employeeId, string roleName, int projectId = 0, bool isLeadership = false);
        Task<ServiceListResponse<GenericType>> GetProjectsByManager(int employeeId, string roleName);
        Task<bool> IsDeliveryDepartment(int employeeId);      
    }
}
