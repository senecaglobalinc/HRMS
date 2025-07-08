using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
  public  interface IWorkFromHomeAttendanceService
    {
        Task<ServiceResponse<bool>> SaveAttendanceDetais(BioMetricAttendance workFromHomeAttendance);
        Task<ServiceResponse<BioMetricAttendance>> GetAttendanceDetais(string employeeCode);
        Task<ServiceResponse<int?>> GetloginStatus(string employeeCode);

    }
}
