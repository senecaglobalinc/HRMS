using HRMS.Project.Database;
using HRMS.Project.Types;
using HRMS.Project.Types.External;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Service
{
    /// <summary>
    /// Service class to get the Associate Skill details
    /// </summary>
    public class AssociateSkillAssessmentService : IAssociateSkillAssessmentService
    {
        #region Global Varibles
        private readonly ProjectDBContext _projectDBContext;
        private readonly IProjectService _projectService;
        private readonly IOrganizationService _organizationService;
        private readonly ILogger<AssociateSkillAssessmentService> _logger;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public AssociateSkillAssessmentService(ProjectDBContext projectDBContext,
                                  IProjectService projectService,
                                  IOrganizationService organizationService,
                                  ILogger<AssociateSkillAssessmentService> logger,
                                  IConfiguration configuration
                                  )
        {
            _projectDBContext = projectDBContext;
            _projectService = projectService;
            _organizationService = organizationService;
            _logger = logger;
            _configuration = configuration;
        }
        #endregion
    }
}
