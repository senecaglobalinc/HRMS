using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class ReportService : IReportService
    {
        #region Global Variables

        private readonly ILogger<ReportService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        private APIEndPoints m_apiEndPoints;

        #endregion

        #region Constructor
        public ReportService(ILogger<ReportService> logger, AdminContext adminContext, IHttpClientFactory clientFactory,
            IOptions<APIEndPoints> apiEndPoints)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, User>();
            });
            m_mapper = config.CreateMapper();
            m_clientFactory = clientFactory;
            m_apiEndPoints = apiEndPoints?.Value;
        }
        #endregion

        #region GetFinanceReportMasters
        /// <summary>
        /// GetFinanceReportMasters
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ReportDetails>> GetFinanceReportMasters()
        {
            ServiceListResponse<ReportDetails> response = new ServiceListResponse<ReportDetails>();

            try
            {
                List<ReportDetails> masterDetails = new List<ReportDetails>();              

                masterDetails.AddRange(await m_AdminContext.Clients
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.ClientId,
                                            Name = c.ClientName,
                                            RecordType = (int)RecordTypeEnum.Client
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Departments
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.DepartmentId,
                                            Name = c.Description,
                                            RecordType = (int)RecordTypeEnum.Department
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Grades
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.GradeId,
                                            Name = c.GradeName,
                                            RecordType = (int)RecordTypeEnum.Grade
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Designations
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.DesignationId,
                                            Name = c.DesignationName,
                                            RecordType = (int)RecordTypeEnum.Designation
                                        })
                                        .ToListAsync());


                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = masterDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion

        #region GetUtilizationReportMasters
        /// <summary>
        /// GetUtilizationReportMasters
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ReportDetails>> GetUtilizationReportMasters()
        {
            ServiceListResponse<ReportDetails> response = new ServiceListResponse<ReportDetails>();

            try
            {
                List<ReportDetails> masterDetails = new List<ReportDetails>();

                masterDetails.AddRange(await m_AdminContext.Clients
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.ClientId,
                                            Name = c.ClientName,
                                            RecordType = (int)RecordTypeEnum.Client
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Departments
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.DepartmentId,
                                            Name = c.Description,
                                            RecordType = (int)RecordTypeEnum.Department
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Grades
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.GradeId,
                                            Name = c.GradeName,
                                            RecordType = (int)RecordTypeEnum.Grade
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Designations
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.DesignationId,
                                            Name = c.DesignationName,
                                            RecordType = (int)RecordTypeEnum.Designation
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.PracticeAreas
                                       .Where(c => c.IsActive == true)
                                       .Select(c => new ReportDetails
                                       {
                                           Id = c.PracticeAreaId,
                                           Name = c.PracticeAreaDescription,
                                           RecordType = (int)RecordTypeEnum.PracticeArea
                                       })
                                       .ToListAsync());

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = masterDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion

        #region GetDomainReportMasters
        /// <summary>
        /// GetDomainReportMasters
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ReportDetails>> GetDomainReportMasters()
        {
            ServiceListResponse<ReportDetails> response = new ServiceListResponse<ReportDetails>();

            try
            {
                List<ReportDetails> masterDetails = new List<ReportDetails>();

                masterDetails.AddRange(await m_AdminContext.Domains
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.DomainID,
                                            Name = c.DomainName,
                                            RecordType = (int)RecordTypeEnum.Domain
                                        })
                                        .ToListAsync());                

                masterDetails.AddRange(await m_AdminContext.Grades
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.GradeId,
                                            Name = c.GradeName,
                                            RecordType = (int)RecordTypeEnum.Grade
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Designations
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.DesignationId,
                                            Name = c.DesignationName,
                                            RecordType = (int)RecordTypeEnum.Designation
                                        })
                                        .ToListAsync());               

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = masterDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion

        #region GetTalentPoolReportMasters
        /// <summary>
        /// GetTalentPoolReportMasters
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ReportDetails>> GetTalentPoolReportMasters()
        {
            ServiceListResponse<ReportDetails> response = new ServiceListResponse<ReportDetails>();

            try
            {
                List<ReportDetails> masterDetails = new List<ReportDetails>();

                masterDetails.AddRange(await m_AdminContext.ProjectTypes
                                        .Where(c => c.IsActive == true && c.ProjectTypeCode == "Talent Pool" || c.ProjectTypeCode=="Training" )
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.ProjectTypeId,
                                            Name = c.Description,
                                            RecordType = (int)RecordTypeEnum.ProjectType
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Grades
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.GradeId,
                                            Name = c.GradeName,
                                            RecordType = (int)RecordTypeEnum.Grade
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Designations
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.DesignationId,
                                            Name = c.DesignationName,
                                            RecordType = (int)RecordTypeEnum.Designation
                                        })
                                        .ToListAsync());

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = masterDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion

        #region GetSkillSearchMasters
        /// <summary>
        /// GetSkillSearchMasters
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ReportDetails>> GetSkillSearchMasters()
        {
            ServiceListResponse<ReportDetails> response = new ServiceListResponse<ReportDetails>();

            try
            {
                List<ReportDetails> masterDetails = new List<ReportDetails>();         

                masterDetails.AddRange(await m_AdminContext.Grades
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.GradeId,
                                            Name = c.GradeName,
                                            RecordType = (int)RecordTypeEnum.Grade
                                        })
                                        .ToListAsync());

                masterDetails.AddRange(await m_AdminContext.Designations
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.DesignationId,
                                            Name = c.DesignationName,
                                            RecordType = (int)RecordTypeEnum.Designation
                                        })
                                        .ToListAsync());


                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = masterDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion

        #region GetSkillsBySearchString
        /// <summary>
        /// Get Skills By SearchString 
        /// </summary>
        /// <param name="SearchString"></param>      
        /// <returns></returns>
        public async Task<ServiceListResponse<GenericType>> GetSkillsBySearchString(string searchString)
        {
            ServiceListResponse<GenericType> response = new ServiceListResponse<GenericType>();

            try
            {
                List<GenericType> masterDetails = new List<GenericType>();

                masterDetails.AddRange(await m_AdminContext.Skills
                                        .Where(c => c.IsActive == true && c.SkillName.IndexOf(searchString) >= 0)
                                        .Select(c => new GenericType
                                        {
                                            Id = c.SkillId,
                                            Name = c.SkillName                                            
                                        }).OrderBy(s => s.Name)
                                        .ToListAsync());               


                return response = new ServiceListResponse<GenericType>()
                {
                    Items = masterDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<GenericType>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }


        }
        #endregion

        #region GetAssociateAllocationMasters
        /// <summary>
        /// GetAssociateAllocationMasters
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<ReportDetails>> GetAssociateAllocationMasters()
        {
            ServiceListResponse<ReportDetails> response = new ServiceListResponse<ReportDetails>();

            try
            {
                List<ReportDetails> masterDetails = new List<ReportDetails>();
                List<string> statusCodes = new List<string>() { "created", "execution" };
                masterDetails = (m_AdminContext.Clients
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.ClientId,
                                            Name = c.ClientName,
                                            RecordType = (int)RecordTypeEnum.Client
                                        })).AsEnumerable().Union(m_AdminContext.Departments
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.DepartmentId,
                                            Name = c.Description,
                                            RecordType = (int)RecordTypeEnum.Department
                                        })).AsEnumerable().Union(m_AdminContext.ProjectTypes
                                        .Where(c => c.IsActive == true)
                                        .Select(c => new ReportDetails
                                        {
                                            Id = c.ProjectTypeId,
                                            Name = c.Description,
                                            RecordType = (int)RecordTypeEnum.ProjectType
                                        })).AsEnumerable().Union(m_AdminContext.PracticeAreas
                                       .Where(c => c.IsActive == true)
                                       .Select(c => new ReportDetails
                                       {
                                           Id = c.PracticeAreaId,
                                           Name = c.PracticeAreaCode,
                                           RecordType = (int)RecordTypeEnum.PracticeArea
                                       })).AsEnumerable().Union(from Status in m_AdminContext.Statuses
                                               join Category in m_AdminContext.Categories on Status.CategoryMasterId equals Category.CategoryMasterId
                                               where statusCodes.Contains(Status.StatusCode.ToLower().Trim())
                                                     && Category.CategoryName.ToLower().Trim() == "PPC".ToLower().Trim()
                                                     && Status.IsActive == true
                                               select new ReportDetails
                                               {
                                                   Id = Status.StatusId,
                                                   Name = Status.StatusCode,
                                                   RecordType = (int)RecordTypeEnum.ProjectStatus
                                               }).ToList();

                //masterDetails.AddRange(await m_AdminContext.Clients
                //                        .Where(c => c.IsActive == true)
                //                        .Select(c => new ReportDetails
                //                        {
                //                            Id = c.ClientId,
                //                            Name = c.ClientName,
                //                            RecordType = (int)RecordTypeEnum.Client
                //                        })
                //                        .ToListAsync());

                //masterDetails.AddRange(await m_AdminContext.Departments
                //                        .Where(c => c.IsActive == true)
                //                        .Select(c => new ReportDetails
                //                        {
                //                            Id = c.DepartmentId,
                //                            Name = c.Description,
                //                            RecordType = (int)RecordTypeEnum.Department
                //                        })
                //                        .ToListAsync());

                //masterDetails.AddRange(await m_AdminContext.ProjectTypes
                //                        .Where(c => c.IsActive == true)
                //                        .Select(c => new ReportDetails
                //                        {
                //                            Id = c.ProjectTypeId,
                //                            Name = c.Description,
                //                            RecordType = (int)RecordTypeEnum.ProjectType
                //                        })
                //                        .ToListAsync());                

                

                //masterDetails.AddRange(await (from Status in m_AdminContext.Statuses
                // join Category in m_AdminContext.Categories on Status.CategoryMasterId equals Category.CategoryMasterId
                // where statusCodes.Contains(Status.StatusCode.ToLower().Trim())
                //       && Category.CategoryName.ToLower().Trim() == "PPC".ToLower().Trim()
                //       && Status.IsActive == true
                // select new ReportDetails
                // {
                //     Id = Status.StatusId,
                //     Name = Status.StatusCode,
                //     RecordType = (int)RecordTypeEnum.ProjectStatus
                // }).ToListAsync());

                //masterDetails.AddRange(await m_AdminContext.PracticeAreas
                //                       .Where(c => c.IsActive == true)
                //                       .Select(c => new ReportDetails
                //                       {
                //                           Id = c.PracticeAreaId,
                //                           Name = c.PracticeAreaCode,
                //                           RecordType = (int)RecordTypeEnum.PracticeArea
                //                       })
                //                       .ToListAsync());

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = masterDetails,
                    IsSuccessful = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                //log the exception
                m_Logger.LogError(ex.Message);

                return response = new ServiceListResponse<ReportDetails>()
                {
                    Items = null,
                    IsSuccessful = false,
                    Message = ""
                };
            }
        }
        #endregion
    }
}
