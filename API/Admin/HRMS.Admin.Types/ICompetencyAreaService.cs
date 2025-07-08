using HRMS.Admin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    /// <summary>
    /// Interface for competency area service
    /// </summary>
    public interface ICompetencyAreaService
    {
        //Create competency area abstract method 
        Task<dynamic> Create(CompetencyArea competencyAreaIn);

        //Get competency area detail by using competency area code
        Task<CompetencyArea> GetByCompetencyAreaCode(string competencyAreaCode);

        //Get competency area details abstract method
        Task<List<CompetencyArea>> GetAll(bool? isActive);

        //Update competency area abstract method
        Task<dynamic> Update(CompetencyArea competencyAreaIn);

        //Deactivates competency area abstract method
        Task<dynamic> Delete(int competencyAreaID);
    }
}
