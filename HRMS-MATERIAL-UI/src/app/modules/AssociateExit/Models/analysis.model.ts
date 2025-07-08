export class AnalysisData {
    public EmployeeCode: string;
    public EmployeeName: string;
    AssociateExitId: number;
    public ExitDate: string;
    ExitTypeId: number;
    public ExitType: string;
    ExitReasonId: number;
    public ExitReasonDetail: string;
    public ExitCauseDetails: string;
    public SummaryOfExitFeedback: string;
    public RootCause: string;
    public ActionItem: string;
    public Responsibility: string;
    public TargetDate: string;
    public ActualDate: string;
    public Remarks: string;
 FromDate: string;
    ToDate: string;
    StatusId: number;
    Status: string;
    EmployeeId: number;
}

export class AnalysisFilterData {
    public FromDate: Date;
    public ToDate: Date;
}
export class ReportsFilterData {
    public causeFilterData: AnalysisFilterData;
    public reportsData: Array<AnalysisData>;
    public TotalCount: number;
}
export class AssociateDetailReviewData {
    public AssociateExitInterviewId:number;
    public AssociateExitId:number;
    public AssociateName:string;
    public InitialRemarks:string;
    public FinalRemarks:string;
    public InitialRemarksNoHtml:string;
    public FinalRemarksNoHtml:string;
    public ShowInitialRemarks:boolean;
    public ShowInitialRemarksStr:string;
}

export class AssociateDetailData {
    public associateExitInterviewReviewId:number;
    public associateExitInterviewId:number;
    public finalRemarks:string;
    public showInitialRemarks:boolean;
}
