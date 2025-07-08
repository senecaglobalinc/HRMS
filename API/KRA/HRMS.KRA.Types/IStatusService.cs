using HRMS.KRA.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.KRA.Types
{
    public interface IStatusService
    {
        Task<List<StatusModel>> GetAllAsync();     
        Task<(bool IsSuccessful, string Message)> CreateAsync(StatusModel model);       
        
    }
}
