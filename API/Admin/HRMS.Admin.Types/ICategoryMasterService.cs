using HRMS.Admin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    /// <summary>
    /// Interface for category master
    /// </summary>
    public interface ICategoryMasterService
    {
        //Create category master abstract method 
        Task<dynamic> Create(CategoryMaster categoryMasterIn);

        //Deactivates category master abstract method
        Task<dynamic> Delete(int CategoryMasterId);

        //Get category master details abstract method
        Task<List<CategoryMaster>> GetAll(bool? isActive);

        //Get category master by using category Id
        Task<CategoryMaster> GetByCategoryMasterId(int? CategoryMasterId);

        //Get category master by using category name
        Task<CategoryMaster> GetByCategoryName(string categoryName);

        //Get parent categories abstract method
        Task<List<CategoryMaster>> GetParentCategoies();        

        //Update category master abstract method
        Task<dynamic> Update(CategoryMaster categoryMasterIn);
    }
}