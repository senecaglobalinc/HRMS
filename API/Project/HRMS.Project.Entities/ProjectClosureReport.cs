namespace HRMS.Project.Entities
{
    public class ProjectClosureReport : BaseEntity
    {
        /// <summary>
        /// ProjectClosureReportId
        /// </summary>
        public int ProjectClosureReportId { get; set; }

        /// <summary>
        /// ProjectClosureId
        /// </summary>
        public int ProjectClosureId { get; set; }

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
        /// LessonsLearned
        /// </summary>
        public string LessonsLearned { get; set; }

        /// <summary>
        /// ReusableArtifacts
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
        /// NewTechnicalSkills
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
        /// ClientFeedbackFile
        /// </summary>
        public string ClientFeedbackFile { get; set; }
        /// <summary>
        /// DeliveryPerformanceFile
        /// </summary>
        public string DeliveryPerformanceFile { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        public int? StatusId { get; set; }
        /// <summary>
        /// RejectRemarks
        /// </summary>
        public string RejectRemarks { get; set; }

        public ProjectClosure projectClosure;
    }

}
