using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class ProjectClosureReportDetails
    {

        /// <summary>
        /// ProjectClosureId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// ClientFeedback
        /// </summary>
        public string ClientFeedback { get; set; }
        /// <summary>
        /// DeliveryPerformance
        /// </summary>
        public string DeliveryPerformance { get; set; }
        
        /// <summary>
        /// ValueDelivered
        /// </summary>
        public string ValueDelivered { get; set; }
        /// <summary>
        /// ManagementChallenges
        /// </summary>
        public string ManagementChallenges { get; set; }
        /// <summary>
        /// TechnologyChallenges
        /// </summary>
        public string TechnologyChallenges { get; set; }
        /// <summary>
        /// EngineeringChallenges
        /// </summary>
        public string EngineeringChallenges { get; set; }
        /// <summary>
        /// BestPractices
        /// </summary>
        public string BestPractices { get; set; }
        /// <summary>
        /// LessonsLearnt
        /// </summary>
        public string LessonsLearned { get; set; }
        /// <summary>
        /// ReusableArticrafts
        /// </summary>
        public string ReusableArtifacts { get; set; }
        /// <summary>
        /// ProcessImprovements
        /// </summary>
        public string ProcessImprovements { get; set; }
        /// <summary>
        /// Awards
        /// </summary>
        public string Awards { get; set; }
        /// <summary>
        /// NewTechSkills
        /// </summary>
        public string NewTechnicalSkills { get; set; }
        /// <summary>
        /// NewTools
        /// </summary>
        public string NewTools { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// CaseStudy
        /// </summary>
        public string CaseStudy { get; set; }
        /// <summary>
        /// StatusID
        /// </summary>
        public int? StatusId { get; set; }
        /// <summary>
        /// ClientFeedback
        /// </summary>
        public string ClientFeedbackFile { get; set; }
        /// <summary>
        /// DeliveryPerformance
        /// </summary>
        public string DeliveryPerformanceFile { get; set; }
        /// <summary>
        /// RejectRemarks
        /// </summary>
        public string RejectRemarks { get; set; }

    }
}
