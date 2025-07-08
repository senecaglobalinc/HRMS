export enum ProgramType {
    certification = 3,
    membership = 4
}

export enum ProficiencyLevel {
    Expert = 1,
    Advanced = 2,
    Intermediate = 3,
    Basic = 4,
    Beginner = 5
}

export enum TRStatusCodes {
    Draft = 1,
    SubmittedForApproval = 2,
    SubmittedForFinanceApproval = 3,
    RejectedByDeliveryHead = 4,
    RejectedByFinanceHead = 5,
    Approved = 6,
    Close = 7
}
export enum KRAStatusCodes {
    Draft = 1,
    SubmittedForDepartmentHeadReview = 2,
    SendBackForHRMReview = 3,
    Approved = 4,
    SubmittedForHRHeadReview = 5,
    SendBackForDepartmentHeadReview = 6
}
export enum RequestType {
    NewRequest = 25,
    Replacement = 26
}

export enum InternalClient {
    SenecaGlobal
}

export enum Roles {
    HRM,
    HRA,
    ITHead,
    FinanceHead,
    HRHead,
    Lead,
    ProgramManager,
    MD
}

export enum DepartmentType {
    Delivery = 1,
    NonDelivery = 2
}

export enum ProjectType {
    TalentPool = 6
}
export enum KRAMeasurementType {
    Scale = 1,
    Percentage = 2,
    Number = 3,
    Date = 4
}
export enum CategoryMaster {
    TalentRequisition = 1,
    AssociateExit = 2,
    Skills = 3,
    EPC = 4,
    KRA = 5,
    ADR = 6
}
export enum Months {
    January = 1,
    february = 2,
    March = 3,
    April = 4,
    May = 5,
    June = 6,
    July = 7,
    August = 8,
    September = 9,
    October = 10,
    November = 11,
    December = 12
}

export enum PersonalDetails {
    PersonalEmailAddress = 1,
    MobileNo = 2,
    AadharNumber = 3,
    PANNumber = 4,
    PFNumber = 5,
    UANNumber = 6,
    PassportNumber = 7
}