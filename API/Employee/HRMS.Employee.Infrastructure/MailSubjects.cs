using System;

namespace HRMS.Employee.Infrastructure
{
    public class AssociateExitMailSubjects
    {
        public string ResignationSubmittedNotification { get; set; }
        public string ResignationReviewedByPMNotification { get; set; }
        public string ResignationApprovedNotification { get; set; }
        public string KTPlanSubmittedNotification { get; set; }
        public string KTPlanInProgressNotification { get; set; }
        public string KTPlanCompletedNotification { get; set; }
        public string ActivitiesInProgressNotification { get; set; }
        public string ClearanceGivenNotification { get; set; }
        public string ResignedNotification { get; set; }
        public string ResignationRevokedNotification { get; set; }
        public string ActivitiesCompletedByDepartmentNotification { get; set; }
        public string FeedbackGivenNotification { get; set; }
        public string ResignationRevokeInitiated { get; set; }
        public string ResignationRevokeRejected { get; set; }
        public string RequestForWithdrawResignation { get; set; }
        public string ITReturnNotification { get; set; }
        public string ExitDateUpdated { get; set; }
        public string ReviewReminder { get; set; }
        public string MarkedForAbscond { get; set; }
        public string AcknowledgeAbscond { get; set; }
        public string ConfirmAbscond { get; set; }
        public string DisproveAbscond { get; set; }
        public string Absconded { get; set; }
    }
}
