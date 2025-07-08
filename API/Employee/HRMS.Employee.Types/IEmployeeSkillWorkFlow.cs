using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Response;

namespace HRMS.Employee.Types
{
  public  interface IEmployeeSkillWorkFlow
    {
        Task<ServiceResponse<EmployeeSkillWorkFlow>> Create(int employeeId , bool Approve=false);

        Task<ServiceResponse<EmployeeSkillWorkFlow>> SkillStatusApprovedByRM(int employeeId);
        Task<ServiceListResponse<EmployeeSkillWorkflow>> GetSkillSubmittedByEmployee(int reportingManager);
        Task<ServiceListResponse<EmployeeSkillDetails>> GetSubmittedSkillsByEmpid(int employeeId);
        Task<ServiceResponse<EmployeeSkill>> UpdateEmpSkillDetails(int employeeId);
        Task<ServiceResponse<EmployeeSkill>> UpdateEmpSkillProficienyByRM(EmployeeSkillWorkflow employeeSkill);
        Task<ServiceListResponse<EmployeeSkillWorkflow>> GetEmployeeSkillHistory(int employeeID,int ID);    

    }
}
