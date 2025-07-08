using HRMS.KRA.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.KRA.Types
{
    public interface IMeasurementTypeService
    {
        Task<List<MeasurementTypeModel>> GetAllAsync();
        Task<(bool IsSuccessful, string Message)> CreateAsync(string name);
        Task<(bool IsSuccessful, string Message)> UpdateAsync(MeasurementTypeModel model);
        Task<(bool IsSuccessful, string Message)> DeleteAsync(int id);
    }
}
