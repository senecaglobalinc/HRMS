using HRMS.Admin.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IKeyFunctionService
    {
        Task<dynamic> Create(SGRole sgRoleIn);
        Task<dynamic> Update(SGRole sgRoleIn);
        Task<List<SGRole>> GetAll(bool isActive = true);
    }
}
