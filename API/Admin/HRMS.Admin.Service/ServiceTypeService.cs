using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
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
    public class ServiceTypeService : IServiceTypeService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<ServiceTypeService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public ServiceTypeService(AdminContext adminContext, ILogger<ServiceTypeService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ServiceType, ServiceType>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a Service Type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(ServiceType serviceType)
        {
            m_Logger.LogInformation("Calling Create method in ServiceTypeService");

            int isCreated;

            //Checking if  already exists
            var isExists = m_AdminContext.ServiceTypes.Where(p => p.ServiceDescription.ToLower().Trim() == serviceType.ServiceDescription.ToLower().Trim()
             && p.ServiceTypeName == serviceType.ServiceTypeName && p.IsActive == true).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "ServiceType Already Exists.");

            ServiceType Activity = new ServiceType();

            if (!serviceType.IsActive.HasValue)
                serviceType.IsActive = true;

            //Add fields
            m_mapper.Map<ServiceType, ServiceType>(serviceType, Activity);

            m_Logger.LogInformation("Add Service Type to list");
            m_AdminContext.ServiceTypes.Add(Activity);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in ServiceType Service");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Service Type created successfully.");
                return CreateResponse(Activity, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No Service Type created");
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the Service Type details
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(ServiceType serviceType)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling Update method in ServiceTypeService");

            //Checking if  already exists
            //var isExists = m_AdminContext.ServiceTypes.Where(p => p.ServiceDescription.ToLower().Trim() == serviceType.ServiceDescription.ToLower().Trim()
            //                             && p.ServiceTypeName == serviceType.ServiceTypeName && p.IsActive == true).Count();

            

            //Fetch Activity for update
            ServiceType type = m_AdminContext.ServiceTypes.Find(serviceType.ServiceTypeId);

            if (type == null)
                return CreateResponse(null, false, "Service Type not found for update");

            if (!serviceType.IsActive.HasValue)
                type.IsActive = serviceType.IsActive;
            serviceType.CreatedBy = type.CreatedBy;
            serviceType.CreatedDate = type.CreatedDate;
            //update fields
            m_mapper.Map<ServiceType, ServiceType>(serviceType, type);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in ServiceTypeService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating  record in ServiceTypeService");
                return CreateResponse(serviceType, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the Service Type details
        /// </summary>
        /// <returns></returns>
        public async Task<List<ServiceType>> GetAll(bool isActive = true) =>
                       await m_AdminContext.ServiceTypes.Where(et => et.IsActive == isActive).OrderBy(x => x.ServiceTypeName).ToListAsync();

        #endregion

        #region GetServiceTypeForDropdown
        /// <summary>
        /// GetServiceTypeForDropdown
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetServiceTypeForDropdown() =>
                        await m_AdminContext.ServiceTypes.Where(cl => cl.IsActive == true).Select(ci => new GenericType { Id = ci.ServiceTypeId, Name = ci.ServiceTypeName }).OrderBy(x => x.Name).ToListAsync();

        #endregion

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(ServiceType serviceType, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in ServiceTypeService");

            dynamic response = new ExpandoObject();
            response.ServiceType = serviceType;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in ServiceTypeService");

            return response;
        }

        #endregion

    }
}
