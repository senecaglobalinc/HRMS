using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IEmployeeProfessionalService
    {
        Task<ServiceResponse<AssociateCertification>> CreateCertificate(AssociateCertification certificate);
        Task<ServiceResponse<AssociateCertification>> UpdateCertificate(AssociateCertification certificate);
        Task<ServiceResponse<AssociateMembership>> CreateMembership(AssociateMembership membership);
        Task<ServiceResponse<AssociateMembership>> UpdateMembership(AssociateMembership membership);
        Task<ServiceResponse<bool>> Delete(int id, int programType);
        Task<ServiceListResponse<ProfessionalDetails>> GetByEmployeeId(int employeeId);
    }
}
