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
    /// <summary>
    /// service class to get Domain details
    /// </summary>
    public class DomainService : IDomainService
    {
        #region Global Varible

        private readonly AdminContext m_AdminContext;
        private readonly ILogger<DomainService> m_Logger;
        private readonly IMapper m_mapper;

        #endregion

        #region constructor
        public DomainService(AdminContext adminContext, ILogger<DomainService> logger)
        {
            m_AdminContext = adminContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Domain, Domain>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a Domain
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public async Task<dynamic> Create(Domain domainIn)
        {
            m_Logger.LogInformation("Calling CreateDomain method in DomainService");

            int isCreated;

            //Checking if  already exists
            var isExists = m_AdminContext.Domains.Where(p => p.DomainName.ToLower().Trim() == domainIn.DomainName.ToLower().Trim()).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Domain Name already exists");

            Domain domain = new Domain();

            if (!domainIn.IsActive.HasValue)
                domainIn.IsActive = true;

            //Add fields
            m_mapper.Map<Domain, Domain>(domainIn , domain);

            m_Logger.LogInformation("Add domain to list");
            m_AdminContext.Domains.Add(domain);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in DomainService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
            {
                m_Logger.LogInformation("Domain created successfully.");
                return CreateResponse(domain, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No domain created");
        }
        #endregion
        
        #region GetAll
        /// <summary>
        /// Gets the Domain details
        /// </summary>
        /// <returns></returns>
        public async Task<List<Domain>> GetAll(bool isActive = true) =>
                       await m_AdminContext.Domains.Where(dm => dm.IsActive == isActive).OrderBy(x => x.DomainName).ToListAsync();
        #endregion
        
        #region Update
        /// <summary>
        /// Updates the Domain details
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public async Task<dynamic> Update(Domain domainIn)
        {
            int isUpdated;

            m_Logger.LogInformation("Calling UpdateDomain method in DomainService");

            //Checking if  already exists
            var isExists = m_AdminContext.Domains.Where(p => p.DomainName.ToLower().Trim() == domainIn.DomainName.ToLower().Trim()
                                            && p.DomainID != domainIn.DomainID).Count();

            if (isExists > 0)
                return CreateResponse(null, false, "Domain Name already exists");

            //Fetch domain for update
            var domain = m_AdminContext.Domains.Find(domainIn.DomainID);

            if (domain == null)
                return CreateResponse(null, false, "Domain not found for update");

            if (!domainIn.IsActive.HasValue)
                domainIn.IsActive = domain.IsActive;

            domainIn.CreatedBy = domain.CreatedBy;
            domainIn.CreatedDate = domain.CreatedDate;

            //update fields
            m_mapper.Map<Domain, Domain>(domainIn, domain);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in DomainService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("Updating CreateDomain record in DomainService");
                return CreateResponse(domain, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated");
        }
        #endregion

        #region GetByDomainId
        /// <summary>
        /// Gets the Domain by Id
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public async Task<Domain> GetByDomainId(int domainId) =>
                        await m_AdminContext.Domains.Where(pa => pa.DomainID == domainId)
                        .FirstOrDefaultAsync();

        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(Domain domain, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in DomainService");

            dynamic response = new ExpandoObject();
            response.Domain = domain;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in DomainService");

            return response;
        }

        #endregion
    }


}
