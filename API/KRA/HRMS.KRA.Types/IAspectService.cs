using HRMS.KRA.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.KRA.Types
{
    public interface IAspectService
    {
        Task<List<AspectModel>> GetAllAsync();
        Task<(bool IsSuccessful, string Message)> CreateAsync(string aspectName);
        Task<(bool IsSuccessful, string Message)> UpdateAsync(AspectModel model);
        Task<(bool IsSuccessful, string Message)> DeleteAsync(int aspectId);
    }
}
