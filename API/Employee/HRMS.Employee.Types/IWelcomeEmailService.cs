using HRMS.Employee.Infrastructure.Models.Request;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IWelcomeEmailService
    {        
        Task<ServiceListResponse<WelcomeEmailEmpRequest>> GetWelcomeEmployeeInfo();
        Task<ServiceResponse<bool>> CreateWelcomeEmailInfo(int employeeId);
        Task<ServiceResponse<bool>> SendWelcomeEmail(IFormFileCollection files, WelcomeEmailRequest welcomeemailreq);
    }
}
