using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    /// <summary>
    /// Interface for practice area service
    /// </summary>
    public interface IPracticeAreaService
    {
        //Create practice area abstract method 
        Task<dynamic> Create(PracticeArea practiceAreaIn);

        //Get practice area detail by using practice area code
        Task<PracticeArea> GetByPracticeAreaCode(string practiceAreaCode);

        //Get practice area details abstract method
        Task<List<PracticeAreaDetails>> GetAll(bool? isActive);

        //Get practice area details abstract method
        Task<List<PracticeArea>> GetPracticeAreaByIds(int[] practiceAreaIds);

        //Get practice area details abstract method
        Task<PracticeArea> GetPracticeAreaById(int practiceAreaId);

        //Update practice area abstract method
        Task<dynamic> Update(PracticeArea practiceAreaIn);

        //Delete practice area abstract method
        Task<dynamic> Delete(int practiceAreaID);
        Task<List<GenericType>> GetTechnologyForDropdown();
    }
}
