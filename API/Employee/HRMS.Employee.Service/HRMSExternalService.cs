using AutoMapper;
using AutoMapper.Configuration;
using HRMS.Employee.Database;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class HRMSExternalService: IHRMSExternalService
    {
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IOrganizationService m_OrgService;
        private readonly IProjectService m_ProjectService;
        private readonly IMapper m_mapper;

        public HRMSExternalService(EmployeeDBContext employeeDBContext,
                               ILogger<HRMSExternalService> logger,
                               IProjectService projectService,
                               IOrganizationService orgService
                               
                               )
        {
            //CreateMapper
           
            m_EmployeeContext = employeeDBContext;
           
            m_ProjectService = projectService;
            m_OrgService = orgService;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<List<Employee.Entities.Employee>,List<EmployeeDetails>>();               
            });
            m_mapper = config.CreateMapper();


        }
        #region GetProjects
        public async Task<ServiceResponse<ProjectDTO>> GetProjects()
        {
            var response = new ServiceResponse<ProjectDTO>();
            try
            {
                var projectsList =(await m_ProjectService.GetAllProjects());
                if(!projectsList.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    return response;
                }

                var projects =  (from proj in projectsList.Items
                                      where proj.ProjectStateId == 19
                                      select new ProjectDetails
                                      {
                                          ProjectCode = proj.ProjectCode,
                                          ProjectId = proj.ProjectId,
                                          ProjectName = proj.ProjectName
                                      }).OrderBy(pro => pro.ProjectName).ToList();
                ProjectDTO project = new ProjectDTO
                {
                    Projects = projects
                };
                response.IsSuccessful = true;
                response.Item = project;

            }
            catch (Exception ex)
            {
                //m_Logger.LogError("Exception occured in - GetProjects method.", ex.StackTrace);
                response.Message = "Exception occured in GetProjects method.";
                response.IsSuccessful = false;
            }
            return response;
        }

        #endregion

        #region GetProjectById
        public async Task<ServiceResponse<ProjectDTO>> GetProjectById(int projectId)
        {
            var response = new ServiceResponse<ProjectDTO>();
            try
            {
                var projects =(await GetProjects());
                if(!projects.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    return response;
                }

                        var project = (from projet in projects.Item.Projects
                                       where projet.ProjectId== projectId
                                       select new ProjectDetails
                                       {
                                           ProjectId = projet.ProjectId,
                                           ProjectCode = projet.ProjectCode,
                                           ProjectName = projet.ProjectName
                                       }).ToList();

                response.IsSuccessful = true;
                response.Item = new ProjectDTO { Projects = project };
            }
            catch (Exception ex)
            {
                //m_Logger.LogError("Exception occured in - GetProjects method.", ex.StackTrace);
                response.Message = "Exception occured in GetProjects method.";
                response.IsSuccessful = false;
            }
            return response;
        }

        #endregion

        #region GetActiveEmployeeNamesAsync
        public async Task<EmployeeDTO> GetActiveEmployeeNamesAsync(CancellationToken cancellationToken = default)
        {
            var employees = new EmployeeDTO();
            var employeeDTOs = await (from emp in m_EmployeeContext.Employees
                                      where emp.IsActive == true && emp.EmployeeId != 1
                                      select new GetActiveEmployeesDTO
                                      {
                                          ID = emp.EmployeeId,
                                          Name = string.IsNullOrWhiteSpace(emp.MiddleName)
                                                 ? emp.FirstName + " " + emp.LastName
                                                 : emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
                                          Mail = emp.WorkEmailAddress
                                      })
                                      .OrderBy(x => x.Name)
                                      .ToListAsync(cancellationToken);

            employees.Employee = employeeDTOs;
            return employees;
        }
        #endregion

        #region GetAllEmployeeDetailsForExternalAsync
        public async Task<EmployeesInfo> GetAllEmployeeDetailsForExternalAsync()
        {
            List<AllEmployeeDetailsDTO> allEmployeeDetails = null;
            EmployeesInfo employeesInfo = new EmployeesInfo();
            Dictionary<int, string> employeeNameMapping = null;
            var activeAllocations =(await m_ProjectService.GetAllAllocationDetails()).Items;
           

            var departments = (await m_OrgService.GetAllDepartments()).Items;
            var designations =(await m_OrgService.GetAllDesignations()).Items;
            var clients=(await m_OrgService.GetAllClients()).Items;
            var roleMaster =(await m_OrgService.GetRoleMasterDetails()).Items;
            var emps = m_EmployeeContext.Employees.Where(emp=>emp.IsActive==true && emp.Nationality.ToUpper()!="USA" && emp.EmployeeId!= 1).ToList();// excude usa and default user
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            var activeEmployees = JsonConvert.DeserializeObject<List<EmployeeDetails>>(JsonConvert.SerializeObject(emps,jss));
            //synchronous code
            if (activeEmployees?.Count() > 0)
            {
                FillDepartmentNames(ref activeEmployees, departments);
                FillDesignations(ref activeEmployees, designations);
            }

            if (activeAllocations.Count() > 0)
            {
                FillClients(ref activeAllocations, clients);
                FillRoles(ref activeAllocations, roleMaster);
            }

            if (activeEmployees?.Count() > 0 && activeAllocations?.Count() > 0)
            {
                employeeNameMapping = BuildIdNameMappings(activeEmployees);

                var employeesLeftJoin = activeEmployees.GroupJoin(activeAllocations,
                                  n => n.EmployeeId,
                                  m => m.EmployeeId,
                                  (n, ms) => new { n, ms = ms.DefaultIfEmpty() })
                       .SelectMany(z => z.ms.Select(m => new { z.n, m }));
                var nullableValues = employeesLeftJoin.Where(x => x.n.ReportingManagerId == null).ToList();
                var nullableVal = employeesLeftJoin.Select(x => x.n.ReportingManager).ToList();
                allEmployeeDetails = employeesLeftJoin.Select(x =>
                {
                    _ = employeeNameMapping.TryGetValue(Convert.ToInt32(x.n.ReportingManager), out string reportingMgrName);

                    int? pgmMgrId = x.m?.ProgramManagerId ?? x.n.ProgramManager;
                    _ = employeeNameMapping.TryGetValue(pgmMgrId ?? 0, out string programMgrName);

                    return new AllEmployeeDetailsDTO
                    {
                        AssociateCode = x.n.EmployeeCode,
                        FirstName = x.n.FirstName,
                        LastName = x.n.LastName,
                        AssociateName =GetFullName(x.n.FirstName, null, x.n.LastName),
                        Gender = x.n.Gender,
                        DateofBirth = x.n.DateofBirth,
                        Email = x.n.WorkEmailAddress,
                        AssociateId = x.n.EmployeeId,
                        ReportingManagerId =Convert.ToInt32(x.n.ReportingManager),
                        ReportingManagerName = reportingMgrName,
                        ProgramManagerId = pgmMgrId,
                        ProgramManagerName = programMgrName,
                        IsActive = x.n.IsActive,
                        DateOfJoining = x.n.JoinDate,
                        BloodGroup = x.n.BloodGroup,
                        ProjectId = x.m?.ProjectId,
                        ProjectName = x.m?.ProjectName,
                        ClientId = x.m?.ClientId,
                        ClientName = x.m?.ClientName,
                        DepartmentId =x.n.DepartmentId??0,
                        DepartmentName=x.n.Department,
                        Designation = x.n.Designation,
                        EffectiveFromDate = x.m?.EffectiveFromDate,
                        EffectiveToDate = x.m?.EffectiveToDate,
                        RoleDescription = x.m?.RoleDescription
                    };
                }).ToList();

                allEmployeeDetails = allEmployeeDetails.OrderBy(x => x.AssociateCode).ToList();
            }
            employeesInfo.Employees = allEmployeeDetails;
            return employeesInfo;
        }
        #endregion

        #region GetDepartmentsList
        /// <summary>
        /// GetDepartmentsList
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResponse<DepartmentDTO>> GetDepartmentsList()
        {
            var response =new ServiceResponse<DepartmentDTO>();
            var departmentDto = new DepartmentDTO();
            var departments=(await m_OrgService.GetAllDepartments());
            if(!departments.IsSuccessful)
            {
                response.IsSuccessful = false;
                return response;
            }
            departmentDto.Departments = departments.Items.Select(dept => new GetDepartmentsDTO { 
                DepartmentCode = dept.DepartmentCode, DepartmentId = dept.DepartmentId, Description = dept.Description }).OrderBy(dept=>dept.DepartmentCode).ToList();
            response.IsSuccessful = true;
            response.Item = departmentDto;
            return response;
        }
        #endregion

        #region GetProjectsByEmailAndRole
        public async Task<ServiceResponse<ProjectsData>> GetProjectsByEmailAndRole(string emailId)
        {
            var response = new ServiceResponse<ProjectsData>();
            var employee = m_EmployeeContext.Employees.Where(emp => emp.WorkEmailAddress.ToLower() == emailId.ToLower() && emp.IsActive == true).FirstOrDefault();
            if(employee!=null)
            {
               var projects=(await m_ProjectService.GetProjectsByEmpIdAndRole(employee.EmployeeId));
               response = projects;               
            }
            response.IsSuccessful = true;
            return response;
        }
        #endregion


        #region GetProgramManagersList
        /// <summary>
        /// GetProgramManagersList
        /// </summary>
        /// <returns></returns>
        public async Task<ProgramManagersData> GetProgramManagersList()
        {
            List<ProgramManagerDTO> listPM = new List<ProgramManagerDTO>();
            ProgramManagersData pmData = new ProgramManagersData();

            var projectManagersList = await m_ProjectService.GetActiveProjectManagers();
            List<ProjectManager> pmList = projectManagersList.Items;
            List<int> pmIds = pmList.Where(x => x.IsActive == true && x.ProgramManagerId.HasValue).Select(x => x.ProgramManagerId.Value).Distinct().ToList();

            var emps = m_EmployeeContext.Employees.Where(emp=>emp.IsActive == true && pmIds.Contains(emp.EmployeeId)).ToList();
            foreach(var emp in emps)
            {
                var pmDTO = new ProgramManagerDTO();
                pmDTO.ApproverId = emp.EmployeeId;
                pmDTO.ApproverCode = emp.EmployeeCode;
                pmDTO.ApproverName = GetFullName(emp.FirstName, emp.MiddleName, emp.LastName);
                listPM.Add(pmDTO);
            }
            pmData.ProgramManagers = listPM;
            return pmData;
        }
        #endregion

        #region     private methods
        private void FillDepartmentNames(ref List<EmployeeDetails> destinationData, IEnumerable<Department> masterData)
        {
            if (masterData?.Count() > 0)
            {
                var joinData = destinationData.GroupJoin(masterData,
                    dest => dest.DepartmentId,
                    md => md.DepartmentId,
                    (dest, md) => new { dest, md = md.DefaultIfEmpty() })
                    .SelectMany(dest => dest.md.Select(department => new { dest.dest, department }));

                destinationData = joinData.Select(x => { x.dest.Department = x.department?.DepartmentCode.Trim(); return x.dest; }).ToList();
            }
        }

        private void FillDesignations(ref List<EmployeeDetails> destinationData, IEnumerable<Designation> masterData)
        {
            if (masterData?.Count() > 0)
            {
                var joinData = destinationData.GroupJoin(masterData,
                    dest => dest.DesignationId,
                    md => md.DesignationId,
                    (dest, md) => new { dest, md = md.DefaultIfEmpty() })
                    .SelectMany(dest => dest.md.Select(destination => new { dest.dest, destination }));

                destinationData = joinData.Select(x => { x.dest.Designation = x.destination?.DesignationName.Trim(); return x.dest; }).ToList();
            }
        }

        private void FillClients(ref List<ActiveAllocationDetails> destinationData, IEnumerable<Client> masterData)
        {
            if (masterData?.Count() > 0)
            {
                var joinData = destinationData.GroupJoin(masterData,
                    dest => dest.ClientId,
                    md => md.ClientId,
                    (dest, md) => new { dest, md = md.DefaultIfEmpty() })
                    .SelectMany(dest => dest.md.Select(cliententity => new { dest.dest, cliententity }));

                destinationData = joinData.Select(x => { x.dest.ClientName = x.cliententity?.ClientName.Trim(); return x.dest; }).ToList();
            }
        }

        private void FillRoles(ref List<ActiveAllocationDetails> destinationData, IEnumerable<RoleMasterDetails> masterData)
        {
            if (masterData?.Count() > 0)
            {
                var joinData = destinationData.GroupJoin(masterData,
                    dest => dest.RoleMasterId,
                    md => md.RoleMasterID,
                    (dest, md) => new { dest, md = md.DefaultIfEmpty() })
                    .SelectMany(dest => dest.md.Select(rolemaster => new { dest.dest, rolemaster }));

                destinationData = joinData.Select(x => { x.dest.RoleDescription = x.rolemaster?.RoleDescription.Trim(); return x.dest; }).ToList();
            }
        }

        private Dictionary<int, string> BuildIdNameMappings(List<EmployeeDetails> activeEmployees, CancellationToken cancellationToken = default)
        {
            return activeEmployees.ToDictionary(keySelector: x => x.EmployeeId, elementSelector: y => GetFullName(y.FirstName, y.MiddleName, y.LastName));
        }

        public static string GetFullName(string firstName, string middleName, string lastName)
        {
            if (!string.IsNullOrWhiteSpace(middleName))
            {
                return $"{firstName.Trim()} {middleName.Trim()} {lastName.Trim()}";
            }

            return $"{firstName.Trim()} {lastName.Trim()}";
        }
        #endregion
    }
}
