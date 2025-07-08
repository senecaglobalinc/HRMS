using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IAssociateExitAnalysisService
    {
        Task<ServiceResponse<int>> CreateExitAnalysis(ExitAnalysis exitAnalysis);
        public  Task<ServiceListResponse<GetExitAnalysis>> GetAssociateExitAnalysis(DateTime? fromDate, DateTime? toDate,int?employeeId);
    }
}
