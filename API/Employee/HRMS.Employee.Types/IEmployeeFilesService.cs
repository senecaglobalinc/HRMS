using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IEmployeeFilesService
    {
        Task<ServiceListResponse<UploadFile>> GetByEmployeeId(int employeeId);
        Task<ServiceResponse<UploadFile>> Save(UploadFiles uploadFiles);
        Task<ServiceResponse<bool>> Delete(int Id, int employeeId);
        Task<byte[]> GeneratePDFReport(int empID);
    }
}
