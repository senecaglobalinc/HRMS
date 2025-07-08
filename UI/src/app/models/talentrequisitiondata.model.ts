export class TalentRequistionData {
    TRCode:string;
    TalentRequisitionId: number;
    DepartmentId: any;    
    PracticeAreaId:number;
    ProjectId: any;
    ProjectName:string;
    RequestedDate: string;
    RequestedBy:string;
    RequiredDate: string;
    TargetFulfillmentDate: string;
    Status: string;
    RequisitionRoleDetails: RequisitionRoleDetails[];
    DeltedRRdetailsIds: any[];
    Department:string;
    RequisitionType: any;
    EmployeeId: number;
    ClientId:number;
    ProjectDuration:number;
    DepartmentTypeId:number;
}


export class RequisitionRoleDetails {
    RequisitionRoleDetailID: number;
    RoleMasterId: number;
    RoleName:string;
    NoOfBillablePositions: number;
    NoOfNonBillablePositions: number;
    TAPositions: number;
    YearsOfExperience: number;
    MinimumExperience:number;
    MaximumExperience:number;
    RoleDescription: string;
    KeyResponsibilities: string;
    EssentialEducationQualification: string;
    DesiredEducationQualification: string;
    Expertise: string;
    ProjectSpecificResponsibilities: string;
    RequisitionRoleSkills: any[];
    DeltedRRSkillIds: any;
    TalentRequisitionId: number;
    EitherExperienceorPositionsChanged:boolean;
}


export class RequisitionRoleSkills {
    CompetencyAreaId: number;
    SkillId: number;
    ProficiencyLevelId: number;
    RequistionRoleDetailID: number;
    TalentRequisitionId: number;
    //TalentRequisitionId: number;
    SkillGroupId: number;
    SkillCode: string;
    SkillGroupName: string;
    ProficiencyLevelCode: string;
    CompetencyAreaCode: string;
}

export class RequisitionLearningAptitude{
    RequisitionLearningAptitudeId: number;
    TalentRequisitionId: number;
    LearningAptitudeId: number;
    LearningAptitudeDescription:string;
    checked:boolean;
    RoleMasterId:number;
}

export class RequisitionWorkPlaceBehavior{
    RequisitionBehaviorAreaId: number;
    TalentRequisitionId: number;
    RoleMasterId:number;
    BehaviorAreaId: number;
    BehaviorCharacteristics: string;
    BehaviorRatingId: number;
    BehaviorArea: string
}
export class RequisitionExpertiseArea{
    RequisitionExpertiseAreaId: number;
    TalentRequisitionId: number;
    ExpertiseAreaId: number;
    ExpertiseDescription: string; 
    ExpertiseAreaDescription: string;
    ExpertiseId: number;
    RoleMasterId:number;
}