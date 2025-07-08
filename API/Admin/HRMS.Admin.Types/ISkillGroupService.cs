using HRMS.Admin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    /// <summary>
    /// Interface for skill group service
    /// </summary>
    public interface ISkillGroupService
    {
        //Get skill group by using competency area ID
        Task<List<SkillGroup>> GetByCompetencyAreaId(int competencyAreaID);

        //Create SkillGroup abstract method 
        Task<dynamic> Create(SkillGroup skillGroupIn);

        //Update SkillGroup abstract method
        Task<dynamic> Update(SkillGroup skillGroupIn);

        //Get Skill Groups abstract method
        Task<List<SkillGroup>> GetAll(bool? isActive = true);

        //Get Skill Groups by CompetencyAreaCode abstract method
        Task<List<SkillGroup>> GetByCompetencyAreaCode(string competencyAreaCode);
        
        //Delete Skill Group
        Task<dynamic> Delete(int skillGroupID);
    }
}
