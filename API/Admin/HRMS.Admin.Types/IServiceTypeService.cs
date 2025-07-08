using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IServiceTypeService
    {
        public Task<List<GenericType>> GetServiceTypeForDropdown();
        public Task<dynamic> Create(ServiceType serviceType);
        public Task<dynamic> Update(ServiceType serviceType);

        public Task<List<ServiceType>> GetAll(bool isActive = true);
    }
}
