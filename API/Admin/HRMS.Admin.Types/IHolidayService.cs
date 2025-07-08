using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IHolidayService
    {
        Task<dynamic> Create(Holiday holidayIn);
        Task<dynamic> Update(Holiday holidayIn);
        Task<List<Holiday>> GetAll();
        Task<Holiday> GetByHolidayId(int holidayId);
        Task<List<GenericType>> GetHolidaysForDropdown();
        public Task<ServiceResponse<int>> GetHolidayIdByName(string occasion);
    }
}
