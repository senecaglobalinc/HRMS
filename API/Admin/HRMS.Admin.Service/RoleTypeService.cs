using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Entities.Models;
using HRMS.Admin.Infrastructure.Models;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class RoleTypeService : IRoleTypeService
    {
        #region Global Varibles
        private readonly ILogger<RoleTypeService> m_Logger;
        private readonly AdminContext m_AdminContext;
        #endregion

        #region RoleTypeService
        public RoleTypeService(ILogger<RoleTypeService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
        }

        #endregion        

        #region GetAll
        /// <summary>
        /// Gets RoleType Data
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleTypeModel>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("RoleTypeService: Calling \"GetAll\" method.");

            var lstRoleType = await (from rt in m_AdminContext.RoleTypes
                                     join fy in m_AdminContext.FinancialYears on rt.FinancialYearId equals fy.Id
                                     join d in m_AdminContext.Departments on rt.DepartmentId equals d.DepartmentId
                                     where rt.IsActive == isActive
                                     orderby rt.RoleTypeName
                                     select new RoleTypeModel()
                                     {
                                         RoleTypeId = rt.RoleTypeId,
                                         RoleTypeName = rt.RoleTypeName,
                                         RoleTypeDescription = rt.RoleTypeDescription,
                                         FinancialYearId = rt.FinancialYearId,
                                         FinancialYearName = Convert.ToString(fy.FromYear) + " - " + Convert.ToString(fy.ToYear),
                                         DepartmentId = rt.DepartmentId,
                                         Department = d.Description,
                                         IsDeliveryDepartment = rt.IsDeliveryDepartment,
                                         IsActive = rt.IsActive
                                     }).OrderBy(x => x.RoleTypeName).ToListAsync();

            return lstRoleType;
        }
        #endregion

        #region GetById
        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="roleTypeId">RoleType Id</param>
        /// <returns></returns>
        public async Task<RoleTypeModel> GetById(int roleTypeId)
        {
            m_Logger.LogInformation("RoleTypeService: Calling \"GetById\" method.");

            var roleType = await (from rt in m_AdminContext.RoleTypes
                                  join fy in m_AdminContext.FinancialYears on rt.FinancialYearId equals fy.Id
                                  join d in m_AdminContext.Departments on rt.DepartmentId equals d.DepartmentId
                                  where rt.RoleTypeId == roleTypeId
                                  orderby rt.RoleTypeName
                                  select new RoleTypeModel()
                                  {
                                      RoleTypeId = rt.RoleTypeId,
                                      RoleTypeName = rt.RoleTypeName,
                                      RoleTypeDescription = rt.RoleTypeDescription,
                                      FinancialYearId = rt.FinancialYearId,
                                      FinancialYearName = Convert.ToString(fy.FromYear) + " - " + Convert.ToString(fy.ToYear),
                                      DepartmentId = rt.DepartmentId,
                                      Department = d.Description,
                                      IsDeliveryDepartment = rt.IsDeliveryDepartment,
                                      IsActive = rt.IsActive
                                  }).FirstOrDefaultAsync();

            return roleType;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create New RoleType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> Create(RoleTypeModel model)
        {
            bool isCreated = false;
            RoleType roleType = null;
            m_Logger.LogInformation("RoleTypeService: Calling \"Create\" method.");

            if (String.IsNullOrEmpty(model.RoleTypeName))
                return (isCreated, "Role Type Name is mandatory.");

            var roleTypeExist = await m_AdminContext.RoleTypes.SingleOrDefaultAsync(x => x.RoleTypeName.ToLower() == model.RoleTypeName.ToLower().Trim()
                                && x.FinancialYearId == model.FinancialYearId && x.DepartmentId == model.DepartmentId);

            if (roleTypeExist != null)
            {
                return (isCreated, "Role Type already mapped to this selected Financial Year and Department.");
            }
            else
            {
                roleType = new RoleType()
                {
                    RoleTypeName = model.RoleTypeName,
                    RoleTypeDescription = model.RoleTypeDescription,
                    FinancialYearId = model.FinancialYearId,
                    DepartmentId = model.DepartmentId,
                    IsDeliveryDepartment = model.DepartmentId == 1,
                    IsActive = true
                };

                await m_AdminContext.RoleTypes.AddAsync(roleType);
                isCreated = await m_AdminContext.SaveChangesAsync() > 0;
            }

            return (isCreated, "Record's created successfully!.");
        }
        #endregion

        #region Update
        /// <summary>
        /// Update roleTypeId in Grades
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> Update(RoleTypeModel model)
        {
            bool updated = false;
            var roleType = await m_AdminContext.RoleTypes.FindAsync(model.RoleTypeId);

            if (roleType != null)
            {
                roleType.RoleTypeDescription = model.RoleTypeDescription;
                roleType.FinancialYearId = model.FinancialYearId;
                roleType.DepartmentId = model.DepartmentId;
                roleType.IsDeliveryDepartment = model.DepartmentId == 1;
                roleType.IsActive = model.IsActive;
                updated = await m_AdminContext.SaveChangesAsync() > 0;
            }

            return (updated, "Record's updated successfully!.");
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete RoleType
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> Delete(int id)
        {
            bool deleted = false;
            var roleType = await m_AdminContext.RoleTypes.FirstOrDefaultAsync(x => x.RoleTypeId == id);

            if (roleType != null)
            {
                m_AdminContext.RoleTypes.Remove(roleType);
                deleted = await m_AdminContext.SaveChangesAsync() > 0;
            }

            return (deleted, "Record's deleted successfully!.");
        }
        #endregion

        #region GetRoleTypesForDropdown
        /// <summary>
        /// GetAllRoleTypes
        /// </summary>
        /// <returns>GenericType</returns>
        public async Task<List<GenericType>> GetRoleTypesForDropdown(int financialYearId, int departmentId)
        {
            var response = new List<GenericType>();
            m_Logger.LogInformation("RoleTypeService: Calling \"GetRoleTypesForDropdown\" method.");

            try
            {
                if (departmentId > 0)
                {
                    response = await (from rt in m_AdminContext.RoleTypes
                                      where rt.FinancialYearId == financialYearId && rt.DepartmentId == departmentId && rt.IsActive == true
                                      select new GenericType { Id = rt.RoleTypeId, Name = rt.RoleTypeName }).OrderBy(x => x.Name).ToListAsync();

                }
                else if(financialYearId>0)
                {
                    response = await (from rt in m_AdminContext.RoleTypes
                                      where rt.FinancialYearId == financialYearId && rt.IsActive == true
                                      select new GenericType { Id = rt.RoleTypeId, Name = rt.RoleTypeName }).OrderBy(x => x.Name).ToListAsync();

                }
                else
                {
                    response = await (from rt in m_AdminContext.RoleTypes
                                      where rt.IsActive == true
                                      select new GenericType { Id = rt.RoleTypeId, Name = rt.RoleTypeName }).OrderBy(x => x.Name).ToListAsync();

                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured while fetching RoleTypes" + ex.StackTrace);
            }
            return response;
        }
        #endregion
        
        #region GetRoleTypesAndDepartmentsAsync
        /// <summary>
        /// GetRoleTypesAndDepartmentsAsync
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<List<RoleTypeDepartment>> GetRoleTypesAndDepartmentsAsync(int departmentId = 0)
        {
            var roleTypesAndDepartments = new List<RoleTypeDepartment>();

            var departments = new List<Department>();

            if (departmentId == 0)
            {
                departments = await m_AdminContext.Departments.Where(dept => dept.IsActive == true).ToListAsync();
            }
            else
            {
                departments = await m_AdminContext.Departments.Where(dept => dept.DepartmentId == departmentId && dept.IsActive == true).ToListAsync();
            }

            foreach (var department in departments)
            {
                var roleTypesAndDepartment = new RoleTypeDepartment
                {
                    DepartmentId = department.DepartmentId,
                    DepartmentName = department.Description
                };
                roleTypesAndDepartment.RoleTypeIds.AddRange(await m_AdminContext.RoleTypes.Where(role => role.DepartmentId == department.DepartmentId).Select(role => role.RoleTypeId).ToListAsync());

                roleTypesAndDepartments.Add(roleTypesAndDepartment);
            }

            return roleTypesAndDepartments;
        }
        #endregion
    }
}
