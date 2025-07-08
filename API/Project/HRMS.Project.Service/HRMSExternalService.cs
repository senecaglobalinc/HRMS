using HRMS.Project.Database;
using HRMS.Project.Infrastructure.Models.Domain;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class HRMSExternalService : IHRMSExternalService
    {
        #region Global Varibles

        private readonly ILogger<HRMSExternalService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private IEmployeeService m_EmployeeService;

        #endregion

        #region Constructor
        public HRMSExternalService(ILogger<HRMSExternalService> logger,
            ProjectDBContext projectContext,
             IEmployeeService employeeService
           )
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            m_EmployeeService = employeeService;
        }
        #endregion

        #region GetEmployeeProjectDetails
        public async Task<ServiceListResponse<GetEmpForExternal>> GetEmployeeProjectDetails()
        {
            var response = new ServiceListResponse<GetEmpForExternal>();
            try
            {

                //List<EmployeeInfo> employees = new List<EmployeeInfo>();
                var empDetails = await m_EmployeeService.GetAll(true);

                var allocationsList = await (from allocation in m_ProjectContext.AssociateAllocation
                                             join p in m_ProjectContext.Projects on allocation.ProjectId equals p.ProjectId
                                             where allocation.IsActive == true && allocation.ReleaseDate == null
                                             && allocation.IsPrimary == true 
                                             select new EmployeeInfo
                                             {
                                                 AssociateId = allocation.EmployeeId ?? 0,
                                                 AssociateName = "",
                                                 ProgramManagerId = allocation.ProgramManagerId,
                                                 ProjectId = allocation.ProjectId,
                                                 ProjectName = p.ProjectName,
                                                 DepartmentId = 0,
                                                 ReportingManagerId = allocation.ReportingManagerId ?? 0
                                             }).Distinct().ToListAsync();

                var empList = (from e in empDetails.Items
                               join al in allocationsList on e.EmployeeId equals al.AssociateId into g1
                               from emp in g1.DefaultIfEmpty()
                               where e.IsActive == true
                               select new GetEmpForExternal
                               {
                                   AssociateId = e.EmployeeId,
                                   AssociateName = e.FirstName + " " + e.LastName,
                                   AssociateCode = e.EmployeeCode,
                                   ProgramManagerID = emp?.ProgramManagerId,
                                   ReportingManagerId = emp?.ReportingManagerId,
                                   ProjectId = emp?.ProjectId,
                                   ProjectName = emp?.ProjectName ?? string.Empty,
                                   DepartmentId = e.DepartmentId ?? 0,
                                   Email = e.WorkEmailAddress,
                                   IsActive = true
                               }).Distinct().OrderBy(x => x.AssociateName).ToList();

                response.IsSuccessful = true;
                response.Items = empList;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Exception occured in Associate Allocation Service - GetAssociates method.", ex.StackTrace);
                response.Message = "Exception occured in Associate Allocation Service - GetAssociates method.";
                response.IsSuccessful = false;
            }
            return response;
        }

        #endregion

        #region GetProjectsByEmpIdAndRole
        public async Task<ServiceResponse<ProjectDTO>> GetProjectsByEmpIdAndRole(int employeeId)
        {
            var response = new ServiceResponse<ProjectDTO>();
            try
            {
                //fetch PM role based active allocations
                var isProjectByRole = await m_ProjectContext.AssociateAllocation.Where(all => all.ProgramManagerId == employeeId && all.IsActive == true).Select(all => all.ProjectId).Distinct().Cast<int>().ToListAsync();
                if (isProjectByRole.Count() == 0)
                {
                    //fetch Lead role based active allocations
                    isProjectByRole =await  m_ProjectContext.AssociateAllocation.Where(all => all.LeadId == employeeId && all.IsActive == true).Select(all => all.ProjectId).Distinct().Cast<int>().ToListAsync();

                    if (isProjectByRole.Count() == 0)
                    {
                        //fetch Associate role based active and primary allocation 
                        isProjectByRole =await  m_ProjectContext.AssociateAllocation.Where(all => all.EmployeeId == employeeId && all.IsActive == true && all.IsPrimary == true).Select(all => all.ProjectId).Distinct().Cast<int>().ToListAsync();
                    }
                }

                if (isProjectByRole.Count() > 0)
                {
                    var projectDetails = (from project in m_ProjectContext.Projects.ToList()
                                          join projectID in isProjectByRole on project.ProjectId equals projectID
                                          select new Projects
                                          {
                                              ProjectCode = project.ProjectCode,
                                              ProjectName = project.ProjectName
                                          }).ToList();
                    ProjectDTO projects = new ProjectDTO
                    {
                        Projects = projectDetails
                    };
                    response.IsSuccessful = true;
                    response.Item = projects;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No Project found";
                    response.Item = new ProjectDTO();
                }
            }
            catch(Exception e)
            {
                response.IsSuccessful = false;
                response.Message = "Failed to fetch the projects";
                return response;
            }
           
            return response;
        }
        #endregion


    }
}
