using AutoMapper;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Project.Service
{
    public class TalentPoolService : ITalentPoolService
    {
        #region Global Varibles

        private readonly ILogger<TalentPoolService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;

        #endregion

        #region TalentPoolService
        public TalentPoolService(ILogger<TalentPoolService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TalentPool, TalentPool>();
            });
            m_mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints.Value;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the TalentPool
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<TalentPool>> GetAll()
        {
            ServiceListResponse<TalentPool> response;
            var talent_pool_list = await m_ProjectContext.TalentPool.Where(w => w.IsActive == true).Select(x => x).ToListAsync();
            if (talent_pool_list == null || talent_pool_list.Count == 0)
            {
                return response = new ServiceListResponse<TalentPool>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Talent Pools found.."
                };
            }
            else
            {
                return response = new ServiceListResponse<TalentPool>()
                {
                    Items = talent_pool_list,
                    IsSuccessful = true,
                    Message = string.Empty
                };
            }
        }

        #endregion

        #region GetById
        /// <summary>
        /// Get TalentPool by id
        /// </summary>
        /// <param name="id">TalentPool Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<TalentPool>> GetById(int id)
        {
            ServiceResponse<TalentPool> response;
            if (id == 0)
            {
                return response = new ServiceResponse<TalentPool>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            else
            {
                var talent_pool = await m_ProjectContext.TalentPool.FindAsync(id);
                if (talent_pool == null)
                {
                    return response = new ServiceResponse<TalentPool>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = "No Talent Pool found with this Id.."
                    };
                }
                else
                {
                    return response = new ServiceResponse<TalentPool>()
                    {
                        Item = talent_pool,
                        IsSuccessful = true,
                        Message = string.Empty
                    };
                }
            }
        }

        #endregion

        #region Create
        /// <summary>
        /// Create TalentPool
        /// </summary>
        /// <param name="projectId">projectId</param>
        /// <param name="practiceAreaId">practiceAreaId</param>
        /// <returns></returns>

        public async Task<ServiceResponse<int>> Create(int projectId, int practiceAreaId)
        {
            ServiceResponse<int> response;
            try
            {


                if (projectId == 0 || practiceAreaId == 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        IsSuccessful = false,
                        Item = 0,
                        Message = "Invalid Request.."
                    };
                }
                TalentPool talentPool = new TalentPool();
                talentPool.PracticeAreaId = practiceAreaId;
                talentPool.ProjectId = projectId;
                talentPool.IsActive = true;
                m_ProjectContext.TalentPool.Add(talentPool);
                int isCreated = await m_ProjectContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    return response = new ServiceResponse<int>()
                    {
                        IsSuccessful = true,
                        Item = 1,
                        Message = "Talent pool Added Successfully"
                    };
                }

                return response = new ServiceResponse<int>()
                {
                    IsSuccessful = false,
                    Item = 0,
                    Message = "Error Occured While Adding TalentPool"
                };
            }
            catch (Exception ex)
            {
                return response = new ServiceResponse<int>()
                {
                    IsSuccessful = false,
                    Item = 0,
                    Message = "Error Occured While Updating Database"
                };
            }

        }
        #endregion
    }
}
