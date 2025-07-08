using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IDesignationService
    {
        //Create Designation abstract method 
        Task<dynamic> Create(Designation designationIn);

        //Get designation detail by using designation code
        Task<Designation> GetByCode(string designationCode);

        //Get designation detail by using designation id
        Task<Designation> GetById(int designationId);

        //Update Designation abstract method
        Task<dynamic> Update(Designation designationDetails);

        //Get Designation details abstract method
        Task<List<Designation>> GetAll(bool? isActive = true);

        //Get Designation details by SearchString abstract method
        Task<List<Designation>> GetBySearchString(string searchString);
        Task<List<GenericType>> GetDesignationsForDropdown();
    }
}
