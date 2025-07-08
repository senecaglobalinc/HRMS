using AutoMapper;
using HRMS.Project.Database;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure;
using HRMS.Project.Infrastructure.Models.Response;
using HRMS.Project.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace HRMS.Project.Service
{
    public class AllocationPercentageService : IAllocationPercentageService
    {
        #region Global Varibles

        private readonly ILogger<AllocationPercentageService> m_Logger;
        private readonly ProjectDBContext m_ProjectContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;

        #endregion

        #region AllocationPercentageService
        public AllocationPercentageService(ILogger<AllocationPercentageService> logger,
            ProjectDBContext projectContext,
            IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_ProjectContext = projectContext;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AllocationPercentage, AllocationPercentage>();
            });
            m_mapper = config.CreateMapper();

            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints != null ? apiEndPoints.Value : null;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get the all the AllocationPercentage
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<AllocationPercentage>> GetAll()
        {
            ServiceListResponse<AllocationPercentage> response;
            var obj = await m_ProjectContext.AllocationPercentage.OrderBy(x => x.Percentage).ToListAsync();
            if (obj == null || obj.Count == 0)
            {
                response = new ServiceListResponse<AllocationPercentage>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = "No Allocation Percentages found.."
                };
            }
            else
            {
                response = new ServiceListResponse<AllocationPercentage>()
                {
                    Items = obj,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            return response;
        }

        #endregion

        #region GetAllocationPercentageForDropdown
        /// <summary>
        /// GetAllocationPercentageForDropdown
        /// </summary>        
        /// <returns></returns>       
        public async Task<ServiceListResponse<GenericType>> GetAllocationPercentageForDropdown()
        {
            var response = new ServiceListResponse<GenericType>();
            try
            {
                List<GenericType> list = await m_ProjectContext.AllocationPercentage
                .OrderBy(c => c.Percentage)
                .Select(c => new GenericType
                {
                    Id = c.AllocationPercentageId,
                    Name = c.Percentage.ToString()
                }).ToListAsync();

                response.Items = list;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the data";
                m_Logger.LogError("Error occured in GetProgramManagersForDropdown() method" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region GetById
        /// <summary>
        /// Get AllocationPercentage by id
        /// </summary>
        /// <param name="id">AllocationPercentage Id</param>
        /// <returns></returns>
        public async Task<ServiceResponse<AllocationPercentage>> GetById(int id)
        {
            ServiceResponse<AllocationPercentage> response;
            if (id == 0)
            {
                response = new ServiceResponse<AllocationPercentage>()
                {
                    Item = null,
                    IsSuccessful = false,
                    Message = "Invalid Request.."
                };
            }
            else
            {
                var obj = await m_ProjectContext.AllocationPercentage.FindAsync(id);
                if (obj == null)
                {
                    response = new ServiceResponse<AllocationPercentage>()
                    {
                        Item = null,
                        IsSuccessful = false,
                        Message = "No Allocation Percentage is found.."
                    };
                }
                else
                {
                    response = new ServiceResponse<AllocationPercentage>()
                    {
                        Item = obj,
                        IsSuccessful = true,
                        Message = ""
                    };
                }
            }
            return response;
        }

        #endregion
    }
}
