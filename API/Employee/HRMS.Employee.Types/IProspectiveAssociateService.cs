using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IProspectiveAssociateService
    {
        Task<ServiceListResponse<EmployeeDetails>> GetProspectiveAssociates(bool? isActive = true);
        Task<ServiceResponse<EmployeeDetails>> GetbyId(int Id);    
        Task<ServiceResponse<ProspectiveAssociate>> Create(ProspectiveAssociate prospectiveAssociateIn);
        Task<ServiceResponse<ProspectiveAssociate>> Update(ProspectiveAssociate prospectiveAssociateIn);
        Task<ServiceResponse<bool>> UpdateEmployeeStatusToPending(int employeeId);
        Task<ServiceResponse<bool>> UpdateEmployeeProfileStatus(EmployeeProfileStatus employeeProfileStatus);
    }
}
