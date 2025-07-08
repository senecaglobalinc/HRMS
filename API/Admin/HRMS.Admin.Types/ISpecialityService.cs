using HRMS.Admin.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface ISpecialityService
    {
        Task<dynamic> Create(SGRoleSuffix sgRole);
        Task<dynamic> Update(SGRoleSuffix sgRole);
        Task<List<SGRoleSuffix>> GetAll(bool isActive = true);
    }
}
