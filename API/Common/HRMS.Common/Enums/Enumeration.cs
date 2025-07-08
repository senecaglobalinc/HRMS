using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HRMS.Common.Enums
{
    #region CategoryMaster
    public enum CategoryMaster
    {
        TalentRequisition = 1,
        AssociateExit = 2,
        Skills = 3,
        EPC = 4,
        KRA = 5,
        ADR = 6,
        EPU = 7,
        ESU = 8,
        PPC = 7,
        LongLeave = 8,
        PPCApproved = 10,
    }
    #endregion

    #region TalentRequisitionStatus
    /// <summary>
    /// TalentRequisitionStatus
    /// </summary>
    public enum TalentRequisitionStatusCodes
    {
        LongLeave = 1
    }
    #endregion

    #region StatusCategory
    /// <summary>
    /// StatusCategory
    /// </summary>
    public enum StatusCategory
    {
        TalentRequisition,
        ProjectManagement,
        Approved,
        Closed,
        AssociateExit,
        LongLeave
    }
    #endregion

    #region EPCStatusCodes
    public enum EPCStatusCode
    {
        [Description("Approved")]
        Approved = 1,
        [Description("Pending")]
        Pending = 2,
        [Description("Rejected")]
        Rejected = 3
    }
    #endregion

    #region EPCNotificationStatusCode
    public enum EPCNotificationStatusCode
    {
        Pending = 1,
        Rejected = 2,
        Approved = 3
    }
    #endregion

    #region DepartmentCodes
    /// <summary>
    /// DepartmentCodes
    /// </summary>
    public enum DepartmentCodes
    {
        [Description("Training Department")]
        TrainingDepartment = 1,
        [Description("Technology & Delivery new")]
        TechnologyDelivery = 1
    }
    #endregion

    #region ProgramType
    /// <summary>
    /// ProgramType
    /// </summary>
    public enum ProgramType
    {
        None = 0,
        FullTime = 1,
        PartTime = 2,
        DistanceEducation = 3,
        Certification = 3,
        MemberShip = 4
    }
    #endregion

    #region AssociateExitStatusCodes
    /// <summary>
    /// AssociateExitStatusCodes
    /// </summary>
    public enum AssociateExitStatusCodes
    {
        SubmittedForResignation = 4,
        ApprovedByPM = 5,
        ApprovedByDeliveryHead = 6,
        ApprovedByIT = 7,
        ApprovedByAdmin = 8,
        ApprovedByFinance = 9,
        ApprovedByHRA = 10,
        Rejected = 11,
        Resigned = 12,
        Exit = 27
    }
    #endregion

    #region AssociateExitStatusCodesNew
    /// <summary>
    /// AssociateExitStatusCodes
    /// </summary>
    public enum AssociateExitStatusCodesNew
    {
        ResignationSubmitted = 20,
        ResignationApproved = 21,
        ResignationRevoked = 22,
        KTPlanInProgress = 23,
        KTPlanSubmitted = 24,
        KTPlanCompleted = 25,
        ResignationInProgress = 26,
        DepartmentActivityInProgress = 27,
        DepartmentActivityCompleted = 28,
        ClearanceGiven = 29,
        Resigned = 30,
        ExitInterviewCompleted = 31,
        RevokeInitiated = 32,
        //Absconding
        MarkedForAbscond = 33,
        AbscondAcknowledged = 34,
        //Termination
        TerminationSubmitted = 35,
        TerminationApproved = 36,
        //SeperationByHR
        SeperationByHRSubmitted = 37,
        SeperationByHRApproved = 38,
        RevokeRequested = 39,
        ReadyForClearance = 40,
        RevokeRejected = 41,
        ResignationReviewed = 42,
        AbscondConfirmed = 43,
        AbscondDisproved = 44,
        Absconded = 45,
        Blacklisted = 46
    }
    #endregion

    #region ProjectClosureStatusCodes
    /// <summary>
    /// ProjectClosureStatusCodes
    /// </summary>
    public enum ProjectClosureStatusCodes
    {
        SubmittedForClosureApproval = 21,
        ClosureApprovedByDH = 22,
        ClosureRejectedByDH = 23,
        InProgress = 24,
        Completed = 25,
        Closed = 18,
        ClosureInitiated = 20,
    }
    #endregion

    #region PracticeAreaCodes
    /// <summary>
    /// PracticeAreaCodes
    /// </summary>
    public enum PracticeAreaCodes
    {
        [Description("Training")]
        Training = 1
    }
    #endregion

    #region Roles
    /// <summary>
    /// Roles
    /// </summary>
    public enum Roles
    {
        [Description("SystemAdmin")]
        SystemAdmin = 1,
        [Description("HRM")]
        HRM = 2,
        [Description("Department Head")]
        DepartmentHead = 3,
        [Description("HRA")]
        HRA = 4,
        [Description("Program Manager")]
        ProgramManager = 5,
        [Description("Delivery")]
        Delivery = 6,
        [Description("Training Department Head")]
        TrainingDepartmentHead = 7,
        [Description("Competency Leader")]
        CompetencyLeader = 8,
        [Description("Team Lead")]
        TeamLead = 9,
        [Description("Associate")]
        Associate = 10,
        [Description("CEO")]
        CEO = 11,
        [Description("Service Manager")]
        ServiceManager = 12,
        [Description("IT Manager")]
        ITManager = 13,
        [Description("Finance Manager")]
        FinanceManager = 14,
        [Description("Admin Manager")]
        AdminManager = 15,
        [Description("Quality and Information Security Manager")]
        QualityandInformationSecurityManager = 16,
        // Description changed to Operation, from Operations to keep in-sync with Roles table
        [Description("Operation Head")]
        OperationsHead = 17,
        [Description("Corporate")]
        Corporate = 18
    }
    #endregion

    #region Notification Type
    /// <summary>
    /// Notification Type
    /// </summary>
    public enum NotificationType
    {
        EPC = 1,
        TRSbmitForApproval = 3,
        TRRejected = 2,
        KRASubmittedForDHReview = 1,
        KRAApproved = 3,
        KRASendBackForHRMReview = 2,
        DeleteKRAGroup = 4,
        SubmittedForHRHeadReview = 7,
        SendBackForDepartmentHeadReview = 8,
        Allocation = 9,
        Release = 10,
        EPU = 1,
        ESU = 11,
        PPC = 1,
        PPCApproved = 2,
        ClosureInitiated = 4,
        TLSubmitForApproval = 5,
        SMSubmit = 6,
        SubmittedForClosureApproval = 7,
        ClosureApprovedByDH = 3,
        ClosureRejectedByDH = 9,
        ClosureReportRejected = 10,
        SkillSubmittedForApproval = 1,
        SkillApprovedByRM = 2,
        LongLeave = 1,
        Resignation = 1,
        Revoke = 2,
        Rejoin = 2,
        //Resignation Codes from old branch -- start
        ResignationSubmitted = 1,
        ResignationApproved = 2,
        ResignationRevoked = 9,
        KTPlanInProgress = 3,
        KTPlanSubmitted = 4,
        KTPlanCompleted = 5,
        ActivitiesInProgress = 6,
        Completed = 10,
        ClearanceGiven = 7,
        Resigned = 8,
        ExitInterviewCompleted = 11,
        WithdrawResignation = 12,
        RevokeInitiated = 13,
        RevokeRejected = 14,
        ResignationReviewed = 15,
        ExitDateUpdated = 16,
        ReviewReminder = 17
        //Resignation Codes from old branch -- end
    }
    #endregion

    #region SkillStatuscode
    public enum SkillStatuscode
    {

        [Description("Created")]
        Created = 0,
        [Description("Approved")]
        Approved = 1,
        [Description("Pending")]
        Pending = 2
    }
    #endregion

    #region AttendanceSigningCode
    public enum AttendanceSigningCode
    {
        Initial = 0,
        SignedIn = 1,
        SignedOut = 2
    }
    #endregion

    #region AttendanceRegularizationStatusCode
    public enum AttendanceRegularizationStatusCode
    {
        AttendanceRegularizationSubmittedForApproval = 1,
        AttendanceRegularizationApproved = 2,
        AttendanceRegularizationRejected = 3
    }
    #endregion
}
