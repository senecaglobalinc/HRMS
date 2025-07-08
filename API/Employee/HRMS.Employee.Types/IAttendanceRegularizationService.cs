using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
   public interface IAttendanceRegularizationService
    {
        Task<ServiceResponse<AttendanceRegularization>> GetNotPunchInDates(AttendanceRegularizationFilter attendanceRegularizationFilter);
        Task<ServiceResponse<bool>> SaveAttendanceRegularizationDetails(List<AttendanceRegularizationWorkFlow> attendanceRegularizationFilter);
        Task<ServiceResponse<bool>> ApproveOrRejectAttendanceRegularizationDetails(AttendanceRegularizationWorkFlowDetails regularizationWorkFlowDetails);
        Task<ServiceListResponse<AttendanceRegularizationWorkFlowDetails>> GetAllAssociateSubmittedAttendanceRegularization(int ManagerId, string RoleName);
        Task<ServiceListResponse<AttendanceRegularizationWorkFlow>> GetAssociateSubmittedAttendanceRegularization(string AssociateId,string roleName);

    }
}
