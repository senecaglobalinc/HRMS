import { EmergencyContactDetail } from "./family.model";

// import { Qualification } from './education.model'
// import { AssociateSkill } from './associate-skills.model'

export class Associate {
    public Id: number;
    public KRARoleId: number;
    //public EmpCode: string;//needs to be removed
    public EmployeeCode: string;
    public EmpId: number;
    public Year: number;
    public roleId: number;
    get name(): string {
        return this.FirstName + " " + this.MiddleName + " " + this.LastName;
    }
    public FirstName: string;
    public MiddleName: string;
    public LastName: string;
    public Gender: string;
    public PersonalEmailAddress: string;
    public JoinDate: Date;
    public JoiningDate: Date;
    public DateofJoining: string;
    public Birthdate: string;
    public DateofBirth: Date;
    public MaritalStatus: string;
    public BloodGroup: string;
    public Nationality: string;
    public Pannumber: string;
    public PassportNumber: string;
    public PassportIssuingOffice:string;
    public AadharNumber: string;
    public Uannumber: string;
    public Bgvstatus: string;
    public BgvStatusId: number;
    public BgvinitiatedDate: Date;
    public BgvStartDate: string;
    public BgvcompletionDate: Date;
    public BgvCompletionDate: string
    public Pfnumber: string;
    public joiningStatusID: number;//needs to be removed
    public GradeId: any;
    public GradeName: string;
    public DesignationId: number;
    public Designation: string;
    public EmploymentType: string;
    public EmployeeTypeId: number;
    public Technology: string;
    public TechnologyID: any;
    public TechnologyId: any;
    public DepartmentId: any;
    public HRAdvisorName: string;
    public Hradvisor: string;
    public ManagerId: any;
    public ReportingManager: number;
    public ReasonForDropOut: string;
    public EmploymentStartDate:Date;
    public StartDateofEmployment: string;
    public CareerBreak:number;
    public Qualifications: any[];
    public PrevEmploymentDetails: any[];
    public ProfReferences: any[];
    public RelationsInfo: any[];
    public Projects: any[];
    public contactDetails: any[];
    public contacts: any;
    public contactDetailsOne: any;
    public contactDetailsTwo: any;
    // public contactDetailsOne: EmergencyContactDetail;
    // public contactDetailsTwo: EmergencyContactDetail;
    public contactDetailsThird: any;
    public RecruitedBy: string;
    public MobileNo: number;
    public TelephoneNo: number;
    public associateType: string;//needs to be removed
    public EmployeeId: number;
    public PassportDateValidUpto: string;
}

export class ContactDetails {
    public addressType: string;
    public currentAddress1: string;
    public currentAddress2: string;
    public currentAddCity: string;
    public currentAddState: string;
    public currentAddZip: string;
    public currentAddCountry: string;
    public permanentAddress1: string;
    public permanentAddress2: string;
    public permanentAddCity: string;
    public permanentAddState: string;
    public permanentAddZip: string;
    public permanentAddCountry: string;
    // public address : string;
    public address: any;
}
