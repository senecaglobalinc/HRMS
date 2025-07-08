using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IEmployeeEmploymentService
    {
        Task<ServiceResponse<EmployeeDetails>> Save(EmployeeDetails employmentDetailsIn);
        Task<ServiceListResponse<PreviousEmploymentDetails>> GetPrevEmploymentDetailsById(int empId);
        Task<ServiceListResponse<ProfessionalReferences>> GetProfReferencesById(int empId);
    }
}
