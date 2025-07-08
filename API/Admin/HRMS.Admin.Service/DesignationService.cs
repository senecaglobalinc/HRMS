using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class DesignationService : IDesignationService
    {
        #region Global Varible

        private readonly ILogger<DesignationService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;

        #endregion

        #region Constructor
        public DesignationService(ILogger<DesignationService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Designation, Designation>();
            });
            m_mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// Create designation
        /// </summary>
        /// <param name="designationIn">designationDetails information</param>
        /// <returns></returns>
        public async Task<dynamic> Create(Designation designationIn)
        {
            int isCreated;
            m_Logger.LogInformation("Calling CreateDesignation method in DesignationService");
            Designation designationAlreadyExits =
               await GetByCode(designationIn.DesignationCode);

            //designation code already exists?
            if (designationAlreadyExits != null)
                return CreateResponse(null, false, "designation code already exists.");

            //Checking if Designation Name already exists
            var isExistsName = m_AdminContext.Designations.Where(d => d.DesignationName.ToLower().Trim() == designationIn.DesignationName.ToLower().Trim()).Count();

            if (isExistsName > 0)
                return CreateResponse(null, false, "Designation Name already exists");

            Designation designation = new Designation();

            if (!designationIn.IsActive.HasValue)
                designationIn.IsActive = true;

            m_mapper.Map<Designation, Designation>(designationIn, designation);

            m_AdminContext.Designations.Add(designation);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in DesignationService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
                return CreateResponse(designation, true, string.Empty);
            else
                return CreateResponse(null, false, "No designation created.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the designation details
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<Designation>> GetAll(bool? isActive)
        {
            return await (from dc in m_AdminContext.Designations
                          join gr in m_AdminContext.Grades on dc.GradeId equals gr.GradeId
                          where dc.IsActive == true && gr.IsActive == true
                          select new Designation
                          {
                              DesignationId = dc.DesignationId,
                              DesignationCode = dc.DesignationCode,
                              DesignationName = dc.DesignationName,
                              IsActive = dc.IsActive,
                              GradeId = gr.GradeId,
                              Grade = gr
                          }).OrderBy(x => x.DesignationName).ToListAsync();
        }
        #endregion       

        #region GetBySearchString
        /// <summary>
        ///GetBySearchString
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<List<Designation>> GetBySearchString(string searchString)
        {
            return await (from dc in m_AdminContext.Designations
                          where dc.DesignationName.ToLower().Contains(searchString.ToLower()) && dc.IsActive == true
                          select new Designation
                          {
                              DesignationId = dc.DesignationId,
                              DesignationName = dc.DesignationName
                          }).ToListAsync();
        }
        #endregion

        #region GetDesignationsForDropdown
        /// <summary>
        ///GetDesignationsForDropdown
        /// </summary>

        /// <returns></returns>
        public async Task<List<GenericType>> GetDesignationsForDropdown()
        {
            return await (from dc in m_AdminContext.Designations
                          where dc.IsActive == true
                          select new GenericType
                          {
                              Id = dc.DesignationId,
                              Name = dc.DesignationName
                          }).OrderBy(c => c.Name).ToListAsync();
        }
        #endregion

        #region GetByCode
        /// <summary>
        /// Get designation code
        /// </summary>
        /// <param name="designationCode">designation code</param>
        public async Task<Designation> GetByCode(string designationCode) =>
                        await m_AdminContext.Designations.Where(dc => dc.DesignationCode.ToLower().Trim() == designationCode.ToLower().Trim())
                        .FirstOrDefaultAsync();

        #endregion

        #region GetById
        /// <summary>
        /// Get designation by id
        /// </summary>
        /// <param name="designationId"></param>
        /// <returns></returns>
        public async Task<Designation> GetById(int designationId) =>
                        await m_AdminContext.Designations.Where(dc => dc.DesignationId == designationId)
                        .FirstOrDefaultAsync();

        #endregion

        #region Update
        /// <summary>
        /// Updates the designation information
        /// </summary>
        /// <param name="designationIn">designationDetails information</param>
        /// <returns></returns>
        public async Task<dynamic> Update(Designation designationIn)
        {
            int isUpdated;
            m_Logger.LogInformation("Calling UpdateGrade method in DesignationService");

            var designation = m_AdminContext.Designations.Find(designationIn.DesignationId);
            if (designation == null)
                return CreateResponse(null, false, "Designation not found for update.");

            Designation DesignationAlreadyExits =
                await GetByCode(designationIn.DesignationCode);

            if (DesignationAlreadyExits != null &&
                DesignationAlreadyExits.DesignationId != designation.DesignationId)
                return CreateResponse(null, false, "Designation code already exists");

            //Checking if Designation Name already exists
            var isExistsName = m_AdminContext.Designations.Where(d => d.DesignationName.ToLower().Trim() == designationIn.DesignationName.ToLower().Trim()).FirstOrDefault();

            if (isExistsName != null && isExistsName.DesignationId != designation.DesignationId)
                return CreateResponse(null, false, "Designation Name already exists");

            if (!designationIn.IsActive.HasValue)
                designationIn.IsActive = designation.IsActive;

            designationIn.CreatedBy = designation.CreatedBy;
            designationIn.CreatedDate = designation.CreatedDate;

            m_mapper.Map<Designation, Designation>(designationIn, designation);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in DesignationService");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
                return CreateResponse(designation, true, string.Empty);
            else
                return CreateResponse(null, false, "No record updated.");
        }
        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="designation"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(Designation designation, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in DesignationService");

            dynamic response = new ExpandoObject();
            response.Designation = designation;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in DesignationService");

            return response;
        }

        #endregion
    }
}
