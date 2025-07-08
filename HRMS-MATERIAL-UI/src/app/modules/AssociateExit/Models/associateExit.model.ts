export class AssociateExit {
  public AssociateExitId: number;
  public SubmitType : string;
  public ExitTypeId: number;
  public ExitType: string;
  public ProjectId: number;
  public ProjectName: string;
  public EmployeeId: number;
  public ReasonId: number;
  public ReasonDetail: string;
  public ExitDate: string;
  public ResignationRecommendation: string;
  public RehireEligibility: boolean;
  public RehireEligibilityDetail: string;
  public TransitionRequired: boolean;
  public ImpactOnClientDelivery: boolean;
  public ImpactOnClientDeliveryDetail: string;
  public LegalExit: boolean;
  public Status: string;
  public ResignationDate: string;
  public AssociateRemarks: string;
  public CalculatedExitDate : string;
  public EmployeeCode: string;
  public EmployeeName: string;
  public DateOfJoin: string;
  public Gender: string;
  public Designation: string;
  public Grade: string;
  public Department: string;
  public Technology: string;
  public ReportingManagerId: number;
  public ProgramManagerId: number;
  public ReportingManager: string;
  public Experience: number;
  public Tenure: number;
  public DepartmentId: number;
  public ProgramManager: string;
  public CauseCategory: string;
  public CauseDetails: string;
  public email: string;
  public abscondedFromDate: string;
  public abscondedToDate: string;
  public StatusDesc : string;
  public RevokeStatusCode : string;
  public RevokeStatusDesc : string;
  public NoticePeriodInDays : number;
  public AssociateAllocationId:number;
}

export class Activity {
  ActivityTypeId : number;
  ActivityType : string;
	TransitionActivityDetails : TransitionActivityDetails[]
} 

export class TransitionActivityDetails {
	ActivityId : number
  Description : string
}

export class RevokeResignation {
  EmployeeId : number;
  RevokeReason : string;
  Comment : string;
}

export class FeedBackDetails{
  
  associateExitInterviewId: number;
  employeeId: number;
  associateExitId:  number;
  reasonId:  number;
  reasonDetail: string;
  alternateMobileNo: string;
  alternateEmail: string;
  alternateAddress:string;
  shareEmploymentInfo: true;
  includeInExAssociateGroup: true;
  remarks: string;
  systemInfo: string;
  createdBy: string;
  modifiedBy:string;
  IsNotified: boolean;

}


export class DashboardData {
  EmployeeId : number;
  EmployeeCode : string;
  EmployeeName : string;
  Designation : string; 
  ExitDate : string; 
  StatusCode : string;
  ActivitiesInProgress : boolean;
}

export class AssociateExitPMRequest
    {
        public AssociateExitId : number;
        public EmployeeId : number;        
        public ExitTypeId : number;
        public RehireEligibility : boolean;
        public RehireEligibilityDetail : string; 
        public ImpactOnClientDelivery : boolean;
        public ImpactOnClientDeliveryDetail : string;
        public ProgramManagerId : number;
    }