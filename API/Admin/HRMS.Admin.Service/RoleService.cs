using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class RoleService : IRoleService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<RoleService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public RoleService(AdminContext adminContext, ILogger<RoleService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoleMaster, RoleMaster>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a Functional role
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(RoleMaster roleMasterIn)
        {
            m_Logger.LogInformation("Calling create method in RoleService");
            int isCreated;

            //Checking if functional role already exists
            var isExists = m_AdminContext.RoleMasters.Where(role => role.SGRoleID == roleMasterIn.SGRoleID && role.PrefixID == roleMasterIn.PrefixID && role.SuffixID == roleMasterIn.SuffixID).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Role already exists");

            RoleMaster roleMaster = new RoleMaster();

            //Add fields
            m_mapper.Map<RoleMaster, RoleMaster>(roleMasterIn, roleMaster);
            roleMaster.IsActive = roleMasterIn.IsActive.HasValue ? roleMasterIn.IsActive : true;

            m_Logger.LogInformation("Add functional role to list");
            m_AdminContext.RoleMasters.Add(roleMaster);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in RoleService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("functional role  created successfully.");
                return CreateResponse(roleMaster, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No functional role  created");
        }
        #endregion

        #region GetAllRoleMasters
        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<RoleMaster>> GetAll() =>
           new ServiceListResponse<RoleMaster> { Items = await m_AdminContext.RoleMasters.ToListAsync() };
        #endregion

        #region GetRoleNames
        public async Task<ServiceListResponse<RoleMasterDetails>> GetRoleNames() =>
           new ServiceListResponse<RoleMasterDetails>
           {
               Items = await (from rm in m_AdminContext.RoleMasters
                              join sgr in m_AdminContext.SGRoles on rm.SGRoleID equals sgr.SGRoleID
                              join rf in m_AdminContext.SGRolePrefixes on rm.PrefixID equals rf.PrefixID into rfg
                              from rolePrefix in rfg.DefaultIfEmpty()
                              join rs in m_AdminContext.SGRoleSuffixes on rm.SuffixID equals rs.SuffixID into rsg
                              from roleSuffix in rsg.DefaultIfEmpty()
                              select new RoleMasterDetails
                              {
                                  RoleMasterId = rm.RoleMasterID,
                                  RoleName = ((rolePrefix == null ? "" : rolePrefix.PrefixName) + " " + sgr.SGRoleName + " " + (roleSuffix == null ? "" : roleSuffix.SuffixName)).Trim(),
                                  DepartmentId = rm.DepartmentId,
                                  RoleDescription = rm.RoleDescription,
                                  SGRoleID = rm.SGRoleID,
                                  PrefixID = rm.PrefixID,
                                  SuffixID = rm.SuffixID,
                                  PrefixName = rolePrefix == null ? "" : rolePrefix.PrefixName,
                                  SuffixName = roleSuffix == null ? "" : roleSuffix.SuffixName,
                                  SGRoleName = sgr.SGRoleName
                              }).OrderBy(x => x.RoleName).ToListAsync()
           };
        #endregion

        #region GetByDepartmentID
        /// <summary>
        /// Gets all roles with given DepartmentID 
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<List<RoleMaster>> GetByDepartmentID(int departmentId) =>
            await m_AdminContext.RoleMasters
            .Where(role => role.DepartmentId == departmentId)
                            .Select(s => new RoleMaster
                            {
                                RoleMasterID = s.RoleMasterID,
                                RoleDescription = s.RoleDescription,
                                SGRoleID = s.SGRoleID,
                                PrefixID = s.PrefixID,
                                SuffixID = s.SuffixID,
                                EducationQualification = s.EducationQualification,
                                KeyResponsibilities = s.KeyResponsibilities,
                                DepartmentId = s.DepartmentId,
                                SGRole = new SGRole
                                {
                                    SGRoleName = s.SGRole.SGRoleName,
                                },
                                SGRolePrefix = new SGRolePrefix
                                {
                                    PrefixName = s.SGRolePrefix.PrefixName
                                },
                                SGRoleSuffix = new SGRoleSuffix
                                {
                                    SuffixName = s.SGRoleSuffix.SuffixName
                                },
                                Department = new Department
                                {
                                    DepartmentCode = s.Department.DepartmentCode
                                }
                            }).ToListAsync();

        #endregion

        #region GetSGRoleSuffixAndPrefix
        /// <summary>
        /// Gets the Role, Suffix and Prefix details
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<dynamic> GetSGRoleSuffixAndPrefix(int departmentId)
        {
            m_Logger.LogInformation("Calling GetRoleSuffixAndPrefix method in RoleService");

            //Get all SGRolePrefixes
            var Prefix = await m_AdminContext.SGRolePrefixes.ToListAsync();

            //Get all SGRoleSuffixes
            var Suffix = await m_AdminContext.SGRoleSuffixes.ToListAsync();

            //Get all SGRoles with given departmentId
            var Roles = await m_AdminContext.SGRoles.Where(role => role.DepartmentId == departmentId).ToListAsync();

            dynamic response = new ExpandoObject();
            response.Roles = Roles;
            response.Prefix = Prefix;
            response.Suffix = Suffix;
            return response;
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates a Functional role
        /// </summary>
        /// <param name="roleMasterIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(RoleMaster roleMasterIn)
        {
            m_Logger.LogInformation("Calling update method in RoleService");
            int isUpdated;

            //Checking if functional role already exists
            var isExists = m_AdminContext.RoleMasters.Where(role => role.SGRoleID == roleMasterIn.SGRoleID && role.PrefixID == roleMasterIn.PrefixID && role.SuffixID == roleMasterIn.SuffixID && role.RoleMasterID != roleMasterIn.RoleMasterID).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Role already exists");

            //Fetch functionalrole for update
            var roleMaster = m_AdminContext.RoleMasters.Find(roleMasterIn.RoleMasterID);

            if (roleMaster == null)
                return CreateResponse(null, false, "Functional role not found for update.");

            //Fetch functional role for update
            m_mapper.Map<RoleMaster, RoleMaster>(roleMasterIn, roleMaster);
            roleMaster.IsActive = roleMasterIn.IsActive.HasValue ? roleMasterIn.IsActive : roleMaster.IsActive;

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in RoleService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("functional role updated successfully.");
                return CreateResponse(roleMaster, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No functional role updated");
        }
        #endregion

        #region GetRolesAndDepartments
        public async Task<List<RolesDepartmentsList>> GetRolesAndDepartments()
        {
            m_Logger.LogInformation("Calling GetRolesAndDepartments method");

            List<RolesDepartmentsList> rolesDepartmentsList = null;
            rolesDepartmentsList = await (from roles in m_AdminContext.Roles
                                          join departments in m_AdminContext.Departments
                                          on roles.DepartmentId equals departments.DepartmentId
                                          where roles.DepartmentId != null && roles.IsActive == true && departments.IsActive == true
                                          orderby roles.RoleId
                                          select new RolesDepartmentsList
                                          {
                                              RoleId = roles.RoleId,
                                              RoleName = roles.RoleName,
                                              DepartmentId = departments.DepartmentId,
                                              DepartmentCode = departments.DepartmentCode,
                                              DepartmentHeadId = departments.DepartmentHeadId
                                          }).ToListAsync();

            return rolesDepartmentsList;
        }
        #endregion

        //Private Method
        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="roleMaster"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(RoleMaster roleMaster, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in RoleService");

            dynamic response = new ExpandoObject();
            response.RoleMaster = roleMaster;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in RoleService");

            return response;
        }

        #endregion
    }
}
