using HRMS.Project.Types;
using HRMS.Project.API.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace HRMS.Project.API.Controllers
{
    [Route("project/api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociateSkillAssessmentController : Controller
    {
        #region Global Variables

        private readonly IAssociateSkillAssessmentService m_AssociateSkillAssessment;
        private readonly ILogger<AssociateSkillAssessmentController> m_Logger;

        #endregion

        #region Constructor
        public AssociateSkillAssessmentController(IAssociateSkillAssessmentService associateSkillAssessmentService, ILogger<AssociateSkillAssessmentController> logger)
        {
            m_AssociateSkillAssessment = associateSkillAssessmentService;
            m_Logger = logger;
        }
        #endregion

    }
}
