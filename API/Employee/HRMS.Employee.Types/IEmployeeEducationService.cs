using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IEmployeeEducationService
    {
        Task<ServiceResponse<EmployeeDetails>> Save(EmployeeDetails educationDetails);
        Task<ServiceListResponse<EducationDetails>> GetById(int Id);
    }
}
