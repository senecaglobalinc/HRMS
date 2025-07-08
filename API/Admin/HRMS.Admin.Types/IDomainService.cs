using HRMS.Admin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IDomainService
    {
        Task<dynamic> Create(Domain domainIn);
        Task<dynamic> Update(Domain domainIn);
        Task<List<Domain>> GetAll(bool isActive = true);
        Task<Domain> GetByDomainId(int domainId);
    }
}
