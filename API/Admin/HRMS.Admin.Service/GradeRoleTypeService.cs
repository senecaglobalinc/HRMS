using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.Entities.Models;

namespace HRMS.Admin.Service
{
    public class GradeRoleTypeService : IGradeRoleTypeService
    {
        #region Global Variables

        private readonly ILogger<GradeRoleTypeService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;

        #endregion

        #region GradeRoleTypeService
        public GradeRoleTypeService(ILogger<GradeRoleTypeService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GradeRoleType, GradeRoleType>();
            });
            m_mapper = config.CreateMapper();

        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the GradeRoleType details
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<GradeRoleType>> GetAll(bool? isActive = true) =>
                        await m_AdminContext.GradeRoleTypes.Where(gr => gr.IsActive == isActive).ToListAsync();
        #endregion       

        #region GetById
        /// <summary>
        /// Get graderoletype by id
        /// </summary>
        /// <param name="gradeRoleTypeId">gradeRoleTypeId</param>
        /// <returns></returns>
        public async Task<GradeRoleType> GetById(int gradeRoleTypeId) =>
                        await m_AdminContext.GradeRoleTypes.FindAsync(gradeRoleTypeId);

        #endregion       

        #region Create
        /// <summary>
        /// Create New GradeRoleType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<RoleType>> Create(GradeRoleTypeRequest model)
        {
            int isCreated = 0;
            var response = new ServiceResponse<RoleType>();           
            
            m_Logger.LogInformation("RoleTypeService: Calling \"Create\" method.");

            var roleTypeExists = await m_AdminContext.GradeRoleTypes.Where(x => x.RoleTypeId == model.RoleTypeId).Select(x => x).ToListAsync();
            if (roleTypeExists != null && roleTypeExists.Count > 0)
            {
                m_AdminContext.GradeRoleTypes.RemoveRange(roleTypeExists);
                await m_AdminContext.SaveChangesAsync();
            }

            //Map the grades after removal
            GradeRoleType gradeRoleType = null;
            foreach (string gradeId in model.GradeIds.Split(','))
            {
                gradeRoleType = new GradeRoleType()
                {
                    RoleTypeId = model.RoleTypeId,
                    GradeId = Convert.ToInt32(gradeId)
                };

                await m_AdminContext.GradeRoleTypes.AddAsync(gradeRoleType);
            }

            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {         
                response.Item = null;
                response.IsSuccessful = true;
            }
            else
            {
                response.IsSuccessful = false;
                response.Message = "No record created.";
            }
            return response;
        }
        #endregion

        #region GetGradesBySearchFilters
        /// <summary>
        /// Gets the Grade details by Financial Year, Department and RoleTypeId(optional)
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GradeRoleTypeModel>> GetGradesBySearchFilters(int financialYearId, int departmentId = 0, int roleTypeId = 0)
        {
            //var gradeRoleTypes = await m_AdminContext.RoleTypes.Where(w => w.FinancialYearId == financialYearId)
            //      .Join(m_AdminContext.GradeRoleTypes, rt => rt.RoleTypeId, grt => grt.RoleTypeId, (rt, grt) => new { rt, grt })
            //      .Join(m_AdminContext.Grades, grtt => grtt.grt.GradeId, grd => grd.GradeId, (grtt, grd) => new { grtt, grd })
            //      .Join(m_AdminContext.Departments, dt => dt., grd => grd.GradeId, (grtt, grd) => new { grtt, grd })
            //      .Select(x =>
            //      new GradeRoleTypeModel
            //      {
            //          GradeRoleTypeId = x.grtt.grt.GradeRoleTypeId,
            //          RoleTypeId = x.grtt.rt.RoleTypeId,
            //          RoleTypeName = x.grtt.rt.RoleTypeName,
            //          RoleTypeDescription = x.grtt.rt.RoleTypeDescription,
            //          FinancialYearId = x.grtt.rt.FinancialYearId,
            //          DepartmentId = x.grtt.rt.DepartmentId,
            //          DepartmentDescription ="",
            //          GradeId = x.grd.GradeId,
            //          GradeName = x.grd.GradeName
            //      }).OrderBy(x => x.RoleTypeId).ToListAsync();

               List<GradeRoleTypeModel> gradeRoleTypes = await (from rt in m_AdminContext.RoleTypes.Where(w => w.FinancialYearId == financialYearId)
                                           join grt in m_AdminContext.GradeRoleTypes
                                           on rt.RoleTypeId equals grt.RoleTypeId
                                           join grd in m_AdminContext.Grades
                                           on grt.GradeId equals grd.GradeId
                                           join dpt in m_AdminContext.Departments
                                           on rt.DepartmentId equals dpt.DepartmentId
                                           select 
                  new GradeRoleTypeModel
                  {
                      GradeRoleTypeId = grt.GradeRoleTypeId,
                      RoleTypeId = rt.RoleTypeId,
                      RoleTypeName = rt.RoleTypeName,
                      RoleTypeDescription = rt.RoleTypeDescription,
                      FinancialYearId = rt.FinancialYearId,
                      DepartmentId = dpt.DepartmentId,
                      DepartmentDescription = dpt.Description,
                      Grade = grd.GradeId,
                      GradeName = grd.GradeName
        }).Distinct().OrderBy(x => x.RoleTypeId).ToListAsync();

            if (departmentId != 0 && roleTypeId != 0)
                gradeRoleTypes = gradeRoleTypes.Where(w => w.RoleTypeId == roleTypeId && w.DepartmentId == departmentId).Select(x => x).OrderBy(x => x.GradeRoleTypeId).ToList();
            else if (departmentId != 0 && roleTypeId == 0)
                gradeRoleTypes = gradeRoleTypes.Where(w => w.DepartmentId == departmentId).Select(x => x).OrderBy(x => x.GradeRoleTypeId).ToList();

            var roleTypes = gradeRoleTypes
                    .GroupBy(x => new { x.RoleTypeId, x.FinancialYearId, x.RoleTypeName, x.RoleTypeDescription, 
                    x.DepartmentId, x.DepartmentDescription }, (key, g) => new GradeRoleTypeModel
                    {
                        GradeRoleTypeId = 0,
                        RoleTypeId = key.RoleTypeId,
                        RoleTypeName = key.RoleTypeName,
                        RoleTypeDescription = key.RoleTypeDescription,
                        FinancialYearId = key.FinancialYearId,
                        DepartmentId = key.DepartmentId,
                        DepartmentDescription = key.DepartmentDescription,
                        GradeId = g.Select(c => c.Grade).ToList(),
                        GradeName = string.Join(", ", g.Select(p => p.GradeName.ToString())) 
                    }).ToList();

            return roleTypes;
        }

        #endregion
    }
}
