using HRMS.KRA.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.KRA.Types
{
    public interface IScaleService
    {
        Task<List<ScaleModel>> GetAllAsync();
        Task<List<ScaleDetailsModel>> GetScaleDetailsAsync();
        Task<List<ScaleDetailsModel>> GetScaleDetailsByIdAsync(int ScaleID);
        Task<(bool IsSuccessful, string Message)> CreateAsync(ScaleModel model);
        Task<(bool IsSuccessful, string Message)> UpdateAsync(ScaleModel model);
        Task<(bool IsSuccessful, string Message)> DeleteAsync(int kraAspectID);
    }
}
