using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HRMS.Admin.Infrastructure.Models.Domain;

namespace HRMS.Admin.Service
{
    public class GradeService : IGradeService
    {
        #region Global Variables

        private readonly ILogger<GradeService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;

        #endregion

        #region UserGradeService
        public GradeService(ILogger<GradeService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Grade, Grade>();
            });
            m_mapper = config.CreateMapper();

        }
        #endregion

        #region Create
        /// <summary>
        /// Create grade
        /// </summary>
        /// <param name="gradeIn">GradeDetails information</param>
        /// <returns></returns>
        public async Task<dynamic> Create(Grade gradeIn)
        {
            int isCreated;

            m_Logger.LogInformation("Calling CreateGrade method in UserGradeService");

            Grade gradeAlreadyExits = await GetByCode(gradeIn.GradeCode);

            if (gradeAlreadyExits != null)
                return CreateResponse(null, false, "Grade code already exists.");

            Grade grade = new Grade();

            if (!gradeIn.IsActive.HasValue)
                gradeIn.IsActive = true;

            m_mapper.Map<Grade, Grade>(gradeIn, grade);

            m_AdminContext.Grades.Add(grade);

            m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in UserGradeService");
            isCreated = await m_AdminContext.SaveChangesAsync();

            if (isCreated > 0)
                return CreateResponse(grade, true, string.Empty);
            else
                return CreateResponse(null, false, "No grade created.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Gets the grade details
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns></returns>
        public async Task<List<Grade>> GetAll(bool? isActive = true) =>
                        await m_AdminContext.Grades.Where(gr => gr.IsActive == isActive).OrderBy(x => x.GradeCode).ToListAsync();
        #endregion

        #region GetGradesForDropdown
        /// <summary>
        /// Gets the Grade details
        /// </summary>        
        /// <returns></returns>
        public async Task<List<GenericType>> GetGradesForDropdown() =>
                        await m_AdminContext.Grades.Where(cl => cl.IsActive == true).Select(ci => new GenericType { Id = ci.GradeId, Name = ci.GradeName }).OrderBy(x => x.Name).ToListAsync();

        #endregion

        #region GetByCode
        /// <summary>
        /// Get grade code
        /// </summary>
        /// <param name="gradeCode">grade code</param>
        public async Task<Grade> GetByCode(string gradeCode) =>
                        await m_AdminContext.Grades.Where(gr => gr.GradeCode == gradeCode)
                        .FirstOrDefaultAsync();

        #endregion

        #region GetById
        /// <summary>
        /// Get grade by id
        /// </summary>
        /// <param name="gradeId">Grade Id</param>
        /// <returns></returns>
        public async Task<Grade> GetById(int gradeId) =>
                        await m_AdminContext.Grades.FindAsync(gradeId);

        #endregion

        #region GetById
        /// <summary>
        /// Get grade by id
        /// </summary>
        /// <param name="gradeId">Grade Id</param>
        /// <returns></returns>
        public async Task<Grade> GetGradeByDesignation(int designationId)
        {

            return await (from gr in m_AdminContext.Grades
                          join desg in m_AdminContext.Designations
on gr.GradeId equals desg.GradeId
                          where desg.DesignationId == designationId
                          select new Grade
                          {
                              GradeId = gr.GradeId,
                              GradeCode = gr.GradeCode,
                              GradeName = gr.GradeName
                          }).FirstOrDefaultAsync();
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the grade information
        /// </summary>
        /// <param name="gradeIn">gradeDetails information</param>
        /// <returns></returns>
        public async Task<dynamic> Update(Grade gradeIn)
        {
            int Updated;
            m_Logger.LogInformation("Calling UpdateGrade method in UserGradeService");

            var grade = m_AdminContext.Grades.Find(gradeIn.GradeId);

            if (grade == null)
                return CreateResponse(null, false, "Grade not found for update.");

            Grade gradeAlreadyExits = await GetByCode(gradeIn.GradeCode);

            if (gradeAlreadyExits != null && gradeAlreadyExits.GradeId != grade.GradeId)
                return CreateResponse(null, false, "Grade code already exists.");

            if (!gradeIn.IsActive.HasValue)
                gradeIn.IsActive = grade.IsActive;

            gradeIn.CreatedBy = grade.CreatedBy;
            gradeIn.CreatedDate = grade.CreatedDate;

            m_mapper.Map<Grade, Grade>(gradeIn, grade);

            m_Logger.LogInformation("Calling SaveChanges method on DB Context in UserGradeService");
            Updated = await m_AdminContext.SaveChangesAsync();

            if (Updated > 0)
                return CreateResponse(grade, true, string.Empty);
            else
                return CreateResponse(null, false, "No record updated.");
        }

        #endregion

        //Private Method

        #region CreateResponse
        /// <summary>
        /// this method creates response object.
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        /// <returns>Boolean</returns>
        private dynamic CreateResponse(Grade grade, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("Calling CreateResponse method in GradeService");

            dynamic response = new ExpandoObject();
            response.Grade = grade;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("Response object created in GradeService");

            return response;
        }

        #endregion
    }
}
