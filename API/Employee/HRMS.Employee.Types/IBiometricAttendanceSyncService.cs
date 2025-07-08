using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IBiometricAttendanceSyncService
    {
        Task<ServiceListResponse<BiometricAttendance>> GetBiometricAttendance(DateTime dateToSync, string location);
        Task<BaseServiceResponse> DeleteBiometricAttendance(DateTime dateFromSync, DateTime dateToSync, string location);
        Task<BaseServiceResponse> WriteBulkData(DateTime dateFromSync, DateTime dateToSync, List<BiometricAttendance> entities);
        Task<ServiceListResponse<ExcludedAssociates>> GetExcludedAssociates();
    }
}
