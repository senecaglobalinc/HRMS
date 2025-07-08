using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class DepartmentTypeService : IDepartmentTypeService
    {
        #region Global Varibles
        
        private readonly AdminContext m_AdminContext;
        private readonly ILogger<DepartmentTypeService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region Constructor
        public DepartmentTypeService(AdminContext adminContext, ILogger<DepartmentTypeService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DepartmentType, DepartmentType>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// creates a new DepartmentType
        /// </summary>
        /// <param name="departmentTypeIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(DepartmentType departmentTypeIn)
        {
            m_Logger.LogInformation("Calling Create method in DepartmentTypeService");

            int isCreated;
            DepartmentType departmentType = new DepartmentType();

            if (!departmentTypeIn.IsActive.HasValue)
                departmentTypeIn.IsActive = true;

            //create fields
            m_mapper.Map<DepartmentType, DepartmentType>(departmentTypeIn , departmentType);

            m_Logger.LogInformation("Add departmentType to list");
            m_AdminContext.DepartmentTypes.Add(departmentType);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in DepartmentTypeService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("DepartmentType created successfully.");
                return CreateResponse(departmentType, true, string.Empty);
            }
            else
            {
                return CreateResponse(null, false, "No DepartmentType created");
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the DepartmentType details
        /// </summary>
        /// <returns></returns>
        public async Task<List<DepartmentType>> GetAll(bool isActive = true) =>
                       await m_AdminContext.DepartmentTypes.Where(dt => dt.IsActive == isActive).ToListAsync();
        #endregion

        #region Update
        /// <summary>
        /// Updates the DepartmentType details
        /// </summary>
        /// <param name="departmentTypeIn"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(DepartmentType departmentTypeIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling Update method in DepartmentTypeService");

            //Fetch departmentType for update
            var departmentType = m_AdminContext.DepartmentTypes.Find(departmentTypeIn.DepartmentTypeId);

            if (departmentType == null)
                return CreateResponse(null, false, "DepartmentType not found for update");

            if (!departmentTypeIn.IsActive.HasValue)
                departmentTypeIn.IsActive = departmentType.IsActive;

            departmentTypeIn.CreatedBy = departmentType.CreatedBy;
            departmentTypeIn.CreatedDate = departmentType.CreatedDate;

            //update fields
            m_mapper.Map<DepartmentType, DepartmentType>(departmentTypeIn, departmentType);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in DepartmentTypeService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating DepartmentType record in DepartmentTypeService");
                return CreateResponse(departmentType, true, string.Empty);
            }
            else
            {
                return CreateResponse(null, false, "No record updated");
            }
        }
        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="departmentType"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(DepartmentType departmentType, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in DepartmentTypeService");

            dynamic response = new ExpandoObject();
            response.DepartmentType = departmentType;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in DepartmentTypeService");

            return response;
        }

        #endregion
    }
}
