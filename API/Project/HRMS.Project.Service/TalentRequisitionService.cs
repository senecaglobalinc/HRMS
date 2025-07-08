using AutoMapper;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class TalentRequisitionService : ITalentRequisitionService
    {

        #region Global Varibles

        private readonly ILogger<TalentPoolService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;

        #endregion

        #region TalentRequisitionService
        public TalentRequisitionService(ILogger<TalentPoolService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TalentRequisition, TalentRequisition>();
            });
            m_mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints.Value;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the TalentRequisition
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<TalentRequisition>> GetAll()
        {
            ServiceListResponse<TalentRequisition> response;
            var talent_requisition_list = await m_ProjectContext.TalentRequisition.ToListAsync();
            if (talent_requisition_list == null || talent_requisition_list.Count == 0)
            {
                return response = new ServiceListResponse<TalentRequisition>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Talent Requisition found.."
                };
            }
            else {
                return response = new ServiceListResponse<TalentRequisition>()
                {
                    Items = talent_requisition_list,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
        }

        #endregion

        #region GetById
        /// <summary>
        /// Get TalentRequisition by id
        /// </summary>
        /// <param name="id">TalentRequisition Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<TalentRequisition>> GetById(int id)
        {
            ServiceResponse<TalentRequisition> response;
            if (id == 0) {
                return response = new ServiceResponse<TalentRequisition>() {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            var talent_requisition = await m_ProjectContext.TalentRequisition.FindAsync(id);
            if (talent_requisition == null)
            {
                return response = new ServiceResponse<TalentRequisition>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "No Talent Requisition found with this Id.."
                };
            }
            else {
                return response = new ServiceResponse<TalentRequisition>()
                {
                    Item = talent_requisition,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
        }

        #endregion
    }
}
