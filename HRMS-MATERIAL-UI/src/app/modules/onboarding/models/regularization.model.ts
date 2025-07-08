export class NotPunchInDate {
    associateId:string;
    fromDate : string;
    toDate : string;
}

export class AttendanceRegularization {
    RegularizationAppliedDate : string;
    SubmittedBy : string;
    Location : string;
    InTime : string;
    OutTime : string;
    SubmittedTo : number;
    RemarksByRM:string;
    Remarks : string;
    AssociateName?: string;
    ProjectName?: string;
    EmployeeId?: number;
    Status?: number;
    ApprovedBy?: number;
    IsActive?: boolean;
    IsApproved?:boolean;
    RegularizationDates: Array<string>;
    RegularizationCount : number;
    RejectedBy:number;
    RejectedDate:Date
}
