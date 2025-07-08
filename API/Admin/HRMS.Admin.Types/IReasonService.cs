using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IReasonService
    {
        Task<dynamic> Create(Reason reasonIn);
        Task<dynamic> Update(Reason reasonIn);
        Task<List<Reason>> GetAll(bool isActive = true);
        Task<Reason> GetByReasonId(int reasonId);
        Task<List<GenericType>> GetReasonsForDropdown();
        Task<List<GenericType>> GetVoluntaryExitReasons();
        Task<List<GenericType>> GetOtherExitReasons();
    }
}
