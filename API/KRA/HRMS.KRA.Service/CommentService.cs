using HRMS.KRA.Database;
using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Response;
using HRMS.KRA.Types;
using HRMS.KRA.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.KRA.Service
{
    public class CommentService : ICommentService
    {
        #region Global Varibles
        private readonly ILogger<CommentService> m_Logger;
        private readonly KRAContext m_kraContext;
        //private readonly ICommentService m_commentService;
        private readonly IOrganizationService m_OrganizationService;
        #endregion

        #region CommentService
        /// <summary>
        /// CommentService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="kraContext"></param>
        /// <param name="commentService"></param>
        public CommentService(ILogger<CommentService> logger, KRAContext kraContext,
                                IOrganizationService organizationService)
        {
            m_Logger = logger;
            m_kraContext = kraContext;
            //m_commentService = commentService;
            m_OrganizationService = organizationService;
        }
        #endregion

        #region Create
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> Create(CommentModel model)
        {
            var response = new BaseServiceResponse();
            GradeRoleType gradeRoleType = null;
            //ApplicableRoleType applicableRoleType = null;
            Comment comment = null;
            bool isCreated = false;

            m_Logger.LogInformation("CommentService: Calling \"Create\" method.");
            try
            {
                if(model.IsCEO == false)
                {
                    var gradeRoleTypes = await m_OrganizationService.GetAllGradeRoleTypes();

                    if (!gradeRoleTypes.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = gradeRoleTypes.Message;
                        return response;
                    }
                    else
                    {
                        gradeRoleType = (from gr in gradeRoleTypes.Items
                                         where gr.GradeId == model.GradeId && gr.RoleTypeId == model.RoleTypeId
                                         select gr).FirstOrDefault();

                        if (gradeRoleType == null)
                        {
                            response.IsSuccessful = false;
                            response.Message = "GradeRoleType not found!";
                            return response;
                        }
                    }
                    applicableRoleType = m_kraContext.ApplicableRoleTypes.Where(x => x.FinancialYearId == model.FinancialYearId
                                                    && x.DepartmentId == model.DepartmentId && x.GradeRoleTypeId == gradeRoleType.GradeRoleTypeId).FirstOrDefault();
                    if (applicableRoleType == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "ApplicableRoleType not found.";
                        return response;
                    }
                }

                if (model.ApplicableRoleTypeId == 0)
                    model.ApplicableRoleTypeId = null;

                comment = new Comment()
                {
                    CommentText = model.CommentText,
                    FinancialYearId = model.FinancialYearId,
                    DepartmentId = model.DepartmentId,
                    ApplicableRoleTypeId = model.IsCEO == true ? model.ApplicableRoleTypeId : applicableRoleType.ApplicableRoleTypeId,
                    CreatedBy = model.Username
                };

                m_kraContext.Comments.Add(comment);
                isCreated = await m_kraContext.SaveChangesAsync() > 0 ? true : false;
                if (isCreated)
                {
                    response.IsSuccessful = true;
                    response.Message = "Comment created successfully!";
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Error occurred while creating a new comment record.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while creating a new Comment record.";
                m_Logger.LogError("Error occured in Create() method in CommentService " + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Get Comments By ApplicableRoleTypeId
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceListResponse<CommentModel>> GetAll(int financialYearId, int departmentId, int gradeId, int roleTypeId, bool isCEO)
        {
            var response = new ServiceListResponse<CommentModel>();
            List<CommentModel> lstComment = null;
            GradeRoleType gradeRoleType = null;
            string comment = string.Empty;
            try
            {
                m_Logger.LogInformation("CommentService: Calling \"GetAll\" method.");

                if (isCEO == false)
                {
                    var gradeRoleTypes = await m_OrganizationService.GetAllGradeRoleTypes();

                    if (!gradeRoleTypes.IsSuccessful)
                    {
                        response.IsSuccessful = false;
                        response.Message = gradeRoleTypes.Message;
                        return response;
                    }
                    else
                    {
                        gradeRoleType = (from gr in gradeRoleTypes.Items
                                         where gr.GradeId == gradeId && gr.RoleTypeId == roleTypeId
                                         select gr).FirstOrDefault();

                        if (gradeRoleType == null)
                        {
                            response.Items = null;
                            response.IsSuccessful = false;
                            response.Message = "GradeRoleType not found!";
                            return response;
                        }
                    }
                    var applicableRoleType = m_kraContext.ApplicableRoleTypes.Where(x => x.FinancialYearId == financialYearId
                                                    && x.DepartmentId == departmentId && x.GradeRoleTypeId == gradeRoleType.GradeRoleTypeId).FirstOrDefault();

                    if (applicableRoleType == null)
                    {
                        response.IsSuccessful = false;
                        response.Message = "ApplicableRoleType not found.";
                        return response;
                    }
                    lstComment = await (from c in m_kraContext.Comments
                                            where c.ApplicableRoleTypeId == applicableRoleType.ApplicableRoleTypeId
                                            orderby c.CreatedDate descending
                                            select new CommentModel
                                            {
                                                CommentID = c.CommentID,
                                                CommentText = c.CreatedBy + ":" + "\n" + c.CommentText
                                            }).ToListAsync();
                }
                else if (isCEO == true)
                {
                    lstComment = await (from c in m_kraContext.Comments
                                        where c.FinancialYearId == financialYearId && c.DepartmentId == departmentId
                                        && c.ApplicableRoleTypeId == null
                                        orderby c.CreatedDate descending
                                        select new CommentModel
                                        {
                                            CommentID = c.CommentID,
                                            CommentText = c.CreatedBy + ":" + "\n" + c.CommentText
                                        }).ToListAsync();
                }

                foreach(CommentModel cmt in lstComment)
                {
                    if (string.IsNullOrEmpty(comment))
                        comment = cmt.CommentText;
                    else comment = comment + "\n\n" + cmt.CommentText;
                }
                foreach (CommentModel cmt in lstComment)
                {
                    cmt.CommentText = comment.Replace("@senecaglobal.com", "");
                }
                response.Items = lstComment;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching comments";
                m_Logger.LogError("Error occured in GetAll() method." + ex.StackTrace);
            }
            return response;
        }
        #endregion
    }
}
