using HRMS.KRA.Infrastructure.Models.Domain;
using HRMS.KRA.Infrastructure.Response;
using HRMS.KRA.Types;
using HRMS.KRA.Types.External;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.KRA.Service
{
    public class RoleTypeService : IRoleTypeService
    {
        #region Global Variables
        private readonly ILogger<RoleTypeService> m_Logger;
        private readonly IOrganizationService m_OrgService;
        #endregion

        #region Constructor
        public RoleTypeService(ILogger<RoleTypeService> logger, IOrganizationService orgService)
        {
            m_Logger = logger;
            m_OrgService = orgService;
        }
        #endregion

        #region GetRoleTypesByGrade
        /// <summary>
        /// Get RoleTypes by Grade 
        /// </summary>
        /// <param name="gradeId">userName</param>
        /// <returns></returns>
        public async Task<ServiceListResponse<RoleType>> GetRoleTypesByGradeIdAsync(int gradeId)
        {
            var response = new ServiceListResponse<RoleType>();
            try
            {
                var roleTypes = await m_OrgService.GetAllRoleTypesAsync();
                
                if (!roleTypes.IsSuccessful)
                {
                    response.IsSuccessful = false;
                    response.Message = roleTypes.Message;
                    return response;
                }

                var roleTypesByGrade = (from rtype in roleTypes.Items
                                        //where rtype.GradeId == gradeId
                                        select new RoleType
                                        {
                                            RoleTypeId = rtype.RoleTypeId,
                                            RoleTypeName = rtype.RoleTypeName,
                                            //GradeId = rtype.GradeId
                                        }).ToList();

                response.Items = roleTypesByGrade;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the Role Types data";
                m_Logger.LogError(ex.StackTrace);
            }
            return response;
        }

        #endregion
    }
}
