using HRMS.Admin.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface ISeniorityService
    {
        Task<dynamic> Create(SGRolePrefix sgRole);
        Task<dynamic> Update(SGRolePrefix sgRole);
        Task<List<SGRolePrefix>> GetAll(bool isActive = true);
    }
}
