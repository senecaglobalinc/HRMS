using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IGradeService
    {
        //Create Grade abstract method 
        Task<dynamic> Create(Grade gradeIn);

        //Update Grade abstract method
        Task<dynamic> Update(Grade gradeIn);

        //Get Grade detail by using grade code
        Task<Grade> GetByCode(string gradeCode);

        //Get Grade detail by designation
        Task<Grade> GetGradeByDesignation(int designationId);

        //Get Grade details abstract method
        Task<List<Grade>> GetAll(bool? isActive = true);

        //Get Grade detail by id abstract method
        Task<Grade> GetById(int gradeId);
        Task<List<GenericType>> GetGradesForDropdown();
    }
}
