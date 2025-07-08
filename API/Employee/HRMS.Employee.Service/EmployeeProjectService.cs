using AutoMapper;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class EmployeeProjectService : IEmployeeProjectService
    {
        #region Global Variables
        private readonly ILogger<EmployeeProjectService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        #endregion

        #region EmployeeProjectService
        public EmployeeProjectService(ILogger<EmployeeProjectService> logger,
                    EmployeeDBContext employeeDBContext)
        {
            m_Logger = logger;
            m_EmployeeContext = employeeDBContext;
            //Create mapper for certification
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmployeeProject, EmployeeProject>();
            });
            m_mapper = config.CreateMapper();

        }
        #endregion

        #region Create
        /// <summary>
        /// Removes existing projects and inserts new projects based on employeeId
        /// </summary>
        /// <param name="employeeDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeeProject>> Create(EmployeeDetails employeeDetails)
        {
            int isCreated;
            var response = new ServiceResponse<EmployeeProject>();
            EmployeeProject empProject = new EmployeeProject();
            try
            {
                m_Logger.LogInformation("EmployeeProjectService: Calling \"Create\" method.");

                //Fetch existing projects Based on employeed id
                List<EmployeeProject> existingProjects = await m_EmployeeContext.EmployeeProjects
                                                                                .Where(id => id.EmployeeId == employeeDetails.EmpId)
                                                                                .ToListAsync();
                //Remove all the existing projects 
                m_EmployeeContext.EmployeeProjects.RemoveRange(existingProjects);

                //Insert new project details
                foreach(EmployeeProject employeeProject in employeeDetails.Projects)
                {
                    employeeProject.EmployeeId = employeeDetails.EmpId;
                    employeeProject.IsActive = true;
                    empProject = new EmployeeProject();
                    //map fields
                    m_mapper.Map<EmployeeProject, EmployeeProject>(employeeProject, empProject);
                    m_EmployeeContext.EmployeeProjects.Add(empProject);
                }

                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in EmployeeProjectService");
                isCreated = await m_EmployeeContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = empProject;
                    m_Logger.LogInformation("Projects created successfully");
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No project created";
                    m_Logger.LogInformation("No project created");
                }

            }
            catch(Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while inserting project details";
                m_Logger.LogError("Error occurred in Create() method of EmployeeProjectService " + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Fetches project details of an employee in previous organization based on employee id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<EmployeeProject>> GetByEmployeeId(int employeeId)
        {
            var response = new ServiceListResponse<EmployeeProject>();
            try
            {
                m_Logger.LogInformation("EmployeeProjectService: Calling \"GetByEmployeeId\" method.");
                List<EmployeeProject> projects = await m_EmployeeContext.EmployeeProjects
                                                        .Where(project => project.EmployeeId == employeeId)
                                                        .ToListAsync();
                response.IsSuccessful = true;
                response.Items = projects;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while fetching Projects";
                m_Logger.LogError("Error occurred while fetching Projects in GetByEmployeeId() method of EmployeeProjectService" + ex.StackTrace);
            }
            return response;
        }
        #endregion 
    }
}
