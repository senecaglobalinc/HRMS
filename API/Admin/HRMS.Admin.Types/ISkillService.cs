using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    /// <summary>
    /// Interface for skill service
    /// </summary>
    public interface ISkillService
    {
        //Get skill by using competency area ID
        Task<List<Skill>> GetByCompetencyAreaId(int competencyAreaID);

        //Create Skill abstract method 
        Task<dynamic> Create(Skill skillIn);

        //Get skill detail by using skill code
        Task<Skill> GetByCode(string skillCode);

        //Update Skil abstract method
        Task<dynamic> Update(Skill skillIn);

        //Get Skills abstract method
        Task<List<Skill>> GetAll(bool? isActive = true);

        //Get skills by SkillGroupIds abstract method
        Task<List<Skill>> GetBySkillGroupId(string skillGroupIds);

        //Get skills by SkillIds abstract method
        Task<List<Skill>> GetById(string skillIds);

        //Get All active skills
        Task<ServiceListResponse<GenericType>> GetActiveSkillsForDropdown();

        //Get skills by skillgroupid

        Task<List<Skill>> GetskillsBySkillGroupId(int skillgroupid);

        //Get skills by skillsearchstring
        Task<List<SkillSearchResponse>> GetSkillsBySearchString(string skillsearchstring);
    }
}